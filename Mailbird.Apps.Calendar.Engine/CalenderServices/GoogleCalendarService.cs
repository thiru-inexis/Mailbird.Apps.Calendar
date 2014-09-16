using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Windows.Media;
using Mailbird.Apps.Calendar.Engine.Interfaces;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Mailbird.Apps.Calendar.Engine.Extensions;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine.CalenderServices
{
    public class GoogleCalendarService 
    {
        #region Properties

        private CalendarService _calendarService;

        public CalenderProvider Name
        {
            get { return CalenderProvider.Google; }
        }

        #endregion


        public GoogleCalendarService()
        {
            Authorize();
        }

     
        private void Authorize()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { CalendarService.Scope.Calendar },
                    "MailbirdCalendar",
                    CancellationToken.None).Result;
            }

            // Create the service.
            _calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MailbirdCalendar",
            });

        }



        #region Calender {Get|Insert|Update|Delete}

        /// <summary>
        /// Fetchs calenders with incremental sync.
        /// </summary>
        /// <param name="nextSyncToken">The sync token to use for next call.</param>
        /// <param name="syncToken">The sync toekn to use to surrent call</param>
        /// <returns></returns>
        /// <remarks>
        /// If the exception thrown is of type Google.GoogleApiException with StatusCode 410('Gone')
        /// The synctoken being used is invalid and a fresh full sync has to be performed with syncToken 'null'
        /// </remarks>
        public IEnumerable<Metadata.Calendar> GetCalendars(out string nextSyncToken, string syncToken = null, bool showDeletedAlso = false, bool showHiddenAlso = false)
        {
            nextSyncToken = null;
            List<Metadata.Calendar> result = new List<Metadata.Calendar>();
            var calenderListRequest = _calendarService.CalendarList.List();
            calenderListRequest.SyncToken = syncToken;
            calenderListRequest.ShowDeleted = showDeletedAlso;
            calenderListRequest.ShowHidden = showHiddenAlso;

            string pageToken = null;
            Google.Apis.Calendar.v3.Data.CalendarList response = null;
            do
            {
                calenderListRequest.PageToken = pageToken;
                try 
                {
                    response = calenderListRequest.Execute();
                }
                catch(Google.GoogleApiException exp)
                {
                    result.Clear();
                    throw exp;
                }


                foreach (var calendarList in response.Items)
                {
                    //ToDo:  add provider to calender
                    result.Add(GetCalendar(new Metadata.Calendar() { Id = calendarList.Id }));
                }

                pageToken = response.NextPageToken;
            }
            while (pageToken != null);
            nextSyncToken = response.NextSyncToken;

            return result;
        }



        // get
        public Metadata.Calendar GetCalendar(Metadata.Calendar calendar)
        {
            Metadata.Calendar result = null;
            try
            {
                var calendarResponse = _calendarService.Calendars.Get(calendar.Id).Execute();
                result = calendarResponse.Clone();
                result.CalenderList = GetCalendarList(result);
                return result;
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted item is being access, a 404 is thrown.
                // Return a default value with deleted set to true.
                if (exp.HttpStatusCode != System.Net.HttpStatusCode.NotFound) { throw exp; }
                result = new Metadata.Calendar() { Id = calendar.Id, CalenderList = new Metadata.CalendarList() { IsDeleted = true } };
            }

            return result;
        }


        public Metadata.CalendarList GetCalendarList(Metadata.Calendar calendar)
        {
            Metadata.CalendarList result = null;
            try
            {
                var calendarListResponse = _calendarService.CalendarList.Get(calendar.Id).Execute();
                result = calendarListResponse.Clone();
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted item is being access, a 404 is thrown.
                // Return a default value with deleted set to true.
                if (exp.HttpStatusCode != System.Net.HttpStatusCode.NotFound) { throw exp; }
                result = new Metadata.CalendarList() { IsDeleted = true };
            }

            return result;
        }



        // insert
        public Metadata.Calendar InsertCalendar(Metadata.Calendar calendar)
        {
            var entry = calendar.Clone();
            var calendarResponse = _calendarService.Calendars.Insert(entry).Execute();
            var result = calendarResponse.Clone();

            result.CalenderList = (calendar.CalenderList != null)
                ? InsertCalendarList(calendar.CalenderList)
                : GetCalendarList(calendar);

            return result;
        }


        public Metadata.CalendarList InsertCalendarList(Metadata.CalendarList calendarList)
        {
            var entry = calendarList.Clone();
            var eventReponse = _calendarService.CalendarList.Insert(entry).Execute();
            return eventReponse.Clone();
        }


        // update
        public Metadata.Calendar UpdateCalendar(Metadata.Calendar calendar)
        {
            var entry = calendar.Clone();
            var calendarResponse = _calendarService.Calendars.Update(entry, entry.Id).Execute();
            var result = calendarResponse.Clone();

            if (calendar.CalenderList != null)
            {
                result.CalenderList = UpdateCalenderList(calendar.CalenderList);
            }

            return result;
        }

        public Metadata.CalendarList UpdateCalenderList(Metadata.CalendarList calendarList)
        {
            var entry = calendarList.Clone();
            var eventReponse = _calendarService.CalendarList.Update(entry, entry.Id).Execute();
            return eventReponse.Clone();
        }

     
        // delete

        public bool DeleteCalendar(Metadata.CalendarList calendar)
        {
            var entry = calendar.Clone();
            var calendarResponse = _calendarService.Calendars.Delete(entry.Id).Execute();
            return (string.IsNullOrEmpty(calendarResponse));
        }


        /// <summary>
        ///  This method is not allowed. As deleting a calenderlist prevent the tracking of calendar
        ///  via CalenderList.get() method
        /// </summary>
        /// <param name="calendarList"></param>
        /// <returns></returns>
        public bool DeleteCalendarList(Metadata.CalendarList calendarList)
        {
            throw new NotImplementedException();
            //var entry = calendarList.Clone();
            //var eventReponse = _calendarService.CalendarList.Delete(entry.Id).Execute();
            //return (string.IsNullOrEmpty(eventReponse));
        }


        #endregion



        #region Calender Events {Get|Insert|Update|Delete}


        public IEnumerable<Appointment> GetAppointments(out string nextSyncToken, Metadata.Calendar calender, string syncToken = null, bool showDeletedAlso = false)
        {
            nextSyncToken = null;
            List<Metadata.Appointment> result = new List<Appointment>();
            var request = _calendarService.Events.List(calender.Id);
            request.SyncToken = syncToken;
            request.ShowDeleted = showDeletedAlso;
            if (string.IsNullOrEmpty(syncToken)) { request.TimeMin = DateTime.MinValue; }

            string pageToken = null;
            Events response = null;
            do
            {
                request.PageToken = pageToken;
                try
                {
                    response = request.Execute();
                }
                catch (Google.GoogleApiException exp)
                {
                    result.Clear();
                    throw exp;
                }


                foreach (var appointment in response.Items)
                {
                    //ToDo:  add calender to appointment
                    result.Add(appointment.Clone(calender.Id));
                }

                pageToken = response.NextPageToken;
            }
            while (pageToken != null);
            nextSyncToken = response.NextSyncToken;

            return result;
        }


        public Appointment InsertAppointment(Appointment appointment)
        {
            var entry = appointment.Clone();
            var eventReponse = _calendarService.Events.Insert(entry, appointment.CalendarId).Execute();
            return eventReponse.Clone(appointment.CalendarId);
        }


        public Appointment UpdateAppointment(Appointment appointment)
        {
           // var entry = _calendarService.Events.Get(appointment.Calendar.CalendarId, appointment.Id.ToString()).Execute();

            var entry = appointment.Clone();
            var eventReponse = _calendarService.Events.Update(entry, appointment.CalendarId, entry.Id).Execute();
            return eventReponse.Clone(appointment.CalendarId);
        }


        public bool DeleteAppointment(Appointment appointment)
        {
            // As per the google spec, the service would return an 
            // empty string if the deletion was success.
            var eventReponse = _calendarService.Events.Delete(appointment.CalendarId, appointment.Id).Execute();
            return (string.IsNullOrEmpty(eventReponse));
        }


        #endregion


        /// <summary>
        /// Only Get is provided by the Google service
        /// </summary>
        /// <returns></returns>
        #region Colour {Get}


        public Dictionary<ColorType, List<Metadata.ColorDefinition>> GetColors(out string nextSyncToken, string syncToken = null)
        {
            nextSyncToken = null;
            var result = new Dictionary<ColorType, List<Metadata.ColorDefinition>>();
            var request = _calendarService.Colors.Get();

            try
            {
                var response = request.Execute();
                if (string.IsNullOrEmpty(syncToken) || !syncToken.Equals(response.UpdatedRaw))
                {
                    Action<ColorType, Dictionary<ColorType, List<Metadata.ColorDefinition>>> colorExtractHelper
                        = delegate(ColorType colorType, Dictionary<ColorType, List<Metadata.ColorDefinition>> target)
                    {
                        if (target == null) { target = new Dictionary<ColorType, List<Metadata.ColorDefinition>>(); }
                        if (target.ContainsKey(colorType)) { target.Remove(colorType); }
                        target.Add(colorType, new List<Metadata.ColorDefinition>());

                        var colorDict = (colorType == ColorType.Calendar) ? response.Calendar : response.Event;
                        foreach (var colorDef in colorDict)
                        {
                            Metadata.ColorDefinition temp = new Metadata.ColorDefinition()
                            {
                                Id = colorDef.Key,
                                Background = colorDef.Value.Background,
                                Foreground = colorDef.Value.Foreground
                            };
                            target[colorType].Add(temp);
                        }
                    };


                    colorExtractHelper(ColorType.Calendar, result);
                    colorExtractHelper(ColorType.Appointment, result);
                    nextSyncToken = response.UpdatedRaw;
                }
            }
            catch (Google.GoogleApiException exp)
            {
                result.Clear();
                nextSyncToken = null;
                throw exp;
            }

            return result;
        }



        #endregion
    }
}
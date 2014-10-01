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
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Util.Store;
using Mailbird.Apps.Calendar.Engine.Exceptions;

namespace Mailbird.Apps.Calendar.Engine.CalenderServices
{

    public class GoogleCalendarService : ISyncableCalendarProvider
    {

        private UserInfo _userInfo;
        private CalendarService _calendarService;

        #region Properties

        public CalenderProvider ProviderType
        {
            get { return CalenderProvider.Google; }
        }

        public UserInfo User
        {
            get { return _userInfo; }
            set { _userInfo = value; }
        }

        #endregion



        #region Constructor(s)

        /// <summary>
        /// Creates a new google calendar service object bound to the user account.
        /// </summary>
        /// <param name="userInfo"></param>
        public GoogleCalendarService(UserInfo userInfo)
        {
            if (userInfo == null) { throw new ArgumentNullException("userInfo", "Can not authorize with an null userinfo"); }
            _userInfo = userInfo;
            Authorize();
        }

        #endregion



        /// <summary>
        /// Authenticated the user account with google service
        /// </summary>
        /// May throw exceptoions
        private void Authorize()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                             GoogleClientSecrets.Load(stream).Secrets,
                             new[] { CalendarService.Scope.Calendar },
                             _userInfo.Id,
                             CancellationToken.None).Result;
            }


            // Create the service.
            _calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Mailbird Calendar"
            });


        }



        #region Calender {Get|Insert|Update|Delete}

        /// <summary>
        /// Fetch the calendars that needs to be synced.
        /// </summary>
        /// <param name="syncToken">Token to use for incemental sync</param>
        /// <param name="showDeletedAlso">Whether to fetch deleted calendars also</param>
        /// <param name="showHiddenAlso">whether to fetch hidden calendars also</param>
        /// <returns>Calendars to be synced and the token to use for next fetch</returns>
        /// <exception cref="InvalidSyncToken">
        /// When an invalid/expired synctokenis being used. Hence a full sync has to be done as 
        /// a counter measure.
        /// </exception>
        public SyncResult<Metadata.Calendar> FetchCalendars(SyncToken syncToken = null, bool showDeletedAlso = true, bool showHiddenAlso = true)
        {
            // Initialize data set to return
            var result = new SyncResult<Metadata.Calendar>() 
            { 
                NextSyncToken = (syncToken != null) 
                ? syncToken 
                : new SyncToken(SyncTokenType.Calendar) { ParentId = User.Id } 
            };

            // Attaching query options
            var calenderListRequest = _calendarService.CalendarList.List();
            calenderListRequest.SyncToken = result.NextSyncToken.Token;
            calenderListRequest.ShowDeleted = showDeletedAlso;
            calenderListRequest.ShowHidden = showHiddenAlso;

            string pageToken = null; // to store next requests page token
            Google.Apis.Calendar.v3.Data.CalendarList response = null; // expected response object

            do
            {
                calenderListRequest.PageToken = pageToken;
                try 
                {
                    response = calenderListRequest.Execute();
                }
                catch(Google.GoogleApiException exp)
                {
                    /// If the exception thrown is of type Google.GoogleApiException with StatusCode 410('Gone')
                    /// The synctoken being used is invalid and a fresh full sync has to be performed with syncToken set 'null'
                    /// Throw a InvalidSyncToekn exception with the google exception set a inner exception

                    if (exp.HttpStatusCode == System.Net.HttpStatusCode.Gone)
                    {
                        throw new InvalidSyncToken("Invalid Sync Token", exp);
                    }

                    throw exp;
                }


                foreach (var calendarList in response.Items)
                {
                    Metadata.Calendar currentCalender = null;

                    //ToDo: add provider to calender, to distinguish different calender providers

                    // A CalendarNotFound will be thrown is a deleted calendar is being accessed, and 
                    // hence this has to be deleted from the local storage, if existing in local, and
                    // also not to break the INCREMENTAL SYNC approach return 
                    // a new dummy calendar with DELETED set TRUE

                    try
                    {
                        currentCalender = GetCalendar(calendarList.Id);
                    }
                    catch (CalendarNotFound exp) 
                    {
                       currentCalender = new Metadata.Calendar()
                        {
                            UserId = User.Id,
                            Id = calendarList.Id,
                            CalenderList = new Metadata.CalendarList() { IsDeleted = true }
                        };
                    }

                    result.Result.Add(currentCalender);
                }

                pageToken = response.NextPageToken;
            }
            while (pageToken != null); // repeat until next page is avaliable
            // If we have come here, our fetch has been successfull,
            // next the store the synctoken to use for next fetch and return fetch results
            result.NextSyncToken.Token = response.NextSyncToken; 

            return result;
        }


        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Metadata.CalendarList> GetCalendars()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Retreives a calendar along with its calendarlist by Id
        /// </summary>
        /// <param name="calendarId">Calendar's google Id</param>
        /// <returns>Calendar if found, Else NULL</returns>
        /// <exception cref="CalendarNotFound">When an non-existing calendar is being accessed</exception>
        public Metadata.Calendar GetCalendar(string calendarId)
        {
            Metadata.Calendar result = null;
            try
            {
                var calendarResponse = _calendarService.Calendars.Get(calendarId).Execute();
                result = calendarResponse.CloneTo(User.Id);

                try
                {
                    // When an deleted calendarlist is accessed CalendarListNotFound may be thrown
                    // Handled it and hence the calendar's lsit will have the default values.
                    result.CalenderList = GetCalendarList(calendarId);
                }
                catch (CalendarListNotFound) { }
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Thorw a CalendarNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound) 
                {
                    throw new CalendarNotFound("Calendar not found", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        /// Gets calendar list by calendar id
        /// </summary>
        /// <param name="calendarId">Calendar's Google id</param>
        /// <returns>CalendarList object if found, Else NULL</returns>
        /// <exception cref="CalendarListNotFound">
        /// When an non-existing calendar is being accessed.
        /// Only exception with status 404 [HttpStatusCode.NotFound] is handled within function.
        /// </exception>
        public Metadata.CalendarList GetCalendarList(string calendarId)
        {
            Metadata.CalendarList result = null;
            try
            {
                var calendarListResponse = _calendarService.CalendarList.Get(calendarId).Execute();
                result = calendarListResponse.CloneTo();
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Thorw a CalendarNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new CalendarListNotFound("Calendar not found", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        /// Insert/Create a new calendar.
        /// The provider will be provide a calendar with its unique key.
        /// </summary>
        /// <param name="calendar">Calendar with prefered properties</param>
        /// <returns>Calendar after inserting, Null if failed</returns>
        public Metadata.Calendar InsertCalendar(Metadata.Calendar calendar)
        {
            var entry = calendar.CloneTo();
            /// Nullify readonly properties to prevent exceptions
            entry.Id = null;
            var calendarResponse = _calendarService.Calendars.Insert(entry).Execute();
            var result = calendarResponse.CloneTo(User.Id);

            /// The calendarlist model has to be validated before on entry.. This is a bad check
            /// If CalendaelIist is present  do insert, Else Get calendarlist, get the default list created
            /// by the provider
            result.CalenderList = (calendar.CalenderList != null && calendar.CalenderList.Id != null)
                ? InsertCalendarList(calendar.CalenderList)
                : GetCalendarList(result.Id);

            return result;
        }


        /// <summary>
        /// Insert/Create a new calendarlist.
        /// </summary>
        /// <param name="calendar">CalendarList with prefered properties</param>
        /// <returns>Calendarlist after inserting, Null if failed</returns>
        public Metadata.CalendarList InsertCalendarList(Metadata.CalendarList calendarList)
        {
            var entry = calendarList.CloneTo();
            // Nullify readonly properties to prevent exceptions
            entry.Summary = entry.Description = entry.AccessRole = null;
            entry.Primary = null;

            var eventReponse = _calendarService.CalendarList.Insert(entry).Execute();
            return eventReponse.CloneTo();
        }


        /// <summary>
        /// Update calendar properties 
        /// </summary>
        /// <param name="calendar">Calendar with modified properties</param>
        /// <returns>Calendar after updation, Null if failed</returns>
        /// <exception cref="CalendarNotFound">When an non-existing calendar is being updated</exception>
        public Metadata.Calendar UpdateCalendar(Metadata.Calendar calendar)
        {
            Metadata.Calendar result = null;

            try
            {
                var entry = calendar.CloneTo();
                var calendarResponse = _calendarService.Calendars.Update(entry, entry.Id).Execute();
                result = calendarResponse.CloneTo(User.Id);

                try
                {
                    // When an deleted calendarlist is accessed CalendarListNotFound may be thrown
                    // Handled it and hence the calendar's list will have the default values.
                    result.CalenderList = UpdateCalenderList(calendar.CalenderList);
                }
                catch (CalendarListNotFound) { }
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Thorw a CalendarNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new CalendarNotFound("Calendar not found", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        /// Update calendarlist properties 
        /// </summary>
        /// <param name="calendarList">CalendarList with modified properties</param>
        /// <returns>CalendarList after updation, Null if failed</returns>
        /// <exception cref="CalendarListNotFound">When an non-existing calendar is being updated</exception>
        public Metadata.CalendarList UpdateCalenderList(Metadata.CalendarList calendarList)
        {
            Metadata.CalendarList result = null;
            try
            {
                var entry = calendarList.CloneTo();
                // Nullify readonly properties to prevent exceptions
                entry.Summary = entry.Description = entry.AccessRole = null;
                entry.Primary = null;

                var eventReponse = _calendarService.CalendarList.Update(entry, entry.Id).Execute();
                result = eventReponse.CloneTo();
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Thorw a CalendarListNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new CalendarListNotFound("CalendarList not found", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        /// Delete a calendar and may also delete its bounded events
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns>True is success, else False</returns>
        /// <exception cref="CalendarNotFound">When an non-existing calendar is being accessed</exception>
        /// <exception cref="CalendarDeletionFailed">When a primary calendar is being deleted</exception>
        /// <remarks>
        /// Only calendar Id is required by the google api to perform a delete,
        /// Put providing the object as a whole, will prevent any other fiiends that may have to be 
        /// add in the future.
        /// </remarks>
        public bool DeleteCalendar(Metadata.Calendar calendar)
        {
            bool result = false;

            try
            { 
                var entry = calendar.CloneTo();
                var calendarResponse = _calendarService.Calendars.Delete(entry.Id).Execute();
                /// If the calendar was successfully deleted, and empty response is received
                result = (string.IsNullOrEmpty(calendarResponse));
            }
            catch(Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Thorw a CalendarNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new CalendarNotFound("Calendar not found.", exp);
                }

                /// When a delete is performed in a primary calendar, google api
                /// throws an Forbidden[403] exception. 
                /// Handle this and throw mailbird exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new CalendarDeletionFailed("Unable to delete calendar", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        ///  This method is not allowed. As deleting a calenderlist prevent the tracking of calendar
        ///  via google api CalenderList.get() method
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

       
        /// <summary>
        /// Fetch the appointment of a calendar that needs to be synced.
        /// </summary>
        /// <param name="calender">The calendar of whose appointments are going to be synced, FYI Only calendarId is needed</param>
        /// <param name="syncToken">Token to use for incemental sync</param>
        /// <param name="showDeletedAlso">Whether to fetch deleted calendars also</param>
        /// <param name="showHiddenAlso">whether to fetch hidden calendars also</param>
        /// <returns>Appointents to be synced and the token to use for next fetch</returns>
        /// <exception cref="InvalidSyncToken">
        /// When an invalid/expired synctokenis being used. Hence a full sync has to be done as 
        /// a counter measure.
        /// </exception>
        /// <exception cref="CalendarNotFound">
        /// When the specific calendar is not found.
        /// </exception>
        public SyncResult<Appointment> FetchAppointments(Metadata.Calendar calender, SyncToken syncToken = null, bool showDeletedAlso = true, bool showHiddenAlso = true)
        {
            // Initialize data set to return
            var result = new SyncResult<Metadata.Appointment>()
            {
                NextSyncToken = (syncToken != null)
                ? syncToken
                : new SyncToken(SyncTokenType.CalenderAppointments) { ParentId = calender.Id }
            };

            // Attaching query options
            var appointmentRequest = _calendarService.Events.List(calender.Id);
            appointmentRequest.SyncToken = result.NextSyncToken.Token;
            appointmentRequest.ShowDeleted = showDeletedAlso;
            if (string.IsNullOrEmpty(result.NextSyncToken.Token)) { appointmentRequest.TimeMin = DateTime.MinValue; }

            string pageToken = null; // to store pagetoken of next request
            Events response = null; // expected response object
            do
            {
                appointmentRequest.PageToken = pageToken;
                try
                {
                    response = appointmentRequest.Execute();
                }
                catch (Google.GoogleApiException exp)
                {
                    /// This exception will be thrown when events of an non-existing
                    /// calendar is being accessed.
                    if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new CalendarNotFound("Calendar not found", exp);
                    }

                    /// If the exception thrown is of type Google.GoogleApiException with StatusCode 410('Gone')
                    /// The synctoken being used is invalid and a fresh full sync has to be performed with syncToken set 'null'
                    /// Throw a InvalidSyncToekn exception with the google exception set a inner exception
                    if (exp.HttpStatusCode == System.Net.HttpStatusCode.Gone)
                    {
                        throw new InvalidSyncToken("Invalid Sync Token", exp);
                    }

                    throw exp;
                }


                foreach (var appointment in response.Items)
                {
                    //ToDo: add calender to appointment
                    result.Result.Add(appointment.CloneTo(calender.Id));
                }

                pageToken = response.NextPageToken;
            }
            while (pageToken != null); // repeat until next page is available
            // If we have reached this point, our fetch has been successfull,
            // next the store the synctoken to use for next fetch and return fetch results
            result.NextSyncToken.Token = response.NextSyncToken;

            return result;
        }


        /// <summary>
        /// Fetch all appopintments related to a user account
        /// NOT Implemented
        /// </summary>
        /// <returns>List of appointments</returns>
        public IEnumerable<Appointment> GetAppointments()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Get appopintments related to a calendar
        /// NOT IMPLEMENTED
        /// </summary>
        public IEnumerable<Appointment> GetAppointments(string calendarId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Fetch an appointmnet Id and calendar Id
        /// </summary>
        /// <param name="appointmentId">Appointments unique Id</param>
        /// <param name="calendarId">Calendar unique Id</param>
        /// <returns>AppointmentId if found, Else NULL</returns>
        /// <exception cref="AppointmentNotFound">When a non-existing appointment/calendar is being accessed</exception>
        public Appointment GetAppointment(string appointmentId, string calendarId)
        {
            Metadata.Appointment result = null;
            try
            {
                var appointmentResponse = _calendarService.Events.Get(calendarId, appointmentId).Execute();
                result = appointmentResponse.CloneTo(calendarId);
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Thorw a CalendarNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new CalendarNotFound("Calendar/Appointment not found", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        /// Insert a new appointment to a calendar
        /// </summary>
        /// <param name="appointment">appointment to insert</param>
        /// <returns>The newly added appointment with a unique id, If failed Null</returns>
        /// <exception cref="CalendarNotFound">When adding to a non-existing calendar</exception>
        public Appointment InsertAppointment(Appointment appointment)
        {
            Metadata.Appointment result = null;

            try
            {
                var entry = appointment.CloneTo();
                var eventReponse = _calendarService.Events.Insert(entry, appointment.CalendarId).Execute();
                return eventReponse.CloneTo(appointment.CalendarId);
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Thorw a CalendarNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new CalendarNotFound("Calendar not found", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        /// Update an appointment 
        /// </summary>
        /// <param name="appointment">appointmnt to update</param>
        /// <returns>The updated appointment, If failed value will be null</returns>AppointmentNotFound
        /// <exception cref="AppointmentNotFound">When updating a non-existing appointment in the specified calendar</exception>
        public Appointment UpdateAppointment(Appointment appointment)
        {
            Metadata.Appointment result = null;

            try
            {
                var entry = appointment.CloneTo();
                var eventReponse = _calendarService.Events.Update(entry, appointment.CalendarId, entry.Id).Execute();
                return eventReponse.CloneTo(appointment.CalendarId);
            }
            catch (Google.GoogleApiException exp)
            {
                // If an non-existing calendar/appointment is being access, a 404 [NotFound] is thrown.
                // Thorw a AppointmentNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new AppointmentNotFound("Appointment/Calendar not found", exp);
                }

                throw exp;
            }

            return result;
        }


        /// <summary>
        /// Delete/remove an appointment from a calendar
        /// </summary>
        /// <param name="appointment">appointment to remove</param>
        /// <returns>True is success, else False.</returns>
        /// <exception cref="AppointmentNotFound">When deleting a non-existing appointment in the specified calendar</exception>
        /// <remarks>
        /// Only appointment Id and calendar Id is required by the google api to perform a delete,
        /// Put providing the object as a whole, will prevent any other fiiends that may have to be 
        /// add in the future.
        /// </remarks>
        public bool DeleteAppointment(Appointment appointment)
        {
            bool result = false;

            try
            {
                var appointmentReponse = _calendarService.Events.Delete(appointment.CalendarId, appointment.Id).Execute();
                /// If deletion was successfully, and empty response is received
                result = (string.IsNullOrEmpty(appointmentReponse));
            }
            catch (Google.GoogleApiException exp)
            {
                // If an deleted/never-existed item is being access, a 404 [NotFound] is thrown.
                // Throw a AppointmentNotFound with GoogleApiException set as inner exception
                if (exp.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new AppointmentNotFound("Appointment not found.", exp);
                }

                throw exp;
            }

            return result;
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
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

namespace Mailbird.Apps.Calendar.Engine.CalendarProviders
{
    public class GoogleCalendarProvider : ICalendarProvider
    {
        private CalendarService _calendarService;

        public GoogleCalendarProvider()
        {
            Name = "GoogleCalendarsStorage";
            Authorize();
        }

        public IEnumerable<Metadata.Calendar> Calendars { get; private set; }

        public string Name { get; private set; }

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

            Calendars = GetCalendars();
        }

        public IEnumerable<Metadata.Calendar> GetCalendars()
        {
            var calendarListEntry = _calendarService.CalendarList.List().Execute().Items;
            var list = calendarListEntry.Select(c => new Metadata.Calendar
            {
                CalendarId = c.Id,
                Name = c.Summary,
                Description = c.Description,
                AccessRights = c.AccessRole == "reader" ? Metadata.Calendar.Access.Read : Metadata.Calendar.Access.Write,
                CalendarColor = (Color)ColorConverter.ConvertFromString(c.BackgroundColor),
                Provider = "GoogleCalendarsStorage"
            });
            return list;
        }
        
        public IEnumerable<Appointment> GetAppointments()
        {
            var appointments = new List<Appointment>();
            foreach (var calendar in Calendars)
            {
                appointments.AddRange(GetAppointments(calendar));
            }
            return appointments;
        }

        public IEnumerable<Appointment> GetAppointments(Metadata.Calendar calendar)
        {
            return GetAppointments(calendar.CalendarId);
        }

        public IEnumerable<Appointment> GetAppointments(string calendarId)
        {
            var calender = GetCalendars().FirstOrDefault(m => (m.CalendarId == calendarId));
            var calendarEvents = _calendarService.Events.List(calendarId).Execute().Items;
            var list = calendarEvents.Select(a => a.Clone(calender));
            return list;
        }

        public Appointment InsertAppointment(Appointment appointment)
        {
            var googleEvent = appointment.Clone();
            var eventReponse = _calendarService.Events.Insert(googleEvent, appointment.Calendar.CalendarId).Execute();
            return eventReponse.Clone(appointment.Calendar);
        }

        public Appointment UpdateAppointment(Appointment appointment)
        {
            var googleEvent = _calendarService.Events.Get(appointment.Calendar.CalendarId, appointment.Id.ToString()).Execute();
            googleEvent.Start.DateTime = appointment.StartTime;
            googleEvent.End.DateTime = appointment.EndTime;
            googleEvent.Summary = appointment.Subject;
            googleEvent.Description = appointment.Description;
            googleEvent.Location = appointment.Location;

            var eventReponse = _calendarService.Events.Update(googleEvent, appointment.Calendar.CalendarId, googleEvent.Id).Execute();
            return eventReponse.Clone(appointment.Calendar) ;
        }


        public bool RemoveAppointment(Appointment appointment)
        {
            // As per the google spec, the service would return an 
            // empty string if the deletion was success.
            var eventReponse = _calendarService.Events.Delete(appointment.Calendar.CalendarId, appointment.Id.ToString()).Execute();
            return (string.IsNullOrEmpty(eventReponse));
        }
    }
}
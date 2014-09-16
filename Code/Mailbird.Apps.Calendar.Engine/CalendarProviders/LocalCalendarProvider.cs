using System;
using System.Collections.Generic;
using Mailbird.Apps.Calendar.Engine.Interfaces;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.Utility;

namespace Mailbird.Apps.Calendar.Engine.CalendarProviders
{
    public class LocalCalendarProvider : ICalendarProvider
    {
        private const string LocalStoragePath = @".\LocalStorage2.txt";

        private readonly List<Metadata.Calendar> _calendars = new List<Metadata.Calendar>();

        private readonly List<Appointment> _calendarsEvents = new List<Appointment>();

        private readonly JsonWorker<DataStore> _worker;

        public string Name { get; private set; }

        public LocalCalendarProvider()
        {
            Name = "LocalCalendarsStorage";
            _worker = new JsonWorker<DataStore>(LocalStoragePath);
            LoadCalendars();
        }

        private void LoadCalendars()
        {
            var calendar = new Metadata.Calendar { CalendarId = Guid.NewGuid().ToString(), Provider = "LocalCalendarsStorage", Name = "LocalCalendar" };
            _calendars.Add(calendar);
            _worker.GetData<Appointment>().ForEach(f => _calendarsEvents.Add(f));
        }

        public IEnumerable<Metadata.Calendar> GetCalendars()
        {
            return _calendars;
        }

        public IEnumerable<Appointment> GetAppointments()
        {
            return _calendarsEvents;
        }

        public IEnumerable<Appointment> GetCalendarEvents(string calendarId)
        {
            return _calendarsEvents;
        }

        public IEnumerable<Appointment> GetAppointments(string empty)
        {
            return _calendarsEvents;
        }

        public Appointment InsertAppointment(Appointment appointment)
        {
            _calendarsEvents.Add(appointment);
            _worker.SaveData(appointment);
            return appointment; // for testing
        }

        public Appointment UpdateAppointment(Appointment appointment)
        {
            return appointment; // for testing
        }

        public bool RemoveAppointment(Appointment appointment)
        {
            return true; // for testing
        }
    }
}
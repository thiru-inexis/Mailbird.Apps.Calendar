using System;
using System.Collections.Generic;
using System.Linq;
using Mailbird.Apps.Calendar.Engine.Interfaces;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.CalenderServices;

namespace Mailbird.Apps.Calendar.Engine
{
    /// <summary>
    /// CalendarCatalog create all instamces of ICalendarProvider and calendars workout
    /// go throught this clas
    /// </summary>
    public class CalendarsCatalog : ICalendarCatalog
    {
        private readonly LocalCalenderService _calenderService;

        public Metadata.CalendarList DefaultCalendar { get; set; }

        public CalendarsCatalog(LocalCalenderService localCalenderService)
        {
            _calenderService = localCalenderService;
        }




        public IEnumerable<Metadata.Calendar> GetCalendars()
        {
            return _calenderService.CalendarRepo.Get();
        }

        public Metadata.Calendar GetCalendar(string calendarId)
        {
            return _calenderService.CalendarRepo.Get(calendarId);
        }

        public Metadata.Calendar InsertCalendar(Metadata.Calendar calender)
        {
            var result = _calenderService.CalendarRepo.Add(calender);
            _calenderService.SaveChanges();
            return result;
        }

        public Metadata.Calendar UpdateCalendar(Metadata.Calendar calender)
        {
            var result = _calenderService.CalendarRepo.Update(calender);
            _calenderService.SaveChanges();
            return result;
        }

        public bool RemoveCalendar(Metadata.Calendar calender)
        {
            var toDeleteApp = GetAppointments(calender.Id);
            foreach(var app in toDeleteApp)
            {
                _calenderService.AppointmentRepo.Delete(app);
            }
            _calenderService.CalendarRepo.Delete(calender);
            _calenderService.SaveChanges();
            return true;
        }





        public IEnumerable<Appointment> GetAppointments()
        {
            return _calenderService.AppointmentRepo.Get();
        }

        public IEnumerable<Appointment> GetAppointments(string calendarId)
        {
            return _calenderService.AppointmentRepo.Get().Where(m => m.CalendarId == calendarId).ToList();
        }

        public Appointment InsertAppointment(Appointment appointment)
        {
            var result = _calenderService.AppointmentRepo.Add(appointment);
            _calenderService.SaveChanges();
            return result;
        }

        public Appointment UpdateAppointment(Appointment appointment)
        {
            var result = _calenderService.AppointmentRepo.Update(appointment);
            _calenderService.SaveChanges();
            return result;
        }

        public bool RemoveAppointment(Appointment appointment)
        {
            var result = _calenderService.AppointmentRepo.Delete(appointment);
            _calenderService.SaveChanges();
            return result;
        }




        public IEnumerable<string> GetLocations()
        {
            var result = new List<string>(){"New Hamphiree","New York", "Colombo", "Paris", "Oslo", "Geneva", "Denmark", "Norway"};
            return result;
        }
    }
}
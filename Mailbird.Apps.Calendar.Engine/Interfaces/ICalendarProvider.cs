using System.Collections.Generic;
using Mailbird.Apps.Calendar.Engine.Metadata;

namespace Mailbird.Apps.Calendar.Engine.Interfaces
{
    /// <summary>
    /// Implemenration of this interface provide all base functionality to work with calendar and appointments
    /// </summary>
    public interface ICalendarProvider
    {
        string Name { get; }

        /// <summary>
        /// return all calendars of the provider
        /// </summary>
        /// <returns></returns>
        IEnumerable<Metadata.Calendar> GetCalendars();

        /// <summary>
        /// return all apppointments of the provider
        /// </summary>
        /// <returns></returns>
        IEnumerable<Appointment> GetAppointments();

        /// <summary>
        /// return all appopintments of the provider and particular calendar
        /// </summary>
        /// <param name="calendarId">calendar uniq identifier</param>
        /// <returns></returns>
        IEnumerable<Appointment> GetAppointments(string calendarId);


        /// <summary>
        /// insert appointment into storage
        /// </summary>
        /// <param name="appointment">appointment to insert</param>
        /// <returns></returns>
        bool InsertAppointment(Appointment appointment);

        /// <summary>
        /// upadte existed appointment
        /// </summary>
        /// <param name="appointment">appointmnt to update</param>
        /// <returns></returns>
        bool UpdateAppointment(Appointment appointment);

        /// <summary>
        /// remove appointment
        /// </summary>
        /// <param name="appointment">appointment to remove</param>
        /// <returns></returns>
        bool RemoveAppointment(Appointment appointment);
    }
}
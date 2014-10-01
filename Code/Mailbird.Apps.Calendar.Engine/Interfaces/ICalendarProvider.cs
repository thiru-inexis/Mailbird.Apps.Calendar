using System.Collections.Generic;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine.Interfaces
{

    /// <summary>
    /// Any calendar provider, that is to connect with the calendar app's
    /// local storage shoould implement this interface.
    /// </summary>
    public interface ICalendarProvider
    {
        /// <summary>
        /// This helps to identify what kind of a service this provder is
        /// </summary>
        CalenderProvider ProviderType { get; }


        /// <summary>
        /// The user account that is attached to the provider
        /// </summary>
        UserInfo User { get; set; }


        #region Calender {Get|Insert|Update|Delete}

        /// <summary>
        /// Gat all calendars bound to the user account.
        /// </summary>
        /// <returns>List of calendars</returns>
        IEnumerable<Metadata.CalendarList> GetCalendars();


        /// <summary>
        /// Retreives a calendar by its id
        /// </summary>
        /// <param name="calendarId">Calendar Id</param>
        /// <returns>Calendar if found, Else NULL</returns>
        /// <exception cref="CalendarNotFound">When an non-existing calendar is being updated</exception>
        Metadata.Calendar GetCalendar(string calendarId);


        /// <summary>
        /// Insert/Create a new calendar.
        /// The provider will be provide a calendar with its unique key.
        /// </summary>
        /// <param name="calendar">Calendar with prefered properties</param>
        /// <returns>Calendar after inserting, Null if failed</returns>
        Metadata.Calendar InsertCalendar(Metadata.Calendar calendar);


        /// <summary>
        /// Update calendar properties 
        /// </summary>
        /// <param name="calendar">Calendar with modified properties</param>
        /// <returns>Calendar after updation, Null if failed</returns>
        /// <exception cref="CalendarNotFound">When an non-existing calendar is being updated</exception>
        Metadata.Calendar UpdateCalendar(Metadata.Calendar calendar);


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
        bool DeleteCalendar(Metadata.Calendar calendar);

        #endregion



        #region Appointment {Get|Insert|Update|Delete}

        /// <summary>
        /// Fetch all appopintments related to a user account
        /// </summary>
        /// <returns>List of appointments</returns>
        IEnumerable<Appointment> GetAppointments();


        /// <summary>
        /// Fetch appopintments related to a calendar
        /// </summary>
        /// <param name="calendarId">calendar Id</param>
        /// <returns>List of appointments</returns>
        /// <exception cref="CalendarNotFound">When an non-existing calendar is being accessed</exception>
        IEnumerable<Appointment> GetAppointments(string calendarId);


        /// <summary>
        /// Fetch an appointmnet Id and calendar Id
        /// </summary>
        /// <param name="appointmentId">Appointments unique Id</param>
        /// <param name="calendarId">Calendar unique Id</param>
        /// <returns>AppointmentId if found, Else NULL</returns>
        /// <exception cref="AppointmentNotFound">When a non-existing appointment is being accessed</exception>
        Metadata.Appointment GetAppointment(string appointmentId, string calendarId);


        /// <summary>
        /// Insert a new appointment to a calendar
        /// </summary>
        /// <param name="appointment">appointment to insert</param>
        /// <returns>The newly added appointment with a unique id, If failed Null</returns>
        /// <exception cref="CalendarNotFound">When adding to a non-existing calendar</exception>
        Appointment InsertAppointment(Appointment appointment);


        /// <summary>
        /// Update an appointment 
        /// </summary>
        /// <param name="appointment">appointmnt to update</param>
        /// <returns>The updated appointment, If failed value will be null</returns>AppointmentNotFound
        /// <exception cref="AppointmentNotFound">When updating a non-existing appointment in the specified calendar</exception>
        Appointment UpdateAppointment(Appointment appointment);


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
        bool DeleteAppointment(Appointment appointment);

        #endregion

    }
}
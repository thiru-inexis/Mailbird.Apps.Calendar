using System.Collections.Generic;
using Mailbird.Apps.Calendar.Engine.Metadata;

namespace Mailbird.Apps.Calendar.Engine.Interfaces
{
    /// <summary>
    /// Implemenration of this interface provide all base functionality to work with calendar and appointments
    /// </summary>
    public interface ICalendarCatalog
    {

        #region User Contracts

        /// <summary>
        /// return all users
        /// </summary>
        /// <returns></returns>
        IEnumerable<Metadata.UserInfo> GetUsers();


        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <returns></returns>
        Metadata.UserInfo GetUSer(string userId);


        /// <summary>
        /// insert user
        /// </summary>
        /// <param name="user">user to insert</param>
        /// <returns>The newly added user, If failed value will be null</returns>
        Metadata.UserInfo InsertUser(Metadata.UserInfo user);


        /// <summary>
        /// update existing user
        /// </summary>
        /// <param name="user">user to update</param>
        /// <returns>The updated user, If failed value will be null</returns>
        Metadata.UserInfo UpdateUser(Metadata.UserInfo user);


        /// <summary>
        /// remove user
        /// </summary>
        /// <param name="user">user to remove</param>
        ///// <returns>True is success, else False.</returns>
        bool RemoveUser(Metadata.UserInfo user);


        #endregion



        #region Calender Contracts

        /// <summary>
        /// return all calendars of the provider
        /// </summary>
        /// <returns></returns>
        IEnumerable<Metadata.Calendar> GetCalendars();


        /// <summary>
        /// return all calendars of the provider
        /// </summary>
        /// <returns></returns>
        Metadata.Calendar GetCalendar(string calendarId);

        /// <summary>
        /// insert appointment into storage
        /// </summary>
        /// <param name="appointment">appointment to insert</param>
        /// <returns>The newly added appointment, If failed value will be null</returns>
        Metadata.Calendar InsertCalendar(Metadata.Calendar calender);


        /// <summary>
        /// upadte existed appointment
        /// </summary>
        /// <param name="appointment">appointmnt to update</param>
        /// <returns>The updated appointment, If failed value will be null</returns>
        Metadata.Calendar UpdateCalendar(Metadata.Calendar calender);


        /// <summary>
        /// remove appointment
        /// </summary>
        /// <param name="appointment">appointment to remove</param>
        ///// <returns>True is success, else False.</returns>
        bool RemoveCalendar(Metadata.Calendar calender);


        #endregion


        #region Appointment Contracts


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
        /// Get and appointment by Id
        /// </summary>
        /// <param name="appointmentId">Id of the to-fetch appointment</param>
        /// <returns>Appointment object if keys match, else null</returns>
        Appointment GetAppointment(string appointmentId);


        /// <summary>
        /// insert appointment into storage
        /// </summary>
        /// <param name="appointment">appointment to insert</param>
        /// <returns>The newly added appointment, If failed value will be null</returns>
        Appointment InsertAppointment(Appointment appointment);


        /// <summary>
        /// upadte existed appointment
        /// </summary>
        /// <param name="appointment">appointmnt to update</param>
        /// <returns>The updated appointment, If failed value will be null</returns>
        Appointment UpdateAppointment(Appointment appointment);


        /// <summary>
        /// remove appointment
        /// </summary>
        /// <param name="appointment">appointment to remove</param>
        ///// <returns>True is success, else False.</returns>
        bool RemoveAppointment(Appointment appointment);


        #endregion



        IEnumerable<string> GetLocations();

    }
}
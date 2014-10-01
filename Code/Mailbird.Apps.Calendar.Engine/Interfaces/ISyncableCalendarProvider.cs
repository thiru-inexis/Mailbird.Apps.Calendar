using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Interfaces
{
    /// <summary>
    /// This is used to encapsulated a result returned from a sync call
    /// </summary>
    public class SyncResult<T> where T : class
    {
        #region Properties

        /// <summary>
        /// The token to be used for next sync
        /// </summary>
        public Metadata.SyncToken NextSyncToken { get; set; }

        /// <summary>
        /// The items that needs to be synced
        /// </summary>
        public IList<T> Result { get; set; }

        #endregion


        public SyncResult()
        {
            NextSyncToken = null;
            Result = new List<T>();
        }

    }


    /// <summary>
    /// Any calendar provider that supports token based sync
    /// should implement this interface.
    /// </summary>
    public interface ISyncableCalendarProvider : ICalendarProvider
    {
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
        SyncResult<Metadata.Calendar> FetchCalendars(Metadata.SyncToken syncToken = null, bool showDeletedAlso = true, bool showHiddenAlso = true);


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
        SyncResult<Metadata.Appointment> FetchAppointments(Metadata.Calendar calender, Metadata.SyncToken syncToken = null, bool showDeletedAlso = true, bool showHiddenAlso = true);
    }
}

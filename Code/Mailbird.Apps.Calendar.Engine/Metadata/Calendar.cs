using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Calendar stucture that is used accross the application
    /// </summary>
    public class Calendar: LocalStorageData
    {
        #region Properties

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public string Etag { get; set; }

        /// <summary>
        /// A user id to which the calendar is bounded to.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Customizable calendar Title/Name
        /// [Optional]
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Calendar description.
        /// [Optional]
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Place that a calendar might be specific to.
        /// [Optional]
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The Timezone of the calendar.
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Holds the visual properties of the calendar.
        /// [Required | Not Nullable]
        /// </summary>
        public CalendarList CalenderList { get; set; }

        #endregion



        #region Constructor(s)

        public Calendar()
            : this(TimeZoneInfo.Local, new CalendarList())
        {
            //this.TimeZone = TimeZone.CurrentTimeZone;
            //this.CalenderList = new CalendarList()
            //{
            //    TimeZone = "UTC"
            //};
        }


        /// <summary>
        /// Creates a new calendar instance with specified time zone and visual settings 
        /// </summary>
        /// <param name="timezone">Time zone of the calendar</param>
        /// <param name="calendarList">Visul settings of the calendar</param>
        public Calendar(TimeZoneInfo timezone, CalendarList calendarList)
            :base()
        {
            this.TimeZone = timezone;
            this.CalenderList = calendarList;
        }

        #endregion
    }


}

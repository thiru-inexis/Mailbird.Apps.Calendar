using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Appointment stucture that is used accross the application
    /// </summary>
    public class Appointment : LocalStorageData
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
        /// Appointment status
        /// </summary>
        public AppointmentStatus Status { get; set; }

        /// <summary>
        /// Appointment tranparency, avalilabele/busy
        /// </summary>
        public AppointmentTransparency Transparency { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public string HtmlLink { get; set; }

        /// <summary>
        /// When the appointment was created on
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// When the appointment was last updated
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Appointment Tile/Name 
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Appointment description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A place specific to the appointment
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Start date and time of the appointment
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End date and time of the appointment
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// An iCal unique id
        /// </summary>
        public string ICalUID { get; set; }

        /// <summary>
        /// Appointment's Reminders
        /// </summary>
        public List<Reminder> Reminders { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// Google provides and interface to choose a color 
        /// from a pallet of 11 default {Backgorund, Foreground} set.
        /// </summary>
        public string ColorId { get; set; }

        /// <summary>
        /// Background and Foreground colors to be used
        /// </summary>
        public ColorDefinition Color { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public object ReminderInfo { get; set; } // For local
        
        /// <summary>
        /// Parent calender Id
        /// </summary>
        public string CalendarId { get; set; } 



        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public bool IsAnyoneCanAddSelf { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public bool IsGuestsCanInviteOthers { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public bool IsGuestsCanModify { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public bool IsGuestsCanSeeOtherGuests { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public bool IsPrivateCopy { get; set; }

        /// <summary>
        /// A Google Calender specific propertry.
        /// </summary>
        public bool IsLocked { get; set; }

        #endregion



        #region Contructor(s)

        public Appointment()
            : base()
        {
            this.StartTime = DateTime.Now;
            this.EndTime = DateTime.Now;
            this.Reminders = new List<Reminder>();
            this.Color = new ColorDefinition();
        }

        #endregion 

    }




    /// <summary>
    /// Represents a appointment reminder
    /// </summary>
    public class Reminder
    {
        #region Properties

        /// <summary>
        /// Reminder type, the typpe of action that should be performed
        /// </summary>
        public ReminderType Type { get; set; }

        /// <summary>
        /// When the reminder should be fired related to the start date of the appointment.
        /// This is normaly a pre duration to the start date, put also can used as post duration too.
        /// </summary>
        /// <example> 
        /// 5 mins before the start of the appointment.
        /// </example>
        public TimeSpan Duration { get; set; }

        #endregion



        #region Constructor(s)

        /// <summary>
        /// Creates a reminder with default values
        /// </summary>
        public Reminder()
        {
            this.Type = ReminderType.PopUp;
            this.Duration = TimeSpan.Zero;
        }

        #endregion
    }




    //public class Resource
    //{
    //    public object Id { get; set; }

    //    public string Name { get; set; }
    //}

    //public class AppointmentLabel
    //{
    //    public object Id { get; set; }

    //    public string DisplayName { get; set; }

    //    public Color Color { get; set; }
    //}

    //public class AppointmentStatus
    //{
    //    public object Id { get; set; }

    //    public string DisplayName { get; set; }

    //    public Color Color { get; set; }
    //}
}

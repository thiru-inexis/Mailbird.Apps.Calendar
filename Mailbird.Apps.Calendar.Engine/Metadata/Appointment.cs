using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Appointment structure
    /// </summary>
    public class Appointment : LocalStorageData
    {
        //public object Id { get; set; }

        public string Subject { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public bool AllDayEvent { get; set; }

        public DateTime EndTime { get; set; }

        public object ResourceId { get; set; }

        public int LabelId { get; set; }

        public object StatusId { get; set; }

        public object ReminderInfo { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public string CalendarId { get; set; }

        public List<Reminder> Reminders { get; set; }


        public Appointment()
            : base()
        {
            this.StartTime = DateTime.Now;
            this.EndTime = DateTime.Now;
            this.Reminders = new List<Reminder>();
        }

    }


    public class Reminder
    {
        public ReminderType Type { get; set; }
        public TimeSpan Duration { get; set; }

        public Reminder()
        {
            this.Type = ReminderType.PopUp;
            this.Duration = TimeSpan.Zero;
        }
    }




    public class Resource
    {
        public object Id { get; set; }

        public string Name { get; set; }
    }

    public class AppointmentLabel
    {
        public object Id { get; set; }

        public string DisplayName { get; set; }

        public Color Color { get; set; }
    }

    public class AppointmentStatus
    {
        public object Id { get; set; }

        public string DisplayName { get; set; }

        public Color Color { get; set; }
    }
}

using System;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Appointment structure
    /// </summary>
    public class Appointment
    {

        public object Id { get; set; }

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

        public Calendar Calendar { get; set; }
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

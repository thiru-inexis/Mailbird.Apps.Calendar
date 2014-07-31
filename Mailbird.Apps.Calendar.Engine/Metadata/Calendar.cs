using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    public class Calendar
    {
        public Access AccessRights { get; set; }

        public string CalendarId { get; set; }

        public Color CalendarColor { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Provider { get; set; }

        public enum Access
        {
            Write,

            Read
        }
    }
}
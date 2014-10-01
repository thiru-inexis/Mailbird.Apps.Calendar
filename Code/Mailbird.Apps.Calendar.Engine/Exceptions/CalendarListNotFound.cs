using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Exceptions
{
    /// <summary>
    /// This exception will be throw when an deleted/non-existing calendar's calendar list being accessed.
    /// This is specific to google calendar service.
    /// </summary>
    public class CalendarListNotFound : MailbirdCalendarException
    {

        public CalendarListNotFound()
            : base()
        { }


        public CalendarListNotFound(string message)
            : base(message)
        { }


        public CalendarListNotFound(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

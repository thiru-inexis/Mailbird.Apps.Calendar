using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Exceptions
{
    /// <summary>
    /// This exception will be throw when an deleted/non-existing calendar being accessed.
    /// </summary>
    public class CalendarNotFound : MailbirdCalendarException
    {

        public CalendarNotFound()
            : base()
        { }


        public CalendarNotFound(string message)
            : base(message)
        { }


        public CalendarNotFound(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

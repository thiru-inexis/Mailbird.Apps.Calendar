using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Exceptions
{
    /// <summary>
    /// Exceptions specific to mailbird should, inherit this class
    /// </summary>
    public class MailbirdCalendarException : Exception
    {
        public MailbirdCalendarException()
            : base()
        { }

        public MailbirdCalendarException(string message)
            : base(message)
        { }


        public MailbirdCalendarException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

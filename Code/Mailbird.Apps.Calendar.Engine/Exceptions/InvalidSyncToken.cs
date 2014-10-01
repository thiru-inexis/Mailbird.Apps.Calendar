using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Exceptions
{
    /// <summary>
    /// This exception will be throw when a invalid/expired token used for fetching 
    /// calendars or appointments via sync mechanism.
    /// As per Google Api
    /// </summary>
    public class InvalidSyncToken : MailbirdCalendarException
    {

        public InvalidSyncToken()
            : base()
        { }


        public InvalidSyncToken(string message)
            : base(message)
        { }


        public InvalidSyncToken(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Exceptions
{
    /// <summary>
    /// This exception will be throw when an primary calendar trying to be deleted.
    /// As per Google a primary calendar can not be created/deleted as its bound to 
    /// an user account
    /// </summary>
    public class CalendarDeletionFailed : MailbirdCalendarException
    {

        public CalendarDeletionFailed()
            : base()
        { }

        public CalendarDeletionFailed(string message)
            : base(message)
        { }


        public CalendarDeletionFailed(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Exceptions
{
    /// <summary>
    /// This exception will be throw when an deleted/non-existing calendar being accessed.
    /// </summary>
    public class AppointmentNotFound : MailbirdCalendarException
    {

        public AppointmentNotFound()
            : base()
        { }


        public AppointmentNotFound(string message)
            : base(message)
        { }


        public AppointmentNotFound(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}

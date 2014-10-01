using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Enums
{
    /// <summary>
    /// The status of an appointment
    /// Maps with iCal format
    /// </summary>
    public enum AppointmentStatus
    {
        Tentative = 0,
        Confirmed = 1,
        Cancelled = 2
    }
}

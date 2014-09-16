using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Enums
{
    /// <summary>
    /// Goolge uses different token for different calender resources.
    /// </summary>
    public enum SyncTokenType
    {
        Calendar = 1,
        CalenderAppointments = 2,
        ColorDefinition = 3
    }
}

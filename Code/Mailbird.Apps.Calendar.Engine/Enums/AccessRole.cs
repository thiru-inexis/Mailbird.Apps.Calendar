using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Enums
{
    /// <summary>
    /// The role the authorized user can has on the calendar.
    /// Maps directly to the Google calendar's property.
    /// </summary>
    public enum AccessRole
    {
        FreeBusyReader = 0,
        Reader = 1,
        Writer = 2,
        Owner = 3
    }

}



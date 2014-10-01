using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Enums
{
    public enum ReminderType
    {
        [Description("Minutes")]
        Minutes,
        [Description("Hours")]
        Hours,
        [Description("Days")]
        Days,
        [Description("Weeks")]
        Weeks

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Enums
{
    /// <summary>
    /// This represents the calendar views that can be toggled within.
    /// </summary>
    public enum CalenderView
    {
        [Description("Day")]
        Day = 0,
        [Description("Work Week (5 Days)")]
        WorkWeek = 1,
        [Description("Week (7 Days)")]
        Week = 2,
        [Description("Month")]
        Month = 3,
        [Description("Agenda")]
        Agenda = 4
    }
}

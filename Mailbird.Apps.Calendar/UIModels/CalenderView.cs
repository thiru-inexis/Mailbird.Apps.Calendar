using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.UIModels
{
    public enum CalenderView
    {
        [Description("Day")]
        Day = 1,
        [Description("Work Week (5 Days)")]
        WorkWeek = 2,
        [Description("Week (7 Days)")]
        Week = 3,
        [Description("Month")]
        Month = 4,
        [Description("Agenda")]
        Agenda = 5
    }
}

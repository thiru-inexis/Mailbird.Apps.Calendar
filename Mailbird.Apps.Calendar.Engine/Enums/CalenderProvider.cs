using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Enums
{
    public enum CalenderProvider
    {
        [Description("LocalCalenderProvider")]
        Local = 0,
        [Description("GoogleCalenderProvider")]
        Google = 1
    }
}

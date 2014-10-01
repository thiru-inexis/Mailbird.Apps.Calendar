using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Enums
{
    /// <summary>
    /// Represents the appointment reminder type
    /// Maps with iCal format
    /// </summary>
    public enum ReminderType
    {
        PopUp = 0,
        Audio = 1,
        Email = 2,
        Sms = 3
    }


}



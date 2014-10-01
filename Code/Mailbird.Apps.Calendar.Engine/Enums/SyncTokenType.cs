using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Enums
{
    /// <summary>
    /// This is for google calendar sysnc purpose.
    /// Differnt tokens are used for different entities and this provides the 
    /// supported token types.
    /// </summary>
    public enum SyncTokenType
    {
        /// <summary>
        /// For calendars and calenderlist
        /// </summary>
        Calendar = 0,

        /// <summary>
        /// For Appointments specific to a calendar
        /// </summary>
        CalenderAppointments = 1,

        /// <summary>
        /// For color pallets
        /// </summary>
        ColorDefinition = 2
    }
}

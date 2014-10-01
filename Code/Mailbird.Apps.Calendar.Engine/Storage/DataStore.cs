using System;
using System.Collections.Generic;
using Mailbird.Apps.Calendar.Engine.Metadata;

namespace Mailbird.Apps.Calendar.Engine.Storage
{

    /// <summary>
    /// This represesnts the data stucture to write-to and read-from
    /// the json file
    /// </summary>
    public class DataStore
    {
        // Each properties represent a table as in the database
        public List<UserInfo> UserInfos { get; set; }
        public List<Metadata.Calendar> Calendars { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<ColorDefinition> CalendarColors { get; set; }
        public List<ColorDefinition> AppointmentColors { get; set; }
        public List<SyncToken> SynTokens { get; set; }



        public DataStore()
        {
            this.UserInfos = new List<UserInfo>();
            this.Calendars = new List<Metadata.Calendar>();
            this.Appointments = new List<Appointment>();
            this.SynTokens = new List<SyncToken>();
            this.CalendarColors = new List<ColorDefinition>();
            this.AppointmentColors = new List<ColorDefinition>();
        }
    }
}

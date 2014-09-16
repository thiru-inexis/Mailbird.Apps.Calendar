using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    public class DataStore
    {
        public List<Calendar> Calendars { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<SyncToken> SynTokens { get; set; }
        public List<ColorDefinition> CalendarColors { get; set; }
        public List<ColorDefinition> AppointmentColors { get; set; }

        public DataStore()
        {
            this.Calendars = new List<Calendar>();
            this.Appointments = new List<Appointment>();
            this.SynTokens = new List<SyncToken>();
            this.CalendarColors = new List<ColorDefinition>();
            this.AppointmentColors = new List<ColorDefinition>();
        }
    }
}

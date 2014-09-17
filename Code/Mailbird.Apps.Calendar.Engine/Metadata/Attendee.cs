using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    public class Attendee: LocalStorageData
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsOrganizer { get; set; }
        public bool IsSelf { get; set; }
        public bool IsResource { get; set; }
        public bool IsOptional { get; set; }
        public string ResponseStatus { get; set; }
        public string Comment { get; set; }
        public int AdditionalGuests { get; set; }

    }
}

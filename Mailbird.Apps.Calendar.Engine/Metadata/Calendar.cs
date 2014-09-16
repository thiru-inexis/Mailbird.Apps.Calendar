using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    public class Calendar: LocalStorageData
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string TimeZone { get; set; }
        public CalendarList CalenderList { get; set; }


        public Calendar()
        {
            this.CalenderList = null;
        }
    }


}

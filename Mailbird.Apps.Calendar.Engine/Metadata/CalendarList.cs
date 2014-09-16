using Mailbird.Apps.Calendar.Engine.Enums;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    public class CalendarList : LocalStorageData
    {
        public string Kind { get; set; }
        public string Etag { get; set; }

        public string Summary { get; set; } // readonly
        public string SummaryOverride { get; set; } // readonly
        public string Description { get; set; } // readonly
        public string Location { get; set; } // readonly
        public string TimeZone { get; set; } // readonly

        public string ColorId { get; set; } // 0 -24
        public ColorDefinition ColorDef { get; set; } // 0 -24

        public string BackgroundColor { get; set; } //#ffffff
        public string ForegroundColor { get; set; } //#ffffff
       
      
        public Access AccessRole { get; set; }
        public bool IsHidden { get; set; }
        public bool IsSelected { get; set; } 
        public bool IsPrimary { get; set; } // readonly
        public bool IsDeleted { get; set; }


        //public Access AccessRights { get; set; }
        //public Color CalendarColor { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string Provider { get; set; }
        //public bool IsDeleted { get; set; }
        //public bool IsSelected { get; set; }
        //public bool IsPrimary { get; set; }
        //public string BackgroundColor { get; set; }
        //public string ForegroundColor { get; set; }
        //public string ColorId { get; set; }


        public CalendarList()
            :base()
        { }
    }
}
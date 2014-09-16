using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    public class ColorDefinition: LocalStorageData
    {
        public string Background { get; set; }
        public string Foreground { get; set; }
    }
}

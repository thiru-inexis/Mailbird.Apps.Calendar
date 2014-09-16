using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// Google calender uses tokens for incremental sync.
    /// </summary>
    public class SyncToken : LocalStorageData
    {
        public string CalenderId { get; set; }
        public string Token { get; set; }

        public SyncTokenType TokenContext { get; set; }

    }

}

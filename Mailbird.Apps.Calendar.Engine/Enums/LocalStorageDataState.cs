using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Enums
{
    /// <summary>
    /// This represent the state of the data records in local storage.
    /// </summary>
    public enum LocalStorageDataState
    {
        Unchanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }
}

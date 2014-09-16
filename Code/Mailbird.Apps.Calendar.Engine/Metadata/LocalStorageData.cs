using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{

    /// <summary>
    /// Any data item that is to be stored in the local storage has to inherit this class.
    /// Hence this holds the value to track the changes and sync with calender providers.
    /// </summary>
    public abstract class LocalStorageData
    {

        public string Id { get; set; }

        public LocalStorageDataState LocalStorageState { get; set; }


        public LocalStorageData()
        {
            this.Id = null;
            this.LocalStorageState = LocalStorageDataState.Unchanged;
        }

    }
}

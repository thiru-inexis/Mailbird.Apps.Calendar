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
        #region Properties

        /// <summary>
        /// Id specific to and Entity.
        /// [Required]
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The state of the entity in the storage.
        /// [Required]
        /// </summary>
        public LocalStorageDataState LocalStorageState { get; set; }

        #endregion


        #region

        /// <summary>
        /// Creates an instance with default values
        /// </summary>
        public LocalStorageData()
        {
            this.Id = null;
            this.LocalStorageState = LocalStorageDataState.Unchanged;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Metadata
{
    /// <summary>
    /// This class is used to store user informations.
    /// The tokens used for each cleint will be stored by the id of the user in the storage.
    /// </summary>
    public class UserInfo: LocalStorageData
    {
        /// <summary>
        /// The user's original login username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// To save user settings
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Background and Foregound of resources 
        /// </summary>
        public ColorDefinition Color { get; set; } 
       
        /// <summary>
        /// When the user was first created.
        /// Just to track history
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// When the user info was last modified.
        /// Just to track history
        /// </summary>
        public DateTime UpdatedOn {get; set;}
    }
}

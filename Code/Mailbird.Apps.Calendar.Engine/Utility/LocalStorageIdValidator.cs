using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Utility
{
    /// <summary>
    /// This class should/will hold static utility functions that are
    /// specific the objects/data in the local storage 
    /// </summary>
    public class LocalStorageIdValidator
    {

        /// <summary>
        /// Checks is the specific string matches to the format of the local storage.
        /// This helps int he sync process. How?
        /// When objects are created in the local, they will be assign a GUID that is unique to the local
        /// Once a sync has been perform, this Id will be overrited with that of the providers.
        /// This also helps in the updation, when a non synced newly created local data is updated,
        /// its state is set as UPDATED/MODIFIED. But this state sould be ADDED as this state is always 
        /// relative to the provider. And htis check is always made to set the state on updation conflicts
        /// </summary>
        /// <param name="Id">The string to determine</param>
        /// <returns>True is its a local GUID, Else false</returns>
        /// <exception cref="ArgumentNullException">
        /// An Id can not be NULL/empty, and perfomring a check on an invalid id will be meaningless 
        /// </exception>
        public static bool IsALocalId(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id)) { throw new ArgumentNullException("Id can not be Null/Empty/WhiteSpaced", "Id"); }

            try
            {
                // May throw Augument | Format exceptions
                // Handle only the format exception
                Guid.Parse(Id);
                return true;
            }
            catch (System.FormatException) { return false; }
        }

    }
}

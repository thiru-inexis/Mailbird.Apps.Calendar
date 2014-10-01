using Mailbird.Apps.Calendar.Engine.CalenderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Interfaces
{
    /// <summary>
    /// All local and proveider based calendar service instances will be managed 
    /// within this class
    /// </summary>
    public interface ICalendarServicesFacade
    {
        /// <summary>
        /// Gets local calendar service to write and read for offline support.
        /// </summary>
        /// <returns></returns>
        LocalCalenderService GetLocalService();


        /// <summary>
        /// Gets the calendar provider instance based on the type of the user
        /// {Eg: Google, Outlook, Yahoo}
        /// </summary>
        /// <param name="userId">The unique Id to which the provider is bounded to.</param>
        /// <returns></returns>
        ISyncableCalendarProvider GetUserService(string userId);
    }

}

using Mailbird.Apps.Calendar.Engine.CalenderServices;
using Mailbird.Apps.Calendar.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine
{
    
    /// <summary>
    /// This class will hold all the user bound calendar providers and local storage instance also
    /// This implements a singleton pattern and hence provide access to the 
    /// user calenar services to any services {Eg: Synchronizer, UI}.
    /// </summary>
    public class CalendarServicesFacade: ICalendarServicesFacade
    {
        private static object _syncLock = new { };
        private static CalendarServicesFacade _instance;

        private LocalCalenderService _localService;
        private IDictionary<string, ISyncableCalendarProvider> _providerPool;


        private CalendarServicesFacade()
        {
            _localService = new LocalCalenderService();
            _providerPool = new Dictionary<string, ISyncableCalendarProvider>();
        }


        public static CalendarServicesFacade GetInstance()
        {
            lock (_syncLock)
            {
                if (_instance == null) { _instance = new CalendarServicesFacade(); }
            }

            return _instance;
        }



        public LocalCalenderService GetLocalService()
        {
            return _localService;
        }


        public ISyncableCalendarProvider GetUserService(string userId)
        {
            lock (_syncLock)
            {
                var userInfo = _localService.UserInfoRepo.Get(userId);
                if (userInfo == null) { return null; }

                if (!_providerPool.ContainsKey(userId))
                {
                    _providerPool.Add(userId, new GoogleCalendarService(userInfo));
                }

                return _providerPool[userId];
            }
        }

    }
}

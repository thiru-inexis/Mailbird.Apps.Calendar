using Mailbird.Apps.Calendar.Engine.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Utility;
using Mailbird.Apps.Calendar.Engine.Enums;
using Mailbird.Apps.Calendar.Engine.Storage;
using Mailbird.Apps.Calendar.Engine.Storage.Repository;

namespace Mailbird.Apps.Calendar.Engine.CalenderServices
{
    public class LocalCalenderService 
    {
        private const string LOCAL_STORAGE_PATH = @".\LocalStorage2.txt";

        private DataStore _localStorage;
        private readonly JsonWorker<DataStore> _worker;

        public BaseRepository<Metadata.UserInfo> UserInfoRepo { get; set; }
        public BaseRepository<Metadata.Calendar> CalendarRepo { get; set; }
        public BaseRepository<Metadata.Appointment> AppointmentRepo { get; set; }
        public BaseRepository<Metadata.ColorDefinition> CalendarColorRepo { get; set; }
        public BaseRepository<Metadata.ColorDefinition> AppointmentColorRepo { get; set; }
        public BaseRepository<Metadata.SyncToken> SyncRepo { get; set; }


        public event EventHandler<EventArugs.LocalStorageChangedArgs> StorageChanged;

        public CalenderProvider Name
        {
            get { return CalenderProvider.Local; }
        }


        public LocalCalenderService()
        {
            _worker = new JsonWorker<DataStore>(LOCAL_STORAGE_PATH);
            LoadFromStore();
        }


        private void LoadFromStore()
        {
            _localStorage = _worker.Read();
            UserInfoRepo = new BaseRepository<Metadata.UserInfo>(_localStorage.UserInfos);
            CalendarRepo = new BaseRepository<Metadata.Calendar>(_localStorage.Calendars);
            AppointmentRepo = new BaseRepository<Metadata.Appointment>(_localStorage.Appointments);
            SyncRepo = new BaseRepository<Metadata.SyncToken>(_localStorage.SynTokens);
            CalendarColorRepo = new BaseRepository<ColorDefinition>(_localStorage.CalendarColors);
            AppointmentColorRepo = new BaseRepository<ColorDefinition>(_localStorage.AppointmentColors);

            Seed();
        }



        public void SaveChanges()
        {
            _worker.Write(_localStorage);
            LoadFromStore();

            if (this.StorageChanged != null)
            { 
                this.StorageChanged(this, new EventArugs.LocalStorageChangedArgs("Changed"));
            }
        }


        /// <summary>
        /// Reloads the local memmory will that of the storage
        /// </summary>
        public void RollBack()
        {
            LoadFromStore();
            if (this.StorageChanged != null)
            {
                this.StorageChanged(this, new EventArugs.LocalStorageChangedArgs("Changed"));
            }
        }


        /// <summary>
        /// This is just a helper function to load the initial storage
        /// with sum default data to start.
        /// 
        /// This is only for test purpose. To start up with..
        /// </summary>
        private void Seed()
        {
            if (!UserInfoRepo.Get().Any())
            {
                var user1 = new UserInfo()
                {
                    Username = "Google Test Account",
                    Color = new ColorDefinition(),
                    IsSelected = true,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                var user2 = new UserInfo()
                {
                    Username = "Secondary Google Account",
                    Color = new ColorDefinition(),
                    IsSelected = true,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };



                UserInfoRepo.Add(user1);
                UserInfoRepo.Add(user2);
                SaveChanges();
            }

        }


    }
}

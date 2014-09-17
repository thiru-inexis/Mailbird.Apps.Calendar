using Mailbird.Apps.Calendar.Engine.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Repository;
using Mailbird.Apps.Calendar.Engine.Utility;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine.CalenderServices
{
    public class LocalCalenderService 
    {
        private const string LOCAL_STORAGE_PATH = @".\LocalStorage2.txt";

        private DataStore _localStorage;
        private readonly JsonWorker<DataStore> _worker;

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
            CalendarRepo = new BaseRepository<Metadata.Calendar>(_localStorage.Calendars);
            AppointmentRepo = new BaseRepository<Metadata.Appointment>(_localStorage.Appointments);
            SyncRepo = new BaseRepository<Metadata.SyncToken>(_localStorage.SynTokens);
            CalendarColorRepo = new BaseRepository<ColorDefinition>(_localStorage.CalendarColors);
            AppointmentColorRepo = new BaseRepository<ColorDefinition>(_localStorage.AppointmentColors);
            //CalenderRepo.ItemCollectionChanged += CalenderRepo_ItemCollectionChanged;
            //AppointmentRepo.ItemCollectionChanged += AppointmentRepo_ItemCollectionChanged;
            //SyncRepo.ItemCollectionChanged += SyncRepo_ItemCollectionChanged;
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


        private void SyncRepo_ItemCollectionChanged(object sender, EventArugs.RepositoryChangedArgs<SyncToken> e)
        {
           // _worker.Write(_localStorage);
        }

        void AppointmentRepo_ItemCollectionChanged(object sender, EventArugs.RepositoryChangedArgs<Appointment> e)
        {
          //  _worker.Write(_localStorage);
        }


        void CalenderRepo_ItemCollectionChanged(object sender, EventArugs.RepositoryChangedArgs<Metadata.CalendarList> e)
        {
         //   _worker.Write(_localStorage);
        }




    }
}

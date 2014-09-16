using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine.Repository
{
    public class CalenderRepository : IRepository<Metadata.Calendar>
    {

        public List<Metadata.Calendar> Collection { get; set; }


        public CalenderRepository(List<Metadata.Calendar> collection)
        {
            this.Collection = collection;
        }



        public List<Metadata.Calendar> Get()
        {
            return this.Collection.ToList();
        }

        public Metadata.Calendar Get(string id)
        {
            return this.Collection.FirstOrDefault(m => m.CalendarId == id);
        }


        public Metadata.Calendar Add(Metadata.Calendar data, Enums.LocalStorageDataState state = LocalStorageDataState.Added)
        {
            if (string.IsNullOrEmpty(data.CalendarId)) { data.CalendarId = Guid.NewGuid().ToString(); }
            if (Get(data.CalendarId) != null) { throw new ArgumentException(); }

            data.LocalStorageState = state;
            this.Collection.Add(data);
            return data;
        }

        public Metadata.Calendar Update(Metadata.Calendar data, Enums.LocalStorageDataState state = LocalStorageDataState.Modified)
        {
            var dbmodel = Get(data.CalendarId);
            if (dbmodel == null) { throw new ArgumentException(); }

            data.LocalStorageState = state;
            this.Collection.Remove(dbmodel);
            this.Collection.Add(data);
            return data;
        }

        public bool Delete(Metadata.Calendar data, Enums.LocalStorageDataState state = LocalStorageDataState.Deleted)
        {
            var dbmodel = Get(data.CalendarId);
            if (dbmodel == null) { throw new ArgumentException(); }

            data.LocalStorageState = state;
            this.Collection.Remove(dbmodel);
            if (state == LocalStorageDataState.Deleted)
            {
                this.Collection.Add(data);
            }
            SaveChanges();
            return true;
        }

        //public event EventHandler<EventArugs.AddedToStoreEventArgs<Metadata.Calendar>> AddingItemToStoreCompleted;

        //public event EventHandler<EventArugs.UpadateToStoreEventArgs<Metadata.Calendar>> UpdatingItemToStoreCompleted;

        //public event EventHandler<EventArugs.DeletedFromStoreEventArgs<Metadata.Calendar>> DeletingItemFromStoreCompleted;

        //public event EventHandler<EventArugs.RepositoryChangedArgs<Metadata.Calendar>> ItemCollectionChanged;




        //public Action SaveChanges
        //{
        //    get;
        //    set;
        //}


    }
}

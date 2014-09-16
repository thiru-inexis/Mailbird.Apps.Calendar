using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine.Repository
{
    public class SyncTokenRepository : IRepository<SyncToken>
    {
        public List<SyncToken> Collection { get; set; }


        public SyncTokenRepository(List<Metadata.SyncToken> collection)
        {
            this.Collection = collection;
        }




        public List<SyncToken> Get()
        {
            return this.Collection.ToList();
        }


        public SyncToken Get(string id)
        {
            return this.Collection.FirstOrDefault(m => m.Id == id); 
        }


        public SyncToken Add(SyncToken data, Enums.LocalStorageDataState state = LocalStorageDataState.Added)
        {
            if (string.IsNullOrEmpty(data.Id)) { data.Id = Guid.NewGuid().ToString(); }
            this.Collection.Add(data);
            SaveChanges();
            return data;
        }

        public SyncToken Update(SyncToken data, Enums.LocalStorageDataState state = LocalStorageDataState.Modified)
        {
            var objToRemove = Get(data.Id);
            this.Collection.Remove(objToRemove);
            this.Collection.Add(data);

            return data;
        }

        public bool Delete(SyncToken data, Enums.LocalStorageDataState state = LocalStorageDataState.Deleted)
        {
            throw new NotImplementedException();
        }

        //public event EventHandler<EventArugs.AddedToStoreEventArgs<SyncToken>> AddingItemToStoreCompleted;

        //public event EventHandler<EventArugs.UpadateToStoreEventArgs<SyncToken>> UpdatingItemToStoreCompleted;

        //public event EventHandler<EventArugs.DeletedFromStoreEventArgs<SyncToken>> DeletingItemFromStoreCompleted;

        //public event EventHandler<EventArugs.RepositoryChangedArgs<SyncToken>> ItemCollectionChanged;


        //public Action SaveChanges
        //{
        //    get;
        //    set;
        //}
    }
}

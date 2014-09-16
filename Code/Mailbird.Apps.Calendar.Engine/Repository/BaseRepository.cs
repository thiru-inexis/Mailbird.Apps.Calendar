using Mailbird.Apps.Calendar.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : Metadata.LocalStorageData
    {
        protected List<T> collection;

        public BaseRepository(List<T> collection)
        {
            this.collection = collection;
        }

        public List<T> Get()
        {
            return collection.ToList();
        }

        public T Get(string id)
        {
            return collection.FirstOrDefault(m => (m.Id.ToString() == id));
        }

        public T Add(T data, LocalStorageDataState state = LocalStorageDataState.Added)
        {
            if (string.IsNullOrEmpty(data.Id)) { data.Id = Guid.NewGuid().ToString(); }
            if (Get(data.Id) != null) { throw new ArgumentException(); }

            data.LocalStorageState = state;
            collection.Add(data);
            return data;
        }

        public T Update(T data, LocalStorageDataState state = LocalStorageDataState.Modified)
        {
            var dbmodel = Get(data.Id);
            if (dbmodel == null) { throw new ArgumentException(); }

            data.LocalStorageState = state;
            collection.Remove(dbmodel);
            collection.Add(data);
            return data;
        }

        public bool Delete(T data, LocalStorageDataState state = LocalStorageDataState.Deleted)
        {
            var dbmodel = Get(data.Id);
            if (dbmodel == null) { throw new ArgumentException(); }

            data.LocalStorageState = state;
            collection.Remove(dbmodel);
            // If the object is not marked as delete. remove permanently
            if (data.LocalStorageState != LocalStorageDataState.Deleted)
            {
                collection.Add(data);
            }
            return true;
        }


        public bool Clear()
        {
            collection.Clear();
            return true;
        }
    }
}

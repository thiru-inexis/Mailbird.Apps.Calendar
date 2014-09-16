using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine.Repository
{
    public class AppointmentRepository : IRepository<Metadata.Appointment>
    {
        public List<Metadata.Appointment> Collection { get; set; }


        public AppointmentRepository(List<Metadata.Appointment> collection)
        {
            this.Collection = collection;
        }




        public List<Appointment> Get()
        {
            return this.Collection.ToList();
        }

        public Appointment Get(string id)
        {
            return this.Collection.FirstOrDefault(m => m.Id.ToString() == id);
        }

        public Appointment Add(Appointment data, Enums.LocalStorageDataState state = LocalStorageDataState.Added)
        {
            if (string.IsNullOrEmpty(data.Id.ToString())) { data.Id = Guid.NewGuid().ToString(); }

            data.LocalStorageState = state;
            this.Collection.Add(data);
            return data;
        }

        public Appointment Update(Appointment data, Enums.LocalStorageDataState state = LocalStorageDataState.Modified)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Appointment data, Enums.LocalStorageDataState state = LocalStorageDataState.Deleted)
        {
            throw new NotImplementedException();
        }

        //public event EventHandler<EventArugs.AddedToStoreEventArgs<Appointment>> AddingItemToStoreCompleted;

        //public event EventHandler<EventArugs.UpadateToStoreEventArgs<Appointment>> UpdatingItemToStoreCompleted;

        //public event EventHandler<EventArugs.DeletedFromStoreEventArgs<Appointment>> DeletingItemFromStoreCompleted;

        //public event EventHandler<EventArugs.RepositoryChangedArgs<Appointment>> ItemCollectionChanged;


        //public Action SaveChanges
        //{
        //    get;
        //    set;
        //}

    }
}

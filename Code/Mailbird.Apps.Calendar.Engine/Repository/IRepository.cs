using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.EventArugs;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine.Repository
{
    public interface IRepository <T> where T : class
    {
        //List<T> Collection { get; set; }

        List<T> Get(bool ignoreDeleted = true);
        T Get(string id, bool ignoreDeleted = true);
        T Add(T data, LocalStorageDataState state = LocalStorageDataState.Added);
        T Update(T data, LocalStorageDataState state = LocalStorageDataState.Modified);
        bool Delete(T data, LocalStorageDataState state = LocalStorageDataState.Deleted);
        bool Clear();


        //Action SaveChanges {get; set;}

        //event EventHandler<AddedToStoreEventArgs<T>> AddingItemToStoreCompleted;
        //event EventHandler<UpadateToStoreEventArgs<T>> UpdatingItemToStoreCompleted;
        //event EventHandler<DeletedFromStoreEventArgs<T>> DeletingItemFromStoreCompleted;
        //event EventHandler<RepositoryChangedArgs<T>> ItemCollectionChanged;
    }
}

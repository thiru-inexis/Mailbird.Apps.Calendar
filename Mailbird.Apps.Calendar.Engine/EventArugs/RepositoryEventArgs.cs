using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.EventArugs
{
    public class RepositoryChangedArgs<T> : EventArgs where T : class
    {
        public Type RepoType { get; private set; }

        public RepositoryChangedArgs()
        {
            this.RepoType = typeof(T);
        }
    }


    public class AddedToStoreEventArgs<T> : EventArgs where T : class
    {
        public T AddedItem { get; private set; }

        public AddedToStoreEventArgs(T addedItem)
        {
            this.AddedItem = addedItem;
        }
    }


    public class UpadateToStoreEventArgs<T> : EventArgs where T : class
    {
        public T UpdatedItem { get; private set; }

        public UpadateToStoreEventArgs(T updatedItem)
        {
            this.UpdatedItem = updatedItem;
        }
    }



    public class DeletedFromStoreEventArgs<T> : EventArgs where T : class
    {
        public Type DeletedType  { get; private set; }

        public DeletedFromStoreEventArgs()
        {
            this.DeletedType = typeof(T);
        }
    }


}

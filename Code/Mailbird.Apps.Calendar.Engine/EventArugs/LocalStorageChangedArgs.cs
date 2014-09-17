using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.EventArugs
{
    public class LocalStorageChangedArgs: EventArgs 
    {
        public string Message { get; private set; }


        public LocalStorageChangedArgs(string message)
            : base()
        {
            this.Message = message;
        }

        public LocalStorageChangedArgs()
            :this("")
        { }


    }

}

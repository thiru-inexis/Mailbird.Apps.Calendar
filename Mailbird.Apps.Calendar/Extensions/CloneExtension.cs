using Mailbird.Apps.Calendar.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Extensions
{
    /// <summary>
    /// This helper extension makes uses of reflection to Clone values form and object to another
    /// </summary>
    /// <remarks>
    /// This could be performed by using AutoMapper as an alternate. 
    /// But why this ? To Ignore dependencies.
    /// </remarks>
  
    public static class CloneExtension
    {
       
        public static UIModels.CalenderUI Clone(this Engine.Metadata.Calendar source)
        {
            UIModels.CalenderUI result = null;

            if (source != null)
            {
                result = new UIModels.CalenderUI(source);
            }

            return result;
        }


        public static Engine.Metadata.Calendar Clone(this UIModels.CalenderUI source)
        {
            Engine.Metadata.Calendar result = null;

            if (source != null)
            {
                result = source.BaseModel;
            }

            return result;
        }



        public static UIModels.AppointmentUI Clone(this Engine.Metadata.Appointment source)
        {
            UIModels.AppointmentUI result = null;

            if (source != null)
            {
                result = new UIModels.AppointmentUI(source);
            }

            return result;
        }


        public static Engine.Metadata.Appointment Clone(this UIModels.AppointmentUI source)
        {
            Engine.Metadata.Appointment result = null;

            if (source != null)
            {
                result = source.BaseModel;
            }

            return result;
        }

    }
}

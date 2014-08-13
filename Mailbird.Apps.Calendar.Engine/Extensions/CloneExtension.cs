using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Mailbird.Apps.Calendar.Engine.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.Engine.Extensions
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
        /// <summary>
        /// Clones google calender event to mailbirds's appointment type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="calender"></param>
        /// <returns></returns>
        public static Appointment Clone(this Event source, Metadata.Calendar calender)
        {
            Appointment result = null;

            if (source != null)
            {
                result = new Appointment()
                {
                    Id = source.Id,
                    StartTime = (source.Start != null && source.Start.DateTime.HasValue) ? source.Start.DateTime.Value : DateTime.Now,
                    EndTime = (source.End != null && source.End.DateTime.HasValue) ? source.End.DateTime.Value : DateTime.Now,
                    Subject = source.Summary,
                    Description = source.Description,
                    Location = source.Location,
                    Calendar = calender

                };
            }

            return result;
        }


        /// <summary>
        /// Clones mailbirds's appointment type to google calender event 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Event Clone(this Appointment source)
        {
            Event result = null;

            if (source != null)
            {
                result = new Event()
                {
                    Start = new EventDateTime { DateTime = source.StartTime},
                    End = new EventDateTime { DateTime = source.EndTime },
                    Summary = source.Subject,
                    Description = source.Description,
                    Location = source.Location
                };
            }
            return result;
        }


    }
}

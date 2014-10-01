using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDay.iCal;
using Mailbird.Apps.Calendar.Engine.Interfaces;
using DDay.iCal.Serialization.iCalendar;
using DDay.iCal.Serialization;
using System.IO;
using Mailbird.Apps.Calendar.Engine.Enums;

namespace Mailbird.Apps.Calendar.Engine
{
    public class ICalenderAppoinmentParser: IAppointmentParser<Metadata.Appointment>
    {
        public IList<Metadata.Appointment> Deserialize(Stream inStream)
        {
            var result = new List<Metadata.Appointment>();

            try
            {
                IICalendarCollection iCalenders = iCalendar.LoadFromStream(inStream);

                Metadata.Appointment tempApt;
                foreach (var iEvent in iCalenders.SelectMany(m => m.Events))
                {
                    tempApt = ConvertTo(iEvent);
                    if (tempApt != null) { result.Add(tempApt); }
                }
            }
            catch (Exception exp) { }

            return result;
        }

        public void Serialize(Stream outStream, IList<Metadata.Appointment> appointments)
        {
            try
            {
                if (appointments == null || appointments.Count <= 0) { throw new InvalidOperationException("Appointments can not be null/empty"); }
                var icalendar = new iCalendar();
                IEvent tempEvent = null;
                foreach (var apt in appointments)
                {
                    tempEvent = ConvertTo(apt);
                    if (tempEvent != null)
                    {
                        icalendar.Events.Add(tempEvent);
                    }
                }

                if (icalendar.Events.Count <= 0) { throw new InvalidOperationException("Calendar has no events"); }
                iCalendarSerializer iCalSerializer = new iCalendarSerializer();
                iCalSerializer.Serialize(icalendar, outStream, Encoding.UTF8);
            }
            catch (InvalidOperationException exp) { }
        }

        public IList<Metadata.Appointment> Deserialize(string icsContent)
        {
            IList<Metadata.Appointment> result = null;

            try
            {
                using(var inStream = new MemoryStream(Encoding.UTF8.GetBytes(icsContent)))
                {
                   result = Deserialize(inStream);
                }
            }
            catch (Exception exp) { }

            return result;
        }

        public string Serialize(IList<Metadata.Appointment> appointments)
        {
            string result = null;
            try
            {
                if (appointments == null || appointments.Count <= 0) { throw new InvalidOperationException("Appointments can not be null/empty"); }
                using (var outStream = new MemoryStream())
                {
                    Serialize(outStream, appointments);
                    using (var readerStream = new StreamReader(outStream))
                    {
                        outStream.Position = 0;
                        result = readerStream.ReadToEnd();
                    }
                }
            }
            catch (InvalidOperationException exp) { }

            return result;
        }




        private Metadata.Appointment ConvertTo(IEvent source)
        {
            Metadata.Appointment result = null;
            try
            {
                if (source == null) { throw new InvalidOperationException("Source is Null."); }
                result = new Metadata.Appointment()
                {
                    StartTime = source.Start.UTC,
                    EndTime = source.End.UTC,
                    Summary = source.Summary,
                    Transparency = (source.Transparency == TransparencyType.Opaque 
                    ? AppointmentTransparency.Opaque : AppointmentTransparency.Transparent),
                    Status = (source.Status == EventStatus.Cancelled ? AppointmentStatus.Cancelled 
                    :source.Status == EventStatus.Confirmed ? AppointmentStatus.Confirmed : AppointmentStatus.Tentative),
                    Location = source.Location,
                    Description = source.Description
                };
            }
            catch (InvalidOperationException exp) { }

            return result;
        }

        private IEvent ConvertTo(Metadata.Appointment source)
        {  
            IEvent result = null;
            try
            {
                if (source == null) { throw new InvalidOperationException("Source is Null."); }
                result = new Event()
                {
                    Summary = source.Summary,
                    Start = new iCalDateTime( source.StartTime),
                    End = new iCalDateTime(source.EndTime),
                    Location = source.Location,
                    Description = source.Description,
                    Status = (source.Status == AppointmentStatus.Cancelled ? EventStatus.Cancelled
                    :source.Status == AppointmentStatus.Confirmed ? EventStatus.Confirmed : EventStatus.Tentative),
                    Transparency = (source.Transparency == AppointmentTransparency.Opaque ? TransparencyType.Opaque : TransparencyType.Transparent),
                };
            }
            catch (InvalidOperationException exp) { }

            return result;

        }

    }
}

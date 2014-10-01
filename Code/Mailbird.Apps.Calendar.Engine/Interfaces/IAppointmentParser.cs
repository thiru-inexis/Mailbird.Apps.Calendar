using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Mailbird.Apps.Calendar.Engine.Interfaces
{
    /// <summary>
    /// Any Ics to calendar appointment type and vice parser should implement this 
    /// interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppointmentParser<T> where T : class
    {
        IList<T> Deserialize(Stream inStream);

        void Serialize(Stream outStream, IList<T> appointments);

        IList<T> Deserialize(string icsContent);

        string Serialize(IList<T> appointments);
    }
}

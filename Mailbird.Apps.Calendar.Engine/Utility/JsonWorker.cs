using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Mailbird.Apps.Calendar.Engine.Utility
{
    /// <summary>
    /// SImple class to serialize/deserialize objects to json format to store for future load local calendars events
    /// </summary>
    public class JsonWorker<T> where T : class
    {
        private readonly string _filePath;

        public JsonWorker(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(filePath)) 
            {  
                Write(Activator.CreateInstance<T>());
            }
        }


        public string Searialize(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }


        public T Desearialize(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }


        public void Write(T data)
        {
            using (FileStream fStream = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter wStream = new StreamWriter(fStream))
                {
                    wStream.Write(Searialize(data));
                }
            }

            //File.WriteAllText(_filePath, Searialize(data));
        }


        public T Read()
        {
            T result = null;

            using (FileStream fStream = new FileStream(_filePath, FileMode.OpenOrCreate))
            {
                using (StreamReader rStream = new StreamReader(fStream))
                {
                    result = Desearialize(rStream.ReadToEnd());
                }
            }

            return result;
        }


    }
}

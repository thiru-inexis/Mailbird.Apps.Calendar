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
    public class JsonWorker
    {
        private readonly string _filePath;

        public JsonWorker(string filePath)
        {
            _filePath = filePath;
        }
        
        public T DeserializeJson<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public void SaveData<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            using (var stream = new StreamWriter(_filePath, true))
            {
               stream.WriteLine(json);
            }
        }

        public List<T> GetData<T>()
        {
            if(!File.Exists(_filePath)) return new List<T>();
            var jsonObjects = new List<string>();
            using (var reader = new StreamReader(_filePath)) //path
            {
                var text = reader.ReadToEnd();
                var array = text.ToCharArray();
                var isStartPoint = false;
                var startIndex = 0;
                var opened = 0;
                var closed = 0;
                for (var i = 0; i < array.Length; i++)
                {
                    if (array[i] == '{')
                    {
                        if (isStartPoint == false)
                        {
                            isStartPoint = true;
                            startIndex = i;
                        }
                        opened++;
                    }
                    if (array[i] == '}')
                    {
                        closed++;
                        if (opened != closed) continue;
                        var obj = text.Substring(startIndex, i-startIndex + 1);
                        jsonObjects.Add(obj);
                        isStartPoint = false;
                        startIndex = 0;
                        opened = 0;
                        closed = 0;
                    }
                }
            }
            return jsonObjects.Select(DeserializeJson<T>).ToList();
        }
    }
}

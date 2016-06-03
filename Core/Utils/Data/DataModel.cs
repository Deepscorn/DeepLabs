// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

namespace Assets.Sources.Util.Data
{
    public static class DataModel
    {
        private const string SaveFileName = "4game.data";

        public static void SaveGameData(string key, object data)
        {
            var allData = LoadAllGameData() ?? new Dictionary<string, object>();
            allData[key] = data;
      
            var filePath = File.Create(Path.Combine(Application.persistentDataPath, SaveFileName));
      
            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(Json.Serialize(allData));
            }
        }
   
        public static T LoadGameData<T>(string key, T defaultValue)
        {
            var allData = LoadAllGameData();
            object r;
            if (allData != null && allData.TryGetValue(key, out r))
                return (T)r;
            return defaultValue;
        }

        public static T LoadGameData<T>(string key)
        {
            return LoadGameData<T>(key, default(T));
        }
   
        private static Dictionary<string, object> _bufferedData;

        private static Dictionary<string, object> LoadAllGameData()
        {
            if (_bufferedData != null)
                return _bufferedData;
      
            var filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
      
            if (File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath))
                {
                    _bufferedData = Json.Deserialize(sr.ReadToEnd()) as Dictionary<string, object>;
                }
            }
      
            return _bufferedData;
        }
    }
}


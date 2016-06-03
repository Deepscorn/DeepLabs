// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;

namespace Assets.Sources.Util.Data
{
    // Note: any field is serialized to file, even private. Private readonly not serialized
    internal class SettingImpl<T>
    {
        private readonly object Lock = new object();
        private T lastValue;
        private string lastFilePath;

        public void Set(T value, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException();
            }
            lock (Lock)
            {
                if (value == null)
                {
                    var absFilePath = GetSaveFilePath(filePath);
                    if (File.Exists(absFilePath))
                    {
                        File.Delete(absFilePath);
                    }
                }
                else if (!value.Equals(lastValue) || !filePath.Equals(lastFilePath))
                {
                    var absFilePath = GetSaveFilePath(filePath);
                    using (var file = File.Create(absFilePath))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(file, value);
                        file.Close();
                    }
                }
                lastValue = value;
                lastFilePath = filePath;
            }
        }

        public T Get(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException();
            }
            lock (Lock)
            {
                if (filePath.Equals(lastFilePath))
                {
                    return lastValue;
                }
                var absFilePath = GetSaveFilePath(filePath);
                lastValue = default(T);
                if (File.Exists(absFilePath))
                {
                    using (var file = File.Open(absFilePath, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        lastValue = (T) serializer.Deserialize(file);
                        file.Close();
                    }
                }
                lastFilePath = filePath;
                return lastValue;
            }
        }

        // Constructors and field initializers are called from the loading thread
        // from which Application.persistentDataPath, Native methods and many other things are
        // inaccessible, so can't just initialize absFilePath in constructor
        private string GetSaveFilePath(string filePath)
        {
            return Path.Combine(Application.persistentDataPath, filePath);
        }
    }
}

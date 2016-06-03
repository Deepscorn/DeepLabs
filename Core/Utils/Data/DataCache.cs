// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using Assets.Sources.Util.Pattern;
using UnityEngine;
using Util.UnityConstants;

namespace Util.Data
{
    public class DataCache<T>: Singleton<DataCache<T>>
    {
        private Dictionary<string, T> tagToDataDict = new Dictionary<string, T>();

        public T GetDataByTag(GameObject obj)
        {
            if (obj.tag == null || obj.tag == UnityTag.Untagged)
            {
                throw new ArgumentException();
            }
            if (!tagToDataDict.ContainsKey(obj.tag))
            {
                var data = (T)Activator.CreateInstance(typeof(T), new object[] { obj });
                tagToDataDict.Add(obj.tag, data);
            }
            return tagToDataDict[obj.tag];
        }
    }
}


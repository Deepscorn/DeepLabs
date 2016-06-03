// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.Log
{
    public class UnityLogImpl : ILogger
    {
        public void Log(object obj)
        {
            Debug.Log(obj);
        }

        public void LogError(string error)
        {
            Debug.LogError("Error: " + error);
        }
    }
}


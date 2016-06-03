// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0

namespace Assets.Sources.Util.Log
{
    public interface ILogger
    {
        void Log(object obj);

        void LogError(string error);
    }

    public static class LogProxy
    {
        private static ILogger LogImpl { get; set; }

        static LogProxy()
        {
            LogImpl = new UnityLogImpl();
        }

        public static void Log(object obj)
        {
            LogImpl.Log(obj);
        }

        public static void LogError(string error)
        {
            LogImpl.LogError(error);
        }
    }
}
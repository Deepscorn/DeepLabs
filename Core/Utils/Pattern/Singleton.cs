// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0

using System.Diagnostics.CodeAnalysis;

namespace Assets.Sources.Util.Pattern
{
    public abstract class Singleton<T> where T: new()
    {
        private static T instance;
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        private static readonly object LockObject = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    CreateInstance();
                }

                return instance;
            }
        }

        private static void CreateInstance()
        {
            lock (LockObject)
            {
                if (instance == null)
                {
                    instance = new T();
                }
            }
        }
    }
}

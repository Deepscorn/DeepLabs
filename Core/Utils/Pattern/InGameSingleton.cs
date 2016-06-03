// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.Pattern
{
    /// <summary>
    /// Be aware this will not prevent a non singleton constructor
    ///   such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// 
    /// As a note, this is made as MonoBehaviour because we need Coroutines.
    /// </summary>
    public class InGameSingleton<T>: MonoBehaviour where T: MonoBehaviour
    {
        private static T instance;
        private static readonly object LockObject = new object();
        private static bool applicationIsQuitting = false;
        private const string NamePrefix = "(singleton) ";

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    //Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    //    "' already destroyed on application quit." +
                    //    " Won't create again - returning null.");
                    return null;
                }

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
                    instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return;
                    }

                    if (instance == null)
                    {
                        var singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = NamePrefix + typeof(T);

                        DontDestroyOnLoad(singleton);

                        //Debug.Log("[Singleton] An instance of " + typeof(T) +
                        //          " is needed in the scene, so '" + singleton +
                        //          "' was created with DontDestroyOnLoad.");
                    }
                    //else
                    //{
                    //    Debug.Log("[Singleton] Using instance already created: " +
                    //              instance.gameObject.name);
                    //}
                }
            }
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public virtual void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}
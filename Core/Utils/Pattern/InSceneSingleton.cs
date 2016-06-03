// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.Pattern
{
    /// <summary>
    /// Same as InGameSingleton, but without DontDestroyOnLoad
    /// It is used for short object access via Instance.
    /// Another way is to use tags:
    /// 1. Create a tag in Editor
    /// 2. Add to Tags.cs to be able to change tag in future without changing client code
    /// 3. Get object:
    /// GameObject.FindObjectWithTag(Tags.OurTag).GetComponent<OurComponent>().DoSome()
    /// 
    /// It's not very friendly. Compare to usage with InSceneSingleton:
    /// 1. Inherit InSceneSingleton
    /// 2. Get object:
    /// OurComponent.Instance.DoSome()
    /// It's shorter and cleaner and you also get null if object was destroyed (implementation uses OnDestroy() for that)
    /// </summary>
    public class InSceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static readonly object LockObject = new object();
        private const string NamePrefix = "(singleton) ";

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
                    }
                }
            }
        }

        public virtual void OnDestroy()
        {
            lock (LockObject)
            {
                instance = null;
            }
        }
    }
}
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Assets.Plugins.DeepLabs.Core.Utils.Log
{
    public class Lc
    {
        public static void D([NotNull] object message)
        {
            Debug.Log(message.ToString());
        }

        // check "development build" in Unity build settings and
        // that method call will result in app crash
        public static void Assertion([NotNull] object message)
        {
            Debug.LogAssertion(message.ToString());
            // TODO log to crashlytics in release
            if (Debug.isDebugBuild)
            {
                // TODO breakpoint
                Application.Quit();
            }
            else if (!EditorApplication.isPlaying)
            {
                // TODO consider just throwing with unhandled exception leads to crash set in Unity
                //Crashlytics.RecordCustomException(string name, string reason, StackTrace stackTrace);
                //Crashlytics.RecordCustomException(string name, string reason, string stackTraceString);
            }
            if (EditorApplication.isPlaying)
            {
                Debug.Break();
            }
        }

    }

}

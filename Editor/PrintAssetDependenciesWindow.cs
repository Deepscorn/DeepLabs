using Assets.Plugins.DeepLabs.Core.Utils.Log;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    // TODO get rid of static (see UnityComponent)
    public class PrintAssetDependenciesWindow : EditorWindow
    {
        [CanBeNull]
        private static GameObject obj;

        [MenuItem("DeepLabs/Print Asset Dependencies")]
        public static void Init()
        {
            var window = (PrintAssetDependenciesWindow) GetWindow(typeof(PrintAssetDependenciesWindow));
            window.Show();
        }

        public void OnGUI()
        {
            // TODO deprecated
            obj = EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Find Dependency", obj, typeof(GameObject)) as GameObject;

            if (!obj)
            {
                EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
            }
            else
            {
                if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Dependencies"))
                {
                    var dependenciesPaths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(obj));
                    foreach (var path in dependenciesPaths)
                    {
                        Lc.D(path);
                    }
                }
            }
                
        }

        public void OnInspectorUpdate()
        {
            Repaint();
        }

    }

}

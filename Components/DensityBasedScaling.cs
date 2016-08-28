using Assets.Sources.NativePlatform;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Plugins.DeepLabs.Components
{
    public class DensityBasedScaling: MonoBehaviour
    {
        public void Start()
        {
            GetComponent<CanvasScaler>().scaleFactor = NativeApi.GuiScale;
        }

    }

}

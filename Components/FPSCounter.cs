// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Main.Sources.Scripting.Basic
{
    [RequireComponent(typeof(Text))]
    public class FPSCounter : MonoBehaviour
    {
        private const float FpsMeasurePeriod = 0.5f;
        private const string Display = "{0} FPS";
        private int fpsAccumulator = 0;
        private float fpsNextPeriod = 0;
        private int currentFps;
        private new Text guiText;

        private void Start()
        {
            fpsNextPeriod = Time.realtimeSinceStartup + FpsMeasurePeriod;
            guiText = GetComponent<Text>();
        }

        private void Update()
        {
            // measure average frames per second
            fpsAccumulator++;
            if (Time.realtimeSinceStartup > fpsNextPeriod)
            {
                currentFps = (int)(fpsAccumulator / FpsMeasurePeriod);
                fpsAccumulator = 0;
                fpsNextPeriod += FpsMeasurePeriod;
                guiText.text = string.Format(Display, currentFps);
            }
        }
    }
}

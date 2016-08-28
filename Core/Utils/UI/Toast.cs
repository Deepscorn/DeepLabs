// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.NativePlatform;
using Assets.Sources.Util.Pattern;
using UnityEngine;
using Util.Extensions;

// toast is allways on top
// todo: recalculate rectWidth & rectheight based on message string
namespace Assets.Sources.Util.UI
{
    public class Toast: InGameSingleton<Toast>
    {
        private string message;
        private const string Title = "";
        private Rect windowRect;
        private Rect contentRect;
        private float showingStartedAt;
        private float showingDuration;
        public const float DurationLong = 3f;
        private GUIStyle style;
        private int fontSize = 18;
        private float rectWidth = 307.2f;
        private float rectheight = 115.2f;
        private float spacing = 5f;

        private void UpdateScale()
        {
            var scale = NativeApi.GuiScale;
            rectWidth *= scale;
            rectheight *= scale;
            spacing *= scale;
            fontSize = (int)(fontSize * scale);
        }

        private void PositionElements()
        {
            windowRect = RectExt.NewRectFromCenterAndWidthHeight(0.5f * Screen.width, 0.5f * Screen.height, rectWidth, rectheight);
            contentRect = new Rect(spacing, spacing, windowRect.width - 2 * spacing, windowRect.height - 2 * spacing);
        }

        // you can only work with styles in OnGUI or exception is thrown
        private void InitStylesIfNeeded()
        {
            if (style == null)
            {
                style = new GUIStyle();
                style.fontSize = fontSize;
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;
                style.wordWrap = true;
            }
        }

        protected void Start()
        {
            UpdateScale();
            PositionElements();
        }

        public static void Show(string message, float duration)
        {
            Instance.ShowInternal(message, duration);
        }

        public static void Show(string message)
        {
            Show(message, DurationLong);
        }

        private void ShowInternal(string message, float duration)
        {
	        Debug.Log("Toast: " + message);
            this.message = message;
            showingDuration = duration;
            showingStartedAt = Time.time;
            Activate();
        }

        protected void OnGUI()
        {
            InitStylesIfNeeded();
            windowRect = GUI.Window(0, windowRect, DrawWindow, Title);
        }

        private void DrawWindow(int windowID)
        {
            GUI.Label(contentRect, message, style);
        }

        protected void Update()
        {
            if (Time.time - showingStartedAt > showingDuration)
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            enabled = true;
        }

        private void Deactivate()
        {
            enabled = false; // remove useless Update() calls
        }
    }
}

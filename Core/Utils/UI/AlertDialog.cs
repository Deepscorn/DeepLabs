// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using Assets.Sources.NativePlatform;
using Assets.Sources.Util.Extensions;
using UnityEngine;
using Util.Extensions;

namespace Assets.Sources.Util.UI
{
    public class AlertDialog: MonoBehaviour
    {
        // todo: replace with some built-in Unity styles with predefines for that concrete app
        private string message;
        private const string Title = "";
        private Rect windowRect;
        private Rect contentRect;
        private Rect buttonRect;
        private GUIStyle style;
        private GUIStyle buttonStyle;
        private int fontSize = 18;
        private float rectWidth = 307.2f;
        private float rectheight = 115.2f;
        private float buttonHeight = 25;
        private float buttonWidth = 60;
        private float spacing = 5f;
        private const string PositiveButtonText = "OK";
        private Action<AlertDialog> onPositiveClick;

        private void UpdateScale()
        {
            var scale = NativeApi.GuiScale;
            fontSize = (int)(fontSize * scale);
            rectWidth *= scale;
            rectheight *= scale;
            buttonHeight *= scale;
            buttonWidth *= scale;
            spacing *= scale;
        }

        private void PositionElements()
        {
            windowRect = RectExt.NewRectFromCenterAndWidthHeight(0.5f * Screen.width, 0.5f * Screen.height, rectWidth, rectheight);
            contentRect = new Rect(0, spacing, windowRect.width, windowRect.height - buttonHeight - 3 * spacing);
            buttonRect = new Rect(
                0.5f * windowRect.width - 0.5f * buttonWidth,
                contentRect.y + contentRect.height + spacing,
                buttonWidth,
                buttonHeight);
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

            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.fontSize = fontSize;
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.alignment = TextAnchor.MiddleCenter;
            }
        }

        protected void Start()
        {
            UpdateScale();
            PositionElements();
        }

        protected void OnGUI()
        {
            InitStylesIfNeeded();
            windowRect = GUI.Window(0, windowRect, DrawWindow, Title);
        }

        private void DrawWindow(int windowID)
        {
            GUI.Label(contentRect, message, style);
            if (GUI.Button(buttonRect, PositiveButtonText, buttonStyle))
            {
                if (onPositiveClick != null)
                {
                    onPositiveClick(this);
                }
            }
        }

        public void Dismiss()
        {
            throw new NotImplementedException("Dismiss");
        }

        public class Builder
        {
            private readonly AlertDialog dialog;
            private readonly GameObject gameObj;

            public Builder()
            {
                gameObj = new GameObject();
                // SetActive(false) for start to be called only when all is initialized
                gameObj.SetActive(false);
                dialog = gameObj.AddComponent<AlertDialog>();
            }

            public Builder Message(string message)
            {
                dialog.message = message;
                return this;
            }

            public Builder OnPositiveClick(Action<AlertDialog> onClick)
            {
                dialog.onPositiveClick = onClick;
                return this;
            }

            public void Build()
            {
                gameObj.SetActive(true);
            }
        }
    }
}

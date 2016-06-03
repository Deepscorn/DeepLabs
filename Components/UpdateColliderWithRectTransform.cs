// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Scripts.Basic
{
    public class UpdateColliderWithRectTransform: MonoBehaviour {

        private BoxCollider2D boxCollider2D;
        private RectTransform rectTransform;
        private Rect lastRect;

        public void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            rectTransform = GetComponentInChildren<RectTransform>();
        }

        // RectTransform is not initialized fully in Update() Tested using Unity 5.3.2f1
        public void LateUpdate()
        {
            if (rectTransform.rect != lastRect)
            {
                boxCollider2D.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
                lastRect = rectTransform.rect;
            }
        }
    }
}

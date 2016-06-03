// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.NativePlatform;
using Assets.Sources.Util.UI.Carousel;
using UnityEngine;

namespace Assets.Sources.Scripts.Basic.Carousel
{
    public class HorizontalLineLayout: AbstractLayout
    {
        public float Spacing = 40F;
        // when changing direction do not forget to change anchors of parent and items
        public enum LayoutDirection
        {
            LeftToRight = 1,
            RightToLeft = -1
        }
        public LayoutDirection Direction = LayoutDirection.RightToLeft;
        private int directionInt;

        public void OnEnable()
        {
            directionInt = (int) Direction;
        }

        public override void Position(GameObject item, int position, GameObject parent)
        {
            var rectTransform = item.GetComponent<RectTransform>();
            
            item.transform.SetParent(parent.transform, false);
            rectTransform.anchoredPosition = new Vector3(
                directionInt * rectTransform.pivot.x * rectTransform.rect.width +
                position * (Spacing + rectTransform.rect.width), 0F, 0F);
        } 

        public override bool Touches(GameObject a, GameObject b)
        {
            var aRectTransform = a.GetComponent<RectTransform>();
            var bRectTransform = b.GetComponent<RectTransform>();
            if (aRectTransform == null || bRectTransform == null)
            {
                return false;
            }
            
            var aPos = Camera.main.WorldToScreenPoint(aRectTransform.position);
            var bPos = Camera.main.WorldToScreenPoint(bRectTransform.position);

            aPos /= NativeApi.GuiScale;
            bPos /= NativeApi.GuiScale;

            var aXExtent = 0.5F * aRectTransform.rect.width;
            var aYExtent = 0.5F * aRectTransform.rect.height;
            var bXExtent = 0.5F * bRectTransform.rect.width;
            var bYExtent = 0.5F * bRectTransform.rect.height;

            var result = (aPos.x - aXExtent < bPos.x + bXExtent &&
                   aPos.x + aXExtent > bPos.x - bXExtent &&
                   aPos.y - aYExtent < bPos.y + bYExtent &&
                   aPos.y + aYExtent > bPos.y - bYExtent);

            return result;
        }
    }
}
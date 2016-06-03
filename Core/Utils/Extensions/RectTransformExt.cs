// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.Extensions
{
    public static class RectTransformExt
    {
        public static void SetHeightKeepAspect(this RectTransform rectTransform, float height)
        {
            if (!Mathf.Approximately(rectTransform.rect.height, 0F) &&
                !Mathf.Approximately(rectTransform.rect.width, 0F))
            {
                var aspect = rectTransform.rect.width / rectTransform.rect.height;
                rectTransform.sizeDelta = new Vector2(height * aspect, height);
            }
        }
    }
}

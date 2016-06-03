// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Util.Extensions
{
    public static class RectExt
    {
        public static Rect Scale(this Rect rect, float scale)
        {
            rect.size = new Vector2(rect.size.x * scale, rect.size.y * scale);

            return rect;
        }

        public static bool ContainsAny(this Rect rect, Bounds bounds)
        {
            // todo
            return false;
        }

        public static bool ContainsAll(this Rect rect, Bounds bounds)
        {
            // todo, note that Bounds are in 3D
            return false;
        }

        public static Rect NewRectFromCenterAndWidthHeight(float centerX, float centerY, float width, float height)
        {
            return new Rect(centerX - 0.5f * width, centerY - 0.5f * height, width, height);
        }
    }
}


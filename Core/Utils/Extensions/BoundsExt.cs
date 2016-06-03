// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.Extensions
{
    public static class BoundsExt
    {
        public static Bounds Intersection2DBounds(this Bounds self, Bounds other)
        {
            var x = Mathf.Max(self.min.x, other.min.x);
            var y = Mathf.Max(self.min.y, other.min.y);
            var x1 = Mathf.Min(self.max.x, other.max.x);
            var y1 = Mathf.Min(self.max.y, other.max.y);

            if (x1 <= x || y1 <= y)
            {
                return new Bounds();
            }
            else
            {
                return new Bounds(
                    new Vector3(0.5f * (x + x1), 0.5f * (y + y1), self.center.z),
                    new Vector3(x1 - x, y1 - y, 0f));
            }
        }
    }
}

// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.Extensions
{
    public static class TransformExt
    {
        public static void SetAngleY(this Transform transform, float angle)
        {
            var curAngle = transform.rotation.eulerAngles.y;
            transform.Rotate(0, angle - curAngle, 0);
        }
    }
}

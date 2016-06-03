// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Main.Sources.Util.Extensions
{
    public static class Vector3Ext
    {
        private const float epsilon = 0.01F;
        private const float epsilon_2 = epsilon * epsilon;

        public static bool EpsilonEquals(this Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < epsilon_2;
        }

        // Note: it's on the left when X looks right and Z looks forward and Y looks up
        public static Vector3 LeftOrthogonalXZ(this Vector3 v)
        {
            return new Vector3(-v.z, v.y, v.x);
        }

        // Note: it's on the left when X looks right and Z looks forward and Y looks up
        public static Vector3 RightOrthogonalXZ(this Vector3 v)
        {
            return new Vector3(v.z, v.y, -v.x);
        }
    }
}

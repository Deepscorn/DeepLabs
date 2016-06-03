// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Main.Sources.Util
{
    public static class ColorExt
    {
        private static readonly Color[] Colors = {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow,
            Color.white
        };

        public static Color RandomColor()
        {
            return Colors[UnityEngine.Random.Range(0, Colors.Length - 1)];
        }
    }
}

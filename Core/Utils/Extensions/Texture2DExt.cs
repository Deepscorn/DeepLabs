// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.Extensions
{
    public static class Texture2DExt
    {
        public static Sprite ToSprite(this Texture2D texture)
        {
            // default value for all regular imported sprites
            var pixelsPerUnit = 100F;
            // SpriteMeshType.Tight is about 10 - 100 times slower than with FullRect
            var spriteMeshType = SpriteMeshType.FullRect;
            // Vector4.one is about 2 times slower than Vector4.zero when SpriteMeshType is FullRect
            var borders = Vector4.zero;

            var rect = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, rect, Vector2.zero, pixelsPerUnit, 0U, spriteMeshType, borders);
        }
    }
}

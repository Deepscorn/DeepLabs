// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.UI.Carousel
{
    public abstract class AbstractLayout: ScriptableObject
    {
        // items must go from left to right and from up to down
        public abstract void Position(GameObject item, int position, GameObject parent);
        public abstract bool Touches(GameObject a, GameObject b);
    }
}

// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.Util.UI.Carousel;
using UnityEngine;

namespace Assets.Sources.Scripts.Basic.Carousel
{
    public class Trigger2DDataProvider: AbstractTriggerDataProvider
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            FireEnterEvent(other.gameObject);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            FireExitEvent(other.gameObject);
        }
    }
}

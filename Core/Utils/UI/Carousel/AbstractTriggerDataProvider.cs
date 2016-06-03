// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.UI.Carousel
{
    public abstract class AbstractTriggerDataProvider: MonoBehaviour
    {
        public delegate void GameObjectDelegate(GameObject obj);
        public event GameObjectDelegate OnEnterEvent = delegate {};
        public event GameObjectDelegate OnExitEvent = delegate {};

        protected void FireEnterEvent(GameObject obj)
        {
            OnEnterEvent(obj);
        }

        protected void FireExitEvent(GameObject obj)
        {
            OnExitEvent(obj);
        }
    }
}
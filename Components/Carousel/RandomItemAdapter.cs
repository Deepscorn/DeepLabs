// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Collections.Generic;
using Assets.Sources.Util.UI.Carousel;
using UnityEngine;

namespace Assets.Main.Sources.Scripting.Basic.Carousel
{
    public class RandomItemAdapter: Adapter
    {
        public List<GameObject> Items;
        private const int Count = 300;

        // called by editor at load, when start playing
        public void OnEnable()
        {
        }

        // called when start playing if was enabled to disable and enable again for play session
        public void OnDisable()
        {
        }

        public override int GetCount()
        {
            return Count;
        }
        
        public override void Reset()
        {

        }

        public override GameObject GetView(int position, GameObject convertView, GameObject parent)
        {
            return convertView ?? Instantiate(Items[Random.Range(0, Items.Count)]);
        }

        public override int GetItemViewType(int position)
        {
            return 0;
        }

        public override int GetViewTypeCount()
        {
            return 1;
        }
    }
}


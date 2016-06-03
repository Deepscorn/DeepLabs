// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Main.Sources.Util.Navigation
{
    public class NavigationLinkData
    {
        public Vector3 StartPosition { get; private set; }
        public Vector3 EndPosition { get; private set; }
        public float Distance { get; private set; }

        public NavigationLinkData(Vector3 startPosition, Vector3 endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Distance = (endPosition - startPosition).magnitude;
        }
    }
}

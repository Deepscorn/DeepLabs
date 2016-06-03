// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;
using System.Collections;

namespace Assets.Sources.Scripts.Basic
{
    // it's MonoBehaviour (not just interface) to be able to drag-n-drop in editor
    public abstract class AbstractScreen : MonoBehaviour
    {
        // set whether screen must start open / close transition
        public abstract void SetOpen(bool open);

        public abstract IEnumerator WaitForClosing();

        public abstract bool IsOpeningOrOpened();
    }
}

// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Scripts.Basic
{
    public class ResetPositionOnEnable: MonoBehaviour
    {
        private Vector3 startPosition;

        public void Awake()
        {
            var rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                startPosition = transform.localPosition;
            }
            else
            {
                startPosition = rectTransform.anchoredPosition;
            }
        }

        public void OnEnable()
        {
            var rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                transform.localPosition = startPosition;
            }
            else
            {
                rectTransform.anchoredPosition = startPosition;
            }
        }
    }
}

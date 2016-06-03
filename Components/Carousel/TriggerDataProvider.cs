// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using Assets.Sources.Util.UI.Carousel;
using UnityEngine;

namespace Assets.Main.Sources.Scripting.Basic.Carousel
{
    public class TriggerDataProvider: AbstractTriggerDataProvider
    {
        public float ScaleCoef = 1F;
        // Note: terrain must be in xz plane !
        public float LowestVisibleTerrainY = 0F;
        public bool SetPositionWithCamera = true;

        public void OnEnable()
        {
            UpdateBounds();
        }

        public void OnTriggerEnter(Collider other)
        {
            FireEnterEvent(other.gameObject);
        }

        public void OnTriggerExit(Collider other)
        {
            FireExitEvent(other.gameObject);
        }

        public void Update()
        {
            UpdateBounds();
        }

        private void UpdateBounds()
        {
            // todo: support camera angle !
            var cam = Camera.main;

            var viewportCenter = new Vector3(cam.transform.position.x, LowestVisibleTerrainY, cam.transform.position.z);
            var distance = Vector3.Distance(viewportCenter, cam.transform.position);
            var frustumHeight = 2.0F * distance * Mathf.Tan(cam.fieldOfView * 0.5F * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * cam.aspect;

            var boxCollider = GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(
                frustumWidth * ScaleCoef,
                boxCollider.size.y,
                frustumHeight * ScaleCoef);
            if (SetPositionWithCamera)
            {
                transform.position = viewportCenter;
                transform.rotation = Quaternion.FromToRotation(Vector3.left, cam.transform.TransformVector(Vector3.right));
            }
        }

    }
}

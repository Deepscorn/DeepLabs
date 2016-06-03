// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Linq;
using Assets.Sources.Util.UI.Carousel;
using UnityEngine;

namespace Assets.Main.Sources.Scripting.Basic.Carousel
{
    public class ColliderBasedLineLayout: AbstractLayout
    {
        public float Spacing = 0.5F;
        public Vector3 FirstItemSpawnPosition = Vector3.zero;
        public Vector3 Direction = Vector3.forward;
        // for using only x coord of object being positioned as offset use Vector3(1, 0, 0)
        // for y Vector3(0, 1, 0) and Vector3(0, 0, 1) for z
        // When using Vector3(1, 1, 0), then Max(x coord, y coord) will be taken
        public Vector3 OffsetCoef = new Vector3(1F, 1F, 1F);
        public Vector3 ItemRotationVector = Vector3.right;

        private Quaternion itemRotation;

        public void OnEnable()
        {
            itemRotation = Quaternion.LookRotation(ItemRotationVector);
        }

        public override void Position(GameObject item, int position, GameObject parent)
        {
            var bounds = item.GetComponent<BoxCollider>().bounds;
            var offset = Mathf.Max(
                OffsetCoef.x * bounds.size.x,
                OffsetCoef.y * bounds.size.y,
                OffsetCoef.z * bounds.size.z);

            var coordinates = FirstItemSpawnPosition + Direction * position * (offset + Spacing);
            item.transform.SetParent(parent.transform, false);
            item.transform.localPosition = coordinates;
            item.transform.localRotation = itemRotation;
        }

        public override bool Touches(GameObject a, GameObject b)
        {
            var aCollider = a.GetComponent<Collider>();
            var bCollider = b.GetComponent<Collider>();

            if (aCollider == null || bCollider == null)
            {
                return false;
            }

            var result = false;
            var radius = Mathf.Max(bCollider.bounds.extents.x, bCollider.bounds.extents.y, bCollider.bounds.extents.z);
            var colliders = Physics.OverlapSphere(b.transform.position, radius);
            result = colliders.Contains(aCollider);
            return result;

        }
    }
}

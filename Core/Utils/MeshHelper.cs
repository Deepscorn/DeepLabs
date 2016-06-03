// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Util
{
    public static class MeshHelper
    {
        public static Bounds GetTotalMeshFilterBounds(Transform objectTransform)
        {
            var meshFilter = objectTransform.GetComponent<MeshFilter>();
            var result = meshFilter != null ? meshFilter.mesh.bounds : new Bounds();
            
            foreach (Transform transform in objectTransform)
            {
                var bounds = GetTotalMeshFilterBounds(transform);
                result.Encapsulate(bounds.min);
                result.Encapsulate(bounds.max);
            }
            var scaledMin = result.min;
            scaledMin.Scale(objectTransform.localScale);
            result.min = scaledMin;
            var scaledMax = result.max;
            scaledMax.Scale(objectTransform.localScale);
            result.max = scaledMax;
            return result;
        }
    }
}


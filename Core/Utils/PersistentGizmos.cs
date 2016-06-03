// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System.Collections.Generic;
using UnityEngine;
using Utils.Creation;

namespace Assets.Main.Sources.Util
{
    public class PersistentGizmos: InGameSingleton<PersistentGizmos>
    {
        private readonly HashSet<IGizmo> gizmos = new HashSet<IGizmo>();
        
        public IGizmo AddRaycast(Vector3 from, Vector3 direction)
        {
	        var result = new RaycastGizmo(from, direction);
            gizmos.Add(result);
	        return result;
        }

        public IGizmo AddSphere(Vector3 center, float radius)
        {
	        var result = new SphereGizmo(center, radius);
            gizmos.Add(result);
	        return result;
        }

	    public void RemoveGizmo(IGizmo gizmo)
	    {
		    gizmos.Remove(gizmo);
	    }

        private void OnDrawGizmos()
        {
            foreach (var gizmo in gizmos)
            {
                gizmo.Draw();
            }
        }

        public interface IGizmo
        {
            void Draw();
        }

        private class SphereGizmo : IGizmo
        {
            private readonly Vector3 center;
            private readonly float radius;


            public SphereGizmo(Vector3 center, float radius)
            {
                this.center = center;
                this.radius = radius;
            }

            public void Draw()
            {
                Gizmos.DrawSphere(center, radius);
            }
        }

        private class RaycastGizmo : IGizmo
        {
            private readonly Vector3 from;
            private readonly Vector3 direction;
            private readonly Color color = ColorExt.RandomColor();

            public RaycastGizmo(Vector3 from, Vector3 direction)
            {
                this.from = from;
                this.direction = direction;
            }

            public void Draw()
            {
                var oldColor = Gizmos.color;
                Gizmos.color = color;
                Gizmos.DrawRay(from, direction);
                Gizmos.color = oldColor;
            }
        }
    }
}

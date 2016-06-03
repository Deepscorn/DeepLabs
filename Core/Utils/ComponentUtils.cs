// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Utils
{
	public static class ComponentUtils
	{
		public static GameObject CreateSphereTrigger(Transform parent, float radius)
		{
			var trigger = new GameObject();
			var collider = trigger.AddComponent<SphereCollider>();
			var body = trigger.AddComponent<Rigidbody>();
			trigger.transform.SetParent(parent, false);
			collider.radius = radius;
			collider.isTrigger = true;
			body.isKinematic = true;
			return trigger;
		}
	}
}
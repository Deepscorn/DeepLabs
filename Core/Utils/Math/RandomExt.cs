// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Util.Math
{
	public static class RandomExt
	{
		public static int Sign()
		{
			return Random.Range (0, 1) * 2 - 1;
		}
	}
}
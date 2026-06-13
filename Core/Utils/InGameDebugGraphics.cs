// Copyright 2026 Tokarev Mikhail Nikolaevich (https://github.com/Deepscorn)
// SPDX-License-Identifier: Apache-2.0
using UnityEngine;

namespace Deepscorn.Utils
{
	// Debug primitives for the editor game window and scene (unlike gizmos, which are not displayed in the game window by design)
	public static class InGameDebugGraphics
	{
#if UNITY_EDITOR
		private static Vector4[] _textLines = new Vector4[]
		{
			new Vector4(-1, 1, 0, 1), // a
			new Vector4(0, 1, 1, 1), // b
			new Vector4(-1, 1, -1, 0), // c
			new Vector4(-1, 1, 0, 0), // d
			new Vector4(0, 1, 0, 0), // e
			new Vector4(1, 1, 0, 0), // f
			new Vector4(1, 1, 1, 0), // g
			new Vector4(-1, 0, 0, 0), // h
			new Vector4(0, 0, 1, 0), // i
			new Vector4(-1, 0, -1, -1), // j
			new Vector4(0, 0, -1, 0 - 1), // k
			new Vector4(0, 0, 0, -1), // l
			new Vector4(0, 0, 1, -1), // m
			new Vector4(1, 0, 1, -1), // n
			new Vector4(-1, -1, 0, -1), // o
			new Vector4(0, -1, 1, -1), // p
		};
		
		private static int[] _textChars =  new[]
		{
			0xe667, // 0
			0x0604, // 1
			0xc3c3, // 2
			0xc287, // 3
			0x2990, // 4
			0xe187, // 5
			0xe1c7, // 6
			0xc410, // 7
			0xe3c7, // 8
			0xe384, // 9

			0xe3c4, // A
			0xca97, // B
			0xe043, // C
			0xca17, // D
			0xe1c3, // E
			0xe140, // F
			0xe0c7, // G
			0x23c4, // H
			0xc813, // I
			0xc852, // J
			0x2548, // K
			0x2043, // L
			0x3644, // M
			0x324c, // N
			0xe247, // O
			0xe3c0, // P
			0xe24f, // Q
			0xe3c8, // R
			0xd087, // S
			0xc810, // T
			0x2247, // U
			0x2460, // V
			0x226c, // W
			0x1428, // X
			0x1410, // Y
			0xc423, // Z

			0x0000, // space
			0x0002, // .
			0x0100, // -
		};
#endif
		
		public static void DebugSceneText(Vector3 textPosition, string inputValue, Color textColor, float size = 1.0f, float duration = 0.0f) // Very dirty shit
		{
#if UNITY_EDITOR
			for (var m = 0; m < inputValue.Length; m++)
			{
				int n = inputValue[m];
				var c = -1;
				if (n >= '0' && n <= '9')
				{
					n = n - '0';
					c = _textChars[n];
				}
				else if (n >= 'A' && n <= 'Z')
				{
					n = n - 'A' + 10;
					c = _textChars[n];
				}
				else if (n >= 'a' && n <= 'z')
				{
					n = n - 'a' + 10;
					c = _textChars[n];
				}
				else if (n == ' ')
				{
					c = _textChars[26 + 10];
				}
				else if (n == '.')
				{
					c = _textChars[26 + 11];
				}
				else if (n == '-')
				{
					c = _textChars[26 + 11];
				}

				for (var i = 0; i < 16; i++)
				{
					if ((c & (1 << (15 - i))) != 0)
					{
						Debug.DrawLine(
							new Vector3(m * 2.0f * size + textPosition.x + _textLines[i].x / 2 * size, textPosition.y, textPosition.z + _textLines[i].y * size),
							new Vector3(m * 2.0f * size + textPosition.x + _textLines[i].z / 2 * size, textPosition.y, textPosition.z + _textLines[i].w * size),
							textColor,
							duration);
					}
				}
			}
#endif
		}

		public static void DebugSceneText(Vector3 textPosition, string inputValue, Color textColor, float size, float duration, int width)
		{
#if UNITY_EDITOR
			DebugSceneText(textPosition, inputValue, textColor, size, duration);
			var w = 0.01f;
			for (var x = 1; x < width; ++x)
			{
				for (var y = 1; y < width; ++y)
				{
					DebugSceneText(textPosition + new Vector3(x * w, 0f, y * w), inputValue, textColor, size, duration);
					DebugSceneText(textPosition + new Vector3(-x * w, 0f, y * w), inputValue, textColor, size, duration);
					DebugSceneText(textPosition + new Vector3(x * w, 0f, -y * w), inputValue, textColor, size, duration);
					DebugSceneText(textPosition + new Vector3(-x * w, 0f, -y * w), inputValue, textColor, size, duration);
				}
			}
#endif
		}
		
		private static readonly Vector4[] _unitSphere = MakeUnitSphere(16);
		
		private static Vector4[] MakeUnitSphere(int len)
		{
			Debug.Assert(len > 2);
			var v = new Vector4[len * 3];
			for (int i = 0; i < len; i++)
			{
				var f = i / (float)len;
				float c = Mathf.Cos(f * (float)(Mathf.PI * 2.0));
				float s = Mathf.Sin(f * (float)(Mathf.PI * 2.0));
				v[0 * len + i] = new Vector4(c, s, 0, 1);
				v[1 * len + i] = new Vector4(0, c, s, 1);
				v[2 * len + i] = new Vector4(s, 0, c, 1);
			}
			return v;
		}
		
		public static void DrawSphere(Vector4 pos, float radius, Color color, float duration = 0)
		{
#if UNITY_EDITOR
			Vector4[] v = _unitSphere;
			int len = _unitSphere.Length / 3;
			for (int i = 0; i < len; i++)
			{
				var sX = pos + radius * v[0 * len + i];
				var eX = pos + radius * v[0 * len + (i + 1) % len];
				var sY = pos + radius * v[1 * len + i];
				var eY = pos + radius * v[1 * len + (i + 1) % len];
				var sZ = pos + radius * v[2 * len + i];
				var eZ = pos + radius * v[2 * len + (i + 1) % len];
				Debug.DrawLine(sX, eX, color, duration);
				Debug.DrawLine(sY, eY, color, duration);
				Debug.DrawLine(sZ, eZ, color, duration);
			}
#endif
		}

		public static void DrawCircle(Vector3 position, float radius, Color color, int detalization = 25, 
			float duration = 0f)
		{
#if UNITY_EDITOR
			var x = 0f;
			var xo = 0f;
			var y = 0f;
			var yo = 0f;
			var z = 0f;
			var zo = 0f;
			var angle = 0f;
			var circlePoint = position;
			var newCenter = Quaternion.identity * circlePoint;
 
			var vo = Vector3.zero;
			var vn = Vector3.zero;
   
			for (var i = 0; i < (detalization + 1); i++)
			{
				x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
				z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
 
				vo.x = newCenter.x + xo;
				vo.y = newCenter.y + yo;
				vo.z = newCenter.z + zo;
 
				vn.x = newCenter.x + x;
				vn.y = newCenter.y + y;
				vn.z = newCenter.z + z;

				if (i > 0)
				{
					Debug.DrawLine(vo, vn, color, duration);
				}
 
				xo = x;
				yo = y;
				zo = z;
				angle += (360f / detalization);
			}
#endif
		}

		public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0f)
		{
#if UNITY_EDITOR
			Debug.DrawLine(start, end, color, duration);
#endif
		}
	}
}
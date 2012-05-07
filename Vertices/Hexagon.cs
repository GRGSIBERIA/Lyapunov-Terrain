/*
 * LyapunovTerrain
 * 
 * Author : Eiichi Takebuchi(GRGSIBERIA)
 * License: MIT License
 * Email  : nanashi4129@gmail.com
 * Twitter: https://twitter.com/#!/GRGSIBERIA
 * Blog   : http://blogs.yahoo.co.jp/nanashi_hippie
 * 
 * (c) Eiichi Takebuchi(GRGSIBERIA) 2012-
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LyapunovTerrain.Vertices
{
	public class Hexagon : VerticesAbs
	{
		/// <summary>
		/// 奥行の定数
		/// </summary>
		public const float constDepth = 0.4330128f;

		public Hexagon(LyapunovGenerator gen)
			: base(gen)
		{
			Setup();
		}

		protected override Vector3[] CreateVertices()
		{
			List<Vector3> result = new List<Vector3>();
			result.Add(new Vector3(-0.2500001f, -0.5f, -0.4330127f));
			result.Add(new Vector3(0.2499999f, -0.5f, -0.4330128f));
			result.Add(new Vector3(0.5f, -0.5f, -7.45058E-08f));
			result.Add(new Vector3(0.2500001f, -0.5f, 0.4330127f));
			result.Add(new Vector3(-0.25f, -0.5f, 0.4330127f));
			result.Add(new Vector3(-0.5f, -0.5f, 0f));
			result.Add(new Vector3(-0.2500001f, 0.5f, -0.4330127f));
			result.Add(new Vector3(0.2499999f, 0.5f, -0.4330128f));
			result.Add(new Vector3(0.5f, 0.5f, -7.45058E-08f));
			result.Add(new Vector3(0.2500001f, 0.5f, 0.4330127f));
			result.Add(new Vector3(-0.25f, 0.5f, 0.4330127f));
			result.Add(new Vector3(-0.5f, 0.5f, 0f));
			result.Add(new Vector3(-0.2500001f, -0.5f, -0.4330127f));
			result.Add(new Vector3(-0.2500001f, 0.5f, -0.4330127f));
			result.Add(new Vector3(-0.2500001f, -0.5f, -0.4330127f));
			result.Add(new Vector3(0.2499999f, -0.5f, -0.4330128f));
			result.Add(new Vector3(-0.5f, -0.5f, 0f));
			result.Add(new Vector3(-0.25f, -0.5f, 0.4330127f));
			result.Add(new Vector3(0.2500001f, -0.5f, 0.4330127f));
			result.Add(new Vector3(0.5f, -0.5f, -7.45058E-08f));
			result.Add(new Vector3(-0.2500001f, 0.5f, -0.4330127f));
			result.Add(new Vector3(-0.5f, 0.5f, 0f));
			result.Add(new Vector3(0.2499999f, 0.5f, -0.4330128f));
			result.Add(new Vector3(0.5f, 0.5f, -7.45058E-08f));
			result.Add(new Vector3(0.2500001f, 0.5f, 0.4330127f));
			result.Add(new Vector3(-0.25f, 0.5f, 0.4330127f));
			result.Add(new Vector3(0.2499999f, -0.5f, -0.4330128f));
			result.Add(new Vector3(0.2499999f, 0.5f, -0.4330128f));
			result.Add(new Vector3(0.5f, -0.5f, -7.45058E-08f));
			result.Add(new Vector3(0.5f, 0.5f, -7.45058E-08f));
			result.Add(new Vector3(0.2500001f, -0.5f, 0.4330127f));
			result.Add(new Vector3(0.2500001f, 0.5f, 0.4330127f));
			result.Add(new Vector3(-0.25f, -0.5f, 0.4330127f));
			result.Add(new Vector3(-0.25f, 0.5f, 0.4330127f));
			result.Add(new Vector3(-0.5f, -0.5f, 0f));
			result.Add(new Vector3(-0.5f, 0.5f, 0f));
			return result.ToArray();
		}

		protected override int[] CreateIndices()
		{
			int[] result = {
								7,
								1,
								6,
								6,
								1,
								0,
								8,
								2,
								27,
								27,
								2,
								26,
								9,
								3,
								29,
								29,
								3,
								28,
								10,
								4,
								31,
								31,
								4,
								30,
								11,
								5,
								33,
								33,
								5,
								32,
								13,
								12,
								35,
								35,
								12,
								34,
								18,
								17,
								15,
								15,
								17,
								16,
								15,
								16,
								14,
								19,
								18,
								15,
								24,
								23,
								21,
								21,
								23,
								22,
								21,
								22,
								20,
								25,
								24,
								21,

						   };
			return result;
		}

		protected override Vector3[] CreateNormals()
		{
			List<Vector3> result = new List<Vector3>();
			result.Add(new Vector3(-2.384185E-07f, 0f, -1f));
			result.Add(new Vector3(-1.788139E-07f, 0f, -1f));
			result.Add(new Vector3(0.8660253f, 0f, -0.5000001f));
			result.Add(new Vector3(0.8660255f, 0f, 0.4999999f));
			result.Add(new Vector3(0f, 0f, 1f));
			result.Add(new Vector3(-0.8660254f, 0f, 0.5f));
			result.Add(new Vector3(-1.788139E-07f, 0f, -1f));
			result.Add(new Vector3(-1.192093E-07f, 0f, -1f));
			result.Add(new Vector3(0.8660253f, 0f, -0.5000001f));
			result.Add(new Vector3(0.8660255f, 0f, 0.4999999f));
			result.Add(new Vector3(0f, 0f, 1f));
			result.Add(new Vector3(-0.8660254f, 0f, 0.5f));
			result.Add(new Vector3(-0.8660255f, 0f, -0.4999999f));
			result.Add(new Vector3(-0.8660255f, 0f, -0.4999999f));
			result.Add(new Vector3(0f, -1f, 0f));
			result.Add(new Vector3(0f, -1f, 0f));
			result.Add(new Vector3(0f, -1f, 0f));
			result.Add(new Vector3(0f, -1f, 0f));
			result.Add(new Vector3(0f, -1f, 0f));
			result.Add(new Vector3(0f, -1f, 0f));
			result.Add(new Vector3(0f, 1f, 0f));
			result.Add(new Vector3(0f, 1f, 0f));
			result.Add(new Vector3(0f, 1f, 0f));
			result.Add(new Vector3(0f, 1f, 0f));
			result.Add(new Vector3(0f, 1f, 0f));
			result.Add(new Vector3(0f, 1f, 0f));
			result.Add(new Vector3(0.8660253f, 0f, -0.5000001f));
			result.Add(new Vector3(0.8660253f, 0f, -0.5000001f));
			result.Add(new Vector3(0.8660255f, 0f, 0.4999999f));
			result.Add(new Vector3(0.8660255f, 0f, 0.4999999f));
			result.Add(new Vector3(1.192093E-07f, 0f, 1f));
			result.Add(new Vector3(0f, 0f, 1f));
			result.Add(new Vector3(-0.8660254f, 0f, 0.5f));
			result.Add(new Vector3(-0.8660254f, 0f, 0.5f));
			result.Add(new Vector3(-0.8660255f, 0f, -0.4999999f));
			result.Add(new Vector3(-0.8660255f, 0f, -0.4999999f));

			return result.ToArray();
		}

		protected override Vector2[] CreateUVs()
		{
			List<Vector2> result = new List<Vector2>();
			result.Add(new Vector2(0f, 0.25f));
			result.Add(new Vector2(0.1666667f, 0.25f));
			result.Add(new Vector2(0.3333333f, 0.25f));
			result.Add(new Vector2(0.5f, 0.25f));
			result.Add(new Vector2(0.6666667f, 0.25f));
			result.Add(new Vector2(0.8333334f, 0.25f));
			result.Add(new Vector2(0f, 0.75f));
			result.Add(new Vector2(0.1666667f, 0.75f));
			result.Add(new Vector2(0.3333333f, 0.75f));
			result.Add(new Vector2(0.5f, 0.75f));
			result.Add(new Vector2(0.6666667f, 0.75f));
			result.Add(new Vector2(0.8333334f, 0.75f));
			result.Add(new Vector2(1f, 0.25f));
			result.Add(new Vector2(1f, 0.75f));
			result.Add(new Vector2(0.7518798f, 0.01674683f));
			result.Add(new Vector2(0.2518798f, 0.01674681f));
			result.Add(new Vector2(1.00188f, 0.125f));
			result.Add(new Vector2(0.7518797f, 0.2332532f));
			result.Add(new Vector2(0.2518796f, 0.2332532f));
			result.Add(new Vector2(0.001879692f, 0.125f));
			result.Add(new Vector2(0.7518797f, 0.9832532f));
			result.Add(new Vector2(1.00188f, 0.875f));
			result.Add(new Vector2(0.2518796f, 0.9832532f));
			result.Add(new Vector2(0.001879692f, 0.875f));
			result.Add(new Vector2(0.2518798f, 0.7667468f));
			result.Add(new Vector2(0.7518798f, 0.7667468f));
			result.Add(new Vector2(0.1666667f, 0.25f));
			result.Add(new Vector2(0.1666667f, 0.75f));
			result.Add(new Vector2(0.3333333f, 0.25f));
			result.Add(new Vector2(0.3333333f, 0.75f));
			result.Add(new Vector2(0.5f, 0.25f));
			result.Add(new Vector2(0.5f, 0.75f));
			result.Add(new Vector2(0.6666667f, 0.25f));
			result.Add(new Vector2(0.6666667f, 0.75f));
			result.Add(new Vector2(0.8333334f, 0.25f));
			result.Add(new Vector2(0.8333334f, 0.75f));

			return result.ToArray();
		}
	}
}

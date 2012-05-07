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

namespace LyapunovTerrain.Terrain
{
	public class Hexagon : TerrainAbs
	{
		public Hexagon(ref TerrainSetup setup)
			: base(ref setup)
		{
			this.meshInst = new Mesh();
			Vertices.Hexagon hex = new Vertices.Hexagon(setup.gen);
			hex.SetMesh(this.meshInst);
		}

		public override GameObject Create()
		{
			GameObject obj = Loop();
			var scr = obj.AddComponent<LyapunovEngine>();
			scr.lyapunovTexture = this.setup.texture;
			return obj;
		}

		protected override GameObject SetupBox(GameObject line, Shader shader, int i, int j)
		{
 			GameObject elem = new GameObject();

			// 座標の指定
			// 六角形なので0.5ずつ上下にずれる
			float x = i * 0.75f;
			float z = j * (Vertices.Hexagon.constDepth * 2);
			if ((i & 1) == 1) z += Vertices.Hexagon.constDepth;

			elem.transform.position = new Vector3(
				x, setup.gen.Normalized(i, j) * setup.positionMagnitude, z);
			elem.transform.localScale = new Vector3(1, setup.height, 1);

			// Collisionとかも設定される
			SetVisual(elem, this.meshInst, new Material(shader), true);
			elem.renderer.sharedMaterial.color = this.setup.texture.GetPixel(i, j);	// 色の設定

			return elem;
		}
	}
}

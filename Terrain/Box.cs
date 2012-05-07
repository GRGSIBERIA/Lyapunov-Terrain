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
using System.Threading;
using UnityEngine;

namespace LyapunovTerrain.Terrain
{
	/// <summary>
	/// 箱の集合でフラクタルを表す
	/// </summary>
	public class Box : TerrainAbs
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="setup">セットアップ</param>
		public Box(ref TerrainSetup setup) : base(ref setup)
		{
			this.boxInst = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}

		/// <summary>
		/// 箱の集合を作る
		/// </summary>
		/// <returns>集合のルート</returns>
		public override GameObject Create()
		{
			// ループ
			GameObject obj = Loop();
			var scr = obj.AddComponent<LyapunovEngine>();
			scr.lyapunovTexture = this.setup.texture;
			return obj;
		}

		/// <summary>
		/// 一つの箱をセットアップする
		/// </summary>
		/// <param name="line">ライン</param>
		/// <param name="i">横インデックス</param>
		/// <param name="j">奥インデックス</param>
		protected override GameObject SetupBox(GameObject line, Shader shader, int i, int j)
		{
			// プリミティブ
			GameObject elem = GameObject.Instantiate(this.boxInst) as GameObject;

			// 基本的なセットアップ
			elem.transform.localScale = new Vector3(1, setup.height, 1);
			elem.transform.position = new Vector3(i, GetHeightPosition(i, j), j);

			// マテリアルを呼び出して設定する
			elem.renderer.sharedMaterial = new Material(shader);
			elem.renderer.sharedMaterial.color = this.setup.texture.GetPixel(i, j);

			return elem;
		}
	}
}

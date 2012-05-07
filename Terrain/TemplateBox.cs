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
	/// <summary>
	/// 何もない素の状態の箱の集合を生成する
	/// </summary>
	public class TemplateBox : TemplateAbs
	{
		public TemplateBox(ref TerrainSetup setup)
			: base(ref setup)
		{
			this.boxInst = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.meshInst = this.boxInst.GetComponent<MeshFilter>().sharedMesh;
		}

		/// <summary>
		/// 何の工夫もないキューブを敷き詰めただけのものを生成する
		/// </summary>
		/// <returns>親オブジェクト</returns>
		public override GameObject Create()
		{
			// ループ
			GameObject master = Loop();

			// 初期化
			InitTemplate(master);

			return master;
		}

		/// <summary>
		/// 箱の設定をする
		/// </summary>
		/// <param name="line">行を表すGameObject</param>
		/// <param name="shader">シェーダ</param>
		/// <param name="i">i</param>
		/// <param name="j">j</param>
		protected override GameObject SetupBox(GameObject line, Shader shader, int i, int j)
		{
			// CreatePrimitiveするよりInstantiateのほうが高速
			GameObject elem = new GameObject();
			elem.transform.position = new Vector3(i, 0, j);
			elem.transform.localScale = new Vector3(1, this.setup.height, 1);

			//外見を設定
			SetVisual(elem, this.meshInst, new Material(shader));

			return elem;
		}
	}
}

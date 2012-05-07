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
	/// Terrainを構築するための抽象クラス
	/// </summary>
	public abstract class TerrainAbs
	{
		/// <summary>
		/// セットアップ
		/// </summary>
		protected TerrainSetup setup;

		/// <summary>
		/// 差分色
		/// </summary>
		protected Color differenceColor;

		/// <summary>
		/// 箱の実体
		/// </summary>
		protected GameObject boxInst;

		/// <summary>
		/// 箱のメッシュ
		/// </summary>
		protected Mesh meshInst;
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="setup">設定用構造体</param>
		public TerrainAbs(ref TerrainSetup setup)
		{
			this.setup = setup;
			this.differenceColor = setup.maxColor - setup.minColor;
			this.differenceColor.r = 1f / this.differenceColor.r;
			this.differenceColor.g = 1f / this.differenceColor.g;
			this.differenceColor.b = 1f / this.differenceColor.b;
		}

		/// <summary>
		/// LyapunovなGameObjectの集合を生成
		/// </summary>
		/// <returns>集合</returns>
		public virtual GameObject Create() { return null; }

		/// <summary>
		/// 色曲線とリアプノフ指数を正規化した物から色を生成
		/// </summary>
		/// <param name="i">i</param>
		/// <param name="j">j</param>
		/// <returns>色</returns>
		protected Color NormalizedDifferenceColor(int i, int j)
		{
			float normalized = setup.colorCurve.Evaluate(setup.gen.Normalized(i, j));
			Color color = this.differenceColor * normalized + setup.minColor;
			color.a = 1;
			return color;
		}

		/// <summary>
		/// 高さを取得する
		/// </summary>
		/// <param name="i">i</param>
		/// <param name="j">j</param>
		/// <returns></returns>
		protected float GetHeightPosition(int i, int j)
		{
			return setup.gen.Result[i, j] * setup.positionMagnitude;
		}

		/// <summary>
		/// 外見の設定をする
		/// </summary>
		/// <param name="obj">Componentを追加したいGameObject</param>
		/// <param name="mesh">メッシュ</param>
		/// <param name="material">マテリアル</param>
		/// <param name="useMeshCollider">MeshColliderを使うかどうか</param>
		protected void SetVisual(GameObject obj, Mesh mesh, Material material, bool useMeshCollider = false)
		{
			SetMeshRenderMaterial(obj, mesh, material);

			if (useMeshCollider)
			{
				var col = obj.AddComponent<MeshCollider>();
				col.sharedMesh = mesh;
				col.convex = true;
			}
			else 
			{
				obj.AddComponent<BoxCollider>();
			}
		}

		/// <summary>
		/// Mesh, Render, Materialの設定
		/// </summary>
		/// <param name="obj">obj</param>
		/// <param name="mesh">メッシュ</param>
		/// <param name="material">マテリアル</param>
		void SetMeshRenderMaterial(GameObject obj, Mesh mesh, Material material)
		{
			var filter = obj.AddComponent<MeshFilter>();
			filter.sharedMesh = mesh;
			obj.AddComponent<MeshRenderer>();
			obj.renderer.sharedMaterial = material;	// 代入しないとピンクになるので注意
		}


		/// <summary>
		/// 行の名前とか、誰が親だとかそこら辺
		/// </summary>
		/// <param name="elem">設定したい対象</param>
		/// <param name="lineName">行の名前</param>
		/// <param name="line">行</param>
		/// <param name="j">j</param>
		protected void SetBasic(GameObject elem, GameObject line, int j)
		{
			elem.name = line.name + "_" + j;
			elem.transform.parent = line.transform;		// 親子関係をどうするかどうか
		}

		/// <summary>
		/// 箱の設定とか
		/// </summary>
		/// <param name="line">行</param>
		/// <param name="shader">利用するシェーダー</param>
		/// <param name="lineName">行の名前</param>
		/// <param name="i">i</param>
		/// <param name="j">j</param>
		/// <returns>生成された箱</returns>
		protected virtual GameObject SetupBox(GameObject line, Shader shader, int i, int j) { return null; }

		/// <summary>
		/// 2重ループを回す
		/// </summary>
		/// <param name="master">親のゲームオブジェクト</param>
		/// <param name="shader">利用したいシェーダー</param>
		protected GameObject Loop()
		{
			GameObject master = new GameObject(this.setup.text);
			Shader shader = Shader.Find("Diffuse");

			for (int i = 0; i < this.setup.width; i++)
			{
				string lineName = this.setup.text + "_line" + i;
				GameObject line = new GameObject(lineName);
				for (int j = 0; j < this.setup.depth; j++)
				{
					// 箱の設定
					GameObject elem = SetupBox(line, shader, i, j);
					SetBasic(elem, line, j);
				}
				line.transform.parent = master.transform;
			}
			GameObject.DestroyImmediate(this.boxInst);	// 削除

			return master;
		}
	}
}

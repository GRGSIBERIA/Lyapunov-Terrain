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
	/// <summary>
	/// 頂点の生成を行う抽象クラス
	/// </summary>
	public abstract class VerticesAbs
	{
		/// <summary>
		/// 頂点
		/// </summary>
		protected Vector3[] vertices = null;

		/// <summary>
		/// 頂点
		/// </summary>
		public Vector3[] Vertices { get { return this.vertices; } }

		/// <summary>
		/// 頂点インデックス
		/// </summary>
		protected int[] indices = null;

		/// <summary>
		/// 頂点インデックス
		/// </summary>
		public int[] Indices { get { return this.indices; } }

		/// <summary>
		/// UV座標
		/// </summary>
		protected Vector2[] uvs;

		/// <summary>
		/// UV座標
		/// </summary>
		public Vector2[] UVs { get { return this.uvs; } }

		/// <summary>
		/// 法線
		/// </summary>
		protected Vector3[] normals;

		/// <summary>
		/// 法線
		/// </summary>
		public Vector3[] Normals { get { return this.normals; } }

		/// <summary>
		/// 作ったリアプノフ
		/// </summary>
		protected LyapunovGenerator gen;

		/// <summary>
		/// トライアングル・ストリップを使うかどうか
		/// </summary>
		protected bool useTriangleStrip = false;

		/// <summary>
		/// 頂点の生成を行う抽象クラス
		/// </summary>
		/// <param name="gen">作ったフラクタル</param>
		public VerticesAbs(LyapunovGenerator gen)
		{
			this.gen = gen;
		}

		/// <summary>
		/// 頂点を生成したい抽象メソッド
		/// </summary>
		/// <returns>頂点の配列</returns>
		protected abstract Vector3[] CreateVertices();

		/// <summary>
		/// 頂点インデックスを生成したい抽象メソッド
		/// </summary>
		/// <returns>頂点インデックスの配列</returns>
		protected abstract int[] CreateIndices();

		/// <summary>
		/// UV座標を生成したい抽象メソッド
		/// </summary>
		/// <returns>UV座標の配列</returns>
		protected abstract Vector2[] CreateUVs();

		/// <summary>
		/// 法線を生成する抽象メソッド
		/// </summary>
		/// <returns>法線の配列</returns>
		protected abstract Vector3[] CreateNormals();

		/// <summary>
		/// メッシュをセットする
		/// </summary>
		/// <param name="mesh">設定したいメッシュ</param>
		public void SetMesh(Mesh mesh)
		{
			mesh.vertices = this.vertices;
			if (this.useTriangleStrip)
				mesh.SetTriangleStrip(this.indices, 0);
			else
				mesh.SetTriangles(this.indices, 0);
			mesh.uv = this.uvs;
			mesh.normals = this.normals;
		}

		/// <summary>
		/// とりあえず全部設定する
		/// </summary>
		protected void Setup()
		{
			this.vertices = CreateVertices();
			this.indices = CreateIndices();
			this.uvs = CreateUVs();
			this.normals = CreateNormals();
		}
	}
}

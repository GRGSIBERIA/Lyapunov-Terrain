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
using UnityEditor;
using LyapunovTerrain.Vertices;

namespace LyapunovTerrain
{
	/// <summary>
	/// リアプノフでメッシュ生成
	/// </summary>
	public class MeshGenerator
	{
		/// <summary>
		/// メッシュの種類
		/// </summary>
		public enum MeshType
		{
			/// <summary>
			/// 板状
			/// </summary>
			Plane,

			/// <summary>
			/// 全てがBox
			/// </summary>
			Box,
		}

		/// <summary>
		/// メッシュ
		/// </summary>
		Mesh mesh;

		/// <summary>
		/// メッシュ
		/// </summary>
		public Mesh Mesh { get { return this.mesh; } }

		/// <summary>
		/// リアプノフ
		/// </summary>
		LyapunovGenerator gen;

		/// <summary>
		/// 生成するメッシュの種類
		/// </summary>
		MeshType type;

		/// <summary>
		/// 隆起の倍率
		/// </summary>
		float magnitude;

		/// <summary>
		/// メッシュを生成する何か
		/// </summary>
		/// <param name="gen">作ったフラクタル</param>
		/// <param name="type">生成するメッシュの種類</param>
		/// <param name="magnitude">隆起の倍率</param>
		public MeshGenerator(LyapunovGenerator gen, MeshType type, float magnitude)
		{
			this.gen = gen;
			this.type = type;
			this.mesh = new Mesh();
			this.magnitude = magnitude;
			CreateMesh();
		}

		/// <summary>
		/// 頂点の生成を行う
		/// </summary>
		void CreateMesh()
		{
			switch (type)
			{
				case MeshType.Plane:
					CreatePlane(new Vertices.Plane(this.gen, this.magnitude));
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// 板状の奴を作る
		/// </summary>
		/// <param name="vtx"></param>
		void CreatePlane(VerticesAbs vtx)
		{
			this.mesh.vertices = vtx.Vertices;
			this.mesh.SetTriangles(vtx.Indices, 0);
			this.mesh.uv = vtx.UVs;
			this.mesh.normals = vtx.Normals;
		}
	}
}

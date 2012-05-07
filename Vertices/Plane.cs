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
	/// プレーン状に並べたものを生成
	/// </summary>
	public class Plane : VerticesAbs
	{
		/// <summary>
		/// 隆起の倍率
		/// </summary>
		float magnitude;

		/// <summary>
		/// プレーン状のオブジェクトを生成
		/// </summary>
		/// <param name="gen">作ったリアプノフ</param>
		/// <param name="magnitude">隆起の倍率</param>
		public Plane(LyapunovGenerator gen, float magnitude)
			: base(gen)
		{
			this.magnitude = magnitude;
			Setup();
		}

		protected override Vector3[] CreateVertices()
		{
			Vector3[] result = new Vector3[this.gen.XLength * this.gen.YLength];
			for (int x = 0; x < this.gen.XLength; x++)
			{
				for (int y = 0; y < this.gen.YLength; y++)
				{
					// Vector3.yにはリアプノフの色味とかが入る
					result[x * this.gen.XLength + y] = new Vector3(x, this.gen.Result[x, y] * magnitude, y);
				}
			}
			return result;
		}

		protected override int[] CreateIndices()
		{
			return IndicesWithVertices();
		}

		/// <summary>
		/// メッシュの周りに必ず3つの頂点が付いちゃう形式
		/// </summary>
		/// <returns>頂点インデックス</returns>
		int[] IndicesWithVertices()
		{
			int width = (int)this.gen.XLength - 1;	// 横のメッシュの数
			int height = (int)this.gen.YLength - 1;
			int num = height * width * 2 * 3;	// 縦と横を掛ける, 四角形のメッシュ, 

			int[] result = new int[num];	// 1メッシュにつき3頂点
			int cnt = 0;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					// 四角形を構成する
					result[cnt++] = (width + 1) * (i + 1) + j + 1;
					result[cnt++] = (width + 1) * (i + 1) + j;
					result[cnt++] = (width + 1) * i + j; 
					result[cnt++] = (width + 1) * i + j; 
					result[cnt++] = (width + 1) * i + j + 1;
					result[cnt++] = (width + 1) * (i + 1) + j + 1; 
				}
			}
			return result;
		}

		/// <summary>
		/// トライアングル・ストリップ！
		/// </summary>
		/// <returns>頂点インデックス</returns>
		int[] SetTriangleStrip()
		{
			int[] result = null;

			return result;
		}

		protected override Vector2[] CreateUVs()
		{
			Vector2[] result = new Vector2[this.vertices.Length];
			float diffX = 1f / this.gen.XLength;	// X差分
			float diffY = 1f / this.gen.YLength;	// Y差分
			for (int i = 0; i < this.gen.XLength; i++)
			{
				for (int j = 0; j < this.gen.YLength; j++)
				{
					// ここでUVの設定してるよ！
					result[this.gen.XLength * i + j] = new Vector2(i * diffX, j * diffY);
				}
			}
			return result;
		}

		protected override Vector3[] CreateNormals()
		{
			Vector3[] result = new Vector3[this.vertices.Length];
			for (int i = 0; i < this.gen.XLength; i++)
			{
				for (int j = 0; j < this.gen.YLength; j++)
				{
					result[this.gen.XLength * i + j] = new Vector3(0, 1, 0);
				}
			}
			return result;
		}
	}
}

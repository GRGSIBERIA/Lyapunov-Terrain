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
using System.Threading;

namespace LyapunovTerrain
{
	/// <summary>
	/// リアプノフ・フラクタルから自動生成したテクスチャ
	/// </summary>
	public class TextureGenerator
	{
		class Setup
		{
			/// <summary>
			/// 画素Xの大きさ
			/// </summary>
			public uint sizeX;

			/// <summary>
			/// 画素Yの大きさ
			/// </summary>
			public uint sizeY;

			/// <summary>
			/// リアプノフ指数の二次元配列
			/// </summary>
			public float[,] result;

			/// <summary>
			/// 自動生成したテクスチャ
			/// </summary>
			public Texture2D texture;

			/// <summary>
			/// 最小値
			/// </summary>
			public float min;

			/// <summary>
			/// 最大値
			/// </summary>
			public float max;

			/// <summary>
			/// 1から見た最小値と最大値の差分の比率
			/// </summary>
			public float difference;

			/// <summary>
			/// 一番大きな色
			/// </summary>
			public Color maxColor;

			/// <summary>
			/// 一番小さな色
			/// </summary>
			public Color minColor;

			/// <summary>
			/// 大小の色の差分
			/// </summary>
			public Color diffColor;

			/// <summary>
			/// 色の変化
			/// </summary>
			public AnimationCurve curve;
		}

		/// <summary>
		/// リアプノフ指数のテクスチャを自動生成する
		/// </summary>
		/// <param name="gen">生成したフラクタル</param>
		public static Texture2D CreateTexture(LyapunovGenerator gen, Color maxColor, Color minColor, AnimationCurve curve)
		{
			Setup setup = new Setup();

			setup.sizeX = gen.XLength;
			setup.sizeY = gen.YLength;
			setup.result = gen.Result;
			setup.texture = new Texture2D((int)setup.sizeX, (int)setup.sizeY);

			setup.min = gen.MinValue;
			setup.max = gen.MaxValue;
			setup.difference = 1 / (0 - setup.min + setup.max);
			setup.minColor = minColor;
			setup.maxColor = maxColor;

			// 差分色を正規化してるところ
			setup.diffColor = setup.maxColor - setup.minColor;
			setup.curve = curve;

			// ここでテクスチャの色を埋めて行く
			SetTextureColor(ref setup, gen);

			return setup.texture;
		}


		/// <summary>
		/// テクスチャの色を設定する
		/// </summary>
		static void SetTextureColor(ref Setup setup, LyapunovGenerator gen)
		{
			for (uint x = 0; x < setup.sizeX; x++)
			{
				for (uint y = 0; y < setup.sizeY; y++)
				{
					// 0から1の間で正規化されたリアプノフ指数
					float normalizedLyapunov = gen.Normalized((int)x, (int)y);

					Color color = setup.diffColor * setup.curve.Evaluate(normalizedLyapunov) + setup.minColor;
					color.a = 1;

					setup.texture.SetPixel((int)x, (int)y, color);
				}
			}
			setup.texture.Apply();
		}

		/// <summary>
		/// 高さマップを生成する
		/// </summary>
		/// <param name="gen">生成したリアプノフ</param>
		/// <param name="curve">色曲線</param>
		/// <returns>高さマップ</returns>
		public static Texture2D CreateHeightMap(LyapunovGenerator gen, AnimationCurve curve)
		{
			Texture2D tex = new Texture2D((int)gen.XLength, (int)gen.YLength);
			float diff = 1f / (gen.MaxValue - gen.MinValue);
			Color[] color = new Color[gen.XLength * gen.YLength];

			for (int i = 0; i < (int)gen.XLength; i++)
			{
				for (int j = 0; j < (int)gen.YLength; j++)
				{
					float val = curve.Evaluate(gen.Result[i, j] * diff);
					color[i * gen.XLength + j] = new Color(val, val, val, 1);
				}
			}
			tex.SetPixels(color);
			tex.Apply();
			return tex;
		}
	}
}

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

namespace LyapunovTerrain
{
	/// <summary>
	/// リアプノフ・フラクタルを生成するクラス
	/// </summary>
	public class LyapunovGenerator
	{
		/// <summary>
		/// 周期文字列
		/// </summary>
		string text;

		/// <summary>
		/// 周期文字列
		/// </summary>
		public string Text { get { return this.text; } }

		/// <summary>
		/// 画素Xの長さ
		/// </summary>
		uint xlength;

		/// <summary>
		/// 画素Xの長さ
		/// </summary>
		public uint XLength { get { return this.xlength; } }

		/// <summary>
		/// 画素Yの長さ
		/// </summary>
		uint ylength;

		/// <summary>
		/// 画素Yの長さ
		/// </summary>
		public uint YLength { get { return this.ylength; } }

		/// <summary>
		/// 打ち切り回数
		/// </summary>
		uint calcTimes;

		/// <summary>
		/// 計算結果を入れておく用
		/// </summary>
		float[,] result;

		/// <summary>
		/// 計算結果
		/// </summary>
		public float[,] Result { get { return this.result; } }

		/// <summary>
		/// 最小値
		/// </summary>
		float minValue = 0;

		/// <summary>
		/// 最小値
		/// </summary>
		public float MinValue { get { return this.minValue; } }

		/// <summary>
		/// 最大値
		/// </summary>
		float maxValue = 0;

		/// <summary>
		/// 最大値
		/// </summary>
		public float MaxValue { get { return this.maxValue; } }

		/// <summary>
		/// 1に対する最大値から最小値を引いた差分の割合
		/// </summary>
		float difference;

		/// <summary>
		/// 1に対する最大値から最小値を引いた差分の割合
		/// </summary>
		public float Difference { get { return this.difference; } }

		float xdiff;

		float ydiff;

		/// <summary>
		/// 凹にするかどうか
		/// </summary>
		bool concave;

		/// <summary>
		/// 凹にするかどうか
		/// </summary>
		public bool Concave { get { return this.concave; } }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="text">AまたはBの周期文字列</param>
		/// <param name="xlength">画素Xの長さ</param>
		/// <param name="ylength">画素Yの長さ</param>
		/// <param name="repeatTimes">文字列を繰り返すの回数</param>
		/// <param name="concave">凹にするかどうか</param>
		/// <param name="calcTimes">計算回数, 通常周期文字列と同じ長さで近似するが途中で打ち切りたいときは指定する</param>
		public LyapunovGenerator(string text, uint xlength, uint ylength, uint repeatTimes=1, bool concave=true, uint calcTimes=uint.MaxValue)
		{
			foreach (var s in text.ToLower())
			{
				if (s == 'a' || s == 'b') continue;
				else throw new FormatException("Use string 'a' or 'b'.");
			}
			string buf = text.ToLower();
			
			// 繰り返しの回数だけ文字列を繰り返す
			this.text = "";
			for (int i = 0; i < repeatTimes; i++) this.text += buf;

			this.xlength = xlength;
			this.ylength = ylength;
			this.xdiff = 4f / xlength;
			this.ydiff = 4f / ylength;
			this.calcTimes = this.text.Length <= calcTimes ? (uint)this.text.Length : calcTimes;
			this.concave = concave;

			this.result = GetLambdas();
			this.minValue = GetMinValue();
			this.maxValue = GetMaxValue();
			this.difference = 1f / (this.maxValue - this.minValue);
		}

		/// <summary>
		/// 正規化されたResult
		/// </summary>
		/// <param name="i">i</param>
		/// <param name="j">j</param>
		/// <returns>正規化されたリアプノフ指数</returns>
		public float Normalized(int i, int j)
		{
			return difference * (result[i, j] - minValue);
		}

		/// <summary>
		/// 計算結果から最小値を見つけてくる
		/// </summary>
		/// <returns>最小値</returns>
		float GetMinValue()
		{
			float r = result[0, 0];
			foreach (var n in result)
			{
				if (r > n) r = n;
			}
			return r;
		}

		/// <summary>
		/// 計算結果から最大値を見つけてくる
		/// </summary>
		/// <returns>最大値</returns>
		float GetMaxValue()
		{
			float r = result[0, 0];
			foreach (var n in result)
			{
				if (r < n) r = n;
			}
			return r;
		}

		/// <summary>
		/// float型の2次元配列でリアプノフ指数を得る
		/// </summary>
		/// <param name="height">どれぐらい計算結果にゲタを履かせるか</param>
		/// <returns>リアプノフ指数が格納された2次元配列, 長さは画素X,Yに対応</returns>
		float[,] GetLambdas(float height=0)
		{
			float[,] result = new float[this.xlength, this.ylength];

			for (int a = 0; a < this.xlength; a++)
			{
				for (int b = 0; b < this.ylength; b++)
				{
					float[] R = GetR(a * this.xdiff, b * this.ydiff);
					float[] X = GetX(R);
					
					result[a, b] = GetLambdaIndex(R, X) + height;
				}
			}
			return result;
		}

		/// <summary>
		/// 配列Rを取得する
		/// 配列Rは文字列を数値列に置き換えたもの
		/// </summary>
		/// <param name="a">対象画素のX座標</param>
		/// <param name="b">対象画素のY座標</param>
		/// <returns>配列R</returns>
		float[] GetR(float a, float b)
		{
			float[] result = new float[this.text.Length];
			for (int n = 0; n < result.Length; n++)
				result[n] = this.text[n] == 'a' ? a : b;
			return result;
		}

		/// <summary>
		/// 配列Xを取得する
		/// </summary>
		/// <param name="R">配列R</param>
		/// <returns>配列Rより1つ大きい配列X</returns>
		float[] GetX(float[] R)
		{
			float[] result = new float[this.text.Length + 1];
			result[0] = 0.5f;
			for (int n = 0; n < R.Length; n++)
				result[n + 1] = R[n] * result[n] * (1 - result[n]);
			return result;
		}

		/// <summary>
		/// リアプノフ指数λの計算
		/// </summary>
		/// <param name="R">配列R</param>
		/// <param name="X">配列X</param>
		/// <returns>リアプノフ指数</returns>
		float GetLambdaIndex(float[] R, float[] X)
		{
			float result;
			float sigma = 0;
			for (int n = 1; n < this.calcTimes; n++)
			{
				float puyo = R[n] * (1f - 2f * X[n + 1]);
				float hoge = (float)Math.Log(Mathf.Abs(puyo));
				if (float.IsInfinity(hoge) || float.IsNaN(hoge)) hoge = 0;
				sigma += hoge;
			}
			result = (1f / this.calcTimes) * sigma;
			if (!this.concave) result *= -1;
			return result;
		}
	}
}

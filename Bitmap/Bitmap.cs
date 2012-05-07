/*
 * Asset-to-Bitmap
 * 
 * Author	: Eiichi Takebuchi(GRGSIBERIA)
 * License	: 3-clause BSD License
 * Email	: nanashi4129@gmail.com
 * Twitter	: https://twitter.com/#!/GRGSIBERIA
 * Facebook	: http://www.facebook.com/takebuchie
 * Blog		: http://blogs.yahoo.co.jp/nanashi_hippie
 * 
 * (c) Eiichi Takebuchi(GRGSIBERIA) 2012-
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using System.IO;

namespace Bitmap
{
	/// <summary>
	/// ビットマップ画像を扱うためのクラス
	/// </summary>
	public class Bitmap
	{
		/// <summary>
		/// ファイル情報
		/// </summary>
		FileFormat foramt;

		/// <summary>
		/// 情報ヘッダ
		/// </summary>
		InformationHeader header;

		/// <summary>
		/// ファイルのパス
		/// </summary>
		string path;

		/// <summary>
		/// カラーの配列
		/// </summary>
		Color[] color;

		/// <summary>
		/// ビットマップ画像を扱うクラス
		/// </summary>
		/// <param name="path">生成したいパス</param>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		/// <param name="color">色の配列</param>
		public Bitmap(string path, uint width, uint height, Color[] color)
		{
			this.foramt = new FileFormat(width, height);
			this.header = new InformationHeader(width, height);
			this.path = path;
			this.color = color;
			Debug.Log(width * height);
		}

		/// <summary>
		/// 作成して書き込む
		/// </summary>
		public void Write()
		{
			FileStream fst = new FileStream(path, FileMode.OpenOrCreate);
			BinaryWriter bin = new BinaryWriter(fst);
			this.foramt.Write(bin);
			this.header.Write(bin);

			uint buf = 0;

			for (uint h = 0; h < header.Height; h++)
			{
				// 色を何かする
				for (uint w = 0; w < this.header.Width; w++)
				{
					buf = h * this.header.Height + w;
					bin.Write((byte)(this.color[buf].r * 255));
					bin.Write((byte)0);		// どうやら何か事情が違うらしい
					bin.Write((byte)(this.color[buf].b * 255));
					bin.Write((byte)(this.color[buf].g * 255));
				}
			}

			bin.Close();
			fst.Close();
		}
	}
}

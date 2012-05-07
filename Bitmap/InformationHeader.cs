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
using System.IO;

namespace Bitmap
{
	/// <summary>
	/// 情報ヘッダ
	/// </summary>
	public class InformationHeader
	{
		/// <summary>
		/// 画像のサイズ
		/// </summary>
		uint size;

		/// <summary>
		/// 幅
		/// </summary>
		uint width;

		/// <summary>
		/// 幅
		/// </summary>
		public uint Width { get { return this.width; } }

		/// <summary>
		/// 高さ
		/// </summary>
		uint height;

		/// <summary>
		/// 高さ
		/// </summary>
		public uint Height { get { return this.height; } }

		/// <summary>
		/// 常に1
		/// </summary>
		ushort planes;

		/// <summary>
		/// 1画素あたりのデータ量, 32bit
		/// </summary>
		ushort bitCount;

		/// <summary>
		/// 圧縮形式, 基本は無圧縮
		/// </summary>
		uint compression;

		/// <summary>
		/// 画像データ部のサイズ, 96dpiだと3780
		/// </summary>
		uint sizeImage;

		/// <summary>
		/// 横方向の解像度, メートル単位で3780
		/// </summary>
		uint XPixPerMeter;

		/// <summary>
		/// 縦方向の解像度, メートル単位で3780
		/// </summary>
		uint YPixPerMeter;

		/// <summary>
		/// 格納されているパレット数, 0
		/// </summary>
		uint clrUsed;

		/// <summary>
		/// 重要なパレットのインデックス, 0
		/// </summary>
		uint clrImportant;

		/// <summary>
		/// コンストラクタ禁止
		/// </summary>
		private InformationHeader() 
		{
			throw new Exception("なんか禁止されてるコンストラクタ呼ばれてるよ！"); 
		}

		/// <summary>
		/// 情報ヘッダのコンストラクタ
		/// </summary>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		public InformationHeader(uint width, uint height)
		{
			this.size = 40;
			this.width = width;
			this.height = height;
			this.planes = 1;
			this.bitCount = 32;
			this.compression = 0;
			this.sizeImage = 3780;
			this.XPixPerMeter = 3780;
			this.YPixPerMeter = 3780;
			this.clrUsed = 0;
			this.clrImportant = 0;
		}

		/// <summary>
		/// ファイルストリームに書き込みする
		/// </summary>
		/// <param name="bin">バイナリ</param>
		public void Write(BinaryWriter bin)
		{
			bin.Write(BitConverter.GetBytes(size));
			bin.Write(BitConverter.GetBytes(width));
			bin.Write(BitConverter.GetBytes(height));
			bin.Write(BitConverter.GetBytes(planes));
			bin.Write(BitConverter.GetBytes(bitCount));
			bin.Write(BitConverter.GetBytes(compression));
			bin.Write(BitConverter.GetBytes(sizeImage));
			bin.Write(BitConverter.GetBytes(XPixPerMeter));
			bin.Write(BitConverter.GetBytes(YPixPerMeter));
			bin.Write(BitConverter.GetBytes(clrUsed));
			bin.Write(BitConverter.GetBytes(clrImportant));
		}
	}
}

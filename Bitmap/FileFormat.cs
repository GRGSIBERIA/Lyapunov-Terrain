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
	/// ファイルフォーマット
	/// </summary>
	public class FileFormat
	{
		/// <summary>
		/// ファイルタイプ, BM固定
		/// </summary>
		char[] type;

		/// <summary>
		/// ファイルサイズ
		/// </summary>
		uint size;

		/// <summary>
		/// 予約域
		/// </summary>
		ushort reserved1;

		/// <summary>
		/// 予約域2
		/// </summary>
		ushort reserved2;

		/// <summary>
		/// ファイル先頭から画像までのオフセット
		/// </summary>
		uint offBits;

		/// <summary>
		/// 禁止！
		/// </summary>
		private FileFormat()
		{
			throw new Exception("なんか禁止されてるコンストラクタ呼ばれてるよ！");
		}

		/// <summary>
		/// ファイルフォーマットの情報
		/// </summary>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		public FileFormat(uint width, uint height)
		{
			this.type = new char[2];
			this.type[0] = 'B';
			this.type[1] = 'M';

			this.size = 16 + 40 + (width * height * 4);
			this.reserved1 = 0;
			this.reserved2 = 0;
			this.offBits = 16 + 40;
		}

		/// <summary>
		/// 書き込む
		/// </summary>
		/// <param name="bin">バイナリ</param>
		public void Write(BinaryWriter bin)
		{
			bin.Write(this.type);
			bin.Write(BitConverter.GetBytes(this.size));
			bin.Write(BitConverter.GetBytes(reserved1));
			bin.Write(BitConverter.GetBytes(reserved2));
			bin.Write(BitConverter.GetBytes(offBits));
		}
	}
}

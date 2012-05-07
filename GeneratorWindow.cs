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
using UnityEngine;
using UnityEditor;
using System.Collections;
using LyapunovTerrain;
using System;
using System.IO;
using System.Text.RegularExpressions;


#if UNITY_EDITOR

/// <summary>
/// プラグイン用の窓を表示する
/// </summary>
public class GeneratorWindow : EditorWindow
{
	/// <summary>
	/// 生成に利用する文字列
	/// </summary>
	string text = "";

	/// <summary>
	/// 文字列の繰り返し回数
	/// </summary>
	int repeatTimes = 1;

	/// <summary>
	/// 生成するオブジェクトの奥行
	/// </summary>
	int depth = 512;

	/// <summary>
	/// 生成するオブジェクトの幅
	/// </summary>
	int width = 512;

	/// <summary>
	/// オブジェクトの高さ
	/// </summary>
	int height = 0;

	/// <summary>
	/// Y軸位置の倍率
	/// </summary>
	float magnitude = 1f;

	/// <summary>
	/// 凹凸はどちらか
	/// </summary>
	public enum UnevenType
	{
		/// <summary>
		/// 凸
		/// </summary>
		Convex,

		/// <summary>
		/// 凹
		/// </summary>
		Concave,
	}

	/// <summary>
	/// 凹凸の種類
	/// </summary>
	UnevenType unevenType = UnevenType.Convex;

	/// <summary>
	/// 生成するオブジェクトの形
	/// </summary>
	public enum TerrainType
	{
		/// <summary>
		/// テクスチャ
		/// </summary>
		Texture,

		/// <summary>
		/// 平面地形
		/// </summary>
		Plane,

		/// <summary>
		/// 箱状地形
		/// </summary>
		Box,

		/// <summary>
		/// 箱何もなし
		/// </summary>
		TemplateBox,

		/// <summary>
		/// 六角柱
		/// </summary>
		Hexagon,

		/// <summary>
		/// 六角柱何もない
		/// </summary>
		TemplateHexagon,
	}

	/// <summary>
	/// オブジェクトの形
	/// </summary>
	TerrainType terrainType = TerrainType.Plane;

	/// <summary>
	/// 最小値の色
	/// </summary>
	Color minColor = Color.black;

	/// <summary>
	/// 最大値の色
	/// </summary>
	Color maxColor = Color.blue;

	/// <summary>
	/// テクスチャを作るときに利用する
	/// 色のカーブ
	/// </summary>
	AnimationCurve colorCurve = AnimationCurve.Linear(0, 0, 1, 1);

	/// <summary>
	/// 高さマップを作るかどうか
	/// </summary>
	bool enableHeightMap;

	/// <summary>
	/// ビットマップ画像を生成するか
	/// </summary>
	bool enableBitmap;

	/// <summary>
	/// ボタンなどの高さ
	/// </summary>
	const int fieldHeight = 20;

	/// <summary>
	/// ボタン同士の隙間
	/// </summary>
	const int fieldSpace = 2;

	/// <summary>
	/// 位置Y
	/// </summary>
	const int positionY = fieldHeight + fieldSpace;


	[MenuItem("Plugins/Lyapunov Terrain")]
	static void Init()
	{
		EditorWindow.GetWindow<GeneratorWindow>();
	}

	void OnGUI()
	{
		int cnt = 0;
		int size = this.width;

		this.terrainType = (TerrainType)EditorGUI.EnumPopup(SetRect(ref cnt), "terrain type", this.terrainType);
		this.text = EditorGUI.TextField(SetRect(ref cnt), "text", this.text);
		this.repeatTimes = EditorGUI.IntField(SetRect(ref cnt), "repeat times", this.repeatTimes);

		if (this.text != "")
		{

			size = EditorGUI.IntField(SetRect(ref cnt), "size", size);
			this.width = size;
			this.depth = size;	// 生成に失敗してしまうのでサイズは両者とも同じにしておく

			// 高さの指定
			if (this.terrainType != TerrainType.Plane && this.terrainType != TerrainType.Texture)
			{
				this.height = EditorGUI.IntField(SetRect(ref cnt), "height", this.height);
			}

			// 倍率設定
			if (this.terrainType != TerrainType.Texture)
				this.magnitude = EditorGUI.FloatField(SetRect(ref cnt), "Y magnitude", this.magnitude);

			if (this.terrainType != TerrainType.Plane && this.terrainType != TerrainType.Texture)
			{
				EditorGUI.TextArea(SetRect(ref cnt), "generate time: " +
					(8f * (this.width * this.depth)) * (Mathf.Log(this.width * this.depth) / 2) / 1000f + "sec");
				cnt++;
			}

			this.unevenType = (UnevenType)EditorGUI.EnumPopup(SetRect(ref cnt), "uneven type", this.unevenType);

			this.enableBitmap = EditorGUI.Toggle(SetRect(ref cnt), "bitmap", this.enableBitmap);	

			// heightmapの設定
			if (this.terrainType != TerrainType.TemplateBox && this.terrainType != TerrainType.TemplateHexagon)
				this.enableHeightMap = EditorGUI.Toggle(SetRect(ref cnt), "height map", this.enableHeightMap);
			else
				this.enableHeightMap = true;	// テンプレートの場合は強制的に生成

			this.minColor = EditorGUI.ColorField(SetRect(ref cnt), "min color", this.minColor);
			this.maxColor = EditorGUI.ColorField(SetRect(ref cnt), "max color", this.maxColor);
			this.colorCurve = EditorGUI.CurveField(SetRect(ref cnt), "color curve", this.colorCurve);

			Rect[] btColorRect = SetTwoRect(ref cnt);
			if (GUI.Button(btColorRect[0], "Add")) AddKeyframe();
			if (GUI.Button(btColorRect[1], "Reduce")) ReduceKeyframe();

			NumericalCheck();
			CurveCheck();

			// フラクタルを生成
			if (GUI.Button(SetRect(ref cnt), "Generate")) Generate();
		}
		
		Repaint();
	}

	/// <summary>
	/// キーフレームを追加
	/// </summary>
	void AddKeyframe()
	{
		const float val = 0.5f;
		const float intan = 0f;
		const float outan = 0f;
		int cnt = this.colorCurve.length - 1;	// 始点と終点を引いてる
		float diff = 1f / this.colorCurve.length;
		
		this.colorCurve.AddKey(new Keyframe(0.00001f, 0, 0, 0));

		// キーフレの位置を調整
		for (int i = 0; i < cnt; i++)
		{
			float time = (i + 1) * diff;
			this.colorCurve.MoveKey(i + 1, new Keyframe(time, val, intan, outan));
		}
		this.colorCurve.MoveKey(this.colorCurve.length - 1, new Keyframe(1f, 1f, intan, outan));
	}

	/// <summary>
	/// キーフレームを削除
	/// </summary>
	void ReduceKeyframe()
	{
		this.colorCurve = AnimationCurve.Linear(0, 0, 1, 1);
	}

	/// <summary>
	/// 描画範囲を作る
	/// </summary>
	/// <param name="cnt">カウント</param>
	/// <returns>矩形</returns>
	Rect SetRect(ref int cnt)
	{
		int fieldWidth = (int)position.width - 20;
		return new Rect(2, 2 + positionY * cnt++, fieldWidth, fieldHeight);
	}

	/// <summary>
	/// 横に二つぐらい作る描画範囲を作る
	/// </summary>
	/// <param name="cnt">カウント</param>
	/// <returns>矩形</returns>
	Rect[] SetTwoRect(ref int cnt)
	{
		Rect[] rect = new Rect[2];
		int fieldWidth = (int)position.width - 20;

		rect[0] = new Rect(2, 2 + positionY * cnt, fieldWidth / 2, fieldHeight);
		rect[1] = new Rect(fieldWidth / 2, 2 + positionY * cnt++, fieldWidth / 2, fieldHeight);
		return rect;
	}

	/// <summary>
	/// 数値が行き過ぎたり不正じゃないかチェック
	/// </summary>
	void NumericalCheck()
	{
		if (this.width <= 0) this.width = 1;
		if (this.height <= 0) this.height = 1;
		if (this.depth <= 0) this.depth = 1;
		if (this.repeatTimes <= 0) this.repeatTimes = 1;
	
		if (this.width * this.depth > 65000 && this.terrainType == TerrainType.Plane)
		{
			this.width = (int)Mathf.Sqrt(65000);
			this.depth = this.width;
		}
	}

	/// <summary>
	/// 文字列のチェック
	/// </summary>
	void TextCheck()
	{
		// aもしくはbでなければ該当文字を強制的に削除する
		if (this.text.Length > 0)
		{
			Regex.Replace(this.text, @"[c-zA-Z0-9]", "");
		}
	}

	/// <summary>
	/// カーブが行き過ぎたかチェック
	/// </summary>
	void CurveCheck()
	{
		if (this.colorCurve[0].time != 0f) FixedKeyframe(0, 0);
		if (this.colorCurve[this.colorCurve.length - 1].time != 1f) FixedKeyframe(1, 1);
	}

	/// <summary>
	/// 指定したキーフレームをある数値で拘束する
	/// </summary>
	/// <param name="num">対象のキーフレーム番号</param>
	/// <param name="time">拘束したい時間</param>
	void FixedKeyframe(int num, float time)
	{
		Keyframe c = this.colorCurve[num];
		this.colorCurve.MoveKey(num, new Keyframe(time, c.value, c.inTangent, c.outTangent));
	}

	/// <summary>
	/// フラクタルを生成するところ
	/// </summary>
	void Generate()
	{
		long now = DateTime.Now.Ticks;
		{
			// 作業フォルダの生成
			string path = "Assets/Lyapunov_" + this.text + "/";
			if (!Directory.Exists(path))
				AssetDatabase.CreateFolder("Assets", "Lyapunov_" + this.text);

			// リアプノフの生成
			bool concave = this.unevenType == UnevenType.Concave ? true : false;
			LyapunovGenerator gen = new LyapunovGenerator(TextReplacement(), (uint)this.width, (uint)this.depth, (uint)this.repeatTimes, concave);

			// テクスチャの生成
			Texture2D tex = TextureGenerator.CreateTexture(gen, this.maxColor, this.minColor, this.colorCurve);
			AssetDatabase.CreateAsset(tex, path + this.text + "_tex.asset");
			if (this.enableBitmap)
				CreateBitmap(tex, path + this.text + "_bitmap.bmp");

			// 高さマップ
			Texture2D heightmap = null;
			if (this.enableHeightMap)
			{
				heightmap = TextureGenerator.CreateHeightMap(gen, colorCurve);
				AssetDatabase.CreateAsset(heightmap, path + this.text + "_height.asset");
			}

			// マテリアルの生成
			Material mat = SetupMaterial(path, tex);

			// テクスチャ以外の場合のみ生成する
			if (this.terrainType != TerrainType.Texture)
			{
				// ゲームオブジェクトを生成
				SetupPrefab(path, gen, mat, tex, heightmap);
			}
		}
		long end = DateTime.Now.Ticks;
		Debug.Log("milli sec:" + (float)(end - now) / 10000f);
	}

	private void CreateBitmap(Texture2D tex, string path)
	{
		Bitmap.Bitmap bmp = new Bitmap.Bitmap(path, (uint)tex.width, (uint)tex.height, tex.GetPixels());
		bmp.Write();
	}

	/// <summary>
	/// テキストをabの列に変換する, 基本的に文字列の1ビット目を利用
	/// </summary>
	/// <returns>文字列ab</returns>
	private string TextReplacement()
	{
		string result = "";
		foreach (var c in this.text)
			result += (c & 1) == 1 ? "a" : "b";
		return result;
	}

	/// <summary>
	/// マテリアルの設定＋生成
	/// </summary>
	/// <param name="path">パス</param>
	/// <param name="tex">テクスチャ</param>
	/// <returns>設定したマテリアル</returns>
	Material SetupMaterial(string path, Texture2D tex)
	{
		Material mat = new Material(Shader.Find("Diffuse"));
		mat.mainTexture = tex;
		mat.color = Color.white;
		AssetDatabase.CreateAsset(mat, path + this.text + "_mat.asset");
		return mat;
	}

	/// <summary>
	/// プレハブを設定する
	/// </summary>
	/// <param name="path">パス</param>
	/// <param name="gen">作ったリアプノフ</param>
	/// <param name="mat">マテリアル</param>
	/// <param name="hmap">高さマップ</param>
	void SetupPrefab(string path, LyapunovGenerator gen, Material mat, Texture2D tex, Texture2D hmap)
	{
		var prefab = EditorUtility.CreateEmptyPrefab(path + this.text + ".prefab");
		GameObject obj = null;
		TerrainSetup setup = new TerrainSetup(
					this.text, gen, mat, tex, hmap,
					this.width, this.height, this.depth, this.magnitude,
					this.maxColor, this.minColor, this.colorCurve);

		// メッシュの生成
		switch (this.terrainType)
		{
			case TerrainType.Plane:	// 板の生成
				obj = TerrainGenerator.CreatePlane(ref setup, ref path);
				break;

			case TerrainType.Box:	// 箱の生成
				mat.mainTexture = null;
				obj = TerrainGenerator.CreateBoxies(ref setup);
				break;

			case TerrainType.TemplateBox:	// 工夫のない箱の生成
				obj = TerrainGenerator.CreateTemplateBox(ref setup);
				break;

			case TerrainType.Hexagon:	// 六角柱
				obj = TerrainGenerator.CreateHexagon(ref setup);
				break;

			case TerrainType.TemplateHexagon:	// 六角柱テンプレ
				obj = TerrainGenerator.CreateTemplateHexagon(ref setup);
				break;

			default:
				break;
		}
		
		EditorUtility.ReplacePrefab(obj, prefab);	// プレハブを作成
	}
}

#endif
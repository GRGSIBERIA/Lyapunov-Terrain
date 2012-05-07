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
	/// TerrainGeneratorのセットアップ用
	/// </summary>
	public struct TerrainSetup
	{
		/// <summary>
		/// 生成した文字
		/// </summary>
		public string text;

		/// <summary>
		/// 作ったリアプノフ
		/// </summary>
		public LyapunovGenerator gen;

		/// <summary>
		/// 作ったマテリアル
		/// </summary>
		public Material material;

		/// <summary>
		/// 作ったテクスチャ
		/// </summary>
		public Texture2D texture;

		/// <summary>
		/// 高さマップ
		/// </summary>
		public Texture2D heightMap;

		/// <summary>
		/// 縦幅
		/// </summary>
		public int width;

		/// <summary>
		/// 高さ
		/// </summary>
		public int height;

		/// <summary>
		/// 横幅
		/// </summary>
		public int depth;

		/// <summary>
		/// Y軸位置の倍率, リアプノフ指数に何倍するか
		/// </summary>
		public float positionMagnitude;

		/// <summary>
		/// 色の最大値
		/// </summary>
		public Color maxColor;

		/// <summary>
		/// 色の最小値
		/// </summary>
		public Color minColor;

		/// <summary>
		/// 色のカーブ
		/// </summary>
		public AnimationCurve colorCurve;

		/// <summary>
		/// TerrainGeneratorの設定用構造体
		/// </summary>
		/// <param name="text">生成文字列</param>
		/// <param name="gen">作ったリアプノフ</param>
		/// <param name="mat">マテリアル</param>
		/// <param name="tex">テクスチャ</param>
		/// <param name="width">横幅</param>
		/// <param name="height">高さ</param>
		/// <param name="depth">縦幅</param>
		/// <param name="magnitude">Y軸位置の倍率</param>
		/// <param name="maxColor">色の最大値</param>
		/// <param name="minColor">色の最小値</param>
		/// <param name="curve">色の曲線</param>
		public TerrainSetup(string text, LyapunovGenerator gen, Material mat, Texture2D tex, Texture2D heightMap, int width, int height, int depth, float magnitude, Color maxColor, Color minColor, AnimationCurve curve)
		{
			this.text = text;
			this.gen = gen;
			this.material = mat;
			this.texture = tex;
			this.heightMap = heightMap;
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.positionMagnitude = magnitude;
			this.maxColor = maxColor;
			this.minColor = minColor;
			this.colorCurve = curve;
		}
	}

	/// <summary>
	/// 地形を構成するための生成クラス
	/// </summary>
	public class TerrainGenerator
	{
		/// <summary>
		/// 箱を並べた奴を生成
		/// </summary>
		/// <param name="setup">設定用構造体</param>
		/// <returns>生成できたGameobject</returns>
		public static GameObject CreateBoxies(ref TerrainSetup setup)
		{
			Terrain.Box box = new Terrain.Box(ref setup);
			return box.Create();
		}

		/// <summary>
		/// 平板な板を生成
		/// </summary>
		/// <param name="setup">設定</param>
		/// <param name="path">パス</param>
		/// <returns>ゲームオブジェクト</returns>
		public static GameObject CreatePlane(ref TerrainSetup setup, ref string path)
		{
			var obj = new GameObject(setup.text);
			var filter = obj.AddComponent<MeshFilter>();
			var renderer = obj.AddComponent<MeshRenderer>();

			// メッシュの生成
			MeshGenerator mgen = new MeshGenerator(setup.gen, MeshGenerator.MeshType.Plane, setup.positionMagnitude);
			AssetDatabase.CreateAsset(mgen.Mesh, path + setup.text + ".asset");
			filter.mesh = mgen.Mesh;	// メッシュの設定
			renderer.sharedMaterial = setup.material;
			return obj;
		}

		/// <summary>
		/// 何も工夫のない箱を生成
		/// </summary>
		/// <param name="setup">設定</param>
		/// <returns>完成した奴</returns>
		public static GameObject CreateTemplateBox(ref TerrainSetup setup)
		{
			Terrain.TemplateBox temp = new Terrain.TemplateBox(ref setup);
			return temp.Create();
		}

		/// <summary>
		/// 六角柱を生成
		/// </summary>
		/// <param name="setup">設定</param>
		/// <returns>完成した奴</returns>
		public static GameObject CreateHexagon(ref TerrainSetup setup)
		{
			Terrain.Hexagon hex = new Terrain.Hexagon(ref setup);
			return hex.Create();
		}

		/// <summary>
		/// 六角柱のテンプレを生成
		/// </summary>
		/// <param name="setup">設定</param>
		/// <returns>完成した奴</returns>
		public static GameObject CreateTemplateHexagon(ref TerrainSetup setup)
		{
			Terrain.TemplateHexagonBox temp = new Terrain.TemplateHexagonBox(ref setup);
			return temp.Create();
		}
	}
}

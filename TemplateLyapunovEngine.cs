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

/// <summary>
/// 全部真っ白何もないけど、テクスチャや高さマップで動的に変更を加えられるテンプレ
/// </summary>
public class TemplateLyapunovEngine : MonoBehaviour
{
	/// <summary>
	/// 横幅
	/// </summary>
	public int width;

	/// <summary>
	/// 縦の長さ
	/// </summary>
	public int height;

	/// <summary>
	/// 奥行
	/// </summary>
	public int depth;

	/// <summary>
	/// 高さの倍率
	/// </summary>
	public float heightMagnitude;

	/// <summary>
	/// 生成したリアプノフのテクスチャ
	/// </summary>
	public Texture2D lyapunovTexture;

	/// <summary>
	/// 高さマップ
	/// </summary>
	public Texture2D heightMap;
	
	/// <summary>
	/// 生成された時に初期化する, 非推奨
	/// </summary>
	public bool startInitializable = false;

	/// <summary>
	/// transform.parentから参照の配列に直したもの
	/// </summary>
	GameObject[,] objectIndices;

	/// <summary>
	/// 変化する時間
	/// </summary>
	float changeTime = 0;

	/// <summary>
	/// 変化する時間で使う
	/// </summary>
	float nowTime = 0;

	/// <summary>
	/// 変化してるかどうか
	/// </summary>
	bool isChanging = true;

	/// <summary>
	/// テクスチャの高さの差分
	/// </summary>
	float[,] textureHeightDifference;

	/// <summary>
	/// テクスチャの色の差分
	/// </summary>
	Color[,] textureColorDifference;

	/// <summary>
	/// 変更を加える前の高さ
	/// </summary>
	float[,] prevHeight;

	/// <summary>
	/// 変更を加える前の色
	/// </summary>
	Color[,] prevColor;

	void Start()
	{
		if (this.startInitializable) Initialize();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	void Initialize()
	{
		if (this.width != this.lyapunovTexture.width ||
			this.depth != this.lyapunovTexture.height)
			throw new UnityException("テクスチャの縦幅もしくは横幅がボックスの数と合ってません");

		InitObjectIndices();
		InitProperty();
	}

	/// <summary>
	/// 初期化
	/// これを外部から呼び出さないと初期化されない
	/// </summary>
	public void Initialize(int width, int height, int depth, float heightMagnitude, Texture2D lyapunovTexture, Texture2D heightMap)
	{
		this.width = width;
		this.height = height;
		this.depth = depth;
		this.heightMagnitude = heightMagnitude;
		this.lyapunovTexture = lyapunovTexture;
		this.heightMap = heightMap;

		this.textureColorDifference = new Color[this.depth, this.width];
		this.textureHeightDifference = new float[this.depth, this.width];

		Initialize();
	}

	/// <summary>
	/// 色や位置、スケールの初期化
	/// </summary>
	void InitProperty()
	{
		Shader shader = Shader.Find("Diffuse");
		Color[] heightColor = this.heightMap.GetPixels();
		Color[] color = this.lyapunovTexture.GetPixels();

		for (int i = 0; i < this.width; i++)
		{
			for (int j = 0; j < this.depth; j++)
			{
				GameObject obj = this.objectIndices[i, j];

				// 色の決定
				obj.renderer.sharedMaterial = new Material(shader);
				obj.renderer.sharedMaterial.color = color[i * this.width + j];
				
				// 高さの決定
				obj.transform.localScale = new Vector3(
					obj.transform.localScale.x, this.height, obj.transform.localScale.z);
				
				// 位置の決定, グレスケなのでrから取ってもいい
				obj.transform.position = new Vector3(
					obj.transform.position.x,
					heightColor[i * this.width + j].r * this.heightMagnitude, 
					obj.transform.position.z);
			}
		}

		// ここで利用しているテクスチャをnullにして使わないようにする
		//this.heightMap = null;
		//this.lyapunovTexture = null;
	}

	/// <summary>
	/// objectIndicesの初期化
	/// GameObjectの参照を投げる
	/// </summary>
	void InitObjectIndices()
	{
		this.objectIndices = new GameObject[this.width, this.depth];
		for (int i = 0; i < this.width; i++)
		{
			Transform line = transform.GetChild(i);
			for (int j = 0; j < this.depth; j++)
			{
				// ここで要素を代入していく
				Transform elem = line.transform.GetChild(j);
				this.objectIndices[i, j] = elem.gameObject;
			}
		}
	}

	void Update()
	{
		if (this.isChanging)
			ChangeHeightProccess();
	}

	/// <summary>
	/// 高さの変化を行うための処理
	/// </summary>
	void ChangeHeightProccess()
	{
		if (this.nowTime < this.changeTime)
		{
			var v = new Vector3();	// スタックさせない

			// ここで色と高さを変化させる
			for (int d = 0; d < this.depth; d++)
			{
				for (int w = 0; w < this.width; w++)
				{
					// 差分値と現在の時間を掛けることで、時間に対応したベクトルを求められる
					v = this.objectIndices[d, w].transform.position;
					v.y = this.textureHeightDifference[d, w] * this.nowTime + this.prevHeight[d, w];
					this.objectIndices[d, w].transform.position = v;

					this.objectIndices[d, w].renderer.sharedMaterial.color =
						this.textureColorDifference[d, w] * this.nowTime + this.prevColor[d, w];
				}
			}
			this.nowTime += Time.deltaTime;
		}
		else
		{
			// 初期化
			this.nowTime = 0;
			this.changeTime = 0;
			this.isChanging = false;
			this.textureColorDifference = null;
			this.textureHeightDifference = null;
			this.prevColor = null;
			this.prevHeight = null;
		}
	}

	/// <summary>
	/// 指定したカラムのオブジェクトを取得する
	/// </summary>
	/// <param name="i">横</param>
	/// <param name="j">縦</param>
	/// <returns>GameObject</returns>
	public GameObject GetObject(int i, int j)
	{
		return this.objectIndices[i, j];
	}

	/// <summary>
	/// 指定した位置からインデックスを推定してオブジェクトを返す
	/// </summary>
	/// <param name="pos">位置</param>
	/// <returns>GameObject</returns>
	public GameObject GetObject(Vector3 pos)
	{
		pos -= transform.position;		// ローカル座標へ戻す

		// スケールを考慮しながらインデックスに変える
		int i = (int)Mathf.Abs(pos.x * transform.localScale.x);
		int j = (int)Mathf.Abs(pos.z * transform.localScale.z);

		return GetObject(i, j);
	}

	/// <summary>
	/// 指定したインデックスのオブジェクトにRigidbodyを付加する
	/// </summary>
	/// <param name="i">i</param>
	/// <param name="j">j</param>
	/// <returns>付加されたRigidbody</returns>
	public Rigidbody GiveRigidbody(int i, int j)
	{
		return this.objectIndices[i, j].AddComponent<Rigidbody>();
	}

	/// <summary>
	/// 指定した絶対座標からオブジェクトを探してRigidbodyを付加する
	/// </summary>
	/// <param name="pos">座標</param>
	/// <returns>付加されたRigidbody</returns>
	public Rigidbody GiveRigidbody(Vector3 pos)
	{
		return GetObject(pos).AddComponent<Rigidbody>();
	}

	/// <summary>
	/// テクスチャを変えて高さを変化させる
	/// </summary>
	/// <param name="texture">テクスチャ</param>
	/// <param name="heightmap">高さマップ</param>
	/// <param name="changeTime">完全に変化させるのに要する時間</param>
	public void ChangeHeightFromTextures(Texture2D texture, Texture2D heightmap, float changeTime)
	{
		var color = texture.GetPixels();
		var heights = heightmap.GetPixels();
		int buf = 0;
		float changeDiff = 1 / changeTime;
		this.prevHeight = new float[this.depth, this.height];
		this.prevColor = new Color[this.depth, this.height];

		for (int d = 0; d < this.depth; d++)
		{
			buf = d * this.depth;
			for (int w = 0; w < this.width; w++)
			{
				// ここで差分を求めてる
				// 時間で割るのはdeltaTime対策, 元の数値にdeltaTimeを掛ける
				this.textureColorDifference[d, w] = 
					(color[buf + w] - this.objectIndices[d, w].renderer.sharedMaterial.color) * changeTime;
				this.textureColorDifference[d, w].a = 0;
				this.textureHeightDifference[d, w] = 
					(heights[buf + w].r - this.objectIndices[d, w].transform.position.y) * changeDiff;

				// ここで前の状態を保存している
				this.prevColor[d, w] = this.objectIndices[d, w].renderer.sharedMaterial.color;
				this.prevHeight[d, w] = this.objectIndices[d, w].transform.position.y;
			}
		}

		this.lyapunovTexture = texture;
		this.heightMap = heightmap;
		this.changeTime = changeTime;
		this.isChanging = true;
	}
}

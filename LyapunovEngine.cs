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

public class LyapunovEngine : MonoBehaviour
{
	/// <summary>
	/// 色用のテクスチャ
	/// </summary>
	public Texture2D lyapunovTexture;

	void Start()
	{
		SetMaterialColor();
	}

	/// <summary>
	/// マテリアルの色をなんとかする
	/// </summary>
	void SetMaterialColor()
	{
		for (int i = 0; i < this.lyapunovTexture.width; i++)
		{
			Transform line = transform.GetChild(i);
			for (int j = 0; j < this.lyapunovTexture.height; j++)
			{
				// 行から要素を抽出
				Transform elem = line.transform.GetChild(j);
				elem.gameObject.renderer.sharedMaterial.color =
					this.lyapunovTexture.GetPixel(i, j);
			}
		}
	}
}

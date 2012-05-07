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

namespace LyapunovTerrain.Terrain
{
	public class TemplateAbs : TerrainAbs
	{
		

		public TemplateAbs(ref TerrainSetup setup)
			: base(ref setup)
		{

		}

		/// <summary>
		/// TemplateLyapunovEngineを設定するときに使う
		/// 初期化用の関数
		/// </summary>
		/// <param name="master">マスター</param>
		protected void InitTemplate(GameObject master)
		{
			var f = master.AddComponent<TemplateLyapunovEngine>();
			f.width = this.setup.width;
			f.depth = this.setup.depth;
			f.height = this.setup.height;
			f.heightMagnitude = this.setup.positionMagnitude;
			f.lyapunovTexture = this.setup.texture;
			f.startInitializable = true;
			f.heightMap = this.setup.heightMap;
		}
	}
}

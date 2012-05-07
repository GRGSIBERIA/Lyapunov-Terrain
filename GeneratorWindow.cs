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
/// �v���O�C���p�̑���\������
/// </summary>
public class GeneratorWindow : EditorWindow
{
	/// <summary>
	/// �����ɗ��p���镶����
	/// </summary>
	string text = "";

	/// <summary>
	/// ������̌J��Ԃ���
	/// </summary>
	int repeatTimes = 1;

	/// <summary>
	/// ��������I�u�W�F�N�g�̉��s
	/// </summary>
	int depth = 512;

	/// <summary>
	/// ��������I�u�W�F�N�g�̕�
	/// </summary>
	int width = 512;

	/// <summary>
	/// �I�u�W�F�N�g�̍���
	/// </summary>
	int height = 0;

	/// <summary>
	/// Y���ʒu�̔{��
	/// </summary>
	float magnitude = 1f;

	/// <summary>
	/// ���ʂ͂ǂ��炩
	/// </summary>
	public enum UnevenType
	{
		/// <summary>
		/// ��
		/// </summary>
		Convex,

		/// <summary>
		/// ��
		/// </summary>
		Concave,
	}

	/// <summary>
	/// ���ʂ̎��
	/// </summary>
	UnevenType unevenType = UnevenType.Convex;

	/// <summary>
	/// ��������I�u�W�F�N�g�̌`
	/// </summary>
	public enum TerrainType
	{
		/// <summary>
		/// �e�N�X�`��
		/// </summary>
		Texture,

		/// <summary>
		/// ���ʒn�`
		/// </summary>
		Plane,

		/// <summary>
		/// ����n�`
		/// </summary>
		Box,

		/// <summary>
		/// �������Ȃ�
		/// </summary>
		TemplateBox,

		/// <summary>
		/// �Z�p��
		/// </summary>
		Hexagon,

		/// <summary>
		/// �Z�p�������Ȃ�
		/// </summary>
		TemplateHexagon,
	}

	/// <summary>
	/// �I�u�W�F�N�g�̌`
	/// </summary>
	TerrainType terrainType = TerrainType.Plane;

	/// <summary>
	/// �ŏ��l�̐F
	/// </summary>
	Color minColor = Color.black;

	/// <summary>
	/// �ő�l�̐F
	/// </summary>
	Color maxColor = Color.blue;

	/// <summary>
	/// �e�N�X�`�������Ƃ��ɗ��p����
	/// �F�̃J�[�u
	/// </summary>
	AnimationCurve colorCurve = AnimationCurve.Linear(0, 0, 1, 1);

	/// <summary>
	/// �����}�b�v����邩�ǂ���
	/// </summary>
	bool enableHeightMap;

	/// <summary>
	/// �r�b�g�}�b�v�摜�𐶐����邩
	/// </summary>
	bool enableBitmap;

	/// <summary>
	/// �{�^���Ȃǂ̍���
	/// </summary>
	const int fieldHeight = 20;

	/// <summary>
	/// �{�^�����m�̌���
	/// </summary>
	const int fieldSpace = 2;

	/// <summary>
	/// �ʒuY
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
			this.depth = size;	// �����Ɏ��s���Ă��܂��̂ŃT�C�Y�͗��҂Ƃ������ɂ��Ă���

			// �����̎w��
			if (this.terrainType != TerrainType.Plane && this.terrainType != TerrainType.Texture)
			{
				this.height = EditorGUI.IntField(SetRect(ref cnt), "height", this.height);
			}

			// �{���ݒ�
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

			// heightmap�̐ݒ�
			if (this.terrainType != TerrainType.TemplateBox && this.terrainType != TerrainType.TemplateHexagon)
				this.enableHeightMap = EditorGUI.Toggle(SetRect(ref cnt), "height map", this.enableHeightMap);
			else
				this.enableHeightMap = true;	// �e���v���[�g�̏ꍇ�͋����I�ɐ���

			this.minColor = EditorGUI.ColorField(SetRect(ref cnt), "min color", this.minColor);
			this.maxColor = EditorGUI.ColorField(SetRect(ref cnt), "max color", this.maxColor);
			this.colorCurve = EditorGUI.CurveField(SetRect(ref cnt), "color curve", this.colorCurve);

			Rect[] btColorRect = SetTwoRect(ref cnt);
			if (GUI.Button(btColorRect[0], "Add")) AddKeyframe();
			if (GUI.Button(btColorRect[1], "Reduce")) ReduceKeyframe();

			NumericalCheck();
			CurveCheck();

			// �t���N�^���𐶐�
			if (GUI.Button(SetRect(ref cnt), "Generate")) Generate();
		}
		
		Repaint();
	}

	/// <summary>
	/// �L�[�t���[����ǉ�
	/// </summary>
	void AddKeyframe()
	{
		const float val = 0.5f;
		const float intan = 0f;
		const float outan = 0f;
		int cnt = this.colorCurve.length - 1;	// �n�_�ƏI�_�������Ă�
		float diff = 1f / this.colorCurve.length;
		
		this.colorCurve.AddKey(new Keyframe(0.00001f, 0, 0, 0));

		// �L�[�t���̈ʒu�𒲐�
		for (int i = 0; i < cnt; i++)
		{
			float time = (i + 1) * diff;
			this.colorCurve.MoveKey(i + 1, new Keyframe(time, val, intan, outan));
		}
		this.colorCurve.MoveKey(this.colorCurve.length - 1, new Keyframe(1f, 1f, intan, outan));
	}

	/// <summary>
	/// �L�[�t���[�����폜
	/// </summary>
	void ReduceKeyframe()
	{
		this.colorCurve = AnimationCurve.Linear(0, 0, 1, 1);
	}

	/// <summary>
	/// �`��͈͂����
	/// </summary>
	/// <param name="cnt">�J�E���g</param>
	/// <returns>��`</returns>
	Rect SetRect(ref int cnt)
	{
		int fieldWidth = (int)position.width - 20;
		return new Rect(2, 2 + positionY * cnt++, fieldWidth, fieldHeight);
	}

	/// <summary>
	/// ���ɓ���炢���`��͈͂����
	/// </summary>
	/// <param name="cnt">�J�E���g</param>
	/// <returns>��`</returns>
	Rect[] SetTwoRect(ref int cnt)
	{
		Rect[] rect = new Rect[2];
		int fieldWidth = (int)position.width - 20;

		rect[0] = new Rect(2, 2 + positionY * cnt, fieldWidth / 2, fieldHeight);
		rect[1] = new Rect(fieldWidth / 2, 2 + positionY * cnt++, fieldWidth / 2, fieldHeight);
		return rect;
	}

	/// <summary>
	/// ���l���s���߂�����s������Ȃ����`�F�b�N
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
	/// ������̃`�F�b�N
	/// </summary>
	void TextCheck()
	{
		// a��������b�łȂ���ΊY�������������I�ɍ폜����
		if (this.text.Length > 0)
		{
			Regex.Replace(this.text, @"[c-zA-Z0-9]", "");
		}
	}

	/// <summary>
	/// �J�[�u���s���߂������`�F�b�N
	/// </summary>
	void CurveCheck()
	{
		if (this.colorCurve[0].time != 0f) FixedKeyframe(0, 0);
		if (this.colorCurve[this.colorCurve.length - 1].time != 1f) FixedKeyframe(1, 1);
	}

	/// <summary>
	/// �w�肵���L�[�t���[�������鐔�l�ōS������
	/// </summary>
	/// <param name="num">�Ώۂ̃L�[�t���[���ԍ�</param>
	/// <param name="time">�S������������</param>
	void FixedKeyframe(int num, float time)
	{
		Keyframe c = this.colorCurve[num];
		this.colorCurve.MoveKey(num, new Keyframe(time, c.value, c.inTangent, c.outTangent));
	}

	/// <summary>
	/// �t���N�^���𐶐�����Ƃ���
	/// </summary>
	void Generate()
	{
		long now = DateTime.Now.Ticks;
		{
			// ��ƃt�H���_�̐���
			string path = "Assets/Lyapunov_" + this.text + "/";
			if (!Directory.Exists(path))
				AssetDatabase.CreateFolder("Assets", "Lyapunov_" + this.text);

			// ���A�v�m�t�̐���
			bool concave = this.unevenType == UnevenType.Concave ? true : false;
			LyapunovGenerator gen = new LyapunovGenerator(TextReplacement(), (uint)this.width, (uint)this.depth, (uint)this.repeatTimes, concave);

			// �e�N�X�`���̐���
			Texture2D tex = TextureGenerator.CreateTexture(gen, this.maxColor, this.minColor, this.colorCurve);
			AssetDatabase.CreateAsset(tex, path + this.text + "_tex.asset");
			if (this.enableBitmap)
				CreateBitmap(tex, path + this.text + "_bitmap.bmp");

			// �����}�b�v
			Texture2D heightmap = null;
			if (this.enableHeightMap)
			{
				heightmap = TextureGenerator.CreateHeightMap(gen, colorCurve);
				AssetDatabase.CreateAsset(heightmap, path + this.text + "_height.asset");
			}

			// �}�e���A���̐���
			Material mat = SetupMaterial(path, tex);

			// �e�N�X�`���ȊO�̏ꍇ�̂ݐ�������
			if (this.terrainType != TerrainType.Texture)
			{
				// �Q�[���I�u�W�F�N�g�𐶐�
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
	/// �e�L�X�g��ab�̗�ɕϊ�����, ��{�I�ɕ������1�r�b�g�ڂ𗘗p
	/// </summary>
	/// <returns>������ab</returns>
	private string TextReplacement()
	{
		string result = "";
		foreach (var c in this.text)
			result += (c & 1) == 1 ? "a" : "b";
		return result;
	}

	/// <summary>
	/// �}�e���A���̐ݒ�{����
	/// </summary>
	/// <param name="path">�p�X</param>
	/// <param name="tex">�e�N�X�`��</param>
	/// <returns>�ݒ肵���}�e���A��</returns>
	Material SetupMaterial(string path, Texture2D tex)
	{
		Material mat = new Material(Shader.Find("Diffuse"));
		mat.mainTexture = tex;
		mat.color = Color.white;
		AssetDatabase.CreateAsset(mat, path + this.text + "_mat.asset");
		return mat;
	}

	/// <summary>
	/// �v���n�u��ݒ肷��
	/// </summary>
	/// <param name="path">�p�X</param>
	/// <param name="gen">��������A�v�m�t</param>
	/// <param name="mat">�}�e���A��</param>
	/// <param name="hmap">�����}�b�v</param>
	void SetupPrefab(string path, LyapunovGenerator gen, Material mat, Texture2D tex, Texture2D hmap)
	{
		var prefab = EditorUtility.CreateEmptyPrefab(path + this.text + ".prefab");
		GameObject obj = null;
		TerrainSetup setup = new TerrainSetup(
					this.text, gen, mat, tex, hmap,
					this.width, this.height, this.depth, this.magnitude,
					this.maxColor, this.minColor, this.colorCurve);

		// ���b�V���̐���
		switch (this.terrainType)
		{
			case TerrainType.Plane:	// �̐���
				obj = TerrainGenerator.CreatePlane(ref setup, ref path);
				break;

			case TerrainType.Box:	// ���̐���
				mat.mainTexture = null;
				obj = TerrainGenerator.CreateBoxies(ref setup);
				break;

			case TerrainType.TemplateBox:	// �H�v�̂Ȃ����̐���
				obj = TerrainGenerator.CreateTemplateBox(ref setup);
				break;

			case TerrainType.Hexagon:	// �Z�p��
				obj = TerrainGenerator.CreateHexagon(ref setup);
				break;

			case TerrainType.TemplateHexagon:	// �Z�p���e���v��
				obj = TerrainGenerator.CreateTemplateHexagon(ref setup);
				break;

			default:
				break;
		}
		
		EditorUtility.ReplacePrefab(obj, prefab);	// �v���n�u���쐬
	}
}

#endif
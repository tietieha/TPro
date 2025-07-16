using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimInstanceShaderGUI : ShaderGUI
{
	private MaterialProperty _AnimInstanceOnProp;
	private MaterialProperty _AnimMapProp;
	private MaterialProperty _AnimLenProp;
	
	MaterialEditor m_MaterialEditor;
	bool animInstanceOn;
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		m_MaterialEditor = materialEditor;
		Material material = materialEditor.target as Material;
		_AnimInstanceOnProp = FindProperty("_AnimInstanceOn", properties, false);
		_AnimMapProp = FindProperty("_AnimMap", properties, false);
		_AnimLenProp = FindProperty("_AnimLen", properties, false);

		if (_AnimInstanceOnProp != null)
		{
			EditorGUI.BeginChangeCheck();
			{
				animInstanceOn = _AnimInstanceOnProp.floatValue > 0;
				animInstanceOn = EditorGUILayout.Toggle("开启动画Instance", animInstanceOn);
				if (_AnimInstanceOnProp.floatValue > 0)
				{
					if (_AnimMapProp != null)
						m_MaterialEditor.TextureProperty(_AnimMapProp, "动画贴图");
					if (_AnimLenProp != null)
						m_MaterialEditor.FloatProperty(_AnimLenProp, "动画时长(s)");
				}
			}

			if (EditorGUI.EndChangeCheck())
			{
				_AnimInstanceOnProp.floatValue = animInstanceOn ? 1 : 0;
				bool enable = animInstanceOn && 
				              _AnimMapProp != null &&
				              _AnimMapProp.textureValue != null;
				SetKeyword(material, "_ANIMINSTANCE_ON", enable);
				material.enableInstancing = enable;
			}
		}

		base.OnGUI(materialEditor, properties);
	}
	
	static void SetKeyword(Material m, string keyword, bool state)
	{
		if (state)
			m.EnableKeyword(keyword);
		else
			m.DisableKeyword(keyword);
	}
}

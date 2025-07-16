// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-09-21 18:45 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using UnityEditor;
using UnityEngine;

public class WorldTerrainShaderGUI : ShaderGUI
{
	const string addpass = "_ADDPASS_ON";
	private Material _material;
	private MaterialProperty _control1;
	
	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		_material = (Material) materialEditor.target;
		_control1 = FindProperty("_Control1", properties, false);
		bool enabled = _material.IsKeywordEnabled(addpass);
		bool hasControl1 = _control1 != null && _control1.textureValue != null;
		string hint = hasControl1 ? "是" : "否";
		EditorGUILayout.LabelField($"是否超过四层： {hint}");

		if (hasControl1 != enabled)
		{
			if (hasControl1)
				_material.EnableKeyword(addpass);
			else
				_material.DisableKeyword(addpass);
		}
		

		base.OnGUI(materialEditor, properties);
	}
}

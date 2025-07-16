#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GEgineRunTime
{
	public class WorldTerrainUVScaleScriptableObject : ScriptableObject
	{
		public Vector4[] uvs = new Vector4[16];
		public float lowUVScale = 100;
		public Vector4 LightDir = Vector4.one;
#if UNITY_EDITOR
		public void Save()
		{
			EditorUtility.SetDirty(this);
		}
#endif
	}
}

using UnityEditor;
using UnityEngine;

namespace UWParticleSystemProfiler
{
	/// <summary>
	/// Inspector
	/// </summary>
	[CustomEditor(typeof(UWParticleEffectProfiler))]
	public class UWParticleEffectProfilerInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			UWParticleEffectProfiler uwParticleEffectProfiler = (UWParticleEffectProfiler) target;

			string autoCullingTips = GetParticleEffectData.GetCullingSupportedString(uwParticleEffectProfiler.gameObject);
			if (!string.IsNullOrEmpty(autoCullingTips))
			{
				GUILayout.Label("ParticleSystem以下选项会导致无法自动剔除：", EditorStyles.whiteLargeLabel);
				GUILayout.Label(autoCullingTips);
			}
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class URPAssetSetting : MonoBehaviour
{
	[SerializeField] private float shadowDistance = 12;

	[Header("级联阴影:不用调这里")]
	[SerializeField][ReadOnly] private int shadowCascadeCount = 1;
	[SerializeField][ReadOnly] private float cascadeBorder = 1.8f;
	[SerializeField][ReadOnly] private float cascade2Split = 3f;
	[SerializeField][ReadOnly] private Vector2 cascade3Split = new Vector2(0.864f, 4.164f);
	[SerializeField][ReadOnly] private Vector3 cascade4Split = new Vector3(0.804f, 2.4f, 5.604f);


	private float lastShadowDistance;
	private int lastShadowCascadeCount;
	private float lastShadowCascadeBorder;
	private float lastShadowCascade2Split;
	private Vector2 lastShadowCascade3Split;
	private Vector3 lastShadowCascade4Split;

	public float ShadowDistance
	{
		get => shadowDistance;
		set
		{
			if (Mathf.Approximately(shadowDistance, value))
				return;
			shadowDistance = value;
			RefreshUrpAsset();
		}
	}

#if UNITY_EDITOR
	[Button("保存当前阴影设置", ButtonSizes.Large)]
	private void ReadCurrentSetting()
	{
		var rpa = (UniversalRenderPipelineAsset) GraphicsSettings.renderPipelineAsset;
		if (rpa == null)
			return;
		shadowDistance = rpa.shadowDistance;
		shadowCascadeCount = rpa.shadowCascadeCount;
		cascadeBorder = rpa.cascadeBorder;
		cascade2Split = rpa.cascade2Split;
		cascade3Split = rpa.cascade3Split;
		cascade4Split = rpa.cascade4Split;

	}

#endif

	[SerializeField] private bool _shadowEnable = true;
	[SerializeField] private List<Transform> _shadowControlTrans;
	[ReadOnly] [ShowInInspector] private List<Material> _materials;

	private void Awake()
	{
		_materials = new List<Material>();
	}

	private void OnEnable()
	{
		RefreshUrpAsset();

		foreach (var trans in _shadowControlTrans)
		{
			if (trans == null)
				continue;
			var renders = new List<Renderer>();
			trans.GetComponentsInChildren<Renderer>(true, renders);
			foreach (var render in renders)
			{
				if (!_materials.Contains(render.sharedMaterial))
				{
					_materials.Add(render.sharedMaterial);
				}
			}
		}
	}

	private void OnDisable()
	{
		RecoveryUrpAsset();
	}

	private void RefreshUrpAsset()
	{
		var rpa = (UniversalRenderPipelineAsset) GraphicsSettings.renderPipelineAsset;
		if (rpa == null)
			return;

		lastShadowDistance = rpa.shadowDistance;
		lastShadowCascadeCount = rpa.shadowCascadeCount;
		lastShadowCascadeBorder = rpa.cascadeBorder;
		lastShadowCascade2Split = rpa.cascade2Split;
		lastShadowCascade3Split = rpa.cascade3Split;
		lastShadowCascade4Split = rpa.cascade4Split;

		rpa.shadowDistance = shadowDistance;
		rpa.shadowCascadeCount = shadowCascadeCount;
		rpa.cascadeBorder = cascadeBorder;
		rpa.cascade2Split = cascade2Split;
		rpa.cascade3Split = cascade3Split;
		rpa.cascade4Split = cascade4Split;
	}

	private void RecoveryUrpAsset()
	{
		var rpa = (UniversalRenderPipelineAsset) GraphicsSettings.renderPipelineAsset;
		if (rpa == null)
			return;

		rpa.shadowDistance = lastShadowDistance;
		rpa.shadowCascadeCount = lastShadowCascadeCount;
		rpa.cascadeBorder = lastShadowCascadeBorder;
		rpa.cascade2Split = lastShadowCascade2Split;
		rpa.cascade3Split = lastShadowCascade3Split;
		rpa.cascade4Split = lastShadowCascade4Split;
	}

	public void SetShadow(bool enable)
	{
		_shadowEnable = enable;
		if (_materials == null)
			return;
		foreach (var mat in _materials)
		{
			mat.SetShaderPassEnabled("ShadowCaster", _shadowEnable);
		}
	}

	public void SwitchShadow()
	{
		_shadowEnable = !_shadowEnable;
		SetShadow(_shadowEnable);
	}

	private void OnValidate()
	{
		SetShadow(_shadowEnable);
	}
}

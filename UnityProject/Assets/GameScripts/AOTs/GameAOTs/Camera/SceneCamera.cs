using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RenderFeature;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Camera))]
public class SceneCamera : MonoBehaviour
{
	public class CameraViewType_EnumComparer : IEqualityComparer<UniversalAdditionalCameraData.CameraViewType>
	{
		public bool Equals(UniversalAdditionalCameraData.CameraViewType x, UniversalAdditionalCameraData.CameraViewType y)
		{
			return x == y;
		}

		public int GetHashCode(UniversalAdditionalCameraData.CameraViewType x)
		{
			return (int) x;
		}
	}

	[SerializeField]
	private UniversalAdditionalCameraData.CameraViewType _cameraViewType = UniversalAdditionalCameraData.CameraViewType.SCENE;
	public UniversalAdditionalCameraData.CameraViewType cameraViewType => _cameraViewType;

	[InfoBox("下面两个开关默认不开启, 有问题找leon")]
	[SerializeField]
	private bool _requiresDepthTexture;
	[SerializeField]
	private bool _requiresColorTexture;

	[Header("是否启用模糊")]
	[SerializeField]
	private bool _isBlur;

	#region private field
	private SceneCameraManager _sceneCameraManager;
	private bool _renderShadowsEnable;
	private bool _postProcessingEnabled;
	#endregion

	[FoldoutGroup("Advance Setting", expanded: false)]
	[SerializeField]
	[InfoBox("角色展示才用, 其他都别开")]
	private bool _forceRenderShadow;
	[FoldoutGroup("Advance Setting")]
	[SerializeField]
	private bool _forcePostProcess;

	private Camera _camera;
	public Camera Camera {
		get
		{
			if (_camera == null)
				_camera = GetComponent<Camera>();
			return _camera;
		}
	}
	private int originalCullingMask;
	private bool isRenderingDisabled = false;

	private UniversalAdditionalCameraData _cameraData;
	public UniversalAdditionalCameraData CameraData
	{
		get
		{
			if (_cameraData == null)
				_cameraData = Camera.GetUniversalAdditionalCameraData();
			return _cameraData;
		}
	}

	private void Awake()
	{
		AwakeAction();
	}

	private void OnEnable()
	{
		OnEnableAction();
	}

	private void OnDisable()
	{
		OnDisableAction();
	}

	protected virtual void AwakeAction()
	{
		_sceneCameraManager = SceneCameraManager.Instance;
		_renderShadowsEnable = CameraData.renderShadows;
		_postProcessingEnabled = CameraData.renderPostProcessing;
		originalCullingMask = Camera.cullingMask;
	}

	protected virtual void OnEnableAction()
	{
		SetGameQuality(_sceneCameraManager.RenderShadowsEnable, _sceneCameraManager.PostProcessingEnabled);
		_sceneCameraManager.EnableSceneCamera(this);

		RefreshBlur();
	}

	protected virtual void OnDisableAction()
	{
		_sceneCameraManager.DisableSceneCamera(this);

		if (BlurRenderFeature.Instance == null)
			return;

		BlurRenderFeature.Instance.RemoveRef(Camera);
	}

	private void RefreshBlur()
	{
		if (BlurRenderFeature.Instance == null)
		{
			return;
		}
		if (_isBlur)
		{
			BlurRenderFeature.Instance.AddRef(Camera);
		}
		else
		{
			BlurRenderFeature.Instance.RemoveRef(Camera);
		}
	}

	public void SetBlur(bool blur)
	{
		if(_isBlur != blur)
		{
			_isBlur = blur;
			RefreshBlur();
		}
	}

	public void SetRenderingEnabled(bool isEnabled)
	{
		if (_camera == null) return;

		if (isEnabled && isRenderingDisabled)
		{
			_camera.cullingMask = originalCullingMask;
			isRenderingDisabled = false;
		}
		else if (!isEnabled && !isRenderingDisabled)
		{
			_camera.cullingMask = 0;
			isRenderingDisabled = true;
		}
	}

	#region GameQuality
	public void SetGameQuality(bool shadowEnable, bool postprocessEnable)
	{
		CameraData.renderShadows = (_renderShadowsEnable && shadowEnable) || _forceRenderShadow;
		CameraData.renderPostProcessing = (_postProcessingEnabled && postprocessEnable) || _forcePostProcess;
	}
	#endregion

#if UNITY_EDITOR
	private void OnValidate()
	{
		CameraData.cameraViewType = _cameraViewType;
		CameraData.requiresDepthTexture = _requiresDepthTexture;
		CameraData.requiresColorTexture = _requiresColorTexture;

		RefreshBlur();
	}

	[Header("Debug")]
	public bool isDebug;
	// debug camera ground
	private Plane xzPlane = new Plane(Vector3.up, Vector3.zero);

	private Vector2 screenSize;
	private Vector3 righttop, leftbottom, rightbottom, lefttop, center;

	private void OnDrawGizmos()
	{
		if (Camera == null)
			return;

		if (!isDebug)
			return;

		screenSize = GetMainGameViewSize();
		center = GetRaycastXZPlanePoint(new Vector3(screenSize.x / 2f, screenSize.y / 2f, 0));
		righttop = GetRaycastXZPlanePoint(new Vector3(screenSize.x, screenSize.y, 0));
		lefttop = GetRaycastXZPlanePoint(new Vector3(0, screenSize.y, 0));
		leftbottom = GetRaycastXZPlanePoint(new Vector3(0, 0, 0));
		rightbottom = GetRaycastXZPlanePoint(new Vector3(screenSize.x, 0, 0));

		Gizmos.color = Color.red;

		Handles.Label(righttop + Vector3.up * 5, "righttop");
		Gizmos.DrawCube(righttop, Vector3.one);

		Handles.Label(lefttop + Vector3.up * 5, "lefttop");
		Gizmos.DrawCube(lefttop, Vector3.one);

		Handles.Label(leftbottom + Vector3.up * 5, "leftbottom");
		Gizmos.DrawCube(leftbottom, Vector3.one);

		Handles.Label(rightbottom + Vector3.up * 5, "rightbottom");
		Gizmos.DrawCube(rightbottom, Vector3.one);

		var rectrightbottom = new Vector3(righttop.x, 0, rightbottom.z);
		var rectleftbottom = new Vector3(lefttop.x, 0, leftbottom.z);
		Gizmos.DrawLine(lefttop, righttop);
		Gizmos.DrawLine(righttop, rightbottom);
		Gizmos.DrawLine(rightbottom, leftbottom);
		Gizmos.DrawLine(leftbottom, lefttop);
	}

	private Vector3 GetRaycastXZPlanePoint(Vector3 screenPos)
	{
		Vector3 posWorld = Vector3.zero;

		Ray mouseray = Camera.ScreenPointToRay(screenPos);
		if (xzPlane.Raycast(mouseray, out float hitdist))
		{
			// check for the intersection point between ray and plane
			return mouseray.GetPoint(hitdist);
		}

		if (hitdist < -1.0f)
		{
			// when point is "behind" plane (hitdist != zero, fe for far away orthographic camera) simply switch sign https://docs.unity3d.com/ScriptReference/Plane.Raycast.html
			return mouseray.GetPoint(-hitdist);
		}

		return posWorld;
	}

	private Vector2 GetMainGameViewSize()
	{
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView =
			T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
		return (Vector2) Res;
	}
#endif
}
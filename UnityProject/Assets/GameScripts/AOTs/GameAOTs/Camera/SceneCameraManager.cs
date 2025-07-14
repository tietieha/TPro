// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-11-01 14:37 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TEngine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
	using UnityEditor;
	using Sirenix.OdinInspector;
#endif

public class SceneCameraManager : MonoBehaviour
{
	public static SceneCameraManager Instance => _instance;
	public static SceneCameraManager _instance;

	public Camera MainCamera;

	private UniversalAdditionalCameraData _mainCameraData;

	public List<SceneCamera> SceneCameras => _sceneCamerasLst;
#if UNITY_EDITOR
	[ListDrawerSettings(IsReadOnly = true, ShowIndexLabels = true), PropertyOrder(int.MaxValue), OnInspectorGUI]
#endif
	private List<SceneCamera> _sceneCamerasLst = new List<SceneCamera>();

	public bool RenderShadowsEnable { get; private set; } = true;
	public bool PostProcessingEnabled { get; private set; } = true;

	private static bool _isCaptureing;
	private static List<Action<Texture2D>> _captureActions = new List<Action<Texture2D>>(3);

	private void Awake()
	{
		_instance = this;
		MainCamera = GetComponent<Camera>();
		_mainCameraData = MainCamera.GetUniversalAdditionalCameraData();
		_mainCameraData.cameraViewType = UniversalAdditionalCameraData.CameraViewType.MAINCAMERA;
	}

	private void OnApplicationQuit()
	{
	}

	public void EnableSceneCamera(SceneCamera sceneCamera)
	{
		if (sceneCamera.Camera == null)
		{
			Debug.LogError($"scene camera no camera, id:{sceneCamera.cameraViewType}");
			return;
		}

		if (!_sceneCamerasLst.Contains(sceneCamera))
		{
			var cameradata = sceneCamera.CameraData;
			if (cameradata.renderType != CameraRenderType.Overlay)
				cameradata.renderType = CameraRenderType.Overlay;

			_sceneCamerasLst.Add(sceneCamera);
			_sceneCamerasLst = _sceneCamerasLst
				.OrderBy(item => item.cameraViewType)
				.ToList();
			var stack = _sceneCamerasLst
				.Select(item => item.Camera)
				.ToList();

			if (_mainCameraData != null)
			{
				_mainCameraData.cameraStack.Clear();
				_mainCameraData.cameraStack.AddRange(stack);
			}
		}
	}

	public void DisableSceneCamera(SceneCamera sceneCamera)
	{
		if (_sceneCamerasLst.Contains(sceneCamera))
			_sceneCamerasLst.Remove(sceneCamera);

		if (_mainCameraData != null && _mainCameraData.cameraStack.Contains(sceneCamera.Camera))
			_mainCameraData.cameraStack.Remove(sceneCamera.Camera);
	}

	public Camera GetCamera(UniversalAdditionalCameraData.CameraViewType cameraID)
	{
		if (cameraID == UniversalAdditionalCameraData.CameraViewType.MAINCAMERA)
			return MainCamera;
		SceneCamera sceneCamera = null;
		foreach (var item in _sceneCamerasLst)
		{
			if (item.cameraViewType == cameraID)
			{
				sceneCamera = item;
				break;
			}
		}

		if (sceneCamera != null)
			return sceneCamera.Camera;

		return null;
	}

	public void GetScreenCapture(Action<Texture2D> callbalk)
	{
		if (callbalk != null)
			_captureActions.Add(callbalk);

		// 防止同一帧截多张图
		if (_isCaptureing)
			return;

		_isCaptureing = true;
		StartCoroutine(StartScreenCapture());
	}

	public IEnumerator StartScreenCapture()
	{
		yield return new WaitForEndOfFrame();

		Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
		for (int i = 0; i < _captureActions.Count; i++)
		{
			_captureActions[i](tex);
		}
		_captureActions.Clear();
		_isCaptureing = false;
	}

	//tip 调用方使用完后需删除Texture
	public void GetSpriteFromScreenCapture(Action<Sprite> callbalk)
	{
		StartCoroutine(StartSpriteFromScreenCapture(callbalk));
	}

	public IEnumerator StartSpriteFromScreenCapture(Action<Sprite> callbalk)
	{
		yield return new WaitForEndOfFrame();
		Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
		Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f);
		callbalk(sp);
	}

	public static void SetGameQuality(bool shadowEnable, bool postprocessEnable, bool supportsHDR)
	{
		Log.Debug($"[AdapterManager] renderShadow:{shadowEnable} renderPostProcessing:{postprocessEnable}");
		Instance.RenderShadowsEnable = shadowEnable;
		Instance.PostProcessingEnabled = postprocessEnable;

		foreach (var sceneCamera in Instance._sceneCamerasLst)
		{
			sceneCamera.SetGameQuality(shadowEnable, postprocessEnable);
		}

		Shader.EnableKeyword("_HGAME_SHADOW_ON");
		Shader.EnableKeyword("_USE_UNITY_SHADOW");

		var rpAsset = UniversalRenderPipeline.asset;
		rpAsset.supportsHDR = supportsHDR;
	}

	public static void SetSceneCameraVisible(bool isVisible)
	{
		var camera = Instance.GetCamera(UniversalAdditionalCameraData.CameraViewType.SCENE);
		camera.enabled = isVisible;
	}

	public static void SetSceneUICameraVisible(bool isVisible)
	{
		var camera = Instance.GetCamera(UniversalAdditionalCameraData.CameraViewType.SCENE_UI);
		if (camera)
		{
			camera.enabled = isVisible;
		}
	}


#if UNITY_EDITOR
	private void OnEnable()
	{
		SceneView.duringSceneGui += OnSceneGUI;
	}

	private void OnDisable()
	{
		SceneView.duringSceneGui -= OnSceneGUI;
	}

	private void OnSceneGUI(SceneView sceneView)
	{
		if(SceneCameras.Count == 0)
			return;
		// 获取场景视图的中心位置
		Vector2 sceneViewCenter = new Vector2(SceneView.lastActiveSceneView.position.width / 2, 0);

		// 设置 GUI 的起始位置
		Handles.BeginGUI();

		// 计算每个按钮的宽度
		float buttonWidth = 150; // 每个按钮的宽度
		float buttonHeight = 30; // 每个按钮的高度
		float margin = 5; // 按钮之间的间距
		float totalWidth = (buttonWidth + margin * 2) * SceneCameras.Count; // 所有按钮的总宽度

		// 创建一个矩形区域来显示相机的堆栈信息
		Rect rect = new Rect(sceneViewCenter.x - totalWidth / 2, sceneViewCenter.y, totalWidth, buttonHeight + margin * 2);

		// 绘制背景框
		GUI.Box(rect, "Camera Stack");

		// 遍历相机堆栈并显示每个相机的名称（横向排列）
		for (int i = 0; i < SceneCameras.Count; i++)
		{
			Rect buttonRect = new Rect(rect.x + i * (buttonWidth + margin * 2) + margin, rect.y + margin, buttonWidth, buttonHeight);

			// 如果点击了相机名称按钮
			if (GUI.Button(buttonRect, SceneCameras[i].cameraViewType.ToString()))
			{
				// 聚焦到对应的相机
				Selection.activeObject = SceneCameras[i].Camera;
				sceneView.in2DMode = SceneCameras[i].cameraViewType == UniversalAdditionalCameraData.CameraViewType.GUICAMERA;
				SyncCameraToSceneView(SceneCameras[i].Camera, sceneView);
				// SceneView.lastActiveSceneView.FrameSelected();
			}
		}

		Handles.EndGUI();
	}

	private void SyncCameraToSceneView(Camera camera, SceneView sceneView)
	{
		if (camera == null)
			return;

		if (sceneView == null)
			return;

		// 同步位置和方向
		sceneView.AlignViewToObject(camera.transform);
		if (sceneView.camera.orthographic)
		{
			sceneView.camera.orthographicSize *= 5;
			sceneView.Repaint();
		}

	}
#endif
}
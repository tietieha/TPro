using System;
using Logic.Modules.LargeMap.Model.CityMist;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using XLua;


[LuaCallCSharp]
public class PVEFogCamera : MonoBehaviour
{
    private const float DetailRootHeight = 6.5f;
    private const string CameraPrefabName = "MistCamera";
    private const string MistMaskPrefabName = "MistMask";
    private const float ExpandRate = 2; // 扩大倍数
    private const float DisAppearTime = 3; // 消散时间
    private const float MASK_OFFSET_START = 30; // 偏移，调整开始时间
    private const float MASK_OFFSET_END = 30; // 偏移，调整结束时间

    #region 迷雾相机

    private Camera _cameraMist;

    private FogOfWarRTGaussianBlur _gaussianBlur;
    private Rect _preCameraRect;
    private bool _needUpdateCamera;
    private CameraVisibleRect _cameraVisibleRect;

    #endregion

    private void Awake()
    {
        _preCameraRect = new Rect(Vector2.negativeInfinity, Vector2.zero);
        _needUpdateCamera = false;
        _cameraMist = gameObject.GetComponent<Camera>();
        _gaussianBlur = gameObject.GetComponent<FogOfWarRTGaussianBlur>();
        _cameraMist.enabled = false;
        _cameraMist.transform.LookAt(_cameraMist.transform.position - Vector3.up, Vector3.forward);
        _cameraMist.backgroundColor = Color.black;
        var rt = new RenderTexture(256, 256, 0, GraphicsFormat.R8G8B8A8_UNorm);
        rt.name = "MistRT";
        _cameraMist.targetTexture = rt;
        SetMistMask();
    }

    public void SetCameraVisibleRect(CameraVisibleRect cameraVisibleRect)
    {
        _cameraVisibleRect = cameraVisibleRect;
        UpdateCamera();
    }

    public void SetMistMask()
    {
        if (_cameraMist == null)
        {
            return;
        }

        Shader.SetGlobalTexture("PVEFogOfWarMask", _cameraMist.targetTexture);
        _needUpdateCamera = true;
    }

    private void Update()
    {
        if (_cameraMist == null || _cameraVisibleRect == null)
        {
            return;
        }

        var rectVisible = _cameraVisibleRect.GetVisibleRect();
        var halfW = rectVisible.width * 0.3;
        var halfH = rectVisible.height * 0.3;
        if (Mathf.Abs(_preCameraRect.xMin - rectVisible.xMin) < halfW ||
            Mathf.Abs(_preCameraRect.xMax - rectVisible.xMax) < halfW ||
            Mathf.Abs(_preCameraRect.yMin - rectVisible.yMin) < halfH ||
            Mathf.Abs(_preCameraRect.yMax - rectVisible.yMax) < halfH)
        {
            UpdateCamera();
        }
    }

    public void UpdateCamera()
    {
        if (_cameraMist == null || _cameraVisibleRect == null)
        {
            return;
        }

        var rectVisible = _cameraVisibleRect.GetVisibleRect();
        _cameraMist.transform.position =
            new Vector3(rectVisible.center.x, 100, rectVisible.center.y);
        _cameraMist.aspect = rectVisible.width / rectVisible.height;
        _cameraMist.orthographicSize = rectVisible.height * 0.5f * ExpandRate;

        _preCameraRect.Set(rectVisible.center.x - rectVisible.width * ExpandRate * 0.5f,
            rectVisible.center.y - rectVisible.height * ExpandRate * 0.5f,
            rectVisible.width * ExpandRate, rectVisible.height * ExpandRate);

        var rt = _cameraMist.targetTexture;
        rt.DiscardContents();
        _cameraMist.Render();
        if (_gaussianBlur != null)
        {
            _gaussianBlur.Process(rt);
        }

        var vPVEFogOfWarMask_ST = new Vector4
        {
            x = 1.0f / _preCameraRect.width,
            y = 1.0f / _preCameraRect.height,
            z = 0.5f - _preCameraRect.center.x / _preCameraRect.width,
            w = 0.5f - _preCameraRect.center.y / _preCameraRect.height
        };

        Shader.SetGlobalVector("PVEFogOfWarMask_ST", vPVEFogOfWarMask_ST);
    }

#if UNITY_EDITOR
    private GUIStyle _gizmosStyle;

    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled || _cameraMist == null) return;
        var preGizmosColor = Gizmos.color;
        var rect = _preCameraRect;
        Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.2f);
        Gizmos.DrawCube(new Vector3(rect.center.x, 0, rect.center.y),
            new Vector3(rect.width, 1, rect.height));
        if (_gizmosStyle == null)
        {
            _gizmosStyle = new GUIStyle();
            _gizmosStyle.alignment = TextAnchor.MiddleCenter;
        }
        _gizmosStyle.normal.textColor = Color.red;
        Handles.Label(new Vector3(rect.center.x, 0, rect.yMax),
            $"({rect.width:F1}, {rect.height:F1})", _gizmosStyle);
    }

#endif
}


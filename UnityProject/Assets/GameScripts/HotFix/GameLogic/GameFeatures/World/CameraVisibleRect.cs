using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class CameraVisibleRect : MonoBehaviour
{
    private Camera _camera;

    private Rect _visibleRect;

    private Vector3 _curCameraForward;
    private Vector3 _curCameraPos;
    private Vector3 _curCameraAngle;
    private float _curCameraFov;
    private float _curCameraTanY;
    private float _camAspect = 1.0f;
    private float _camHalfFovTan = 1.0f;
    private Vector2 _centerPos;

    private readonly Vector2[] _sideWorldPt = new Vector2[4];
    private readonly Vector2[] _sidePos = new Vector2[2]; // 右下和右上坐标

    private void OnEnable()
    {
        if (_camera == null)
            _camera = transform.GetComponent<Camera>();
    }

    public void Update()
    {
        if (isActiveAndEnabled && _camera && _camera.isActiveAndEnabled &&
            _curCameraPos != _camera.transform.position)
        {
            UpdateVisibleRect();
        }
    }

    public void UpdateVisibleRect()
    {
        UpdateCameraCalcParams();
        SetSidePos();
        var x = _sideWorldPt[2].x;
        var y = _sideWorldPt[0].y;
        var width = _sideWorldPt[1].x - x;
        var height = _sideWorldPt[1].y - y;
        _visibleRect.Set(x, y, width, height);
    }

    private void UpdateCameraCalcParams()
    {
        _curCameraForward = _camera.transform.forward;
        _curCameraAngle = _camera.transform.eulerAngles;
        _curCameraFov = _camera.fieldOfView;
        _curCameraPos = _camera.transform.position;
        _camAspect = _camera.aspect;
        _curCameraTanY = Mathf.Tan((90 - _curCameraAngle.x) * Mathf.Deg2Rad);
        _camHalfFovTan = Mathf.Tan(_curCameraFov * 0.5f * Mathf.Deg2Rad);
        _centerPos = GetCameraXYCenterPos();
    }

    // 设置镜头四个边角对应的地图坐标（右下，右上，左上，左下）
    private void SetSidePos()
    {
        _sideWorldPt[0] = CalcPosByViewPoint(1, 0);
        _sideWorldPt[1] = CalcPosByViewPoint(1, 1);

        for (int i = 0; i < 2; i++)
        {
            var sideVector = _sideWorldPt[i] - _centerPos;
            _sidePos[i] = sideVector;
            _sideWorldPt[3 - i].Set(_centerPos.x - sideVector.x, sideVector.y + _centerPos.y);
        }
    }

    // TODO BY QQ why use it?
    private Vector2 InnerPosByViewPointCamera(float x, float y)
    {
        Vector3 vp = new Vector3(x, y, 100);
        var p1 = _camera.ViewportToWorldPoint(vp);
        vp.z = 200;
        var p2 = _camera.ViewportToWorldPoint(vp);
        var diff = p2.y - p1.y;
        float resultX = p1.x - p1.y * (p2.x - p1.x) / diff;
        float resultY = p1.z - p1.y * (p2.z - p1.z) / diff;
        return new Vector2(resultX, resultY);
    }

    /// 计算y为0的情况
    public Vector2 CalcPosByViewPoint(float x, float y)
    {
        return _camera.orthographic
            ? InnerPosByViewPointCamera(x, y)
            : CalcPosByViewPoint(_curCameraTanY, _curCameraForward, x, y, _curCameraPos.y,
                _curCameraPos.x, _curCameraPos.z);
    }

    // TODO BY QQ why use it?
    public Vector2 CalcPosByViewPoint(float tanY, Vector3 forward, float x, float y,
        float cameraHeight, float startX = 0, float startY = 0)
    {
        // 使用三角函数来计算
        var xPct = x - 0.5f;
        var yPct = y - 0.5f;

        // 使用三角函数简化 (TAN(A) + TAN(B)) / (1 - TAN(A) * TAN(B))
        var tanA = tanY;
        var tanB = _camHalfFovTan * 2 * yPct;
        var diffZ = cameraHeight * (tanA + tanB) / (1 - tanA * tanB);

        // 计算射线y在forward的投影长度得到投影平台到镜头距离
        var len = forward.y * -cameraHeight + forward.z * diffZ;

        // 根据aspect与m_CamHalfFovTan得到些平台的宽
        var yHalfW = len * _camAspect * _camHalfFovTan;

        // 根据百分比处理x位置
        return new Vector2(startX + yHalfW * 2 * xPct, startY + diffZ);
    }

    private Vector2 GetCameraXYCenterPos()
    {
        // 默认相机的位置是在y轴上
        var normal = Vector3.Normalize(Vector3.up);
        var distance = -Vector3.Dot(normal, Vector3.zero);
        // 相机相对于xz平面的距离
        var cameraDistance = Vector3.Dot(normal, _curCameraPos) + distance;
        // 相机在xz平面的位置
        var cameraInXYPlane = _curCameraPos - Vector3.up * cameraDistance;
        // 计算相机相对的xz平面的交点
        float a = Vector3.Dot(_curCameraForward.normalized, normal);
        float num = -Vector3.Dot(_curCameraPos, normal) - distance;
        if (Mathf.Approximately(a, 0.0f))
        {
            return new Vector2(cameraInXYPlane.x, cameraInXYPlane.y);
        }

        // 相机视口在xz平面的交点
        float viewPortDistance = num / a;
        if (viewPortDistance > 0)
        {
            cameraInXYPlane = _curCameraPos + _curCameraForward * viewPortDistance;
        }

        return new Vector2(cameraInXYPlane.x, cameraInXYPlane.z);
    }

    public Rect GetVisibleRect()
    {
        return _visibleRect;
    }

    public Vector3 GetCameraCenterPos()
    {
        return _centerPos;
    }

    public static Vector3 ToVector3ZeroY(Vector2 target)
    {
        Vector3 vector3ZeroY;
        vector3ZeroY.x = target.x;
        vector3ZeroY.y = 0.0f;
        vector3ZeroY.z = target.y;
        return vector3ZeroY;
    }

#if UNITY_EDITOR
    private GUIStyle _gizmosStyle;
    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled || _camera == null) return;

        var preGizmosColor = Gizmos.color;
        var rect = GetVisibleRect();
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawCube(new Vector3(rect.center.x, 0, rect.center.y),
            new Vector3(rect.width, 1, rect.height));
        if (_gizmosStyle == null)
        {
            _gizmosStyle = new GUIStyle();
            _gizmosStyle.alignment = TextAnchor.MiddleCenter;
        }

        _gizmosStyle.normal.textColor = Color.white;

        //显示实际可视范围
        Gizmos.color = Color.green;
        var length = _sideWorldPt.Length;
        for (int i = 0; i < length; i++)
        {
            var start = ToVector3ZeroY(_sideWorldPt[i]);
            var end = ToVector3ZeroY(_sideWorldPt[(i + 1) % length]);
            Gizmos.DrawLine(start, end);
            Handles.Label(start, $"{start.x:F1},{start.z:F1}", _gizmosStyle);
        }

        _gizmosStyle.normal.textColor = Color.red;
        Handles.Label(new Vector3(rect.center.x, 0, rect.yMax),
            $"({rect.width:F1}, {rect.height:F1})", _gizmosStyle);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_camera.transform.localPosition,
            new Vector3(_centerPos.x, 0, 0 + _centerPos.y));
        _gizmosStyle.normal.textColor = Color.yellow;
        Handles.Label(new Vector3(_centerPos.x, 0, _centerPos.y),
            $"({_centerPos.x:F1}, {_centerPos.y:F1})", _gizmosStyle);
        Gizmos.color = preGizmosColor;
    }
#endif
}
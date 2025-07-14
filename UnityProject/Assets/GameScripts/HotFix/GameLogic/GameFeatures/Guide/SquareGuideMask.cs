using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using System.Collections.Generic;
using TEngine;
using UW;
using XLua;


// 矩形遮罩镂空引导
[XLua.LuaCallCSharp]
public class SquareGuideMask : MonoBehaviour
{
    //遮罩材质
    private Material MaskMaterial;
    private Image MaskImage;
    private Canvas canvas;
    private Camera uiCamera;
    private Camera sceneCamera;
    //区域范围缓存
    private Vector3[] _corners = new Vector3[4];


    //当前高亮区域的额外长度，从屏幕四角最大开始缩减
    private float currentExtraDistance;
    private float finalDistance;

    //高亮区域缩放的动画时间
    public float _shrinkTime = 0.5f;
    private Vector4 centerRectMatPos = Vector4.zero;
    private Vector2 centerRectPos = Vector2.zero;
    private Vector2 centerWorldPos = Vector2.zero;
    //-1 = 全遮盖   0 = 无镂空   1,2 圆形or矩形
    private int showHollowType = 2;
    private float extraDistance = 2f;

    private bool is3DObject = false;
    //要高亮显示的目标
    private RectTransform Target2D;
    private Transform Target3D;

    private Vector2 centerOffset3D;
    private float target3DLength;
    private float target3DWidth;

    public Color maskColor;

    private void Awake()
    {
        MaskImage = GetComponent<Image>();
        MaskMaterial = MaskImage.material;

        canvas = GetCanvas(this.transform);

        //uiCamera = GameObject.Find("UIRoot/GUICamera").GetComponent<Camera>();

    }

    private void ShowHollowEffect()
    {

        MaskMaterial.color = maskColor;


        if (canvas == null)
        {
            Log.Error("NOT FIND CANVAS");
        }

        //将界面画布顶点距离高亮区域中心最远的距离作为当前高亮区域半径的初始值
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if (canRectTransform != null)
        {
            canRectTransform.GetWorldCorners(_corners);
            foreach (Vector3 corner in _corners)
                currentExtraDistance = Mathf.Max(Vector3.Distance(ScreenPosToRectTransformPos(canvas, corner), centerRectPos), currentExtraDistance);
        }

        MaskMaterial.SetFloat("_Slider", currentExtraDistance);
    }


    public void CalculateCenterDataByDimension()
    {
        if (!is3DObject)
        {
            //获取高亮区域的四个顶点的世界坐标
            Target2D.GetWorldCorners(_corners);

            //计算最终高亮显示区域的长宽
            float length = Vector2.Distance(uiCamera.WorldToScreenPoint(_corners[0]), uiCamera.WorldToScreenPoint(_corners[3]));
            float width = Vector2.Distance(uiCamera.WorldToScreenPoint(_corners[0]), uiCamera.WorldToScreenPoint(_corners[1]));
                

            centerWorldPos.x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
            centerWorldPos.y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
            Vector3 screenPos = uiCamera.WorldToScreenPoint(centerWorldPos);
            centerRectPos = ScreenPosToRectTransformPos(canvas, screenPos);
            //传入中心点距离矩形右边的长度（长的一半），中心点距离矩形底部的长度（宽的一半）
            centerRectMatPos = new Vector4(screenPos.x, screenPos.y, length / 2, width / 2);
            finalDistance = extraDistance;
        }
        else
        {
            Vector3 screenPos = WorldBuildPosToUICamScreenPos(canvas, Target3D.position);
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, uiCamera, out position);
            centerRectPos = position;
            centerRectMatPos = new Vector4(screenPos.x + centerOffset3D.x, screenPos.y + centerOffset3D.y, target3DLength, target3DWidth);
            finalDistance = 0;
        }

    }



    //---------------------功能函数---------------------------
    //屏幕坐标向画布坐标转换
    private Vector2 ScreenPosToRectTransformPos(Canvas canvas, Vector3 screenPos)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, uiCamera, out position);
        return position;
    }

    //不同相机之间的视角坐标转换
    private Vector2 WorldBuildPosToUICamScreenPos(Canvas canvas, Vector3 worldPos)
    {
        Vector3 viewPos = sceneCamera.WorldToViewportPoint(worldPos);
        Vector3 uiCameraWorldPos = uiCamera.ViewportToWorldPoint(viewPos);

        Vector3 screenPos = uiCamera.WorldToScreenPoint(uiCameraWorldPos);
        return screenPos;
    }
    //向父物体找Canvas，找到为止
    private Canvas GetCanvas(Transform gameObject)
    {
        if (gameObject == null)
            return null;
        Canvas canvas = gameObject.GetComponent<Canvas>();
        if (canvas != null)
            return canvas;
        else
            return GetCanvas(gameObject.transform.parent);
    }





    //---------------------Lua调用部分---------------------------
    public Vector2 GetTargetCenter()
    {
        return centerRectPos;
    }
    public void SetTarget2DGameObject(GameObject gameObject)
    {
        Target2D = gameObject.GetComponent<RectTransform>();
        is3DObject = false;
    }
    public void SetTarget3DGameObject(GameObject gameObject)
    {
        Target3D = gameObject.transform;
        is3DObject = true;
    }
    public void SetIsShowHollow(int state)
    {
        showHollowType = state;
    }
   
    public void SetUICamera(Camera camera)
    {
        uiCamera = camera;
    }
    public void SetSceneCamera(Camera camera)
    {
        sceneCamera = camera;
    }

    public void SetTarget2DHollowSize(float distance)
    {
        extraDistance = distance;
    }
    public void SetTarget3DHollowSize(float[] size)
    {
        if (size.Length < 4)
            return;
        centerOffset3D.x = size[0];
        centerOffset3D.y = size[1];
        target3DLength = size[2];
        target3DWidth = size[3];
    }




    //------------------------其他---------------------------
    public void OnClickShowHollow()
    {
        if (showHollowType == 0)
            MaskMaterial.color = new Color(0, 0, 0, 0);
        else if (showHollowType == -1)
        {
            MaskMaterial.color = maskColor;
            MaskMaterial.SetFloat("_Slider", 0);
        }
        else
            ShowHollowEffect();
    }



    //收缩速度
    private float _shrinkVelocity = 0f;
    private void Update()
    {
        if (!is3DObject && Target2D == null) return;
        if (is3DObject && Target3D == null) return;
        if (MaskMaterial == null) return;

        //实时计算中心点
        CalculateCenterDataByDimension();
        MaskMaterial.SetVector("_SquareData", centerRectMatPos);

        //从当前半径到目标半径差值显示收缩动画
        float value = Mathf.SmoothDamp(currentExtraDistance, finalDistance, ref _shrinkVelocity, _shrinkTime);

        if (!Mathf.Approximately(value, currentExtraDistance))
        {
            currentExtraDistance = value;
            MaskMaterial.SetFloat("_Slider", currentExtraDistance);
        }
    }
    public void OnDestroy()
    {
        StopAllCoroutines();
        Target2D = null;
        Target3D = null;
        MaskMaterial = null;
    }

    //public void Reset()
    //{
    //    StopAllCoroutines();
    //    MaskMaterial.SetFloat("_Slider", 1500);

    //    Target2D = null;
    //    Target3D = null;

    //    isShowHollow = false;
    //    centerOffset3D = Vector2.zero;
    //    target3DLength = 0;
    //    target3DWidth = 0;
    //}
}
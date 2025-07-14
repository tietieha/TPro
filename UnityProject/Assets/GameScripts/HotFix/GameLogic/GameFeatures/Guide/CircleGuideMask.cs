using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using UnityEngine.EventSystems;
using XLua;
using UW;
using System;
using TEngine;


// 圆形遮罩镂空引导
// 不显示镂空效果时，通过该脚本计算Target的中心位置
[XLua.LuaCallCSharp]
public class CircleGuideMask : MonoBehaviour
    //, ICanvasRaycastFilter
{

    //遮罩材质
    private Material MaskMaterial;
    private Image MaskImage;
    private Canvas canvas;
    private Camera uiCamera;
    private Camera sceneCamera;
    //区域范围缓存
    private Vector3[] _corners = new Vector3[4];


    // 最后的镂空区域半径
    private float finalRadius;
    private List<float> finalRadiusList = new List<float>();
    //当前高亮区域的半径
    private float _currentRadius;
    private float _currentRadius2;
    //最小半径基础上往外扩多少
    private float extratRadius = 30f;

    //高亮区域缩放的动画时间
    public float _shrinkTime = 0.5f;
    private List<Vector4> centerRectMatPosList = new List<Vector4>();
    private List<Vector2> centerRectPosList = new List<Vector2>();
    private Vector2 centerWorldPos = Vector2.zero;
    //-1 = 全遮盖   0 = 无镂空   1,2 圆形or矩形
    private int showHollowType = 1;
    
    private bool is3DObject = false;
    //要高亮显示的目标
    private List<RectTransform> Target2DList = new List<RectTransform>();
    private List<Vector3> Target3DPosList = new List<Vector3>();
    private Vector2 centerOffset3D;
    private float target3DRadius;
    private List<float> target3DRadiusList = new List<float>();

    public Color maskColor;


    private void Awake()
    {
        MaskImage = GetComponent<Image>();
        MaskMaterial = MaskImage.material;
        MaskMaterial.color = new Color(0, 0, 0, 0);
        
        canvas = GetCanvas(this.transform);

        //uiCamera = GameObject.Find("UIRoot/GUICamera").GetComponent<Camera>();
    }


    private void ShowHollowEffect()
    {   
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
            {
                _currentRadius = Mathf.Max(Vector3.Distance(ScreenPosToRectTransformPos(canvas, corner), centerRectPosList[0]), _currentRadius);
                
            }

            if (centerRectPosList.Count > 1)
            {
                foreach (Vector3 corner in _corners)
                {
                    _currentRadius2 = Mathf.Max(Vector3.Distance(ScreenPosToRectTransformPos(canvas, corner), centerRectPosList[1]), _currentRadius2);
                }
                
            }
        }
        MaskMaterial.SetFloat("_Slider", finalRadiusList[0]);
        if (finalRadiusList.Count > 1)
        {
            MaskMaterial.SetFloat("_Slider2", finalRadiusList[1]);
        }
    }

    public void CalculateCenterDataByDimension()
    {
        centerRectPosList.Clear();
        centerRectMatPosList.Clear();
        finalRadiusList.Clear();
        
        if (!is3DObject)
        {
            //Debug.LogError("镂空中心为： "+centerRectPos);
            for (int i = 0; i < Target2DList.Count; i++)
            {
                Calculate2DTargetData(i);
            }
        }
        else
        {
            for (int i = 0; i < Target3DPosList.Count; i++)
            {
                Calculate3DTargetData(i);
            }
        }
    }

    private void Calculate2DTargetData(int index)
    {
        if (Target2DList[index] == null)
        {
            return;
        }
        
        //获取高亮区域的四个顶点的世界坐标
        Target2DList[index].GetWorldCorners(_corners);

        //计算最终高亮显示区域的半径
        Vector3 pos0 = uiCamera.WorldToScreenPoint(_corners[0]);
        Vector3 pos1 = uiCamera.WorldToScreenPoint(_corners[2]);
        Vector3 distanceV3 = pos0 - pos1;
        float distance = new Vector2(distanceV3.x, distanceV3.y).magnitude;
        finalRadiusList.Add(extratRadius + distance / (float)2);


        //计算高亮显示区域的圆心  材质中用逐像素来比较
        centerWorldPos.x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        centerWorldPos.y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        Vector3 screenPos = uiCamera.WorldToScreenPoint(centerWorldPos);
        centerRectPosList.Add(ScreenPosToRectTransformPos(canvas, screenPos));
        centerRectMatPosList.Add(new Vector4(screenPos.x, screenPos.y, 0, 0));
    }

    private void Calculate3DTargetData(int index)
    {
        Vector3 screenPos = WorldPosToUICamScreenPos(canvas, Target3DPosList[index]);
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, uiCamera, out position);
        centerRectPosList.Add(position);
        centerRectMatPosList.Add(new Vector4(screenPos.x + centerOffset3D.x, screenPos.y + centerOffset3D.y, 0, 0));
        finalRadiusList.Add(target3DRadiusList[index]);
    }
    
    //---------------------功能函数---------------------------
    //屏幕坐标向画布坐标转换
    private Vector2 ScreenPosToRectTransformPos(Canvas canvas, Vector3 screenPos)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,screenPos, uiCamera, out position);
        return position;
    }

    //不同相机之间的视口坐标转换
    private Vector2 WorldPosToUICamScreenPos(Canvas canvas, Vector3 worldPos)
    {
        var viewPos = sceneCamera.WorldToViewportPoint(worldPos);
        var uiCameraWorldPos = uiCamera.ViewportToWorldPoint(viewPos);

        var screenPos = uiCamera.WorldToScreenPoint(uiCameraWorldPos);
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
        return centerRectPosList[0];
    }
    
    public void SetTarget2DGameObject(GameObject gameObject)
    {
        is3DObject = false;
        
        Target2DList.Add(gameObject.GetComponent<RectTransform>());
    }
    
    public void SetTarget3DGoWorldPos(Vector3 worldPos)
    {
        is3DObject = true;
        
        Target3DPosList.Add(worldPos);
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

    public void SetTarget2DHollowSize(float radius)
    {
        extratRadius = radius;
    }
    
    public void SetTarget3DHollowSize(float[] size)
    {
        if (size.Length < 3)
            return;
        centerOffset3D.x = size[0];
        centerOffset3D.y = size[1];
        
        target3DRadiusList.Add(size[2]);
    }
    
    //------------------------其他---------------------------
    public void OnClickShowHollow()
    {
        CalculateCenterDataByDimension();
        
        if (showHollowType == 0)
            MaskMaterial.color = new Color(0, 0, 0, 0);
        else if (showHollowType == -1)
        {
            MaskMaterial.color = maskColor;
            MaskMaterial.SetFloat("_Slider", 0);
        }
        else
        {
            MaskMaterial.color = maskColor;
            ShowHollowEffect();
        }
    }

    //收缩速度
    private float _shrinkVelocity = 0f;

    private void Update()
    {
        if (!is3DObject && Target2DList.Count <= 0 )return;
        if (is3DObject && Target3DPosList.Count <= 0 ) return;
        if (MaskMaterial == null) return;

        //实时计算中心点
        CalculateCenterDataByDimension();

        if (centerRectMatPosList.Count > 0)
        {
            MaskMaterial.SetVector("_Center", centerRectMatPosList[0]);
        }
        
        //从当前半径到目标半径差值显示收缩动画
        float value = Mathf.SmoothDamp(_currentRadius, finalRadiusList[0], ref _shrinkVelocity, _shrinkTime);
        
        if (!Mathf.Approximately(value, _currentRadius))
        {
            _currentRadius = value;
            MaskMaterial.SetFloat("_Slider", _currentRadius);
        }

        if (centerRectMatPosList.Count > 1)
        {
            MaskMaterial.SetVector("_Center2", centerRectMatPosList[1]);
            
            float value2 = Mathf.SmoothDamp(_currentRadius2, finalRadiusList[1], ref _shrinkVelocity, _shrinkTime);
            if (!Mathf.Approximately(value, _currentRadius2))
            {
                _currentRadius2 = value;
                MaskMaterial.SetFloat("_Slider2", _currentRadius2);
            }
        }
    }
    
    public void OnDestroy()
    {
        StopAllCoroutines();
        MaskMaterial = null;
        Target2DList = null;
        Target3DPosList = null;
    }

    //public void Reset()
    //{
    //    StopAllCoroutines();
    //    MaskMaterial.SetVector("_Center", new Vector4(27.18f, 9.3f, 0, 0));
    //    MaskMaterial.SetFloat("_Slider", 1500);
    //}

    public void ResetMask()
    {
        is3DObject = false;
        centerOffset3D = Vector3.zero;
        finalRadius = 0;
        Target2DList.Clear();
        Target3DPosList.Clear();
        centerRectPosList.Clear();
        centerRectMatPosList.Clear();
        
        MaskMaterial.SetFloat("_Slider", 0);
        MaskMaterial.SetFloat("_Slider2", 0);
    }
    
    // public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    // {
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, sp, uiCamera, out var uiPos);
    //     for (int i = 0; i < centerRectPosList.Count; i++)
    //     {
    //         if (Vector2.Distance(centerRectPosList[i], uiPos) <= finalRadiusList[i])
    //         {
    //             return false;
    //         }
    //     }
    //
    //     return true;
    // }
}
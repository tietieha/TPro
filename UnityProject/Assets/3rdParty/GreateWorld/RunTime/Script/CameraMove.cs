using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    public BoxCollider2D Bounds = null; //移动的边界
    private Vector3 deceleration = new Vector3(100f, 0, 100f);
    public Vector3 minVec3, maxVec3;
    private Vector2 beginP = Vector2.zero;//鼠标第一次落下点  
    private Vector2 endP = Vector2.zero;//鼠标第二次位置（拖拽位置）  
    private Vector3 speed = Vector3.zero;
    public Camera eyeCamera = null; // 视图相机
    public bool isUpdateTouch = true; //是否更新touch 
    public void Start()
    {
        //SetBox();
        transform.eulerAngles = new Vector3(45, 0, 0);

        if (eyeCamera == null)
        {
            eyeCamera = Camera.main;
        }

        if (Bounds)
        {
            minVec3 = Bounds.bounds.min;//包围盒  
            maxVec3 = Bounds.bounds.max;
        }
        var x = transform.position.x;
        var y = transform.position.z;
        if (Bounds)
        {
            x = x - speed.x;//向量偏移  
            y = y - speed.z;
            float cameraHeight = Camera.main.orthographicSize * 2;
            var cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
            var cameraHalfWidth = eyeCamera.orthographic ? cameraSize.x / 2 : 0;
            var cameraHalfHeight = eyeCamera.orthographic ? cameraSize.y / 2 : 0;
            //保证不会移除包围盒  

            x = Mathf.Clamp(x, minVec3.x + cameraHalfWidth, maxVec3.x - cameraHalfWidth);
            y = Mathf.Clamp(y, minVec3.y + cameraHalfHeight, maxVec3.y - cameraHalfHeight);
        }
        transform.position = new Vector3(x, transform.position.y, y);
    }

    public void OnGUI()
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        return;
#endif

        if (Event.current.type == EventType.MouseDown)
        {
            MoveBegin(Input.mousePosition);
        }
        else if (Event.current.type == EventType.MouseDrag)
        {
            Moveing(Input.mousePosition);
        }
    }
    //移动对象
    void UpdateTargetPositon()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        if (!isUpdateTouch)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Canceled || Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    isUpdateTouch = true;
                    break;
                }
            }
        }
        if (Input.touchCount == 1)
        {
            if (isUpdateTouch)
            {
                MoveBegin(Input.GetTouch(0).position);
                isUpdateTouch = false;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Moveing(Input.GetTouch(0).position);
            }
        }

    }
    ///初始化位置，为接下来的move做准备
    void MoveBegin(Vector3 point)
    {
        beginP = point;
        speed = Vector3.zero;
    }
    ///更新目标位置
    void Moveing(Vector3 point)
    {
        //记录鼠标拖动的位置 　　  
        endP = point;
        Vector3 fir = eyeCamera.ScreenToWorldPoint(new Vector3(beginP.x, beginP.y, eyeCamera.nearClipPlane));//转换至世界坐标  
        Vector3 sec = eyeCamera.ScreenToWorldPoint(new Vector3(endP.x, endP.y, eyeCamera.nearClipPlane));
        speed = sec - fir;//需要移动的 向量  
    }
    ///Move结束，清除数据
    void MoveEnd(Vector3 point)
    {
        MoveBegin(point);
    }

    public void Update()
    {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        UpdateTargetPositon();
#endif

        if (speed == Vector3.zero)
        {
            return;
        }
        var x = transform.position.x;
        var y = transform.position.z;
        x = x - speed.x * 10;//向量偏移  
        y = y - speed.z * 10;
        if (Bounds)
        {
            float cameraHeight = Camera.main.orthographicSize * 2;
            var cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
            var cameraHalfWidth = eyeCamera.orthographic ? cameraSize.x / 2 : 0;
            var cameraHalfHeight = eyeCamera.orthographic ? cameraSize.y / 2 : 0;
            //保证不会移除包围盒  

            x = Mathf.Clamp(x, minVec3.x + cameraHalfWidth, maxVec3.x - cameraHalfWidth);
            y = Mathf.Clamp(y, minVec3.y + cameraHalfHeight, maxVec3.y - cameraHalfHeight);
        }
        else
        {
            x = Mathf.Clamp(x, -128, 128);
            y = Mathf.Clamp(y, -128, 128);
        }

        transform.position = new Vector3(x, transform.position.y, y);
        if (System.Math.Abs(speed.x) < 0.01f)
        {
            speed.x = 0;
        }
        else
        {
            if (speed.x > 0)
            {
                speed.x -= deceleration.x * Time.deltaTime;
                //if (speed.x < 0)
                //{
                //    speed.x = 0;
                //}
            }
            else
            {
                speed.x += deceleration.x * Time.deltaTime;
                //if (speed.x > 0)
                //{
                //    speed.x = 0;
                //}
            }
        }
        if (System.Math.Abs(speed.z) < 0.01f)
        {
            speed.z = 0;
        }
        else
        {
            if (speed.z > 0)
            {
                speed.z -= deceleration.z * Time.deltaTime;
                //if (speed.z < 0)
                //{
                //    speed.z = 0;
                //}
            }
            else
            {
                speed.z += deceleration.z * Time.deltaTime;
                //if (speed.z > 0)
                //{
                //    speed.z = 0;
                //}
            }
        }
        beginP = endP;
        if (speed.x == 0 && speed.z == 0)
        {
            speed = Vector3.zero;
        }
    }


    private static GameObject s_aroundCols;
    public static void CreateAroundColliders()
    {
        if (s_aroundCols != null) return;
        s_aroundCols = new GameObject("aroundCols");
        s_aroundCols.transform.position = Vector3.zero;
        s_aroundCols.transform.localScale = Vector3.one;
        s_aroundCols.transform.localEulerAngles = Vector3.zero;

        Vector3 top = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height, 3f));
        Vector3 bottom = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0f, 3f));
        Vector3 left = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height / 2f, 3f));
        Vector3 right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, 3f));
        float width = Vector3.Distance(left, right);
        float height = Vector3.Distance(bottom, top);

        PhysicMaterial pmat = new PhysicMaterial();
        pmat.dynamicFriction = 0f;      //动摩擦力//
        pmat.staticFriction = 0f;       //静摩擦力//
        pmat.bounciness = 1f;       //弹力//
        pmat.frictionCombine = PhysicMaterialCombine.Maximum;       //接触物体之间的摩擦力计算//
        pmat.bounceCombine = PhysicMaterialCombine.Maximum;         //接触物体之间的弹力计算//

        createOnCollider("top", s_aroundCols.transform, Vector3.zero, top, new Vector3(width, 0.05f, 0.05f), pmat);
        createOnCollider("bottom", s_aroundCols.transform, Vector3.zero, bottom, new Vector3(width, 0.05f, 0.05f), pmat);
        createOnCollider("left", s_aroundCols.transform, Vector3.zero, left, new Vector3(0.05f, height, 0.05f), pmat);
        createOnCollider("right", s_aroundCols.transform, Vector3.zero, right, new Vector3(0.05f, height, 0.05f), pmat);
    }

    private static void createOnCollider(string name, Transform parent, Vector3 pos, Vector3 center, Vector3 size, PhysicMaterial mat)
    {
        GameObject col = new GameObject(name);
        col.transform.position = pos;
        col.transform.parent = parent;
        BoxCollider box = col.AddComponent<BoxCollider>();
        box.center = center;
        box.size = size;
        box.material = mat;
    }

    private void SetBox()
    {
        Bounds.size = new Vector2(Screen.width, Screen.height);
    }
}
using System;
using System.Collections.Generic;
using BitBenderGames;
using TEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UW;
using XLua;

namespace World
{
    [LuaCallCSharp]
    public class TestCameraMove : MonoBehaviourWrapped
    {
        private Quaternion _oldR;
        [SerializeField]private Camera myCamera;
        [SerializeField]private Camera uiCamera;
        [SerializeField]private MobileTouchCamera touchCamera;
        [SerializeField]private TouchInputController touchInputController;

        private void Awake()
        {
            _oldR = transform.rotation;
            touchCamera.TargetZoomMax = touchCamera.CamZoomMax;
            touchCamera.TargetZoomMin = touchCamera.CamZoomMin;
        }

        public void Update()
        {
            uiCamera.orthographicSize = myCamera.orthographicSize;
        }


        #region MyRegion 无用代码
        // void Update()
        // {
//             float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
//             if (scrollWheel != 0)
//             {
//                 Camera_Scale(scrollWheel); 
//             }
//             else if (Input.GetMouseButton(1))
//             {
//                     Camera_Rotate();
//             }
//             else if (Input.GetMouseButton(0))
//             {
//                 // Camera_Move(); 
//             }
//    
//           
// #if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
// 			HandleTouchInput();
// #else
//             HandleMouseInput();
// #endif
        // }
 // private Vector3 m_prevPosition;
        // void HandleTouchInput()
        // {
        //     if (Input.touchCount == 1)
        //     {
        //         Touch touch = Input.GetTouch(0);
        //         if (touch.phase == TouchPhase.Began)
        //         {
        //             m_prevPosition = touch.position;
        //         }
        //         else if (touch.phase == TouchPhase.Moved)
        //         {
        //             Vector3 curPosition = touch.position;
        //             MoveCamera(m_prevPosition, curPosition);
        //             m_prevPosition = curPosition;
        //         }
        //     }
        //     else if (Input.touchCount == 2)
        //     {
        //         Touch touch = Input.GetTouch(0);
        //         if (touch.phase == TouchPhase.Began)
        //         {
        //             m_prevPosition = touch.position;
        //         }
        //         else if (touch.phase == TouchPhase.Moved)
        //         {
        //             Vector3 curPosition = touch.position;
        //             var offset = (m_prevPosition - curPosition);
        //             Camera_Scale(offset.y * 0.003f);
        //             m_prevPosition = curPosition;
        //         }
        //     }
        //     else if (Input.touchCount == 3)
        //     {
        //         Touch touch = Input.GetTouch(0);
        //         if (touch.phase == TouchPhase.Began)
        //         {
        //             m_prevPosition = touch.position;
        //         }
        //         else if (touch.phase == TouchPhase.Moved)
        //         {
        //             Vector3 curPosition = touch.position;
        //             var offset = (m_prevPosition - curPosition);
        //             Touch_Camera_Rotate(offset.y * 0.003f);
        //             m_prevPosition = curPosition;
        //         }
        //     }
        //     
        //     
        // }
        //
        // void HandleMouseInput()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         m_prevPosition = Input.mousePosition;
        //     }
        //     else if (Input.GetMouseButton(0))
        //     {
        //         Vector3 curMousePosition = Input.mousePosition;
        //         MoveCamera(m_prevPosition, curMousePosition);
        //         // var offset = (m_prevPosition - curMousePosition);
        //         // Touch_Camera_Rotate(offset.y * 0.01f);
        //         m_prevPosition = curMousePosition;
        //         
        //     }
        // }
        //
        // public float moveSpeed = 0.5f;
        // private void MoveCamera(Vector3 prevPosition, Vector3 curPosition)
        // {
        //     //注意这里的myCamera.nearClipPlaen。由于我使用的是透视相机，所以需要将z值改为这个
        //     //如果读者使用的是正交相机，可能不需要这个
        //     Vector3 offset = prevPosition-curPosition;
        //     // Debug.LogError(offset);
        //     //这里的m_cameraScale,因为我不想修改nearClipPlaen的值来达到移动的快慢，所以加了个移动参数
        //     Vector3 newPos = new Vector3(transform.localPosition.x + offset.x * moveSpeed, transform.position.y,transform.localPosition.z + offset.y * moveSpeed);
        //     // Debug.LogError(prevPosition-curPosition );
        //     // Debug.LogError(newPos);
        //     transform.position = newPos;
        // }
         //
        // private void Camera_Scale(float scrollWheel)
        // {
        //     scrollWheel = scrollWheel * Time.deltaTime * 5000;
        //     var temp2 = transform.position;
        //     temp2.y += scrollWheel;
        //     transform.Translate(Vector3.forward * scrollWheel);
        // }
        //
        // private void Camera_Rotate()
        // {
        //     float mouseX = Input.GetAxis("Mouse X");
        //     float mouseY = Input.GetAxis("Mouse Y");
        //     var position = transform.position;
        //     // transform.RotateAround(position, Vector3.up, mouseX * 5);
        //  
        //     transform.RotateAround(position, transform.right, -mouseY * 5);
        //     if ( GetCurrentTiltAngleDeg() >90)
        //     {
        //         transform.rotation = Quaternion.Euler(90,0,0);
        //     }
        //     if ( transform.rotation.eulerAngles.x<45)
        //     {
        //         transform.rotation = Quaternion.Euler(45,0,0);
        //     }
        // }
        //
        // private void Touch_Camera_Rotate(float mouseY)
        // {
        //
        //     var position = transform.position;
        //     transform.RotateAround(position, transform.right, mouseY);
        //     if ( GetCurrentTiltAngleDeg() >90)
        //     {
        //         transform.rotation = Quaternion.Euler(90,0,0);
        //     }
        //     if ( transform.rotation.eulerAngles.x<45)
        //     {
        //         transform.rotation = Quaternion.Euler(45,0,0);
        //     }
        // }
        // private Plane refPlaneXZ = new Plane(new Vector3(0, 1, 0), 0);
        // private float GetCurrentTiltAngleDeg()
        // {
        //     Vector3 camForwardOnPlane = Vector3.Cross(refPlaneXZ.normal, transform.right);
        //     float tiltAngle = Vector3.Angle(camForwardOnPlane, -transform.forward);
        //     return (tiltAngle);
        // }
        // private void Camera_Move()
        // {
        //     float mouseX = Input.GetAxis("Mouse X");
        //     float mouseY = Input.GetAxis("Mouse Y");
        //     Transform transform1;
        //     (transform1 = transform).Translate(Vector3.left * mouseX);
        //     var temp = transform1.position;
        //     transform1.Translate(Vector3.up * mouseY * -1);
        //     var temp2 = transform1.position;
        //     transform1.position = new Vector3(temp2.x, temp.y, temp2.z);
        //
        // }
        //

        #endregion
       

        public RaycastHit? TestHit(Vector3 v,bool isSkipUI = false)
        {
            
            if (!isSkipUI && CheckIsTouchInUIPanel())
            {
                Debug.Log("点击到UI");
                return null;
            }
            var ray = myCamera.ScreenPointToRay(v);
            RaycastHit hitInfo;
            var layer = LayerMask.GetMask("BigWorld", "BigWorldGrid","BigWorldShip");
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue,layer))
            {
                return hitInfo;
            }
            return null;
        }
        
        PointerEventData pointer_eventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> ray_list = new List<RaycastResult>();
        public bool CheckIsTouchInUIPanel()
        {

            //如果没有点击，直接返回false
            if (TouchWrapper.IsFingerDown == false)
                return false;
            pointer_eventData.Reset();
            pointer_eventData.pressPosition = Input.mousePosition;
            pointer_eventData.position = Input.mousePosition;
            ray_list.Clear();
            EventSystem.current.RaycastAll(pointer_eventData, ray_list);
            for (int i = 0; i < ray_list.Count; i++)
            {
                // UI 自身屏蔽交互，不影响场景交互
                // var skip = ray_list[i].gameObject.GetComponent<UISkipPick>();
                // if (skip == null)
                    return true;
            }

            return false;
        }
        
        
        #if ODIN_INSPECTOR
        [Button("相机复位", ButtonSizes.Large)]
        #endif
        private void Function1()
        {
            transform.rotation = _oldR;
        }
        #if ODIN_INSPECTOR
        [Button("取消平滑(开/关)-服务器需要", ButtonSizes.Large)]
        #endif
        private void Function2()
        {
            LuaModule.CallLuaFunc("BigWorldCSCallLua","CancelSmoothMovement");
        }
        
        #if ODIN_INSPECTOR
        [Button("隐藏格子上的文本", ButtonSizes.Large)]
        #endif
        private void Function3()
        {
            LuaModule.CallLuaFunc("BigWorldCSCallLua","HideText");
        }

        #if ODIN_INSPECTOR
        [Button("显示测试用的格子", ButtonSizes.Large)]
        #endif
        private void Function4()
        {
            LuaModule.CallLuaFunc("BigWorldCSCallLua","ShowDebugRedGrid");
        }
        #if ODIN_INSPECTOR
        [Button("卸载场景", ButtonSizes.Large)]
        #endif
        private void Function5()
        {
            LuaModule.CallLuaFunc("BigWorldCSCallLua","DebugUnInit");
        }
#if ODIN_INSPECTOR
        [Button("取消相机锁定", ButtonSizes.Large)]
#endif
        private void Function6()
        {
            LuaModule.CallLuaFunc("BigWorldCSCallLua","cancelFollow");
        }
        
        private Vector3 lastCamPosition;
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            var curPos = myCamera.transform.position;
            // if (lastCamPosition == curPos)
            //     return;
            //
            // lastCamPosition = curPos;
            var p0 = GetRaycastGroundPoint(new Vector3(0, Screen.height));
            var p1 = new Vector3(curPos.x + curPos.x - p0.x, 0, p0.z);
            var p2 = GetRaycastGroundPoint(new Vector3(0, 0));
            var p3 = new Vector3(curPos.x + (curPos.x - p2.x), 0, p2.z);
            var lastRect = new Rect(p0.x, p0.z, p1.x - p0.x, p2.z - p0.z);
            //Gizmos.color = new Color(0, 0, 0.8f, 1);
            //Gizmos.DrawSphere(camPos, 1);
            Gizmos.color = Color.red;
            var p01 = new Vector3(lastRect.xMin, 0f, lastRect.yMin);
            var p11 = new Vector3(lastRect.xMax, 0f, lastRect.yMin);
            var p21 = new Vector3(lastRect.xMax, 0f, lastRect.yMax);
            var p31= new Vector3(lastRect.xMin, 0f, lastRect.yMax);

            Gizmos.DrawLine(p01, p11);
            Gizmos.DrawLine(p11, p21);
            Gizmos.DrawLine(p21, p31);
            Gizmos.DrawLine(p31, p01);
        
        }
        public Vector3 GetRaycastGroundPoint(Vector3 screenPos)
        {
            Vector3 posWorld = Vector3.zero;
            if (myCamera != null && touchCamera != null)
            {
                touchCamera.RaycastGround(myCamera.ScreenPointToRay(screenPos), out posWorld);
            }
            return posWorld;
        }
        
        public Vector3 WorldToScreenPoint(Vector3 worldPos)
        {
            Vector3 camNormal = myCamera.transform.forward;
            Vector3 vectorFromCam = worldPos - myCamera.transform.position;
            float camNormDot = Vector3.Dot(camNormal, vectorFromCam);
            if (camNormDot <= 0)
            {
                // we are behind the camera forward facing plane, project the position in front of the plane
                Vector3 proj = (camNormal * camNormDot * 1.01f);
                worldPos = myCamera.transform.position + (vectorFromCam - proj);
            }
            return RectTransformUtility.WorldToScreenPoint(myCamera, worldPos);
        }
        
        public bool CheckGuiRaycastObjects(Vector2 mousePos)
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = mousePos;
            eventData.position = mousePos;
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, list);

            return list.Count > 0;
        }
    }
}
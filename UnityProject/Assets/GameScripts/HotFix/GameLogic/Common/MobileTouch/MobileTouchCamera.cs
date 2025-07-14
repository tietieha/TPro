using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

using DG.Tweening;
using TEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.EventSystems;
using XLua;

namespace BitBenderGames
{
    [RequireComponent(typeof(TouchInputController))]
    [RequireComponent(typeof(Camera))]
    [LuaCallCSharp]
    public class MobileTouchCamera : MonoBehaviourWrapped
    {
        #region inspector

        [SerializeField]
        [Tooltip(
            "You need to define whether your camera is a side-view camera (which is the default when using the 2D mode of unity) or if you chose a top-down looking camera. This parameter tells the system whether to scroll in XY direction, or in XZ direction.")]
        private CameraPlaneAxes cameraAxes = CameraPlaneAxes.XY_2D_SIDESCROLL;

        [Tooltip("不同场景操作不同")] [SerializeField] protected WorldType worldType = WorldType.ShipWorld;

        [SerializeField]
        [Tooltip(
            "When using a perspective camera, the zoom can either be performed by changing the field of view, or by moving the camera closer to the scene.")]
        private PerspectiveZoomMode perspectiveZoomMode = PerspectiveZoomMode.FIELD_OF_VIEW;

        [SerializeField]
        [Tooltip(
            "For perspective cameras this value denotes the min field of view used for zooming (field of view zoom), or the min distance to the ground (translation zoom). For orthographic cameras it denotes the min camera size.")]
        private float camZoomMin = 4;

        [SerializeField]
        [Tooltip(
            "For perspective cameras this value denotes the max field of view used for zooming (field of view zoom), or the max distance to the ground (translation zoom). For orthographic cameras it denotes the max camera size.")]
        private float camZoomMax = 12.5f;

        [SerializeField] private float currentZoom = 0;

        [SerializeField]
        [Tooltip(
            "The cam will overzoom the min/max values by this amount and spring back when the user releases the zoom.")]
        private float camOverzoomMargin = 1;

        [SerializeField]
        [Tooltip(
            "When dragging the camera close to the defined border, it will spring back when the user stops dragging. This value defines the distance from the border where the camera will spring back to.")]
        private float camOverdragMargin = 5.0f;

        [SerializeField]
        [Tooltip(
            "These values define the scrolling borders for the camera. The camera will not scroll further than defined here. When a top-down camera is used, these 2 values are applied to the X/Z position.")]
        protected Vector2 boundaryMin = new Vector2(-1000, -1000);

        [SerializeField]
        [Tooltip(
            "These values define the scrolling borders for the camera. The camera will not scroll further than defined here. When a top-down camera is used, these 2 values are applied to the X/Z position.")]
        protected Vector2 boundaryMax = new Vector2(1000, 1000);

        [Header("Advanced")]
        [SerializeField]
        [Tooltip(
            "The lower the value, the slower the camera will follow. The higher the value, the more direct the camera will follow movement updates. Necessary for keeping the camera smooth when the framerate is not in sync with the touch input update rate.")]
        private float camFollowFactor = 15.0f;

        [SerializeField] [Tooltip("地面的X轴旋转角度")]
        public float groundRotX = 24f;

        [SerializeField] [Tooltip("高于多少开始进行摄像机旋转")]
        public float maxGoundHeight = 5f;

        [SerializeField]
        [Tooltip("Set the behaviour of the damping (e.g. the slow-down) at the end of auto-scrolling.")]
#pragma warning disable 414 //NOTE: This field is actually used by the custom inspector. The pragma disables the warning that appears because the variable isn't accessed directly, but only through reflection.
        private AutoScrollDampMode autoScrollDampMode = AutoScrollDampMode.DEFAULT;
#pragma warning restore 414
        [SerializeField]
        [Tooltip(
            "When dragging quickly, the camera will keep autoscrolling in the last direction. The autoscrolling will slowly come to a halt. This value defines how fast the camera will come to a halt.")]
        private float autoScrollDamp = 300;

        [SerializeField] [Tooltip("This curve allows to modulate the auto scroll damp value over time.")]
        private AnimationCurve autoScrollDampCurve = new AnimationCurve(new Keyframe(0, 1, 0, 0),
            new Keyframe(0.7f, 0.9f, -0.5f, -0.5f), new Keyframe(1, 0.01f, -0.85f, -0.85f));

        [SerializeField]
        [Tooltip(
            "The camera assumes that the scrollable content of your scene (e.g. the ground of your game-world) is located at y = 0 for top-down cameras or at z = 0 for side-scrolling cameras. In case this is not valid for your scene, you may adjust this property to the correct offset.")]
        private float groundLevelOffset = 0;

        [SerializeField] [Tooltip("When enabled, the camera can be rotated using a 2-finger rotation gesture.")]
        private bool enableRotation = false;

        [SerializeField] [Tooltip("When enabled, the camera can be tilted using a synced 2-finger up or down motion.")]
        private bool enableTilt = false;

        [SerializeField] [Tooltip("The minimum tilt angle for the camera.")]
        private float tiltAngleMin = 45;

        [SerializeField] [Tooltip("The maximum tilt angle for the camera.")]
        private float tiltAngleMax = 90;

        [SerializeField] [Tooltip("When enabled, the camera is tilted automatically when zooming.")]
        private bool enableZoomTilt = false;

        [SerializeField] [Tooltip("The minimum tilt angle for the camera when using zoom tilt.")]
        private float zoomTiltAngleMin = 45;

        [SerializeField] [Tooltip("The maximum tilt angle for the camera when using zoom tilt.")]
        private float zoomTiltAngleMax = 90;

        [Header("Event Callbacks")]
        [SerializeField]
        [Tooltip("Here you can set up callbacks to be invoked when an item with Collider is tapped on.")]
        private UnityEventWithRaycastHit OnPickItem;

        [SerializeField]
        [Tooltip("Here you can set up callbacks to be invoked when an item with Collider2D is tapped on.")]
        private UnityEventWithRaycastHit2D OnPickItem2D;

        [SerializeField]
        [Tooltip("Here you can set up callbacks to be invoked when an item with Collider is double-tapped on.")]
        private UnityEventWithRaycastHit OnPickItemDoubleClick;

        [SerializeField]
        [Tooltip("Here you can set up callbacks to be invoked when an item with Collider2D is double-tapped on.")]
        private UnityEventWithRaycastHit2D OnPickItem2DDoubleClick;

        #endregion

        public CameraPlaneAxes CameraAxes
        {
            get { return (cameraAxes); }
            set { cameraAxes = value; }
        }

        protected TouchInputController touchInputController;

        protected Vector3 dragStartCamPos;
        protected Vector3 cameraScrollVelocity;

        public bool pinchEnabled = true;
        private float pinchStartCamZoomSize;
        private Vector3 pinchStartIntersectionCenter;
        private Vector3 pinchCenterCurrent;
        private float pinchDistanceCurrent;
        private float pinchAngleCurrent = 0;
        private float pinchDistanceStart;
        private Vector3 pinchCenterCurrentLerp;
        private float pinchDistanceCurrentLerp;
        private float pinchAngleCurrentLerp;
        private bool isRotationLock = true;
        private bool isRotationActivated = false;
        private float pinchAngleLastFrame = 0;
        private float pinchTiltCurrent = 0;
        private float pinchTiltAccumulated = 0;
        private bool isTiltModeEvaluated = false;
        private float pinchTiltLastFrame;
        private bool isPinchTiltMode;

        private float timeRealDragStop;

        private bool isForceGuide = false;

        public bool IsAutoScrolling
        {
            get { return (cameraScrollVelocity.sqrMagnitude > float.Epsilon); }
        }

        public bool IsForceGuide
        {
            set { isForceGuide = value; }
        }

        public bool IsPinching { get; private set; }
        public bool IsDragging { get; protected set; }

        private Action beforeUpdate;

        public Action BeforeUpdate
        {
            set { beforeUpdate = value; }
        }

        private Action _afterSetCameraPosition;

        public Action AfterSetCameraPosition
        {
            get { return _afterSetCameraPosition; }

            set { _afterSetCameraPosition = value; }
        }

        //正在缩放的回调
        private Action _zoomingCallBack = null;

        public Action ZoomingCallback
        {
            get { return _zoomingCallBack; }
            set { _zoomingCallBack = value; }
        }

        //正在缩放的回调
        private Action _pinchingCallBack = null;

        public Action PinchinggCallback
        {
            get { return _pinchingCallBack; }
            set { _pinchingCallBack = value; }
        }

        private Vector3 lastCamPosition = Vector3.zero;

        #region expert mode tweakables

        [Header("Expert Mode")] [SerializeField]
        private bool expertModeEnabled;

        [SerializeField]
        [Tooltip(
            "Depending on your settings the camera allows to zoom slightly over the defined value. When releasing the zoom the camera will spring back to the defined value. This variable defines the speed of the spring back.")]
        private float zoomBackSpringFactor = 20;

        [SerializeField]
        [Tooltip(
            "When close to the border the camera will spring back if the margin is bigger than 0. This variable defines the speed of the spring back.")]
        private float dragBackSpringFactor = 10;

        [SerializeField]
        [Tooltip(
            "When swiping over the screen the camera will keep scrolling a while before coming to a halt. This variable limits the maximum velocity of the auto scroll.")]
        private float autoScrollVelocityMax = 60;

        [SerializeField] [Tooltip("This value defines how quickly the camera comes to a halt when auto scrolling.")]
        private float dampFactorTimeMultiplier = 2;

        [SerializeField]
        [Tooltip(
            "When setting this flag to true, the camera will behave like a popular tower defense game. It will either go into an exclusive tilt mode, or into a combined zoom/rotate mode. When set to false, the camera will behave like a popular city building game. The camera won't pan with 2 fingers, and instead zoom, rotate and tilt are done in parallel.")]
        private bool isPinchModeExclusive = true;

        [SerializeField]
        [Tooltip(
            "This value should be kept at 1 for pixel perfect zoom. In case you need a non-pixel perfect, slower or faster zoom however, you can change this value. 0.5f for example will make the camera zoom half as fast as in pixel perfect mode. This value is currently tested only in perspective camera mode with translation based zoom.")]
        private float customZoomSensitivity = 1.0f;

        [SerializeField]
        [Tooltip(
            "Optional. When assigned, the terrain collider will be used to align items on the ground following the terrain.")]
        private TerrainCollider terrainCollider;

        [SerializeField]
        [Tooltip(
            "Optional. When assigned, the given transform will be moved and rotated instead of the one where this component is located on.")]
        private Transform cameraTransform;

        #endregion

        #region cam rotation tweakables

        [SerializeField]
        [Tooltip(
            "A gesture may be interpreted as intended rotation in case the relative rotation angle between 2 frames becomes bigger than this value.")]
        private float rotationDetectionDeltaThreshold = 0.25f;

        [SerializeField]
        [Tooltip(
            "Relative pinch distance must be bigger than this value in order to detect a rotation. This is to prevent errors that occur when the fingers are too close together to properly detect a clean rotation.")]
        private float rotationMinPinchDistance = 0.125f;

        [SerializeField]
        [Tooltip(
            "The rotation mode is enabled as soon as the rotation by the user becomes bigger than this value (in degrees). The value is used to prevent micro rotations from regular jittering of the fingers to be interpreted as rotation and helps keeping the camera more steady and less jittery.")]
        private float rotationLockThreshold = 2.5f;

        #endregion

        #region tilt tweakables

        [SerializeField]
        [Tooltip(
            "After this amount of finger-movement (relative to screen size), the pinch mode is decided. E.g. whether tilt mode or regular mode is used.")]
        private float pinchModeDetectionMoveTreshold = 0.025f;

        [SerializeField] [Tooltip("A threshold used to detect the up or down tilting motion.")]
        private float pinchTiltModeThreshold = 0.0075f;

        [SerializeField] [Tooltip("The tilt sensitivity once the tilt mode has started.")]
        private float pinchTiltSpeed = 180;

        #endregion

        private bool isStarted = false;

        public Camera Cam { get; private set; }

        private bool IsTranslationZoom
        {
            get { return (Cam.orthographic == false && perspectiveZoomMode == PerspectiveZoomMode.TRANSLATION); }
        }

        private bool canZooming = true;

        public bool CanZooming
        {
            get => canZooming;
            set => canZooming = value;
        }

        private bool canDrag = true;

        public bool CanDrag
        {
            get => canDrag;
            set => canDrag = value;
        }

        private bool canWheel = true;

        public bool CanWheel
        {
            get => canWheel;
            set => canWheel = value;
        }

        public float CustomZoomSensitivity
        {
            get => customZoomSensitivity;
        }

        //zlh 缓存当前帧的zoom 避免一帧内多次重复计算
        private float _lastCamZoom;

        //private int _lastCamZoomFrame;
        protected bool _lastCamZoomDirty = true;

        protected Vector3 dragDirection = Vector3.zero;
        protected Vector3 _lastDragPos = Vector3.zero;
        protected Vector3 _dragMoveVector = Vector3.zero;

        /// <summary>
        /// 世界地图缩放速度
        /// </summary>
        private float factorSpeed = 1;

        public float FactorSpeed
        {
            get { return factorSpeed; }
            set { factorSpeed = value; }
        }

        public float CamZoom
        {
            get
            {
                if (Cam.orthographic == true)
                {
                    return Cam.orthographicSize;
                }
                else
                {
                    switch (PerspectiveZoomMode)
                    {
                        case PerspectiveZoomMode.TRANSLATION:
                        {
                            if (!_lastCamZoomDirty)
                            {
                                return _lastCamZoom;
                            }

                            Vector3 camCenterIntersection = GetIntersectionPoint(GetCamCenterRay());
                            var zoom = (Vector3.Distance(camCenterIntersection, Transform.position));

                            _lastCamZoom = zoom;
                            _lastCamZoomDirty = false;

                            return zoom;
                        }
                        case PerspectiveZoomMode.TRANSLATION_Y:
                        {
                            return Transform.position.y;
                        }
                        case PerspectiveZoomMode.FIELD_OF_VIEW:
                        {
                            return Cam.fieldOfView;
                        }
                        default:
                            return 0;
                    }
                }
            }
            set
            {
                if (!canZooming)
                {
                    // Log.Info("====> Cam CanZoom = false");
                    return;
                }

                if (Cam.orthographic == true)
                {
                    Cam.orthographicSize = value;
                }
                else
                {
                    switch (PerspectiveZoomMode)
                    {
                        case PerspectiveZoomMode.TRANSLATION:
                        {
                            Vector3 camCenterIntersection = GetIntersectionPoint(GetCamCenterRay());
                            // patch : 进入剧情脚本enabled为false，同步zoom时不设置坐标，防止剧情镜头不对
                            if (enabled)
                            {
                                Transform.position = camCenterIntersection - Transform.forward * value;
                            }

                            _lastCamZoomDirty = true;
                            break;
                        }

                        case PerspectiveZoomMode.TRANSLATION_Y:
                        {
                            Vector3 camCenterIntersection = GetIntersectionPoint(GetCamCenterRay());
                            Vector3 pos = Transform.position;
                            Vector3 newPos = new Vector3(pos.x, value, pos.z);
                            Transform.position = newPos;
                            Transform.forward = (camCenterIntersection - newPos).normalized;
                            break;
                        }

                        case PerspectiveZoomMode.FIELD_OF_VIEW:
                        {
                            Cam.fieldOfView = value;
                            break;
                        }
                    }
                }

                ComputeCamBoundaries();
                _afterSetCameraPosition?.Invoke();
                _zoomingCallBack?.Invoke();
                currentZoom = CamZoom;
            }
        }

        private Action OnAutoMoveDone;
        private Vector3 startPos;
        private Vector3 midPosDiff = Vector3.zero;
        public Vector3 endPos;
        private float endRotY;

        private float startZoom;
        private float endZoom;
        protected bool aotoMove;

        public void reSetPosAndZoom()
        {
            startPos = transform.localPosition;
            startZoom = Math.Abs(startPos.z);
            CamZoom = startZoom;
        }

        public bool AotoMove
        {
            get => aotoMove;
            set => aotoMove = value;
        }

        private float autoMoveLerpTime;
        private float m_PredictTime;

        // private Tweener moveTweener = null;

        // 射线处理结果，防止GC
        PointerEventData pointer_eventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> ray_list = new List<RaycastResult>();

        public float CamOverdragMargin
        {
            get { return camOverdragMargin; }
            set { camOverdragMargin = value; }
        }

        public float AutoScrollVelocityMax
        {
            get { return autoScrollVelocityMax; }
            set { autoScrollVelocityMax = value; }
        }

        // /// <summary>
        // /// 加上一个dotween rotate
        // /// </summary>
        // public void DoTweenRotate(Vector3 rotate, float camRotateTime, Action completeAction = null)
        // {
        //     moveTweener = transform.DORotate(rotate, camRotateTime);
        //     moveTweener.onComplete += () => { completeAction(); };
        // }

        // /// <summary>
        // /// 加上一个dotween rotate
        // /// </summary>
        // public void DoTweenRotateY(float rotateYNum, float camRotateTime, Action completeAction = null)
        // {
        //     moveTweener = transform.DORotate(new Vector3(0f, rotateYNum, 0f), camRotateTime);
        //     moveTweener.onComplete += () => { completeAction?.Invoke(); };
        // }

        // public void DoTweenMoveX(float adjustEndX, float moveTime, Action completeAction = null)
        // {
        //     moveTweener = transform.DOMoveX(adjustEndX, moveTime);
        //     moveTweener.onComplete += () => { completeAction?.Invoke(); };
        // }
        //
        // public void MoveToPosY(float adjustEndY, float moveTime, Action completeAction = null)
        // {
        //     moveTweener = transform.DOMoveY(adjustEndY, moveTime);
        //     moveTweener.onComplete += () => { completeAction?.Invoke(); };
        // }

        /// <summary>
        /// 曲线运动
        /// </summary>
        /// <param name="target"></param>
        /// <param name="_midPosDiff">中心点的偏差值</param>
        /// <param name="zoom">缩放值，控制摄像机z值</param>
        /// <param name="_time">移动时间</param>
        /// <param name="rotY">最终摄像机的旋转角度</param>
        /// <param name="handle">移动结束后的返回</param>
        public void AutoCurveMove(Vector3 target, float zoom, float _time, Vector3 pos1Dis, float rotY = 0,
            Action handle = null)
        {
            //#if UNITY_EDITOR

            //#endif

            //这里限制了速度
            AotoMove = false;
            var curTarget = GetIntersectionPoint(GetCamCenterRay());
            if (target == curTarget && Mathf.Approximately(zoom, CamZoom))
            {
                handle?.Invoke();
                return;
            }

            target = AdjustTarget(target);

            autoMoveLerpTime = 0;
            m_PredictTime = Mathf.Clamp(_time, 0.02f, 2f);

            startZoom = CamZoom;

            // 装备制造房间的制造分页相机突破了最小的camZoom的限制。
            endZoom = zoom; //Mathf.Clamp(zoom, CamZoomMin, CamZoomMax);

            startPos = transform.position;
            endPos = target - Vector3.forward * endZoom;
            //中心点的偏差值，用来计算二阶贝塞尔曲线里面的p1点(暂时用终点和起点的z差值)
            var difPos = pos1Dis;
            if (rotY < 0 || rotY > 180) //rotY小于0往右弯，大于0就往左弯
            {
                midPosDiff = new Vector3(Mathf.Abs(endPos.x - startPos.x) / 2f, 0f, 0f) + difPos;
            }
            else
            {
                midPosDiff = new Vector3(-Mathf.Abs(endPos.x - startPos.x) / 2f, 0f, 0f) - difPos;
            }

            //计算直线xz平面的垂直向量,然后在线段的中心点加上difZ长度的垂直向量
            //var difV3 = endPos - startPos;
            //var verticalDir = Vector3.zero;
            //if (difV3.x.Equals(0))
            //{
            //    verticalDir = new Vector3(1f, 0, 0);
            //}
            //else
            //{
            //    verticalDir = new Vector3(-difV3.z / difV3.x, 0, 1f);
            //}
            //if (rotY < 0)//rotY小于0往右弯，大于0就往左弯
            //{
            //    midPosDiff = verticalDir.normalized * difZ;
            //}
            //else
            //{
            //    midPosDiff = -verticalDir.normalized * difZ;
            //}

            endRotY = rotY;

            OnAutoMoveDone = handle;
            AotoMove = true;
        }

        // 二阶贝塞尔曲线
        Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            Vector3 p0p1 = (1 - t) * p0 + t * p1;
            Vector3 p1p2 = (1 - t) * p1 + t * p2;
            Vector3 result = (1 - t) * p0p1 + t * p1p2;
            return result;
        }

        public void AutoLookAt(Vector3 target, float zoom = 0, float _time = 0, Action handle = null, float rotY = 0,
            bool hasLimit = true, bool isLockHor = false)
        {
            ////#if UNITY_EDITOR
            //if (CinemachineCameraManager.Instance != null)
            //{
            //    CinemachineCameraManager.Instance.EndFollow();
            //}

            ////#endif

            //这里限制了速度
            AotoMove = false;
            endZoom = hasLimit ? Mathf.Clamp(zoom, CamZoomMin, CamZoomMax) : zoom;
            //GameEntry.Event.Fire(this, EventId.TZ_MainScene_CameraOperationStart);

            var curTarget = GetIntersectionPoint(GetCamCenterRay());
            if (target == curTarget && Mathf.Approximately(zoom, Mathf.Abs(transform.position.z)))
            {
                handle?.Invoke();
                AutoMoveDoneCheckCameraZoom();
                //GameEntry.Event.Fire(this, EventId.OnMainCameraAutoMoveDoneEvent);
                return;
            }

            Action onComplete = () =>
            {
                target = AdjustTarget(target);

                autoMoveLerpTime = 0;
                m_PredictTime = Mathf.Clamp(_time, 0.02f, 2f);

                startZoom = CamZoom;


                startPos = transform.position;
                midPosDiff = Vector3.zero;
                //Modify
                //if (this.dragState == DragState.Down)
                //{
                //    endPos = target - Vector3.forward * endZoom;
                //}
                //else
                {
                    if (isLockHor)
                    {
                        endPos = target;
                        endPos.z = -endZoom;
                    }
                    else
                    {
                        endPos = target - Transform.forward * endZoom;
                    }
                }

                //M end
                endRotY = rotY;
                OnAutoMoveDone = handle;
            };

            if (this.dragState == DragState.Down)
            {
                this.dragDirection = Vector3.zero;
                onComplete();
                AotoMove = true;
            }
            else
            {
                onComplete();
                AotoMove = true;
            }
        }

        public void AutoLookAtByRoomManager(Vector3 target, float zoom = 0, float _time = 0, Action handle = null,
            float rotY = 0, bool hasLimit = true)
        {
            ////#if UNITY_EDITOR
            //if (CinemachineCameraManager.Instance != null)
            //{
            //    CinemachineCameraManager.Instance.EndFollow();
            //}

            ////#endif

            //这里限制了速度
            AotoMove = false;
            endZoom = hasLimit ? Mathf.Clamp(zoom, CamZoomMin, CamZoomMax) : zoom;

            var curTarget = GetIntersectionPoint(GetCamCenterRay());
            if (target == curTarget && Mathf.Approximately(zoom, Mathf.Abs(transform.position.z)))
            {
                handle?.Invoke();
                return;
            }

            Action onComplete = () =>
            {
                target = AdjustTarget(target);

                autoMoveLerpTime = 0;
                m_PredictTime = Mathf.Clamp(_time, 0.02f, 2f);

                startZoom = CamZoom;


                startPos = transform.position;
                midPosDiff = Vector3.zero;
                //Modify(城内与世界的endPos计算方法不一样)
                //endPos = target - Transform.forward * endZoom;
                endPos = target - Vector3.forward * endZoom;
                //M end
                endRotY = rotY;
                OnAutoMoveDone = handle;
            };
            if (this.dragState == DragState.Down)
            {
                this.dragDirection = Vector3.zero;
                onComplete();
                AotoMove = true;
            }
            else
            {
                onComplete();
                AotoMove = true;
            }
        }

        public void AutoLookAt_CityBuilding(Vector3 target, float _time = 0, float zoom = 0, Action handle = null,
            float rotY = 0, bool hasLimit = true)
        {
            //这里限制了速度
            AotoMove = false;

            var curTarget = GetIntersectionPoint(GetCamCenterRay());
            if (target == curTarget)
            {
                handle?.Invoke();
                return;
            }

            Action onComplete = () =>
            {
                target = AdjustTarget(target);

                autoMoveLerpTime = 0;
                m_PredictTime = Mathf.Clamp(_time, 0.02f, 2f);

                startPos = transform.position;
                midPosDiff = Vector3.zero;
                endZoom = Mathf.Clamp(zoom, CamZoomMin, CamZoomMax);
                endPos = target - Vector3.forward * endZoom;

                OnAutoMoveDone = handle;
            };
            if (this.dragState == DragState.Down)
            {
                this.dragDirection = Vector3.zero;
                onComplete();
                AotoMove = true;
            }
            else
            {
                onComplete();
                AotoMove = true;
            }
        }

        public bool AutoLookAt_Origin(Vector3 target, float zoom = 0, float _time = 0, Action handle = null)
        {
            //这里限制了速度
            AotoMove = false;
            var curTarget = GetIntersectionPoint(GetCamCenterRay());
            if (target == curTarget && Mathf.Approximately(zoom, CamZoom))
            {
                handle?.Invoke();
                return false;
            }

            target = AdjustTarget(target);
            var realCamPos = GetClampToBoundaries(target);

            if (Vector3.Distance(transform.position, realCamPos) < 0.1)
            {
                return false;
            }

            autoMoveLerpTime = 0;
            m_PredictTime = Mathf.Clamp(_time, 0.02f, 4f);

            startZoom = CamZoom;
            endZoom = Mathf.Clamp(zoom, CamZoomMin, CamZoomMax);

            startPos = transform.position;
            endPos = target - Transform.forward * endZoom;

            OnAutoMoveDone = handle;
            AotoMove = true;
            return true;
        }

        public void StopAutoMove()
        {
            AotoMove = false;
        }

        public void AutoMoveTo(Vector3 target, float zoom = 0, float _time = 0, Action handle = null)
        {
            //#if UNITY_EDITOR

            //#endif


            //这里限制了速度
            AotoMove = false;
            var curTarget = GetIntersectionPoint(GetCamCenterRay());
            if (target == curTarget && Mathf.Approximately(zoom, CamZoom))
            {
                handle?.Invoke();
                return;
            }

            target = AdjustTarget(target);

            autoMoveLerpTime = 0;
            m_PredictTime = _time; //Mathf.Clamp(_time, 0.02f, 2f);

            startZoom = CamZoom;
            endZoom = Mathf.Clamp(zoom, CamZoomMin, CamZoomMax);

            endRotY = 0f;

            startPos = transform.position;
            midPosDiff = Vector3.zero;
            endPos = target - Vector3.forward * endZoom;

            OnAutoMoveDone = handle;
            if (this.dragState == DragState.Down)
            {
                this.dragDirection = Vector3.zero;

                AotoMove = true;
            }
            else
            {
                AotoMove = true;
            }
        }

        private Vector3 AdjustTarget(Vector3 target)
        {
            if (Cam.orthographic)
            {
                if (cameraAxes == CameraPlaneAxes.XZ_TOP_DOWN)
                    target.y = 0;
                else
                    target.z = 0;
            }
            else
            {
                if (cameraAxes == CameraPlaneAxes.XZ_TOP_DOWN)
                {
                    if (Mathf.Abs(target.y) > Mathf.Epsilon)
                    {
                        target = GetIntersectionPoint(new Ray(target,
                            target.y >= 0 ? transform.forward : -1 * transform.forward));
                    }
                }
                else if (cameraAxes == CameraPlaneAxes.X_SIDSCROLL)
                {
                    target.y = _initCamPos.y;
                    target.z = _initCamPos.z;
                    target.x = Mathf.Clamp(target.x, CamPosMin.x, CamPosMax.x);
                }
                else
                {
                    if (Mathf.Abs(target.z) > Mathf.Epsilon)
                    {
                        target = GetIntersectionPoint(new Ray(target,
                            target.z >= 0 ? transform.forward : -1 * transform.forward));
                    }
                }
            }

            return target;
        }

        /// <summary>
        /// Looks at target.
        /// </summary>
        /// <param name="target">要看向的世界坐标</param>
        public bool LookAt(Vector3 target)
        {
            //#if UNITY_EDITOR

            //#endif
            AotoMove = false;
            var curTarget = GetIntersectionPoint(GetCamCenterRay());
            target = AdjustTarget(target);
            var offset = target - curTarget;
            var lookAtPos = transform.position + offset;
            //这个地方限制一下摄像机的z值  //这样限制会影响世界相机 临时加个条件
            //if (SceneContainer.Instance.CurrentSceneID == SceneContainer.SceneID.MainCity)
            //{
            //    lookAtPos.z = Mathf.Clamp(lookAtPos.z, -CamZoomMax, -CamZoomMin);
            //}

            var realCamPos = GetClampToBoundaries(lookAtPos);

            if (Vector3.Distance(transform.position, realCamPos) < 0.1)
            {
                return false;
            }
            transform.position = lookAtPos;
            return true;
        }

        /// <summary>
        /// Gets the init intersection point.
        /// </summary>
        /// <returns>The init intersection point.</returns>
        public Vector3 GetInitIntersectionPoint()
        {
            Ray ray = new Ray(Transform.position, Transform.forward);

            bool success = RefPlane.Raycast(ray, out float distance);
            if (success == false)
            {
                Log.Error("InitIntersection is fail.");
            }

            return (ray.direction * distance);
        }

        /// <summary>
        /// Gets the world point.
        /// </summary>
        /// <returns>The world point.</returns>
        public Vector3 GetWorldPoint()
        {
            return GetIntersectionPoint(GetCamCenterRay());
        }

        public float CamZoomMin
        {
            get { return (camZoomMin); }
            set
            {
                camZoomMin = value;
                targetZoomMin = camZoomMin;
            }
        }

        public float CamZoomMax
        {
            get { return (camZoomMax); }
            set
            {
                // if (ShipModule.Instance.IsInMainCity())
                // {
                //     camZoomMax = Mathf.Clamp(value, 10f, 50f);
                // }
                // else
                // {
                camZoomMax = value;
                // }

                targetZoomMax = camZoomMax;
            }
        }

        private float targetZoomMin;

        public float TargetZoomMin
        {
            set
            {
                //if (GameEntry.SceneContainer.IsInMainCity())
                // if (ShipModule.Instance.IsInMainCity())
                if (worldType == WorldType.ShipWorld)
                    targetZoomMin = Mathf.Clamp(value, 0.2f, 10f);
                else
                    targetZoomMin = value;
            }
            get { return targetZoomMin; }
        }

        private float targetZoomMax;

        public float TargetZoomMax
        {
            set
            {
                //if (GameEntry.SceneContainer.IsInMainCity())
                // if(ShipModule.Instance.IsInMainCity())
                if (worldType == WorldType.ShipWorld)
                    targetZoomMax = Mathf.Clamp(value, 10f, 50f);
                else
                    targetZoomMax = value;
            }
            get { return targetZoomMax; }
        }

        public float CamOverzoomMargin
        {
            get { return (camOverzoomMargin); }
            set { camOverzoomMargin = value; }
        }

        public float CamFollowFactor
        {
            get { return (camFollowFactor); }
            set { camFollowFactor = value; }
        }

        public float AutoScrollDamp
        {
            get { return autoScrollDamp; }
            set { autoScrollDamp = value; }
        }

        public AnimationCurve AutoScrollDampCurve
        {
            get { return (autoScrollDampCurve); }
            set { autoScrollDampCurve = value; }
        }

        public float DampFactorTimeMultiplier
        {
            get { return (dampFactorTimeMultiplier); }
        }

        public float GroundLevelOffset
        {
            get { return groundLevelOffset; }
            set { groundLevelOffset = value; }
        }

        public Vector2 BoundaryMin
        {
            get { return boundaryMin; }
            set
            {
                boundaryMin = value;
                ComputeCamBoundaries();
            }
        }

        public Vector2 BoundaryMax
        {
            get { return boundaryMax; }
            set
            {
                boundaryMax = value;
                ComputeCamBoundaries();
            }
        }

        public PerspectiveZoomMode PerspectiveZoomMode
        {
            get { return (perspectiveZoomMode); }
            set { perspectiveZoomMode = value; }
        }

        public bool EnableRotation
        {
            get { return enableRotation; }
            set { enableRotation = value; }
        }

        public bool EnableTilt
        {
            get { return enableTilt; }
            set { enableTilt = value; }
        }

        public float TiltAngleMin
        {
            get { return tiltAngleMin; }
            set { tiltAngleMin = value; }
        }

        public float TiltAngleMax
        {
            get { return tiltAngleMax; }
            set { tiltAngleMax = value; }
        }

        public bool EnableZoomTilt
        {
            get { return enableZoomTilt; }
            set { enableZoomTilt = value; }
        }

        public float ZoomTiltAngleMin
        {
            get { return zoomTiltAngleMin; }
            set { zoomTiltAngleMin = value; }
        }

        public float ZoomTiltAngleMax
        {
            get { return zoomTiltAngleMax; }
            set { zoomTiltAngleMax = value; }
        }

        protected bool isDraggingSceneObject;

        private Plane refPlaneXY_1 = new Plane(new Vector3(0, 0, 1), 0);
        private Plane refPlaneXY = new Plane(new Vector3(0, 0, -1), 0);
        private Plane refPlaneXZ = new Plane(new Vector3(0, 1, 0), 0);

        public Plane RefPlane
        {
            get
            {
                if (CameraAxes == CameraPlaneAxes.XZ_TOP_DOWN)
                {
                    return refPlaneXZ;
                }
                else
                {
                    return refPlaneXY;
                }
            }
        }

        protected List<Vector3> DragCameraMoveVector { get; set; }
        private const int momentumSamplesCount = 5;

        private const float pinchDistanceForTiltBreakout = 0.05f;
        private const float pinchAccumBreakout = 0.025f;

        protected Vector3 targetPositionClamped = Vector3.zero;

        public bool IsSmoothingEnabled { get; set; }

        private float ScreenRatio { get; set; }

        [SerializeField] public Vector2 CamPosMin;
        [SerializeField] public Vector2 CamPosMax;

        public TerrainCollider TerrainCollider
        {
            get { return terrainCollider; }
            set { terrainCollider = value; }
        }

        #region work in progress //Features that are currently being worked on, but not fully polished and documented yet. Use them at your own risk.

        private bool
            enableOvertiltSpring =
                false; //Allows to enable the camera to spring being when being tilted over the limits.

        private float camOvertiltMargin = 5.0f;
        private float tiltBackSpringFactor = 30;

        private float
            minOvertiltSpringPositionThreshold =
                0.1f; //This value is necessary to reposition the camera and do boundary update computations while the auto spring back from overtilt is active and larger than this value.

        private Vector3 _initCamPos = Vector3.zero;

        public Vector3 InitCamPos
        {
            get { return _initCamPos; }
            set { _initCamPos = value; }
        }

        #endregion

        protected void Awake()
        {
            if (cameraTransform != null)
            {
                cachedTransform = cameraTransform;
            }

            _initCamPos = transform.position;

            Cam = GetComponent<Camera>();

            IsSmoothingEnabled = true;
            touchInputController = GetComponent<TouchInputController>();
            dragStartCamPos = Vector3.zero;
            cameraScrollVelocity = Vector3.zero;
            timeRealDragStop = 0;
            pinchStartCamZoomSize = 0;
            IsPinching = false;
            IsDragging = false;
            DragCameraMoveVector = new List<Vector3>();
            refPlaneXY = new Plane(new Vector3(0, 0, -1), groundLevelOffset);
            refPlaneXZ = new Plane(new Vector3(0, 1, 0), -groundLevelOffset);
            ScreenRatio = GetScreenRatio();
            if (EnableZoomTilt == true)
            {
                ResetZoomTilt();
            }

            ComputeCamBoundaries();

            if (CamZoomMax < CamZoomMin)
            {
                Log.Warning("The defined max camera zoom (" + CamZoomMax + ") is smaller than the defined min (" +
                            CamZoomMin + "). Automatically switching the values.");
                float camZoomMinBackup = CamZoomMin;
                CamZoomMin = CamZoomMax;
                CamZoomMax = camZoomMinBackup;
            }

            TargetZoomMax = CamZoomMax;
            TargetZoomMin = CamZoomMin;

            //Errors for certain incorrect settings.
            string cameraAxesError = CheckCameraAxesErrors();
            if (string.IsNullOrEmpty(cameraAxesError) == false)
            {
                Log.Error(cameraAxesError);
            }
        }

        public void Start()
        {
            touchInputController.OnInputClick += InputControllerOnInputClick;
            touchInputController.OnDragStart += InputControllerOnDragStart;
            touchInputController.OnDragUpdate += InputControllerOnDragUpdate;
            touchInputController.OnDragStop += InputControllerOnDragStop;
            touchInputController.OnFingerDown += InputControllerOnFingerDown;
            touchInputController.OnFingerUp += InputControllerOnFingerUp;
            touchInputController.OnPinchStart += InputControllerOnPinchStart;
            touchInputController.OnPinchUpdateExtended += InputControllerOnPinchUpdate;
            touchInputController.OnPinchStop += InputControllerOnPinchStop;
            isStarted = true;
            StartCoroutine(InitCamBoundariesDelayed());
        }

        private IEnumerator InitCamBoundariesDelayed()
        {
            yield return null;
            ComputeCamBoundaries();
        }

        public void OnDestroy()
        {
            if (isStarted)
            {
                touchInputController.OnInputClick -= InputControllerOnInputClick;
                touchInputController.OnDragStart -= InputControllerOnDragStart;
                touchInputController.OnDragUpdate -= InputControllerOnDragUpdate;
                touchInputController.OnDragStop -= InputControllerOnDragStop;
                touchInputController.OnFingerDown -= InputControllerOnFingerDown;
                touchInputController.OnFingerUp -= InputControllerOnFingerUp;
                touchInputController.OnPinchStart -= InputControllerOnPinchStart;
                touchInputController.OnPinchUpdateExtended -= InputControllerOnPinchUpdate;
                touchInputController.OnPinchStop -= InputControllerOnPinchStop;
            }
        }

        /// <summary>
        /// MonoBehaviour method override to assign proper default values depending on
        /// the camera parameters and orientation.
        /// </summary>
        private void Reset()
        {
            //Compute camera tilt to find out the camera orientation.
            Vector3 camForwardOnPlane = Vector3.Cross(Vector3.up, GetTiltRotationAxis());
            float tiltAngle = Vector3.Angle(camForwardOnPlane, -Transform.forward);
            if (tiltAngle < 45)
            {
                CameraAxes = CameraPlaneAxes.XY_2D_SIDESCROLL;
            }
            else
            {
                CameraAxes = CameraPlaneAxes.XZ_TOP_DOWN;
            }

            //Compute zoom default values based on the camera type.
            Camera cameraComponent = GetComponent<Camera>();
            if (cameraComponent.orthographic == true)
            {
                CamZoomMin = 4;
                CamZoomMax = 13;
                CamOverzoomMargin = 1;
            }
            else
            {
                CamZoomMin = 5;
                CamZoomMax = 40;
                CamOverzoomMargin = 3;
                PerspectiveZoomMode = PerspectiveZoomMode.TRANSLATION;
            }
        }

        /// <summary>
        /// Method for resetting the camera boundaries. This method may need to be invoked
        /// when resetting the camera transform (rotation, tilt) by code for example.
        /// </summary>
        public void ResetCameraBoundaries()
        {
            ComputeCamBoundaries();
        }

        /// <summary>
        /// This method tilts the camera based on the values
        /// defined for the zoom tilt mode.
        /// </summary>
        public void ResetZoomTilt()
        {
            UpdateTiltForAutoTilt(CamZoom);
        }

        /// <summary>
        /// Helper method for retrieving the world position of the
        /// finger with id 0. This method may only return a valid value when
        /// there is at least 1 finger touching the device.
        /// </summary>
        public Vector3 GetFinger0PosWorld()
        {
            Vector3 posWorld = Vector3.zero;
            if (TouchWrapper.TouchCount > 0)
            {
                Vector3 fingerPos = TouchWrapper.Touch0.Position;
                RaycastGround(Cam.ScreenPointToRay(fingerPos), out posWorld);
            }

            return (posWorld);
        }

        /// <summary>
        /// Method for performing a raycast against either the refplane, or
        /// against a terrain-collider in case the collider is set.
        /// </summary>
        public bool RaycastGround(Ray ray, out Vector3 hitPoint)
        {
            bool hitSuccess = false;
            hitPoint = Vector3.zero;
            if (TerrainCollider != null)
            {
                RaycastHit hitInfo;
                hitSuccess = TerrainCollider.Raycast(ray, out hitInfo, Mathf.Infinity);
                if (hitSuccess == true)
                {
                    hitPoint = hitInfo.point;
                }
            }
            else
            {
                hitSuccess = RefPlane.Raycast(ray, out float hitDistance);
                if (hitSuccess == true)
                {
                    hitPoint = ray.GetPoint(hitDistance);
                }
            }

            return hitSuccess;
        }

        /// <summary>
        /// Method for retrieving the intersection-point between the given ray and the ref plane.
        /// </summary>
        public Vector3 GetIntersectionPoint(Ray ray)
        {
            bool success = RefPlane.Raycast(ray, out float distance);
            if (success == false)
            {
                //Log.Info("Ray = {0} {1}", ray, CameraAxes);
                //Log.Error("Failed to compute intersection between camera ray and reference plane. Make sure the camera Axes are set up correctly.");
            }

            return (ray.origin + ray.direction * distance);
        }

        /// <summary>
        /// Custom planet intersection method that doesn't take into account rays parallel to the plane or rays shooting in the wrong direction and thus never hitting.
        /// May yield slightly better performance however and should be safe for use when the camera setup is correct (e.g. axes set correctly in this script, and camera actually pointing towards floor).
        /// </summary>
        public Vector3 GetIntersectionPointUnsafe(Ray ray)
        {
            float distance = Vector3.Dot(RefPlane.normal, Vector3.zero - ray.origin) /
                             Vector3.Dot(RefPlane.normal, (ray.origin + ray.direction) - ray.origin);
            return (ray.origin + ray.direction * distance);
        }

        /// <summary>
        /// Method that does all the computation necessary when the pinch gesture of the user
        /// has changed.
        /// </summary>
        public void UpdatePinch(float deltaTime)
        {
            if (!pinchEnabled)
            {
                return;
            }

            if (IsPinching == true)
            {

                if (isTiltModeEvaluated == true)
                {
                    if (isPinchTiltMode == true || isPinchModeExclusive == false)
                    {
                        //Tilt
                        float pinchTiltDelta = pinchTiltLastFrame - pinchTiltCurrent;
                        UpdateCameraTilt(pinchTiltDelta * pinchTiltSpeed);
                        pinchTiltLastFrame = pinchTiltCurrent;
                    }

                    if (isPinchTiltMode == false || isPinchModeExclusive == false)
                    {
                        if (isRotationActivated == true && isRotationLock == true &&
                            Mathf.Abs(pinchAngleCurrent) >= rotationLockThreshold)
                        {
                            isRotationLock = false;
                        }

                        if (IsSmoothingEnabled == true)
                        {
                            float lerpFactor = Mathf.Clamp01(Time.deltaTime * camFollowFactor);
                            pinchDistanceCurrentLerp =
                                Mathf.Lerp(pinchDistanceCurrentLerp, pinchDistanceCurrent, lerpFactor);
                            pinchCenterCurrentLerp =
                                Vector3.Lerp(pinchCenterCurrentLerp, pinchCenterCurrent, lerpFactor);
                            if (isRotationLock == false)
                            {
                                pinchAngleCurrentLerp =
                                    Mathf.Lerp(pinchAngleCurrentLerp, pinchAngleCurrent, lerpFactor);
                            }
                        }
                        else
                        {
                            pinchDistanceCurrentLerp = pinchDistanceCurrent;
                            pinchCenterCurrentLerp = pinchCenterCurrent;
                            if (isRotationLock == false)
                            {
                                pinchAngleCurrentLerp = pinchAngleCurrent;
                            }
                        }

                        //Rotation
                        if (isRotationActivated == true && isRotationLock == false)
                        {
                            float pinchAngleDelta = pinchAngleCurrentLerp - pinchAngleLastFrame;
                            Vector3 rotationAxis = GetRotationAxis();
                            Transform.RotateAround(pinchCenterCurrent, rotationAxis, pinchAngleDelta);
                            pinchAngleLastFrame = pinchAngleCurrentLerp;
                            ComputeCamBoundaries();
                        }

                        //Zoom
                        float zoomFactor = (pinchDistanceStart /
                                            Mathf.Max(
                                                ((pinchDistanceCurrentLerp - pinchDistanceStart) *
                                                 customZoomSensitivity) + pinchDistanceStart, 0.0001f));
                        float cameraSize = pinchStartCamZoomSize * zoomFactor;
                        cameraSize = Mathf.Clamp(cameraSize, camZoomMin - camOverzoomMargin,
                            camZoomMax + camOverzoomMargin);
                        if (enableZoomTilt == true)
                        {
                            UpdateTiltForAutoTilt(cameraSize);
                        }

                        CamZoom = cameraSize;
                        PinchinggCallback?.Invoke();
                    }

                    //Position update.
                    DoPositionUpdateForTilt(false);
                }
            }
            else
            {
                //Spring back.
                if (EnableTilt == true && enableOvertiltSpring == true)
                {
                    float overtiltSpringValue = ComputeOvertiltSpringBackFactor(camOvertiltMargin);
                    if (Mathf.Abs(overtiltSpringValue) > minOvertiltSpringPositionThreshold)
                    {
                        UpdateCameraTilt(overtiltSpringValue * deltaTime * tiltBackSpringFactor);
                        DoPositionUpdateForTilt(true);
                    }
                }
            }
        }

        private void UpdateTiltForAutoTilt(float newCameraSize)
        {
            float zoomProgress = Mathf.Clamp01((newCameraSize - camZoomMin) / (camZoomMax - camZoomMin));
            float tiltTarget = Mathf.Lerp(zoomTiltAngleMin, zoomTiltAngleMax, zoomProgress);
            float tiltAngleDiff = tiltTarget - GetCurrentTiltAngleDeg(GetTiltRotationAxis());
            UpdateCameraTilt(tiltAngleDiff);
        }

        /// <summary>
        /// Method that computes the updated camera position when the user tilts the camera.
        /// </summary>
        private void DoPositionUpdateForTilt(bool isSpringBack)
        {
            //Position update.
            Vector3 intersectionDragCurrent;
            if (isSpringBack == true || (isPinchTiltMode == true && isPinchModeExclusive == true))
            {
                intersectionDragCurrent =
                    GetIntersectionPoint(
                        GetCamCenterRay()); //In exclusive tilt mode always rotate around the screen center.
            }
            else
            {
                intersectionDragCurrent = GetIntersectionPoint(Cam.ScreenPointToRay(pinchCenterCurrentLerp));
            }

            Vector3 dragUpdateVector = intersectionDragCurrent - pinchStartIntersectionCenter;
            if (isSpringBack == true && isPinchModeExclusive == false)
            {
                dragUpdateVector = Vector3.zero;
            }

            Vector3 targetPos = GetClampToBoundaries(Transform.position - dragUpdateVector);

            Transform.position =
                targetPos; //Disable smooth follow for the pinch-move update to prevent oscillation during the zoom phase.
            SetTargetPosition(targetPos);
        }

        /// <summary>
        /// Helper method for computing the tilt spring back.
        /// </summary>
        private float ComputeOvertiltSpringBackFactor(float margin)
        {
            float springBackValue = 0;
            Vector3 rotationAxis = GetTiltRotationAxis();
            float tiltAngle = GetCurrentTiltAngleDeg(rotationAxis);
            if (tiltAngle < tiltAngleMin + margin)
            {
                springBackValue = (tiltAngleMin + margin) - tiltAngle;
            }
            else if (tiltAngle > tiltAngleMax - margin)
            {
                springBackValue = (tiltAngleMax - margin) - tiltAngle;
            }

            return springBackValue;
        }

        /// <summary>
        /// Method that computes all necessary parameters for a tilt update caused by the user's tilt gesture.
        /// </summary>
        private void UpdateCameraTilt(float angle)
        {
            Vector3 rotationAxis = GetTiltRotationAxis();
            Vector3 rotationPoint = GetIntersectionPoint(new Ray(Transform.position, Transform.forward));
            Transform.RotateAround(rotationPoint, rotationAxis, angle);
            ClampCameraTilt(rotationPoint, rotationAxis);
            ComputeCamBoundaries();
        }

        /// <summary>
        /// Method that ensures that all limits are met when the user tilts the camera.
        /// </summary>
        private void ClampCameraTilt(Vector3 rotationPoint, Vector3 rotationAxis)
        {
            float tiltAngle = GetCurrentTiltAngleDeg(rotationAxis);
            if (tiltAngle < tiltAngleMin)
            {
                float tiltClampDiff = tiltAngleMin - tiltAngle;
                Transform.RotateAround(rotationPoint, rotationAxis, tiltClampDiff);
            }
            else if (tiltAngle > tiltAngleMax)
            {
                float tiltClampDiff = tiltAngleMax - tiltAngle;
                Transform.RotateAround(rotationPoint, rotationAxis, tiltClampDiff);
            }
        }

        /// <summary>
        /// Method to get the current tilt angle of the camera.
        /// </summary>
        private float GetCurrentTiltAngleDeg(Vector3 rotationAxis)
        {
            Vector3 camForwardOnPlane = Vector3.Cross(RefPlane.normal, rotationAxis);
            float tiltAngle = Vector3.Angle(camForwardOnPlane, -Transform.forward);
            return (tiltAngle);
        }

        /// <summary>
        /// Returns the rotation axis of the camera. This purely depends
        /// on the defined camera axis.
        /// </summary>
        private Vector3 GetRotationAxis()
        {
            return (RefPlane.normal);
        }

        /// <summary>
        /// Returns the rotation of the camera.
        /// </summary>
        private float GetRotationDeg()
        {
            if (CameraAxes == CameraPlaneAxes.XY_2D_SIDESCROLL)
            {
                return (Transform.rotation.eulerAngles.z);
            }
            else
            {
                return (Transform.rotation.eulerAngles.y);
            }
        }

        /// <summary>
        /// Returns the tilt rotation axis.
        /// </summary>
        private Vector3 GetTiltRotationAxis()
        {
            Vector3 rotationAxis = Transform.right;
            return (rotationAxis);
        }

        /// <summary>
        /// Method to compute all the necessary updates when the user moves the camera.
        /// </summary>
        public void UpdatePosition(float deltaTime)
        {
            if (IsPinching == true && isPinchTiltMode == true)
            {
                return;
            }

            if (IsDragging == true || IsPinching == true)
            {
                Vector3 posOld = Transform.position;
                if (IsSmoothingEnabled == true)
                {

                    Transform.position = Vector3.Lerp(Transform.position, targetPositionClamped,
                        Mathf.Clamp01(Time.deltaTime * camFollowFactor));
                    AfterSetCameraPosition?.Invoke();
                }
                else
                {
                    Transform.position = targetPositionClamped;
                    AfterSetCameraPosition?.Invoke();
                }

                DragCameraMoveVector.Add((posOld - Transform.position) / Time.deltaTime);
                if (DragCameraMoveVector.Count > momentumSamplesCount)
                {
                    DragCameraMoveVector.RemoveAt(0);
                }
            }

            Vector2 autoScrollVector = -cameraScrollVelocity * deltaTime;
            Vector3 camPos = Transform.position;
            switch (cameraAxes)
            {
                case CameraPlaneAxes.XY_2D_SIDESCROLL:
                    camPos.x += autoScrollVector.x;
                    camPos.y += autoScrollVector.y;
                    break;
                case CameraPlaneAxes.XZ_TOP_DOWN:
                    camPos.x += autoScrollVector.x;
                    camPos.z += autoScrollVector.y;
                    break;
                case CameraPlaneAxes.X_SIDSCROLL:
                    if (!AotoMove)
                    {
                        camPos.y = _initCamPos.y;
                    }
                    break;
            }

            if (IsDragging == false && IsPinching == false)
            {
                Vector3 overdragSpringVector =
                    ComputeOverdragSpringBackVector(camPos, camOverdragMargin, ref cameraScrollVelocity);
                if (overdragSpringVector.magnitude > float.Epsilon)
                {
                    camPos += Time.deltaTime * overdragSpringVector * dragBackSpringFactor;
                }
            }

            Transform.position = GetClampToBoundaries(camPos);

            //if(IsDragging == false && IsPinching == false)
            //    KeyboardInputController.MoveCameraByWASD(Transform, GameEntry.SceneContainer.CurrentSceneID == SceneContainer.SceneID.MainCity);

            if (lastCamPosition != Transform.position)
            {
                AfterSetCameraPosition?.Invoke();
                lastCamPosition = Transform.position;

                //LateUpdateForSpecialUi();
            }
        }

        /// <summary>
        /// Computes the camera drag spring back when the user is close to a boundary.
        /// </summary>
        private Vector3 ComputeOverdragSpringBackVector(Vector3 camPos, float margin,
            ref Vector3 currentCamScrollVelocity)
        {
            Vector3 springBackVector = Vector3.zero;
            if (camPos.x < CamPosMin.x + margin)
            {
                springBackVector.x = (CamPosMin.x + margin) - camPos.x;
                currentCamScrollVelocity.x = 0;
            }
            else if (camPos.x > CamPosMax.x - margin)
            {
                springBackVector.x = (CamPosMax.x - margin) - camPos.x;
                currentCamScrollVelocity.x = 0;
            }

            switch (cameraAxes)
            {
                case CameraPlaneAxes.XY_2D_SIDESCROLL:
                    if (camPos.y < CamPosMin.y + margin)
                    {
                        springBackVector.y = (CamPosMin.y + margin) - camPos.y;
                        currentCamScrollVelocity.y = 0;
                    }
                    else if (camPos.y > CamPosMax.y - margin)
                    {
                        springBackVector.y = (CamPosMax.y - margin) - camPos.y;
                        currentCamScrollVelocity.y = 0;
                    }

                    break;
                case CameraPlaneAxes.XZ_TOP_DOWN:
                    if (camPos.z < CamPosMin.y + margin)
                    {
                        springBackVector.z = (CamPosMin.y + margin) - camPos.z;
                        currentCamScrollVelocity.z = 0;
                    }
                    else if (camPos.z > CamPosMax.y - margin)
                    {
                        springBackVector.z = (CamPosMax.y - margin) - camPos.z;
                        currentCamScrollVelocity.z = 0;
                    }

                    break;
            }

            return springBackVector;
        }

        /// <summary>
        /// Internal helper method for setting the desired cam position.
        /// </summary>
        protected void SetTargetPosition(Vector3 newPositionClamped)
        {
            targetPositionClamped = newPositionClamped;
        }

        /// <summary>
        /// Returns whether or not the camera is at the defined boundary.
        /// </summary>
        public virtual bool GetIsBoundaryPosition(Vector3 testPosition)
        {
            bool isBoundaryPosition = false;
            switch (cameraAxes)
            {
                case CameraPlaneAxes.XY_2D_SIDESCROLL:
                    isBoundaryPosition = testPosition.x <= CamPosMin.x;
                    isBoundaryPosition |= testPosition.x >= CamPosMax.x;
                    isBoundaryPosition |= testPosition.y <= CamPosMin.y;
                    isBoundaryPosition |= testPosition.y >= CamPosMax.y;
                    break;
                case CameraPlaneAxes.XZ_TOP_DOWN:
                    isBoundaryPosition = testPosition.x <= CamPosMin.x;
                    isBoundaryPosition |= testPosition.x >= CamPosMax.x;
                    isBoundaryPosition |= testPosition.z <= CamPosMin.y;
                    isBoundaryPosition |= testPosition.z >= CamPosMax.y;
                    break;
            }

            return (isBoundaryPosition);
        }

        /// <summary>
        /// Returns a position that is clamped to the defined boundary.
        /// </summary>
        public virtual Vector3 GetClampToBoundaries(Vector3 newPosition, bool includeSpringBackMargin = false)
        {
            float margin = 0;
            if (includeSpringBackMargin == true)
            {
                margin = camOverdragMargin;
            }

            switch (cameraAxes)
            {
                case CameraPlaneAxes.XY_2D_SIDESCROLL:
                    newPosition.x = Mathf.Clamp(newPosition.x, CamPosMin.x + margin, CamPosMax.x - margin);
                    newPosition.y = Mathf.Clamp(newPosition.y, CamPosMin.y + margin, CamPosMax.y - margin);
                    break;
                case CameraPlaneAxes.XZ_TOP_DOWN:
                    newPosition.x = Mathf.Clamp(newPosition.x, CamPosMin.x + margin, CamPosMax.x - margin);
                    newPosition.z = Mathf.Clamp(newPosition.z, CamPosMin.y + margin, CamPosMax.y - margin);
                    break;
                case CameraPlaneAxes.X_SIDSCROLL:
                    newPosition.x = Mathf.Clamp(newPosition.x, CamPosMin.x + margin, CamPosMax.x - margin);
                    newPosition.y = _initCamPos.y;
                    break;

            }

            return (newPosition);
        }

        /// <summary>
        /// Rotates a Vector2 by the given degrees.
        /// </summary>
        private Vector2 RotateVector2(Vector2 v, float degrees)
        {
            Vector2 vNormalized = v.normalized;
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = vNormalized.x;
            float ty = vNormalized.y;
            vNormalized.x = (cos * tx) - (sin * ty);
            vNormalized.y = (sin * tx) + (cos * ty);
            return vNormalized * v.magnitude;
        }

        /// <summary>
        /// Method that computes the cam boundaries used for the current rotation and tilt of the camera.
        /// This computation is complex and needs to be invoked when the camera is rotated or tilted.
        /// </summary>
        protected virtual void ComputeCamBoundaries()
        {
            if (null == Cam)
                return;
            float camRotation = GetRotationDeg();

            Vector2 camProjectedMin = Vector2.zero;
            Vector2 camProjectedMax = Vector2.zero;
            Vector2
                camProjectedCenter =
                    GetIntersection2d(new Ray(Transform.position,
                        -RefPlane
                            .normal)); //Get camera position projected vertically onto the ref plane. This allows to compute the offset that arises from camera tilt.

            //Fetch camera boundary as world-space coordinates projected to the ground.
            Vector2 camRight =
                GetIntersection2d(Cam.ScreenPointToRay(new Vector3(Screen.width, Screen.height * 0.5f, 0)));
            Vector2 camLeft = GetIntersection2d(Cam.ScreenPointToRay(new Vector3(0, Screen.height * 0.5f, 0)));
            Vector2 camUp = GetIntersection2d(Cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height, 0)));
            Vector2 camDown = GetIntersection2d(Cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, 0, 0)));
            camProjectedMin = GetVector2Min(camRight, camLeft, camUp, camDown);
            camProjectedMax = GetVector2Max(camRight, camLeft, camUp, camDown);

            //Create rotated bounding box from boundaryMin/Max
            Vector2 computeBoundaryMin, computeBoundaryMax;
            RotateBoundingBox(boundaryMin, boundaryMax, -camRotation, out computeBoundaryMin, out computeBoundaryMax);

            Vector2 projectionCorrectionMin = new Vector2(camProjectedCenter.x - camProjectedMin.x,
                camProjectedCenter.y - camProjectedMin.y);
            Vector2 projectionCorrectionMax = new Vector2(camProjectedCenter.x - camProjectedMax.x,
                camProjectedCenter.y - camProjectedMax.y);

            CamPosMin = boundaryMin + projectionCorrectionMin;
            CamPosMax = boundaryMax + projectionCorrectionMax;

            if (CamPosMax.x - CamPosMin.x < camOverdragMargin * 2)
            {
                float midPoint = (CamPosMax.x + CamPosMin.x) * 0.5f;
                CamPosMax = new Vector2(midPoint + camOverdragMargin, CamPosMax.y);
                CamPosMin = new Vector2(midPoint - camOverdragMargin, CamPosMin.y);
            }

            if (CamPosMax.y - CamPosMin.y < camOverdragMargin * 2)
            {
                float midPoint = (CamPosMax.y + CamPosMin.y) * 0.5f;
                CamPosMax = new Vector2(CamPosMax.x, midPoint + camOverdragMargin);
                CamPosMin = new Vector2(CamPosMin.x, midPoint - camOverdragMargin);
            }
        }

        //根据可移动区域的中心点 x,y 以及zoom的值,计算出摄像机的可移动区域--非射线方式
        public void ComputeCamBoundaries(Vector3 pos, ref Vector2 _CamPosMin, ref Vector2 _CamPosMax)
        {
            if (null == Cam)
                return;

            Vector2
                camProjectedCenter =
                    GetIntersection2d(new Ray(pos,
                        -RefPlane
                            .normal)); //Get camera position projected vertically onto the ref plane. This allows to compute the offset that arises from camera tilt.
            var distance = RefPlane.GetDistanceToPoint(pos);

            //Fetch camera boundary as world-space coordinates projected to the ground.
            var rect = GetCorners(distance, Cam, pos);
            Vector2 camProjectedMin = new Vector2(rect[2].x, rect[2].y);
            Vector2 camProjectedMax = new Vector2(rect[1].x, rect[1].y);
            Vector2 projectionCorrectionMin = new Vector2(camProjectedCenter.x - camProjectedMin.x,
                camProjectedCenter.y - camProjectedMin.y);
            Vector2 projectionCorrectionMax = new Vector2(camProjectedCenter.x - camProjectedMax.x,
                camProjectedCenter.y - camProjectedMax.y);

            _CamPosMin = boundaryMin + projectionCorrectionMin;
            _CamPosMax = boundaryMax + projectionCorrectionMax;

            if (_CamPosMax.x - _CamPosMin.x < camOverdragMargin * 2)
            {
                float midPoint = (_CamPosMax.x + _CamPosMin.x) * 0.5f;
                _CamPosMax = new Vector2(midPoint + camOverdragMargin, _CamPosMax.y);
                _CamPosMin = new Vector2(midPoint - camOverdragMargin, _CamPosMin.y);
            }

            if (_CamPosMax.y - _CamPosMin.y < camOverdragMargin * 2)
            {
                float midPoint = (_CamPosMax.y + _CamPosMin.y) * 0.5f;
                _CamPosMax = new Vector2(_CamPosMax.x, midPoint + camOverdragMargin);
                _CamPosMin = new Vector2(_CamPosMin.x, midPoint - camOverdragMargin);
            }
        }

        //内城中摄像机没有旋转值,使用和世界坐标方向一致的 Vector3.up  Vector3.right   Vector3.forward
        //否则使用摄像机的实际 up   right   forward
        Vector3[] GetCorners(float distance, Camera theCamera, Vector3 position)
        {
            Vector3[] corners = new Vector3[4];

            float halfFOV = (theCamera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
            float aspect = theCamera.aspect;

            float height = distance * Mathf.Tan(halfFOV);
            float width = height * aspect;

            // UpperLeft
            corners[0] = position - (Vector3.right * width);
            corners[0] += Vector3.up * height;
            corners[0] += Vector3.forward * distance;

            // UpperRight
            corners[1] = position + (Vector3.right * width);
            corners[1] += Vector3.up * height;
            corners[1] += Vector3.forward * distance;

            // LowerLeft
            corners[2] = position - (Vector3.right * width);
            corners[2] -= Vector3.up * height;
            corners[2] += Vector3.forward * distance;

            // LowerRight
            corners[3] = position + (Vector3.right * width);
            corners[3] -= Vector3.up * height;
            corners[3] += Vector3.forward * distance;

            return corners;
        }

        /// <summary>
        /// Helper method for rotating a boundary box.
        /// </summary>
        private void RotateBoundingBox(Vector2 min, Vector2 max, float rotationDegrees, out Vector2 resultMin,
            out Vector2 resultMax)
        {
            Vector2 v0 = new Vector2(max.x, 0);
            Vector2 v1 = new Vector2(0, max.y);
            Vector2 v2 = new Vector2(min.x, 0);
            Vector2 v3 = new Vector2(0, min.y);
            Vector2 v0Rot = RotateVector2(v0, rotationDegrees);
            Vector2 v1Rot = RotateVector2(v1, rotationDegrees);
            Vector2 v2Rot = RotateVector2(v2, rotationDegrees);
            Vector2 v3Rot = RotateVector2(v3, rotationDegrees);
            resultMin = new Vector2(Mathf.Min(v0Rot.x, v1Rot.x, v2Rot.x, v3Rot.x),
                Mathf.Min(v0Rot.y, v1Rot.y, v2Rot.y, v3Rot.y));
            resultMax = new Vector2(Mathf.Max(v0Rot.x, v1Rot.x, v2Rot.x, v3Rot.x),
                Mathf.Max(v0Rot.y, v1Rot.y, v2Rot.y, v3Rot.y));
        }

        /// <summary>
        /// Method for retrieving the intersection of the given ray with the defined ground
        /// in 2d space.
        /// </summary>
        private Vector2 GetIntersection2d(Ray ray)
        {
            Vector3 intersection3d = GetIntersectionPoint(ray);
            Vector2 intersection2d = new Vector2(intersection3d.x, 0);
            switch (cameraAxes)
            {
                case CameraPlaneAxes.XY_2D_SIDESCROLL:
                    intersection2d.y = GetIntersectionPoint(ray).y;
                    break;
                case CameraPlaneAxes.XZ_TOP_DOWN:
                    intersection2d.y = GetIntersectionPoint(ray).z;
                    break;
            }

            return (intersection2d);
        }

        private Vector2 GetVector2Min(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            return new Vector2(Mathf.Min(v0.x, v1.x, v2.x, v3.x), Mathf.Min(v0.y, v1.y, v2.y, v3.y));
        }

        private Vector2 GetVector2Max(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            return new Vector2(Mathf.Max(v0.x, v1.x, v2.x, v3.x), Mathf.Max(v0.y, v1.y, v2.y, v3.y));
        }

        public void SetZoomInDuration(float zoom, float dt, Action callback = null)
        {
            var progress = CamZoom;
            Tween tween = DOTween.To(() => progress, x => progress = x, zoom, dt)
                .OnUpdate(() =>
                {
                    CamZoom = progress;
                    UpdatePosition(Time.deltaTime);
                }).OnComplete(() => callback?.Invoke());
        }

        public Tween SetZoomInDuration_Curve(float zoom, float dt, Ease easeType, Action callback = null)
        {
            var progress = CamZoom;
            Tween tween = DOTween.To(() => progress, x => progress = x, zoom, dt).SetEase(easeType)
                .OnUpdate(() => { CamZoom = progress; }).OnComplete(() => callback?.Invoke());
            return tween;
        }

        //原始的Update
        private void Update_Origin()
        {
            if (AotoMove)
            {
                autoMoveLerpTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, autoMoveLerpTime / m_PredictTime);
                CamZoom = Mathf.Lerp(startZoom, endZoom, autoMoveLerpTime / m_PredictTime);

                if (autoMoveLerpTime >= m_PredictTime)
                {
                    AotoMove = false;
                    transform.position = endPos;
                    CamZoom = endZoom;
                    // 这里有问题，第一次赋值经常会被校正到一个中间位置，先重新赋值一次，后面整理
                    CamZoom = endZoom;
                    //Log.Warning(" == 移动完成 == ");
                    autoMoveLerpTime = 0;
                    AutoMoveDoneCheckCameraZoom();
                    OnAutoMoveDone?.Invoke();
                    //GameEntry.Event.Fire(this, EventId.OnMainCameraAutoMoveDoneEvent);
                }

            }
        }

        private void Update_CityBuild()
        {
            if (AotoMove)
            {
                autoMoveLerpTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, autoMoveLerpTime / m_PredictTime);
                CamZoom = Mathf.Lerp(startZoom, endZoom, autoMoveLerpTime / m_PredictTime);

                if (autoMoveLerpTime >= m_PredictTime)
                {
                    AotoMove = false;
                    transform.position = endPos;
                    CamZoom = endZoom;
                    // 这里有问题，第一次赋值经常会被校正到一个中间位置，先重新赋值一次，后面整理
                    CamZoom = endZoom;
                    //Log.Warning(" == 移动完成 == ");
                    autoMoveLerpTime = 0;
                    AutoMoveDoneCheckCameraZoom();
                    OnAutoMoveDone?.Invoke();
                    //GameEntry.Event.Fire(this, EventId.OnMainCameraAutoMoveDoneEvent);
                }
            }
        }


        public void RefreshCameraToPos(Vector3 target, float zoom)
        {
            AotoMove = false;
            target = AdjustTarget(target);
            startZoom = CamZoom;
            endZoom = Mathf.Clamp(zoom, CamZoomMin, CamZoomMax);
            var transform1 = transform;
            startPos = transform1.position;
            endPos = target - Transform.forward * endZoom;
            transform1.position = endPos;
            CamZoom = endZoom;
            CamZoom = endZoom;
            AutoMoveDoneCheckCameraZoom();
            touchInputController.RestartDrag();
        }

        public void Update()
        {
            //****************下面内城相关的业务逻辑写在这里太耦合了 影响了世界等其他使用这个类的地方 先暂且这么处理下************
            if (worldType == WorldType.BigWorld)
            {
                Update_Origin();
                return;
            }
            //**********************************************************************************************************

            if (worldType == WorldType.CityBuild)
            {
                Update_CityBuild();
                return;
            }


            if (CanMoveing && AotoMove)
            {
                if (dragState == DragState.Wait)
                {
                    //等待状态是向上
                    return;
                }

                // if (moveTweener != null)
                // {
                //     moveTweener.Kill();
                //     moveTweener = null;
                // }


                autoMoveLerpTime += Time.deltaTime;
                //transform.position = Vector3.Lerp(startPos, endPos, autoMoveLerpTime / m_PredictTime);

                //StartCoroutine(RotateCam());
                //transform.rotation = Quaternion.Euler(new Vector3(0, endRotY, 0));

                if (dragState != DragState.Down && dragState != DragState.Wait)
                {
                    transform.rotation = Quaternion.Lerp(Quaternion.identity,
                        Quaternion.Euler(new Vector3(0, endRotY, 0)), autoMoveLerpTime / m_PredictTime);
                    transform.position = Bezier(startPos, (startPos + endPos) / 2f + midPosDiff, endPos,
                        autoMoveLerpTime / m_PredictTime);
                    CamZoom = Mathf.Lerp(startZoom, endZoom, autoMoveLerpTime / m_PredictTime);
                }
                else
                {
                    dragState = DragState.None;
                    var pos = Bezier(startPos, (startPos + endPos) / 2f + midPosDiff, endPos,
                        autoMoveLerpTime / m_PredictTime);

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity,
                        autoMoveLerpTime / m_PredictTime);

                    transform.position = new Vector3(pos.x, transform.position.y, transform.position.z);
                }

                if (autoMoveLerpTime >= m_PredictTime)
                {
                    AotoMove = false;
                    if (dragState != DragState.Down)
                    {
                        transform.position = endPos;
                        CamZoom = endZoom;
                    }

                    midPosDiff = Vector3.zero;
                    //add+
                    //transform.rotation = Quaternion.Euler(new Vector3(0, endRotY, 0));
                    //+end
                    //Log.Warning(" == 移动完成 == ");
                    OnAutoMoveDone?.Invoke();
                    autoMoveLerpTime = 0;
                    AutoMoveDoneCheckCameraZoom();
                    //GameEntry.Event.Fire(this, EventId.OnMainCameraAutoMoveDoneEvent);
                }
            }
        }

        private void AutoMoveDoneCheckCameraZoom()
        {
            if (!camZoomMin.Equals(targetZoomMin))
            {
                camZoomMin = targetZoomMin;
            }

            if (!camZoomMax.Equals(targetZoomMax))
            {
                camZoomMax = targetZoomMax;
            }
        }

        IEnumerator RotateCam()
        {
            yield return new WaitForSeconds(0.25f);
            transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(new Vector3(0, endRotY, 0)),
                Time.time * .5f);
        }

        #region 地面上摄像机仰角

        protected bool isOpenUpGroundDrag = true;


        public bool IsOpenUpGroundDrag
        {
            set { isOpenUpGroundDrag = value; }
        }


        public enum DragState
        {
            None,
            Down,
            Up,
            Wait,
            Zoom,
            Left,
            Right
        }

        protected DragState dragState = DragState.None;

        // public void DoRotationDown(float time, float endZoom)
        // {
        //     dragState = DragState.Wait;
        //     Vector2 boundaryCenter2d = 0.5f * (boundaryMin + boundaryMax);
        //     //计算出.最终zoom的可移动范围,
        //     Vector2 posMin = Vector2.zero, posMax = Vector2.zero;
        //     ComputeCamBoundaries(new Vector3(boundaryCenter2d.x, boundaryCenter2d.y, endZoom),ref posMin,ref posMax);
        //     var x = Mathf.Clamp(transform.position.x, posMin.x, posMax.x);
        //     var y = Mathf.Clamp(transform.position.y, posMin.y, posMax.y);
        //     var endPos = new Vector3(x,y, endZoom);
        //     transform.DORotate(new Vector3(0, 0, 0), time);
        //     var mover= transform.DOMove(endPos, time);
        //     mover.onUpdate=() =>
        //     {
        //         GameEntry.Event.Fire(this, new CommonEventArgs(EventId.TZ_Camera_Moved));
        //     };
        //     mover.onComplete =() =>
        //     {
        //         touchInputController.RestartDrag();
        //         dragState = DragState.None;
        //         _lastCamZoomDirty = true;
        //         ComputeCamBoundaries();
        //         GameEntry.Event.Fire(this, new CommonEventArgs(EventId.TZ_Camera_Moved));
        //     };
        // }

        #endregion

        private const float groundZ = -22;


        public virtual void LateUpdate()
        {
            // bool forceCheckPinch = true;
            //bool forceCheckPinch = ShipModule.Instance.MainScene != null &&
            //                       ShipModule.Instance.MainScene.InputManager.IsGuidePinch;
            if ( /*!forceCheckPinch && IsUIPickedWithOutRoomBubbles() ||*/
                !CanMoveing || isForceGuide || AotoMove) //AotoMove 自动运行的时候应该毙掉,如果有问题去掉
                return;

            beforeUpdate?.Invoke();

            //Pinch.
            UpdatePinch(Time.deltaTime);

            UpdatePosition(Time.deltaTime);
            UpdateWheel();

            //When the camera is zoomed in further than the defined normal value, it will snap back to normal using the code below.
            if (IsPinching == false && IsDragging == false)
            {
                ForceCorrectCameraZoom();
            }

            if (dragState == DragState.Down)
            {
                Transform.position = new Vector3(Transform.position.x, targetPositionClamped.y,
                    targetPositionClamped.z);
            }
        }

        void UpdateWheel()
        {
            #region editor codepath

            if (!canWheel)
            {
                return;
            }

            //Allow to use the middle mouse wheel in editor to be able to zoom without touch device during development.
            float mouseScrollDelta = Input.GetAxis("Mouse ScrollWheel");
            bool isEditorInputRotate = false;
            bool isEditorInputTilt = false;

            if (PlatformUtils.IsEditor())
            {
#if UNITY_EDITOR
                Vector3 mousePosition = Input.mousePosition;
                if (!(mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
                      mousePosition.y >= 0 && mousePosition.y <= Screen.height))
                {
                    mouseScrollDelta = 0;
                }

                var gameView = EditorWindow.focusedWindow;
                if (gameView != null && gameView.titleContent.text != "Game")
                {
                    mouseScrollDelta = 0;
                }
#endif

                bool anyModifierPressed = Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt) ||
                                          Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                if (anyModifierPressed == true)
                {
                    if (Input.GetKeyDown(KeyCode.KeypadPlus))
                    {
                        mouseScrollDelta = 0.05f;
                    }
                    else if (Input.GetKeyDown(KeyCode.KeypadMinus))
                    {
                        mouseScrollDelta = -0.05f;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        mouseScrollDelta = 0.05f;
                        isEditorInputRotate = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        mouseScrollDelta = -0.05f;
                        isEditorInputRotate = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        mouseScrollDelta = 0.05f;
                        isEditorInputTilt = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        mouseScrollDelta = -0.05f;
                        isEditorInputTilt = true;
                    }
                }
            }

            if (Mathf.Approximately(mouseScrollDelta, 0) == false /*&& GameEntry.UI.IsSupportForPCBehaviour()*/)
            {
                if (isEditorInputRotate == true)
                {
                    if (PlatformUtils.IsEditor() && EnableRotation == true)
                    {
                        Vector3 rotationAxis = GetRotationAxis();
                        Vector3 intersectionScreenCenter =
                            GetIntersectionPoint(Cam.ScreenPointToRay(Input.mousePosition));
                        Transform.RotateAround(intersectionScreenCenter, rotationAxis, mouseScrollDelta * 100);
                        ComputeCamBoundaries();
                    }
                }
                else if (isEditorInputTilt == true)
                {
                    if (PlatformUtils.IsEditor() && EnableTilt == true)
                    {
                        UpdateCameraTilt(mouseScrollDelta * 100);
                    }
                }
                else
                {
                    float editorZoomFactor = 15;
                    if (Cam.orthographic)
                    {
                        editorZoomFactor = 15;
                    }
                    else
                    {
                        if (IsTranslationZoom)
                        {
                            editorZoomFactor = 30;
                        }
                        else
                        {
                            editorZoomFactor = 100;
                        }
                    }

                    float zoomAmount = mouseScrollDelta * editorZoomFactor * factorSpeed;
                    float camSizeDiff = DoEditorCameraZoom(zoomAmount);
                    Vector3 intersectionScreenCenter = GetIntersectionPoint(
                        Cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0)));
                    Vector3 pinchFocusVector = GetIntersectionPoint(Cam.ScreenPointToRay(Input.mousePosition)) -
                                               intersectionScreenCenter;
                    float multiplier = (1.0f / CamZoom * camSizeDiff);
                    Transform.position += pinchFocusVector * multiplier;
                }
            }

            // if (PlatformUtils.IsEditor())
            // {
            //     for (int i = 0; i < 3; ++i)
            //     {
            //         if (Input.GetKeyDown((KeyCode) ((int) KeyCode.Alpha1 + i)))
            //         {
            //             StartCoroutine(
            //                 ZoomToTargetValueCoroutine(Mathf.Lerp(CamZoomMin, CamZoomMax, (float) i / 2.0f)));
            //         }
            //     }
            // }

            #endregion
        }

        /// <summary>
        /// ⚠️⚠️⚠️️
        /// 这里有个 漏洞
        /// 这段代码的意思应该是当相机缩放值超过最大值，或小于最小值时
        /// 以一个平缓的速度矫正相机缩放，无限趋近于目标值
        /// 在 Mac 电脑上，缩放会超过限制，所以会自动矫正
        /// 但是这个矫正方式是无限趋近于目标值，永远到不了目标值
        /// 就导致相机缩放永远在微弱的变化
        /// 相机缩放会触发一系列事件，如关闭世界上打开的 UI 等...
        ///
        /// 在手机和 Windows 电脑上未复现，所以先不改了
        ///
        /// 如果出了问题，把比较精度改小一些应该就好了
        /// 当 Zoom 差值小于一定值时，直接矫正到目标值就好了
        ///
        /// -----------------------------------------------------------------------------------------
        /// 2021.11.17 居然复现了，召唤卡亚弹板点开自动关闭（相机缩放触发关闭弹板），定位到这里，已尝试修复
        /// </summary>
        ///
        /// 误差精度
        private float m_AccuracyError = 0.001f;

        public void ForceCorrectCameraZoom()
        {
            float camZoomDeltaToNormal = 0;
            if (CamZoom > camZoomMax)
            {
                camZoomDeltaToNormal = CamZoom - camZoomMax;
            }
            else if (CamZoom < camZoomMin)
            {
                camZoomDeltaToNormal = CamZoom - camZoomMin;
            }

            if (Mathf.Abs(camZoomDeltaToNormal) > m_AccuracyError)
            {
                float cameraSizeCorrection = Mathf.Lerp(0, camZoomDeltaToNormal, zoomBackSpringFactor * Time.deltaTime);
                if (Mathf.Abs(Mathf.Abs(cameraSizeCorrection) - Mathf.Abs(camZoomDeltaToNormal)) < m_AccuracyError)
                {
                    cameraSizeCorrection = camZoomDeltaToNormal;
                }

                CamZoom -= cameraSizeCorrection;
            }
        }

        /// <summary>
        /// Editor helper code.
        /// </summary>
        private float DoEditorCameraZoom(float amount)
        {
            float newCamZoom = CamZoom - amount;
            newCamZoom = Mathf.Clamp(newCamZoom, camZoomMin, camZoomMax);
            float camSizeDiff = CamZoom - newCamZoom;
            if (enableZoomTilt == true)
            {
                UpdateTiltForAutoTilt(newCamZoom);
            }

            CamZoom = newCamZoom;
            PinchinggCallback?.Invoke();
            return (camSizeDiff);
        }

        public void FixedUpdate()
        {
            ScreenRatio = GetScreenRatio();

            if (cameraScrollVelocity.sqrMagnitude > float.Epsilon)
            {
                float timeSinceDragStop = Time.realtimeSinceStartup - timeRealDragStop;
                float dampFactor = Mathf.Clamp01(timeSinceDragStop * dampFactorTimeMultiplier);
                float camScrollVel = cameraScrollVelocity.magnitude;
                float camScrollVelRelative = camScrollVel / autoScrollVelocityMax;
                Vector3 camVelDamp =
                    dampFactor * cameraScrollVelocity.normalized * autoScrollDamp * Time.fixedDeltaTime;
                camVelDamp *= EvaluateAutoScrollDampCurve(1.0f - camScrollVelRelative);
                if (camVelDamp.sqrMagnitude >= cameraScrollVelocity.sqrMagnitude)
                {
                    cameraScrollVelocity = Vector3.zero;
                }
                else
                {
                    cameraScrollVelocity -= camVelDamp;
                }
            }
        }

        /// <summary>
        /// Helper method used for auto scroll.
        /// </summary>
        private float EvaluateAutoScrollDampCurve(float t)
        {
            if (autoScrollDampCurve == null || autoScrollDampCurve.length == 0)
            {
                return (1);
            }

            return autoScrollDampCurve.Evaluate(t);
        }

        // 判断是否点到ui上面。
        public bool IsUIPicked()
        {
            //PointerEventData eventData = new PointerEventData(EventSystem.current)
            //{
            //    pressPosition = Input.mousePosition,
            //    position = Input.mousePosition
            //};

            //List<RaycastResult> list = new List<RaycastResult>();
            if (EventSystem.current == null)
                return false;
            pointer_eventData.Reset();
            pointer_eventData.pressPosition = Input.mousePosition;
            pointer_eventData.position = Input.mousePosition;
            ray_list.Clear();
            EventSystem.current.RaycastAll(pointer_eventData, ray_list);
            return ray_list.Count > 0;
        }

        // public bool IsUIPickedWithOutRoomBubbles(bool IsStart = false)
        // {
        //     //if (SceneContainer.Instance.CurrentSceneID != SceneContainer.SceneID.MainCity)
        //     //{
        //     //    return false;
        //     //}
        //
        //     //如果没有点击，直接返回false
        //     if (TouchWrapper.IsFingerDown == false)
        //     {
        //         return false;
        //     }
        //
        //     pointer_eventData.Reset();
        //     pointer_eventData.pressPosition = Input.mousePosition;
        //     pointer_eventData.position = Input.mousePosition;
        //     ray_list.Clear();
        //     EventSystem.current.RaycastAll(pointer_eventData, ray_list);
        //     for (int i = 0; i < ray_list.Count; i++)
        //     {
        //         //Log.Info("IsUIPicked gameobject name:" + list[i].gameObject.name);
        //         // UI 自身屏蔽交互，不影响场景交互
        //         //var skip = ray_list[i].gameObject.GetComponent<UISkipPick>();
        //         ////if (skip == null && list[i].gameObject.transform.root.name == "GameFramework")
        //         ////修改一下 暂时只屏蔽roomBubble 现在屏蔽所有不在ui框架下的ui会有问题，有部分挂在房间canvas下的ui不能屏蔽，比如驻扎部分ui
        //         ////挂了LFUICanMoveAndZoom的也不会影响摄像机移动和缩放，不挂UISkipPick，因为有的ui需要不影响拖动和缩放但是屏蔽场景交互
        //         ////if (!IsStart)
        //         //{
        //         //    if (skip == null && ray_list[i].gameObject.GetComponentInParent<LFRoomBubble>() == null && ray_list[i].gameObject.GetComponentInParent<LFUICanMoveAndZoom>() == null)
        //         //    {
        //         //        return true;
        //         //    }
        //         //}
        //         //else
        //         //{
        //         //    if (skip == null && ray_list[i].gameObject.GetComponentInParent<LFRoomBubble>() == null)
        //         //    {
        //         //        return true;
        //         //    }
        //         //}
        //         if (ray_list[i].gameObject.GetComponentInParent<LFRoomBubble>() == null)
        //         {
        //             return true;
        //         }
        //     }
        //
        //     return false;
        // }

        // private void InputDonwOnUI()
        // {
        //     if (GameEntry.Event == null)
        //         return;
        //
        //     if (TouchWrapper.IsFingerDown)
        //     {
        //         if (Input.touchCount > 0)
        //         {
        //             if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //             {
        //                 //if (!GameMovieController.Instance.Runing && !LuaManager.Instance.CallLuaFunctionEntry1ParamReturnBool("NoviceGuideController", "IsRun", null))
        //                 //{
        //                 if (Input.touchCount > 0 && IsUIPickedWithOutRoomBubbles(true))
        //                 {
        //                     CanMoveing = false;
        //                 }
        //                 //}
        //             }
        //         }
        //         else
        //         {
        //             //if (!GameMovieController.Instance.Runing /*&&!LuaManager.Instance.CallLuaFunctionEntry1ParamReturnBool("NoviceGuideController", "IsRun", null)*/)
        //             //{
        //             if (IsUIPickedWithOutRoomBubbles(true))
        //             {
        //                 CanMoveing = false;
        //             }
        //             //}
        //         }
        //     }
        // }

        private void InputControllerOnFingerDown(Vector3 pos)
        {
            if (IsUIPicked())
                CanMoveing = false;

            cameraScrollVelocity = Vector3.zero;
        }

        private void InputControllerOnFingerUp(Vector3 endPos)
        {
            CanMoveing = true;
            isDraggingSceneObject = false;
        }

        private Vector3 GetDragVector(Vector3 dragPosStart, Vector3 dragPosCurrent)
        {
            Vector3 intersectionDragStart = GetIntersectionPoint(Cam.ScreenPointToRay(dragPosStart));
            Vector3 intersectionDragCurrent = GetIntersectionPoint(Cam.ScreenPointToRay(dragPosCurrent));
            if (CameraAxes == CameraPlaneAxes.X_SIDSCROLL && worldType == WorldType.CityBuild)
            {
                return (intersectionDragStart - intersectionDragCurrent);
            }
            return (intersectionDragCurrent - intersectionDragStart);
        }

        /// <summary>
        /// Helper method that computes the suggested auto cam velocity from
        /// the last few frames of the user drag.
        /// </summary>
        private Vector3 GetVelocityFromMoveHistory()
        {
            Vector3 momentum = Vector3.zero;
            if (DragCameraMoveVector.Count > 0)
            {
                for (int i = 0; i < DragCameraMoveVector.Count; ++i)
                {
                    momentum += DragCameraMoveVector[i];
                }

                momentum /= DragCameraMoveVector.Count;
            }

            if (CameraAxes == CameraPlaneAxes.XZ_TOP_DOWN)
            {
                momentum.y = momentum.z;
                momentum.z = 0;
            }

            return (momentum);
        }

        private bool canMoveing = true;

        //public bool CanMoveing = true;
        public bool CanMoveing
        {
            get { return canMoveing; }
            set
            {
                if (canMoveing != value)
                {
                    //Log.Error("change CanMoveing =={0}", value);
                }

                canMoveing = value;
            }
        }

        public virtual void InputControllerOnDragStart(Vector3 dragPosStart, bool isLongTap, Vector3 offset)
        {
            if (AotoMove)
            {
                //AotoMove = false;
                return;
            }

            if (!canDrag)
            {
                return;
            }

            if (isDraggingSceneObject == false && CanMoveing)
            {
                _lastDragPos = dragPosStart;
                cameraScrollVelocity = Vector3.zero;
                dragStartCamPos = Transform.position;
                IsDragging = true;
                DragCameraMoveVector.Clear();
                SetTargetPosition(Transform.position);
            }
        }

        private void InputControllerOnDragUpdate(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
        {
            // patch : 剧情序章结束前，不能拖动相机
            if ( /*GameEntry.SceneContainer.SceneIdIsNone() || */
                isDraggingSceneObject == false && CanMoveing &&
                canDrag /*&& GameMovieController.Instance.isPreludeOver*/)
            {
                _dragMoveVector = _lastDragPos - dragPosCurrent;
                _lastDragPos = dragPosCurrent;
                Vector3 dragVector = GetDragVector(dragPosStart, dragPosCurrent + correctionOffset);
                Vector3 posNewClamped = GetClampToBoundaries(dragStartCamPos - dragVector);
                this.dragDirection = correctionOffset.normalized;
                SetTargetPosition(posNewClamped);
            }
            else
            {
                IsDragging = false;
            }
        }

        private void InputControllerOnDragStop(Vector3 dragStopPos, Vector3 dragFinalMomentum)
        {
            if (isDraggingSceneObject == false && canDrag)
            {
                _lastDragPos = Vector3.zero;
                cameraScrollVelocity = GetVelocityFromMoveHistory();
                if (cameraScrollVelocity.sqrMagnitude >= autoScrollVelocityMax * autoScrollVelocityMax)
                {
                    cameraScrollVelocity = cameraScrollVelocity.normalized * autoScrollVelocityMax;
                }

                timeRealDragStop = Time.realtimeSinceStartup;
                DragCameraMoveVector.Clear();
            }

            IsDragging = false;
        }

        private void InputControllerOnPinchStart(Vector3 pinchCenter, float pinchDistance)
        {
            pinchStartCamZoomSize = CamZoom;
            pinchStartIntersectionCenter = GetIntersectionPoint(Cam.ScreenPointToRay(pinchCenter));

            pinchCenterCurrent = pinchCenter;
            pinchDistanceCurrent = pinchDistance;
            pinchDistanceStart = pinchDistance;

            pinchCenterCurrentLerp = pinchCenter;
            pinchDistanceCurrentLerp = pinchDistance;

            SetTargetPosition(Transform.position);
            IsPinching = true;
            isRotationActivated = false;
            ResetPinchRotation(0);

            pinchTiltCurrent = 0;
            pinchTiltAccumulated = 0;
            pinchTiltLastFrame = 0;
            isTiltModeEvaluated = false;
            isPinchTiltMode = false;

            if (EnableTilt == false)
            {
                isTiltModeEvaluated = true; //Early out of this evaluation in case tilt is not enabled.
            }
        }

        private void InputControllerOnPinchUpdate(PinchUpdateData pinchUpdateData)
        {
            //if (ShipModule.Instance.MainScene != null && ShipModule.Instance.MainScene.InputManager.IsGuidePinch)
            //{
            //    if (pinchUpdateData.pinchDistance >= 0.05f)
            //    {
            //        LuaManager.Instance.CallLuaFunctionEntry1Param("NoviceGuideController", "Close", true);
            //        ShipModule.Instance.MainScene.InputManager.IsGuidePinch = false;
            //    }
            //}
            if (EnableTilt == true)
            {
                pinchTiltCurrent += pinchUpdateData.pinchTiltDelta;
                pinchTiltAccumulated += Mathf.Abs(pinchUpdateData.pinchTiltDelta);

                if (isTiltModeEvaluated == false &&
                    pinchUpdateData.pinchTotalFingerMovement > pinchModeDetectionMoveTreshold)
                {
                    isPinchTiltMode = Mathf.Abs(pinchTiltCurrent) > pinchTiltModeThreshold;
                    isTiltModeEvaluated = true;
                    if (isPinchTiltMode == true && isPinchModeExclusive == true)
                    {
                        pinchStartIntersectionCenter = GetIntersectionPoint(GetCamCenterRay());
                    }
                }
            }

            if (isTiltModeEvaluated == true)
            {
#pragma warning disable 162
                if (isPinchModeExclusive == true)
                {
                    pinchCenterCurrent = pinchUpdateData.pinchCenter;

                    if (isPinchTiltMode == true)
                    {
                        //Evaluate a potential break-out from a tilt. Under certain tweak-settings the tilt may trigger prematurely and needs to be overrided.
                        if (pinchTiltAccumulated < pinchAccumBreakout)
                        {
                            bool breakoutZoom = Mathf.Abs(pinchDistanceStart - pinchUpdateData.pinchDistance) >
                                                pinchDistanceForTiltBreakout;
                            bool breakoutRot = enableRotation == true &&
                                               Mathf.Abs(pinchAngleCurrent) > rotationLockThreshold;
                            if (breakoutZoom == true || breakoutRot == true)
                            {
                                InputControllerOnPinchStart(pinchUpdateData.pinchCenter, pinchUpdateData.pinchDistance);
                                isTiltModeEvaluated = true;
                                isPinchTiltMode = false;
                            }
                        }
                    }
                }
#pragma warning restore 162
                pinchDistanceCurrent = pinchUpdateData.pinchDistance;

                if (enableRotation == true)
                {
                    if (Mathf.Abs(pinchUpdateData.pinchAngleDeltaNormalized) > rotationDetectionDeltaThreshold)
                    {
                        pinchAngleCurrent += pinchUpdateData.pinchAngleDelta;
                    }

                    if (pinchDistanceCurrent > rotationMinPinchDistance)
                    {
                        if (isRotationActivated == false)
                        {
                            ResetPinchRotation(0);
                            isRotationActivated = true;
                        }
                    }
                    else
                    {
                        isRotationActivated = false;
                    }
                }
            }
        }

        private void ResetPinchRotation(float currentPinchRotation)
        {
            pinchAngleCurrent = currentPinchRotation;
            pinchAngleCurrentLerp = currentPinchRotation;
            pinchAngleLastFrame = currentPinchRotation;
            isRotationLock = true;
        }

        private void InputControllerOnPinchStop()
        {
            IsPinching = false;
            DragCameraMoveVector.Clear();
            isPinchTiltMode = false;
            isTiltModeEvaluated = false;
        }

        private void InputControllerOnInputClick(Vector3 clickPosition, bool isDoubleClick, bool isLongTap)
        {
            if (isLongTap == true)
            {
                return;
            }

            Ray camRay = Cam.ScreenPointToRay(clickPosition);
            if (OnPickItem != null || OnPickItemDoubleClick != null)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(camRay, out hitInfo) == true)
                {
                    if (OnPickItem != null)
                    {
                        OnPickItem.Invoke(hitInfo);
                    }

                    if (isDoubleClick == true)
                    {
                        if (OnPickItemDoubleClick != null)
                        {
                            OnPickItemDoubleClick.Invoke(hitInfo);
                        }
                    }
                }
            }

            if (OnPickItem2D != null || OnPickItem2DDoubleClick != null)
            {
                RaycastHit2D hitInfo2D = Physics2D.Raycast(camRay.origin, camRay.direction);
                if (hitInfo2D == true)
                {
                    if (OnPickItem2D != null)
                    {
                        OnPickItem2D.Invoke(hitInfo2D);
                    }

                    if (isDoubleClick == true)
                    {
                        if (OnPickItem2DDoubleClick != null)
                        {
                            OnPickItem2DDoubleClick.Invoke(hitInfo2D);
                        }
                    }
                }
            }
        }

        public void OnDragSceneObject()
        {
            isDraggingSceneObject = true;
        }

        private float GetScreenRatio()
        {
            return ((float)Screen.width / (float)Screen.height);
        }

        private IEnumerator ZoomToTargetValueCoroutine(float target)
        {
            if (Mathf.Approximately(target, CamZoom) == false)
            {
                float startValue = CamZoom;
                const float duration = 0.3f;
                float timeStart = Time.time;
                while (Time.time < timeStart + duration)
                {
                    float progress = (Time.time - timeStart) / duration;
                    CamZoom = Mathf.Lerp(startValue, target,
                        Mathf.Sin(-Mathf.PI * 0.5f + progress * Mathf.PI) * 0.5f + 0.5f);
                    yield return null;
                }

                CamZoom = target;
            }
        }

        public string CheckCameraAxesErrors()
        {
            string error = "";
            if (Transform.forward == Vector3.down && cameraAxes != CameraPlaneAxes.XZ_TOP_DOWN)
            {
                error =
                    "Camera is pointing down but the cameraAxes is not set to TOP_DOWN. Make sure to set the cameraAxes variable properly.";
            }

            if (Transform.forward == Vector3.forward && cameraAxes != CameraPlaneAxes.XY_2D_SIDESCROLL)
            {
                error =
                    "Camera is pointing sidewards but the cameraAxes is not set to 2D_SIDESCROLL. Make sure to set the cameraAxes variable properly.";
            }

            return (error);
        }

        private Ray GetCamCenterRay()
        {
            return new Ray(transform.position, transform.forward);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector2 boundaryCenter2d = 0.5f * (boundaryMin + boundaryMax);
            Vector2 boundarySize2d = boundaryMax - boundaryMin;
            Vector3 boundaryCenter = UnprojectVector2(boundaryCenter2d, groundLevelOffset);
            Vector3 boundarySize = UnprojectVector2(boundarySize2d);
            Gizmos.DrawWireCube(boundaryCenter, boundarySize);
        }


        /// <summary>
        /// Helper method that unprojects the given Vector2 to a Vector3
        /// according to the camera axes setting.
        /// </summary>
        public Vector3 UnprojectVector2(Vector2 v2, float offset = 0)
        {
            if (CameraAxes == CameraPlaneAxes.XY_2D_SIDESCROLL)
            {
                return new Vector3(v2.x, v2.y, offset);
            }
            else
            {
                return new Vector3(v2.x, offset, v2.y);
            }
        }

        public Vector2 ProjectVector3(Vector3 v3)
        {
            if (CameraAxes == CameraPlaneAxes.XY_2D_SIDESCROLL)
            {
                return new Vector2(v3.x, v3.y);
            }
            else
            {
                return new Vector2(v3.x, v3.z);
            }
        }

        public void StopCameraScroll()
        {
            cameraScrollVelocity = Vector3.zero;
        }

        public Vector3 ViewportPointToWorldPos(Vector3 vec)
        {
            var ray = Cam.ViewportPointToRay(vec);
            return GetIntersectionPoint(ray);
        }


        public void UpdatePosXRange(float min, float max)
        {
            CamPosMin = new Vector2(min, CamPosMin.y);
            CamPosMax = new Vector2(max, CamPosMax.y);
        }
    }

}
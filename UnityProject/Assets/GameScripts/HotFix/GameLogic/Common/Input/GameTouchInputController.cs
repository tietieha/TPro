using System;
using UnityEngine;

namespace UW
{
    /// <summary>
    /// 处理游戏中通过屏幕的输入
    /// 1. 单指触屏
    /// 2. 双指触屏
    /// 不支持超过双指的触屏
    /// </summary>
    [XLua.LuaCallCSharp]
    public class GameTouchInputController : MonoBehaviour, IInputControlListener
    {
        // 控制参数
        [SerializeField] private InputControlParameters m_Parameters;

        private IInputController m_InputController = new InputController_Empty();

        public delegate void InputDragStartDelegate(Vector3 position, bool isLongTap, Vector3 offset);
        public delegate void Input1PositionDelegate(Vector3 position);
        public delegate void DragUpdateDelegate(Vector3 dragPositionStart, Vector3 dragPositionCurrent, Vector3 correctionOffset);
        public delegate void DragStopDelegate(Vector3 dragStopPosition, Vector3 dragFinalMomentum);
        public delegate void PinchStartDelegate(Vector3 pinchCenter, float pinchDistance);
        public delegate void PinchUpdateDelegate(Vector3 pinchCenter, float pinchDistance, float pinchStartDistance);
        public delegate void InputLongTapProgress(float progress);
        public delegate void InputClickDelegate(Vector3 clickPosition, bool isDoubleClick, bool isLongTap);

        public event Input1PositionDelegate OnFingerDown;
        public event Input1PositionDelegate OnFingerUp;

        public event InputDragStartDelegate OnDragStart;
        public event DragUpdateDelegate     OnDragUpdate;
        public event DragStopDelegate       OnDragStop;

        public event PinchStartDelegate     OnPinchStart;
        public event PinchUpdateDelegate    OnPinchUpdate;
        public event Action                 OnPinchStop;

        public event InputLongTapProgress   OnLongTapProgress;
        public event InputClickDelegate     OnInputClick;

        // Start is called before the first frame update
        void Start()
        {
            InitializeInputController();
        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = Time.deltaTime;
            m_InputController.Update(deltaTime);
        }

        private void InitializeInputController()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL || GOOGLE_PC
            m_InputController = new InputController_Editor(m_Parameters, this);
#else
            m_InputController = new InputController_Mobile(m_Parameters, this);
#endif

            m_InputController.Initialize();
        }

        public void RestartDrag()
        {
            m_InputController.ResetDrag();
        }

        #region Input Control Listener

        public void InputDragStart(Vector3 position, bool isLongTap, Vector3 offset)
        {
            OnDragStart?.Invoke(position, isLongTap, offset);
        }

        public void DragUpdate(Vector3 dragPositionStart, Vector3 dragPositionCurrent, Vector3 correctionOffset)
        {
            OnDragUpdate?.Invoke(dragPositionStart, dragPositionCurrent, correctionOffset);
        }

        public void DragStop(Vector3 dragStopPosition, Vector3 dragFinalMomentum)
        {
            OnDragStop?.Invoke(dragStopPosition, dragFinalMomentum);
        }

        public void PinchStart(Vector3 pinchCenter, float pinchDistance)
        {
            OnPinchStart?.Invoke(pinchCenter, pinchDistance);
        }

        public void PinchUpdate(Vector3 pinchCenter, float pinchDistance, float pinchStartDistance)
        {
            OnPinchUpdate?.Invoke(pinchCenter, pinchDistance, pinchStartDistance);
        }

        public void ProgressLongTap(float progress)
        {
            OnLongTapProgress?.Invoke(progress);
        }

        public void InputClick(Vector3 clickPosition, bool isDoubleClick, bool isLongTap)
        {
            OnInputClick?.Invoke(clickPosition, isDoubleClick, isLongTap);
        }

        public void TouchDown(Vector3 position)
        {
            OnFingerDown?.Invoke(position);
        }

        public void TouchUp(Vector3 position)
        {
            OnFingerUp?.Invoke(position);
        }

        public void PinchStop()
        {
            OnPinchStop?.Invoke();
        }

        public Vector3 GetCurrentInputPosition()
        {
            return m_InputController.GetCurrentInputPosition();
        }

        #endregion
    }
}

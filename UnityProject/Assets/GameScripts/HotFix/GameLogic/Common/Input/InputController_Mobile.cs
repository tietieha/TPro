using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UW
{
    /// <summary>
    /// 移动端触屏输入，支持：
    /// 1. 单指触屏（点击、双击、长按、拖拽）
    /// 2. 双指触屏（一指在UI上，一指在非UI上；指在非UI上）
    /// </summary>
    public class InputController_Mobile: IInputController
    {
        private InputControlParameters m_Parameters;
        private IInputControlListener m_InputListener;

        private bool m_IsTouchOnUI              = false;
        private bool m_WasTouchDownLastFrame    = false;
        private bool m_IsDragging               = false;
        private bool m_WasDraggingLastFrame     = false;
        private bool m_IsTouchDown              = false;
        private bool m_IsClickPrevented         = false;
        private bool m_IsPinching               = false;
        private bool m_WasPinchingLastFrame     = false;
        private bool m_PinchToDragCurrentFrame  = false;

        private float m_LastTouchDownTimeReal   = 0;
        private float m_TimeSinceDragStart      = 0;
        private float m_LastClickTimeReal       = 0;
        private float m_PinchStartDistance      = 0;

        /// <summary>
        /// 当前帧有效触摸ID，用于处理单指的点击、拖拽、长按等
        /// </summary>
        private int m_ActivePointerId           = -1;

        /// <summary>
        /// 上一帧有效触摸ID
        /// </summary>
        private int m_LastActivePointerId       = -1;

        private Vector3 m_DragStartPosition         = Vector3.zero;
        private Vector3 m_DragStartOffset           = Vector3.zero;
        private Vector3 m_LastTouchDownPosition     = Vector3.zero;
        private Vector3 m_PinchRotationVectorStart  = Vector3.zero;
        private Vector3 m_PinchVectorLastFrame      = Vector3.zero;

        private List<Vector3> m_DragFinalMomentumVector = new List<Vector3>();
        private List<Vector3> m_PinchStartPositions     = null;
        private List<Vector3> m_TouchPositionLastFrame  = null;
        
        private const int c_MomentumSamplesCount = 5;

        public InputController_Mobile(in InputControlParameters parameters, IInputControlListener controlListener)
        {
            m_Parameters    = parameters;
            m_InputListener = controlListener;
        }

        public void Initialize()
        {
            m_DragFinalMomentumVector.Clear();

            m_IsTouchOnUI               = false;
            m_PinchStartPositions       = new List<Vector3> { Vector3.zero, Vector3.zero };
            m_TouchPositionLastFrame    = new List<Vector3> { Vector3.zero, Vector3.zero };
            m_PinchStartDistance        = 1;
            m_IsPinching                = false;
            m_IsDragging                = false;
            m_IsClickPrevented          = false;
            m_ActivePointerId           = -1;
            m_LastActivePointerId       = -1;
        }

        public void Update(float deltaTime)
        {
            // 每帧按当前帧的数据来处理，有需要的地方要缓存上一帧数据
            CheckTouchStatus();

            if(!m_IsTouchOnUI)
            {
                UpdateWhenNotOnUI();
            }
            else
            {
                UpdateWhenOnUI();
            }

            if (m_IsDragging && Input.touchCount > 0 && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount && !m_PinchToDragCurrentFrame)
            {
                Vector3 startPosition = Input.touches[m_ActivePointerId].position;

                m_DragFinalMomentumVector.Add(startPosition - m_LastTouchDownPosition);
                if (m_DragFinalMomentumVector.Count > c_MomentumSamplesCount)
                {
                    m_DragFinalMomentumVector.RemoveAt(0);
                }
            }

            // 有一个有效触摸
            m_WasTouchDownLastFrame = m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount;
            if (m_WasTouchDownLastFrame && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount)
            {
                m_LastTouchDownPosition = Input.touches[m_ActivePointerId].position;
            }

            m_WasDraggingLastFrame = m_IsDragging;
            m_WasPinchingLastFrame = m_IsPinching;

            // 没有有效触摸时
            if (Input.touchCount == 0 || (Input.touchCount > 0 && m_ActivePointerId < 0))
            {
                m_IsClickPrevented = false;
                if (m_IsTouchDown)
                {
                    FingerUp();
                }
            }
        }

        public void ResetDrag()
        {
            if (!m_IsDragging)
                return;

            DragStop(m_LastTouchDownPosition);

            if (Input.touchCount > 0 && m_ActivePointerId >= 0)
            {
                m_DragStartOffset = Vector3.zero;
                m_DragStartPosition = Input.touches[m_ActivePointerId].position;
                DragStart(m_DragStartPosition, false, Vector3.zero);
            }
        }

        public void Clear()
        {
            
        }

        private void CheckTouchStatus()
        {
            // 记住上一帧的有效触摸ID
            m_LastActivePointerId = m_ActivePointerId;

            m_IsTouchOnUI = false;
            m_ActivePointerId = -1;
            int touchCount = Input.touchCount;
            if (touchCount > 0)
            {
                // 有触摸
                for(int index = 0; index < touchCount; ++index)
                {
                    int pointerId = Input.touches[index].fingerId;
                    if (EventSystem.current.IsPointerOverGameObject(pointerId))
                    {
                        m_IsTouchOnUI |= true;
                    }
                    else if (m_ActivePointerId < 0)
                    {
                        // 记录一个不在UI上的有效触摸ID
                        m_ActivePointerId = pointerId;
                    }
                }
            }
        }

        /// <summary>
        /// 处理没有touch在UI上的情况
        /// </summary>
        private void UpdateWhenNotOnUI()
        {
            #region pinch
            // 先判断是否能双指挤压，根据当前帧的数据处理
            if (!m_IsPinching)
            {
                if (Input.touchCount == 2)
                {
                    StartPinch();
                    m_IsPinching = true;
                }
            }
            else
            {
                if (Input.touchCount < 2)
                {
                    StopPinch();
                    m_IsPinching = false;
                }
                else if (Input.touchCount == 2)
                {
                    UpdatePinch();
                }
            }
            #endregion

            #region drag
            // 当前帧没有pinch，开始处理drag的情形
            if (!m_IsPinching)
            {
                // 上一帧也没有pinch
                if (!m_WasPinchingLastFrame)
                {
                    if (m_WasTouchDownLastFrame && (m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount && m_ActivePointerId == m_LastActivePointerId) && !m_IsDragging)
                    {
                        float dragDistance = GetRelativeDragDistance(Input.touches[m_ActivePointerId].position, m_DragStartPosition);
                        float dragTime = Time.realtimeSinceStartup - m_LastTouchDownTimeReal;

                        bool isLongTap = dragTime > m_Parameters.ClickDurationThreshold;

                        float longTapProgress = 0;
                        if (Mathf.Approximately(m_Parameters.ClickDurationThreshold, 0) == false)
                        {
                            longTapProgress = Mathf.Clamp01(dragTime / m_Parameters.ClickDurationThreshold);
                        }
                        m_InputListener.ProgressLongTap(longTapProgress);

                        // 单指拖拽
                        if (Input.touchCount == 1 &&
                            dragDistance >= m_Parameters.DragStartDistanceThresholdRelative &&
                            dragTime >= m_Parameters.DragDurationThreshold)
                        {
                            m_IsDragging = true;
                            m_DragStartOffset = m_LastTouchDownPosition - m_DragStartPosition;

                            DragStart(m_DragStartPosition, isLongTap, m_DragStartOffset);
                        }
                    }
                }
                else
                {
                    // 上一帧pinch但是当前帧没有pinch，此时因为是双指触摸变单指触摸
                    if (Input.touchCount > 0 && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount)
                    {
                        m_IsDragging = true;
                        m_DragStartPosition = Input.touches[m_ActivePointerId].position;
                        DragStart(m_DragStartPosition, false, Vector3.zero);
                        m_PinchToDragCurrentFrame = true;
                    }
                }

                if (m_IsDragging && Input.touchCount > 0 && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount)
                {
                    DragUpdate(Input.touches[m_ActivePointerId].position);
                }

                if (m_IsDragging && (Input.touchCount <= 0 || m_ActivePointerId < 0))
                {
                    m_IsDragging = false;
                    DragStop(m_LastTouchDownPosition);
                }
            }
            #endregion

            #region click
            if (!m_IsPinching && !m_IsDragging && !m_WasPinchingLastFrame && !m_WasDraggingLastFrame && !m_IsClickPrevented)
            {
                if (!m_WasTouchDownLastFrame && Input.touchCount > 0 && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount)
                {
                    m_LastTouchDownTimeReal = Time.realtimeSinceStartup;
                    m_DragStartPosition = Input.touches[m_ActivePointerId].position;
                    FingerDown(GetAverageTouchPosFromInputTouches());
                }

                if (m_WasTouchDownLastFrame && (Input.touchCount <= 0 || m_ActivePointerId < 0))
                {
                    float fingerDownUpDuration = Time.realtimeSinceStartup - m_LastTouchDownTimeReal;

                    if (!m_WasDraggingLastFrame && !m_WasPinchingLastFrame)
                    {
                        float clickDuration = Time.realtimeSinceStartup - m_LastClickTimeReal;

                        bool isDoubleClick = clickDuration < m_Parameters.DoubleclickDurationThreshold;
                        bool isLongTap = fingerDownUpDuration > m_Parameters.ClickDurationThreshold;

                        m_InputListener.InputClick(m_LastTouchDownPosition, isDoubleClick, isLongTap);

                        m_LastClickTimeReal = Time.realtimeSinceStartup;
                    }
                }
            }
            #endregion
        }

        private void UpdateWhenOnUI()
        {
            // 只有单指触摸，且在UI上，后续不需要处理
            // 并且清理掉之前的状态
            if (Input.touchCount == 1 || m_ActivePointerId < 0)
            {
                // 如果有pinch，停掉pinch
                m_IsPinching = false;
                if (m_WasPinchingLastFrame)
                {
                    StopPinch();
                }

                // 如果在拖拽，则停止
                if(m_IsDragging || m_WasDraggingLastFrame)
                {
                    //m_IsDragging = false;
                    //DragStop(m_LastTouchDownPosition);
                    //Input.touches[index].fingerId 
                    //TODO 补充UI上的拖拽、整体实现思路待定、先解决BUG touches[0].fingerId
                    if (m_IsDragging)
                    {
                        DragUpdate(Input.mousePosition);
                    }
                    return;
                }

                // 有click的，则停止click处理
                if (m_WasTouchDownLastFrame)
                {
                    float fingerDownUpDuration = Time.realtimeSinceStartup - m_LastTouchDownTimeReal;

                    if (!m_WasDraggingLastFrame && !m_WasPinchingLastFrame)
                    {
                        float clickDuration = Time.realtimeSinceStartup - m_LastClickTimeReal;

                        bool isDoubleClick = clickDuration < m_Parameters.DoubleclickDurationThreshold;
                        bool isLongTap = fingerDownUpDuration > m_Parameters.ClickDurationThreshold;

                        m_InputListener.InputClick(m_LastTouchDownPosition, isDoubleClick, isLongTap);

                        m_LastClickTimeReal = Time.realtimeSinceStartup;
                    }
                }
                return;
            }
            else if( Input.touchCount == 2 && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount)
            {
                // 双指触屏，但是有一个在UI上，另一个是有效触摸
                // 如果有pinch，停掉pinch
                m_IsPinching = false;
                if (m_WasPinchingLastFrame)
                {
                    StopPinch();
                }

                if (m_ActivePointerId != m_LastActivePointerId && m_IsDragging)
                {
                    // 如果切换了触摸ID，重新计算拖拽
                    m_IsDragging = false;
                    DragStop(m_LastTouchDownPosition);
                }
            }

            // 处理单指的点击、拖拽、长按等
            if (m_WasTouchDownLastFrame && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount && !m_IsDragging)
            {
                float dragDistance = GetRelativeDragDistance(Input.touches[m_ActivePointerId].position, m_DragStartPosition);
                float dragTime = Time.realtimeSinceStartup - m_LastTouchDownTimeReal;

                bool isLongTap = dragTime > m_Parameters.ClickDurationThreshold;

                float longTapProgress = 0;
                if (Mathf.Approximately(m_Parameters.ClickDurationThreshold, 0) == false)
                {
                    longTapProgress = Mathf.Clamp01(dragTime / m_Parameters.ClickDurationThreshold);
                }
                m_InputListener.ProgressLongTap(longTapProgress);

                // 单指拖拽
                if (dragDistance >= m_Parameters.DragStartDistanceThresholdRelative &&
                    dragTime >= m_Parameters.DragDurationThreshold)
                {
                    m_IsDragging = true;
                    m_DragStartOffset = m_LastTouchDownPosition - m_DragStartPosition;

                    DragStart(m_DragStartPosition, isLongTap, m_DragStartOffset);
                }
            }

            if (m_IsDragging)
            {
                if (m_ActivePointerId >= Input.touches.Length || m_ActivePointerId < 0)
                {
                    Debug.LogError($"-------------------------InputController_Mobile: UpdateWhenOnUI m_IsDragging = true , Id = {m_ActivePointerId}, touches length = {Input.touches.Length}");
                }
                else
                {
                    DragUpdate(Input.touches[m_ActivePointerId].position);
                }
            }

            if (!m_IsPinching &&
                !m_IsDragging &&
                !m_WasPinchingLastFrame &&
                !m_WasDraggingLastFrame &&
                !m_IsClickPrevented &&
                !m_WasTouchDownLastFrame)
            {
                m_LastTouchDownTimeReal = Time.realtimeSinceStartup;
                if (m_ActivePointerId >= Input.touches.Length || m_ActivePointerId < 0)
                {
                    Debug.LogError($"-------------------------InputController_Mobile: UpdateWhenOnUI m_IsDragging = false and Id = {m_ActivePointerId}, touches length = {Input.touches.Length}");
                }
                else
                {
                    m_DragStartPosition = Input.touches[m_ActivePointerId].position;
                }

                FingerDown(GetAverageTouchPosFromInputTouches());
            }
        }

        private void StartPinch()
        {
            m_PinchStartPositions[0] = m_TouchPositionLastFrame[0] = Input.touches[0].position;
            m_PinchStartPositions[1] = m_TouchPositionLastFrame[1] = Input.touches[1].position;

            m_PinchStartDistance = GetPinchDistance(m_PinchStartPositions[0], m_PinchStartPositions[1]);
            
            m_InputListener.PinchStart((m_PinchStartPositions[0] + m_PinchStartPositions[1]) * 0.5f, m_PinchStartDistance);
            m_IsClickPrevented = true;
            m_PinchRotationVectorStart = Input.touches[1].position - Input.touches[0].position;
            m_PinchVectorLastFrame = m_PinchRotationVectorStart;
        }

        private void UpdatePinch()
        {
            float pinchDistance = GetPinchDistance(Input.touches[0].position, Input.touches[1].position);
            Vector3 pinchVector = Input.touches[1].position - Input.touches[0].position;

            Vector3 pinchCenter = (Input.touches[0].position + Input.touches[1].position) * 0.5f;

            m_InputListener.PinchUpdate(pinchCenter, pinchDistance, m_PinchStartDistance);

            m_PinchVectorLastFrame = pinchVector;
            m_TouchPositionLastFrame[0] = Input.touches[0].position;
            m_TouchPositionLastFrame[1] = Input.touches[1].position;
        }

        private float GetPinchDistance(Vector3 position0, Vector3 position1)
        {
            float distanceX = Mathf.Abs(position0.x - position1.x) / Screen.width;
            float distanceY = Mathf.Abs(position0.y - position1.y) / Screen.height;
            return (Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY));
        }

        private void StopPinch()
        {
            m_DragStartOffset = Vector3.zero;
            m_InputListener.PinchStop();
        }

        private void DragStart(Vector3 position, bool isLongTap, Vector3 offset)
        {
            m_InputListener.InputDragStart(position, isLongTap, offset);

            m_IsClickPrevented = true;
            m_TimeSinceDragStart = 0;
            m_DragFinalMomentumVector.Clear();
        }

        private void DragUpdate(Vector3 position)
        {
            m_TimeSinceDragStart += Time.deltaTime;
            Vector3 offset = Vector3.Lerp(Vector3.zero, m_DragStartOffset, Mathf.Clamp01(m_TimeSinceDragStart * 10.0f));

            m_InputListener.DragUpdate(m_DragStartPosition, position, offset);
        }

        private void DragStop(Vector3 position)
        {
            Vector3 momentum = Vector3.zero;
            if (m_DragFinalMomentumVector.Count > 0)
            {
                for (int i = 0; i < m_DragFinalMomentumVector.Count; ++i)
                {
                    momentum += m_DragFinalMomentumVector[i];
                }
                momentum /= m_DragFinalMomentumVector.Count;
            }

            m_InputListener.DragStop(position, momentum);

            m_DragFinalMomentumVector.Clear();
        }

        private void FingerDown(Vector3 position)
        {
            m_IsTouchDown = true;
            m_InputListener.TouchDown(position);
        }

        private void FingerUp()
        {
            m_IsTouchDown = false;
            m_InputListener.TouchUp(m_LastTouchDownPosition);
        }

        private Vector2 GetAverageTouchPosFromInputTouches()
        {
            Vector2 averagePos = Vector2.zero;
            if (Input.touches != null && Input.touches.Length > 0)
            {
                foreach (var touch in Input.touches)
                {
                    averagePos += touch.position;
                }
                averagePos /= (float)Input.touches.Length;
            }
            return (averagePos);
        }

        private float GetRelativeDragDistance(Vector3 pos0, Vector3 pos1)
        {
            Vector2 dragVector = pos0 - pos1;
            return new Vector2(dragVector.x / Screen.width, dragVector.y / Screen.height).magnitude;
        }

        public Vector3 GetCurrentInputPosition()
        {
            if(Input.touchCount > 0 && m_ActivePointerId >= 0 && m_ActivePointerId < Input.touchCount)
            {
                Vector3 inputPosition = Input.touches[m_ActivePointerId].position;
                return inputPosition;
            }

            return Vector3.zero;
        }
    }
}

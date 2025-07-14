using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UW
{
    /// <summary>
    /// Editor下的输入控制
    /// Editor下只有鼠标操作（不接受键盘输入，暂不考虑远程），所以
    /// 在Editor下只可以支持点击、长按、双击、拖拽操作
    /// </summary>
    public class InputController_Editor : IInputController
    {
        private InputControlParameters  m_Parameters;
        private IInputControlListener   m_InputListener;

        /// <summary>
        /// 每帧更新状态：当前帧鼠标是否在屏幕上活动（按下）
        /// </summary>
        private bool m_IsMouseActive;

        private bool m_IsTouchOnUI;
        private bool m_WasClickDownLastFrame;
        private bool m_IsDragging;
        private bool m_WasDraggingLastFrame;
        private bool m_IsClickDown;
        private bool m_IsClickPrevented;

        private float m_LastClickDownTimeReal;
        private float m_LastClickTimeReal;
        private float m_TimeSinceDragStart;

        private Vector3 m_DragStartPosition;
        private Vector3 m_DragStartOffset;
        private Vector3 m_LastClickDownPosition;

        private List<Vector3> m_DragFinalMomentumVector = new List<Vector3>();

        private const int c_MomentumSamplesCount = 5;

        public InputController_Editor(in InputControlParameters parameters, IInputControlListener controlListener)
        {
            m_Parameters    = parameters;
            m_InputListener = controlListener;
        }

        public void Initialize()
        {
            m_IsTouchOnUI = false;
            m_DragFinalMomentumVector.Clear();
        }

        public void Update(float deltaTime)
        {
            UpdateMouseState(); // 更新鼠标是否活动的状态

            CheckTouchOnUI();

            if(!m_IsMouseActive)
            {
                m_IsTouchOnUI = false;
            }

            // 不在UI上，可以触发拖拽和click
            if (!m_IsTouchOnUI)
            {
                UpdateDrag();
                UpdateClick();
            }
            else
            {
                //在拖转状态下，如果拖拽到UI上也应该拖拽
                if (m_IsTouchOnUI)
                {
                    UpdateDrag();
                }
            }

            // 持续拖拽中，记录拖拽位置
            if (m_IsDragging && m_IsMouseActive && !m_IsClickPrevented)
            {
                m_DragFinalMomentumVector.Add(Input.mousePosition - m_LastClickDownPosition);
                if (m_DragFinalMomentumVector.Count > c_MomentumSamplesCount)
                {
                    m_DragFinalMomentumVector.RemoveAt(0);
                }
            }

            if (!m_IsTouchOnUI)
            {
                // 当前帧鼠标不在UI上活动，则保存按下状态
                m_WasClickDownLastFrame = m_IsMouseActive;
            }
            if (m_WasClickDownLastFrame)
            {
                // 保存按下时鼠标位置
                m_LastClickDownPosition = Input.mousePosition;
            }

            m_WasDraggingLastFrame = m_IsDragging;

            if(!m_IsMouseActive)
            {
                m_IsClickPrevented = false;
                if (m_IsClickDown)
                {
                    ClickUp();
                }
            }
        }

        public void ResetDrag()
        {
            if (!m_IsDragging)
                return;

            DragStop(m_LastClickDownPosition);

            if (m_IsMouseActive)
            {
                m_DragStartOffset = Vector3.zero;
                m_DragStartPosition = Input.mousePosition;
                DragStart(m_DragStartPosition, false, Vector3.zero);
            }
        }

        public void Clear()
        {
            
        }

        private void UpdateMouseState()
        {
            m_IsMouseActive = Input.GetMouseButton(0);
        }

        /// <summary>
        /// 检查是否在UI上
        /// </summary>
        private void CheckTouchOnUI()
        {
            if(m_IsTouchOnUI)
            {
                return;
            }

            if (m_IsMouseActive &&
                EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
            {
                m_IsTouchOnUI = true;
            }
        }


        private void UpdateDrag()
        {
            // 上一帧鼠标按下，这一帧鼠标也按下，没有在拖拽的情况下，检查是否可以拖拽
            if (m_WasClickDownLastFrame && m_IsMouseActive && !m_IsDragging)
            {
                // 得到拖拽距离和拖拽时长
                float dragDistance = GetRelativeDragDistance(Input.mousePosition, m_DragStartPosition);
                float dragTime = Time.realtimeSinceStartup - m_LastClickDownTimeReal;

                // 长按处理
                bool isLongTap = dragTime > m_Parameters.ClickDurationThreshold;
                float longTapProgress = 0;
                if (!Mathf.Approximately(m_Parameters.ClickDurationThreshold, 0))
                {
                    longTapProgress = Mathf.Clamp01(dragTime / m_Parameters.ClickDurationThreshold);
                }

                m_InputListener?.ProgressLongTap(longTapProgress);

                // 拖拽处理
                if (dragDistance >= m_Parameters.DragStartDistanceThresholdRelative && dragTime >= m_Parameters.DragDurationThreshold)
                {
                    m_IsDragging = true;
                    m_DragStartOffset = m_LastClickDownPosition - m_DragStartPosition;

                    DragStart(m_DragStartPosition, isLongTap, m_DragStartOffset);
                }
            }

            // 处于拖拽状态，且当前帧鼠标活跃，持续拖拽中
            if (m_IsDragging && m_IsMouseActive)
            {
                DragUpdate(Input.mousePosition);
            }

            // 处于拖拽状态，且当前帧鼠标不再活跃，拖拽结束
            if (m_IsDragging && !m_IsMouseActive)
            {
                m_IsDragging = false;
                DragStop(m_LastClickDownPosition);
            }
        }

        private void UpdateClick()
        {
            // 拖拽的时候，不处理点击
            if (m_IsDragging || m_IsClickPrevented)
            {
                return;
            }

            // 上一帧没有点击，当前帧鼠标按下，处于按下状态
            if (!m_WasClickDownLastFrame && m_IsMouseActive)
            {
                m_LastClickDownTimeReal = Time.realtimeSinceStartup;
                m_DragStartPosition = Input.mousePosition;
                ClickDown(Input.mousePosition);
            }

            if (m_WasClickDownLastFrame && !m_IsMouseActive)
            {
                float fingerDownUpDuration = Time.realtimeSinceStartup - m_LastClickDownTimeReal;

                if (!m_WasDraggingLastFrame)
                {
                    float clickDuration = Time.realtimeSinceStartup - m_LastClickTimeReal;

                    bool isDoubleClick = clickDuration < m_Parameters.DoubleclickDurationThreshold;
                    bool isLongTap = fingerDownUpDuration > m_Parameters.ClickDurationThreshold;

                    m_InputListener?.InputClick(m_LastClickDownPosition, isDoubleClick, isLongTap);

                    m_LastClickTimeReal = Time.realtimeSinceStartup;
                }
            }
        }

        private float GetRelativeDragDistance(Vector3 position0, Vector3 position1)
        {
            Vector2 dragVector = position0 - position1;
            return new Vector2(dragVector.x / Screen.width, dragVector.y / Screen.height).magnitude;
        }

        private void DragStart(Vector3 dragStartPosition, bool isLongTap, Vector3 dragStartOffset)
        {
            m_InputListener?.InputDragStart(dragStartPosition, isLongTap, dragStartOffset);

            m_IsClickPrevented = true;
            m_TimeSinceDragStart = 0;
            m_DragFinalMomentumVector.Clear();
        }

        private void DragUpdate(Vector3 position)
        {
            m_TimeSinceDragStart += Time.deltaTime;
            Vector3 offset = Vector3.Lerp(Vector3.zero, m_DragStartOffset, Mathf.Clamp01(m_TimeSinceDragStart * 10.0f));

            m_InputListener?.DragUpdate(m_DragStartPosition, position, offset);
        }

        private void DragStop(Vector3 position)
        {
            Vector3 momentum = Vector3.zero;
            int momentumCount = m_DragFinalMomentumVector.Count;
            if (momentumCount > 0)
            {
                for (int i = 0; i < momentumCount; ++i)
                {
                    momentum += m_DragFinalMomentumVector[i];
                }
                momentum /= momentumCount;
            }

            m_InputListener?.DragStop(position, momentum);

            m_DragFinalMomentumVector.Clear();
        }

        private void ClickDown(Vector3 position)
        {
            m_IsClickDown = true;
            m_InputListener?.TouchDown(position);
        }

        private void ClickUp()
        {
            m_IsClickDown = false;
            m_InputListener?.TouchUp(m_LastClickDownPosition);
        }

        public Vector3 GetCurrentInputPosition()
        {
            return Input.mousePosition;
        }
    }
}

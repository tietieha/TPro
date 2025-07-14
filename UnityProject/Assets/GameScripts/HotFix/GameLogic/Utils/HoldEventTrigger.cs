using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XLua;

namespace UW
{
    [LuaCallCSharp]
    public class HoldEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private float m_DurationThreshold = 1.0f;
        [SerializeField] private float m_Interval = 0.3f;
        //控制长按后是否还能触发抬起事件
        [SerializeField] private bool longPressUpTrigger = true;

        public UnityEvent HoldEvent         = new UnityEvent();        
        public UnityEvent DelayEvent        = new UnityEvent(); // 第一次触发时的等待时间，每帧执行        
        public UnityEvent PressDownEvent    = new UnityEvent(); // 按下事件        
        public UnityEvent PressUpEvent      = new UnityEvent(); // 抬起事件

        public  bool    IsRepeatTrigger         = true;
        private bool    m_IsPointerDown         = false;
        private bool    m_LongPressTriggered    = false;
        private float   m_TimePressStarted;

        private WaitForSeconds m_WaitSeconds;

        protected override void Start()
        {
            m_WaitSeconds = new WaitForSeconds(m_Interval);
        }

        protected override void OnDisable()
        {
            m_IsPointerDown = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_IsPointerDown && !m_LongPressTriggered)
            {
                if (Time.time - m_TimePressStarted > m_DurationThreshold)
                {
                    m_LongPressTriggered = true;
                    if (IsRepeatTrigger)
                    {
                        StartCoroutine(OnHold());
                    }
                    else
                    {
                        HoldEvent.Invoke();
                    }
                }
                else
                {
                    DelayEvent?.Invoke();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_TimePressStarted = Time.time;
            m_IsPointerDown = true;
            m_LongPressTriggered = false;
            PressDownEvent?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_IsPointerDown = false;
            StopAllCoroutines();
            if(!m_LongPressTriggered || longPressUpTrigger)
                PressUpEvent?.Invoke();
            m_LongPressTriggered = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_IsPointerDown = false;
            StopAllCoroutines();
        }

        private IEnumerator OnHold()
        {
            while (m_IsPointerDown && m_LongPressTriggered)
            {
                HoldEvent.Invoke();
                yield return m_WaitSeconds;
            }
        }
    }
}

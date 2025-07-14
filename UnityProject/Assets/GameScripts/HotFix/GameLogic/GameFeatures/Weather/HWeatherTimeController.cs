using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.HWeather
{
    [ExecuteInEditMode]
    [AddComponentMenu("HWeather/HWeather Time Controller")]
    [RequireComponent(typeof(HWeatherController))]
    public class HWeatherTimeController : MonoBehaviour
    {
        // References
        private HWeatherController m_skyController;
        // Options
        public HWeatherTimeSystem timeSystem = HWeatherTimeSystem.Simple;
        public HWeatherTimeDirection timeDirection = HWeatherTimeDirection.Forward;
        // Cycle of day
        public float timeline = 12.0f;
        private float m_timeOfDay = 12.0f;
        private float m_timeProgressionStep;
        // Time and date
        // Time and date
        public int hour = 6;
        public int minute = 0;
        public float dayLength = 24.0f; //单位分钟。
        public bool isTimeEvaluatedByCurve;
        public AnimationCurve dayLengthCurve = AnimationCurve.Linear(0, 0, 24, 24);
        // 2022/9/6 19:04 teddysjwu: 避免每帧获取dayLengthCurve.keys，这个是有GC的
        private Keyframe[] m_keys;
        public Keyframe[] keys
        {
            get { return m_keys ??= dayLengthCurve.keys; }
            set
            {
                dayLengthCurve.keys = value;
                m_keys = value;
            }
        }
        private int lastIndex;
        private bool reDrawShadowEnd = false;
        public Action<bool> DayTypeChange;
        public Action DayTimeFinish;

        private void Reset()
        {
        }

        private void Start()
        {
            m_skyController = GetComponent<HWeatherController>();
            // Calculates the progression step to move the timeline
            m_timeProgressionStep = GetTimeProgressionStep();
            UpdateTimeSystem();
            keys = dayLengthCurve.keys;
            lastIndex = -1;
        }

        //夜晚，清晨，白天，黄昏，夜晚
        private bool IsNextKeyFrame(float timeline)
        {
            var flag = false;
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                if (timeline >= keys[i].time && i > lastIndex)
                {
                    lastIndex = i;
                    flag = true;
                    break;
                }
            }

            return flag;
        }


        private void Update()
        {
            // Update time of day
#if HMM_DEBUG_TEMP
            m_timeProgressionStep = GetTimeProgressionStep();
#endif
            if (HWeatherCommon.s_openDayTime)
            {
                timeline += m_timeProgressionStep * Time.deltaTime;
            }

            m_timeOfDay = isTimeEvaluatedByCurve ? dayLengthCurve.Evaluate(timeline) : timeline;
            hour = (int) Mathf.Floor(m_timeOfDay);
            minute = (int) Mathf.Floor(m_timeOfDay * 60 % 60);

            // Only in gameplay
            if (Application.isPlaying)
            {
                //下一帧重置。
                if (reDrawShadowEnd && HWeatherCommon.s_reDrawShadow)
                {
                    HWeatherCommon.s_reDrawShadow = false;
                }
                if (keys != null)
                {
                    if (lastIndex != keys.Length - 1)
                    {
                        if (IsNextKeyFrame(timeline))
                        {
                            //奇数代表过渡期的起始点
                            //偶数代表过期的结束点
                            reDrawShadowEnd = lastIndex % 2 == 0;
                            HWeatherCommon.s_reDrawShadow = true;
                            DayTypeChange?.Invoke(lastIndex % 2 == 1);
                        }
                    }
                    else if (lastIndex == keys.Length - 1)
                    {
                        lastIndex = 0;
                    }
                }
                // // Timeline transition
                // if(m_isTimelineTransitionInProgress) DoTimelineTransition(m_timelineSourceTransitionTime, m_timelineDestinationTransitionTime);
                // Next day
                if (timeline > 24)
                {
                    timeline = 0;
                    DayTimeFinish?.Invoke();
                }
                UpdateTimeSystem();
            }
            // Editor only
#if UNITY_EDITOR
            UpdateTimeSystem();
#endif
        }

        public void UpdateTimeSystem()
        {
            m_skyController.timeOfDay = m_timeOfDay;
        }

        /// <summary>
        /// Adjust the calendar when there is a change in the date.
        /// </summary>
        public void UpdateCalendar()
        {
        }

        /// <summary>
        /// Computes the time progression step based on the day length value.
        /// </summary>
        private float GetTimeProgressionStep()
        {
            if (dayLength > 0.0f)
                return (24.0f / 60.0f) / dayLength;
            else
                return 0.0f;
        }
    }
}

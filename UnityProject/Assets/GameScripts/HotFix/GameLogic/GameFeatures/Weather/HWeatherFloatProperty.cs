using System;

namespace UnityEngine.HWeather
{
    [Serializable]
    public sealed class HWeatherFloatProperty
    {
        public enum PropertyType
        {
            Slider,
            TimelineCurve,
        }

        public PropertyType type = PropertyType.Slider;
        public float slider;
        public AnimationCurve timelineCurve;

        public HWeatherFloatProperty(float slider, AnimationCurve timelineCurve)
        {
            this.slider = slider;
            this.timelineCurve = timelineCurve;
        }
        public float GetValue(float time )
        {
            switch (type)
            {
                case PropertyType.Slider:
                    return slider;
                
                case PropertyType.TimelineCurve:
                    return timelineCurve.Evaluate(time);
                
                default:
                    return slider;
            }
        }
    }
}
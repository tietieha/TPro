using System;

namespace UnityEngine.HWeather
{
    [Serializable]
    public sealed class HWeatherColorProperty
    {
        public enum PropertyType
        {
            Color,
            TimelineGradient
        }

        public PropertyType type = PropertyType.Color;
        [ColorUsage(true, true)]
        public Color color;
        [GradientUsage(true)]
        public Gradient timelineGradient;
 


        public HWeatherColorProperty(Color color, Gradient timelineGradient)
        {
            this.color = color;
            this.timelineGradient = timelineGradient;
        }
        
        public Color GetValue(float time)
        {
            switch (type)
            {
                case PropertyType.Color:
                    return color;
                
                case PropertyType.TimelineGradient:
                    return timelineGradient.Evaluate(time / 24.0f);
                default:
                    return color;
            }
        }
    }
}
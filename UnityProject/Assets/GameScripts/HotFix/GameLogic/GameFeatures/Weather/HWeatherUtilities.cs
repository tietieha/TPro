using System;
using UnityEngine.Events;

namespace UnityEngine.HWeather
{
    [Serializable]
    public struct HWeatherGlobalWeather
    {
        public HWeatherProfile profile;
        public float transitionTime;
    }
    
    public enum HWeatherTimeSystem
    {
        Simple,
        Realistic
    }
    
    public enum HWeatherTimeRepeatMode
    {
        Off,
        ByDay,
        ByMonth,
        ByYear
    }
    
    public enum HWeatherScatteringMode
    {
        Automatic,
        CustomColor
    }

    public enum HWeatherCloudMode
    {
        EmptySky,
        StaticClouds,
        DynamicClouds
    }

    public enum HWeatherTimeDirection
    {
        Forward,
        Back
    }

    public enum HWeatherEventScanMode
    {
        ByMinute,
        ByHour
    }
    
    public enum HWeatherOutputType
    {
        Slider,
        TimelineCurve,
        SunCurve,
        MoonCurve,
        Color,
        TimelineGradient,
        SunGradient,
        MoonGradient
    }

    public enum HWeatherReflectionProbeState
    {
        On,
        Off
    }

    public enum HWeatherShaderUpdateMode
    {
        Global,
        ByMaterial
    }

    [Serializable]
    public sealed class HWeatherEventAction
    {
        // Not included in build
        #if UNITY_EDITOR
        public bool isExpanded = true;
        #endif
        
        public UnityEvent eventAction;
        public int hour = 6;
        public int minute = 0;
        public int year = 2020;
        public int month = 1;
        public int day = 1;
    }
    
    /// <summary>
    /// Thunder settings container.
    /// </summary>
    [Serializable]
    public sealed class HWeatherThunderSettings
    {
        public Transform thunderPrefab;
        public AudioClip audioClip;
        public AnimationCurve lightFrequency;
        public float audioDelay;
        public Vector3 position;
    }
}
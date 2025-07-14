using System.Collections.Generic;

namespace UnityEngine.HWeather
{
    [CreateAssetMenu(fileName = "HWeather Profile", menuName = "HWeather/New Sky Profile", order = 1)]
    public sealed class HWeatherProfile : ScriptableObject
    {
        // Not included in the build
        #if UNITY_EDITOR

        public bool showLightingGroup = true;
        public bool showWeatherGroup = true;
        public bool showCloudShadowGroup = true;
        public bool showFogGroup = true;
        public bool showWindGroup = true;
        #endif
        ///////////////////////////////////////////////// 光 /////////////////////////////////////////////////////////////
        // Directional light intensity
        public HWeatherFloatProperty directionalLightIntensity = new HWeatherFloatProperty
        (
	        1.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        // Directional light color
        public HWeatherColorProperty directionalLightColor = new HWeatherColorProperty
        (
	        Color.white,
	        new Gradient()
        );
        public HWeatherFloatProperty directionalLightRotationX = new HWeatherFloatProperty
        (
	        0.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty directionalLightRotationY = new HWeatherFloatProperty
        (
	        45.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty directionalLightRotationZ = new HWeatherFloatProperty
        (
	        0.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        // Ambient intensity
        public HWeatherFloatProperty environmentIntensity = new HWeatherFloatProperty
        (
	        1.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
		
        // Ambient color
        public HWeatherColorProperty environmentAmbientColor = new HWeatherColorProperty
        (
	        Color.white,
	        new Gradient()
        );
		
        // Ambient equator color
        public HWeatherColorProperty environmentEquatorColor = new HWeatherColorProperty
        (
	        Color.white,
	        new Gradient()
        );
		
        // Ambient ground color
        public HWeatherColorProperty environmentGroundColor = new HWeatherColorProperty
        (
	        Color.white,
	        new Gradient()
        );
        ///////////////////////////////////////////////// 雨 /////////////////////////////////////////////////////////////
        // Light rain intensity
		public HWeatherFloatProperty lightRainIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
		
		// Medium rain intensity
		public HWeatherFloatProperty mediumRainIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
		
		// Heavy rain intensity
		public HWeatherFloatProperty heavyRainIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
        // Rain color
		public HWeatherColorProperty rainColor = new HWeatherColorProperty
		(
			Color.white,
			new Gradient()
		);

        ///////////////////////////////////////////////// 雪 /////////////////////////////////////////////////////////////
		// Snow intensity
		public HWeatherFloatProperty lightSnowIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
        public HWeatherFloatProperty mediumSnowIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
        public HWeatherFloatProperty heavySnowIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
		// Snow color
		public HWeatherColorProperty snowColor = new HWeatherColorProperty
		(
			Color.white,
			new Gradient()
		);
        ///////////////////////////////////////////////// 沙 /////////////////////////////////////////////////////////////
		// Snow intensity
		public HWeatherFloatProperty lightSandIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
        public HWeatherFloatProperty mediumSandIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
        public HWeatherFloatProperty heavySandIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
		// Snow color
		public HWeatherColorProperty sandColor = new HWeatherColorProperty
		(
			Color.white,
			new Gradient()
		);
      ///////////////////////////////////////////////// 落叶 /////////////////////////////////////////////////////////////
        public HWeatherFloatProperty screenLeafIntensity = new HWeatherFloatProperty
		(
			0.0f,
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f)
		);
		public HWeatherColorProperty screenLeafColor = new HWeatherColorProperty
		(
			Color.white,
			new Gradient()
		);

        
        ///////////////////////////////////////////////// 云投影 /////////////////////////////////////////////////////////////
        public HWeatherFloatProperty g_CloudShadowNearScale = new HWeatherFloatProperty
        (
	        0.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_CloudShadowFarScale = new HWeatherFloatProperty
        (
	        0.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_CloudShadowScaleMixStart = new HWeatherFloatProperty
        (
	        0.0f,
	        AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_CloudShadowScaleMixEnd = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_CloudShadowOverlay = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_CloudShadowOverlayPow = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_CloudShadowInt = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        ///////////////////////////////////////////////// 距离雾 /////////////////////////////////////////////////////////////
        public HWeatherFloatProperty g_FogLineStart = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_FogLineEnd = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherColorProperty g_FogLineColor = new HWeatherColorProperty
        (
	        Color.white,
	        new Gradient()
        );
        public HWeatherColorProperty g_FogLineFarColor = new HWeatherColorProperty
        (
	        Color.white,
	        new Gradient()
        );
        public HWeatherFloatProperty g_FoglineDisNearOffset = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_FoglineDisFarOffset = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_FoglineDisMin = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_FogHeightStart = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_FogHeightEnd = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_FogLineOverrideHeight = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_FogLineOverrideHeightDisOffset = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        ///////////////////////////////////////////////// 风 /////////////////////////////////////////////////////////////
        public HWeatherFloatProperty g_WindStrenght = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_WindSpeed = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_Shake = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_WindTurbulence = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        public HWeatherFloatProperty g_WindBending = new HWeatherFloatProperty
        (
            0.0f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );
        //新风
        public HWeatherFloatProperty g_windPower = new HWeatherFloatProperty
        (
            0.5f,
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f)
        );

    }
}
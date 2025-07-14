namespace UnityEngine.HWeather
{
    public sealed class HWeatherSettings
    {
        // Lighting
        public float DirectionalLightIntensity = 1.0f;
        public Color DirectionalLightColor = Color.white;
        public float EnvironmentIntensity = 1.0f;
        public float directionalLightRotationX = 0.0f;
        public float directionalLightRotationY = 45.0f;
        public float directionalLightRotationZ = 0.0f;

        public Color EnvironmentAmbientColor = Color.white;
        public Color EnvironmentEquatorColor = Color.white;
        public Color EnvironmentGroundColor = Color.white;
        
        // Weather
        public float LightRainIntensity = 0.0f;
        public float MediumRainIntensity = 0.0f;
        public float HeavyRainIntensity = 0.0f;
        public Color RainColor = Color.white;

        public float LightSnowIntensity = 0.0f;
        public float MediumSnowIntensity = 0.0f;
        public float HeavySnowIntensity = 0.0f;
        public Color SnowColor = Color.white;

        public float LightSandIntensity = 0.0f;
        public float MediumSandIntensity = 0.0f;
        public float HeavySandIntensity = 0.0f;
        public Color SandColor = Color.white;

        public float ScreenLeafIntensity = 0.0f;
        public Color ScreenLeaColor = Color.white;
        
        
        // public float SnowIntensity = 0.0f;


        public float WindSpeed = 0.0f;
        public float WindDirection = 0.0f;

        //距离雾
        public float g_FogLineStart = 35.0f;
        public float g_FogLineEnd = 75.0f;
        public Color g_FogLineColor = Color.white;
        public Color g_FogLineFarColor = Color.white;
        public float g_FoglineDisNearOffset = 0.75f;
        public float g_FoglineDisFarOffset = 1.50f;
        public float g_FoglineDisMin = 100.0f;
        public float g_FogHeightStart = 0.0f;
        public float g_FogHeightEnd = 40.0f;
        public float g_FogLineOverrideHeight = 1000.0f;
        public float g_FogLineOverrideHeightDisOffset = 1.0f;
        //云投影
        // Shader.SetGlobalTexture("g_CloudShadow", g_CloudShadow);
        // Shader.SetGlobalVector("g_CloudShadow_ST", g_CloudShadow_ST);
        public float g_CloudShadowNearScale = 1.0f;
        public float g_CloudShadowFarScale = 1.0f;
        public float g_CloudShadowScaleMixStart = 0.0f;
        public float g_CloudShadowScaleMixEnd = 0.0f;
        public float g_CloudShadowInt = 1.0f;
        public float g_CloudShadowOverlay = 0.0f;
        public float g_CloudShadowOverlayPow = 4.0f; //尝试优化城固定值
        // //风
        // public float g_WindStrenght = 0.5f;
        // public float g_WindSpeed = 1.5f;
        // public float g_Shake = 1.0f;
        // public float g_WindTurbulence = 0.0f;
        // public float g_WindBending = 0.0f;
        //新风
        public float g_windPower = 0.0f;
    }
}
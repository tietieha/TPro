namespace UnityEngine.HWeather
{
    internal static class HWeatherShaderUniforms
    {
        //// Textures
        // internal static readonly int StaticCloudSourceTexture = Shader.PropertyToID("_Azure_StaticCloudSourceTexture");
        // internal static readonly int StaticCloudTargetTexture = Shader.PropertyToID("_Azure_StaticCloudTargetTexture");
        //距离雾
        internal static readonly int g_FogLineStart = Shader.PropertyToID("g_FogLineStart");
        internal static readonly int g_FogLineEnd = Shader.PropertyToID("g_FogLineEnd");
        internal static readonly int g_FogLineColor = Shader.PropertyToID("g_FogLineColor");
        internal static readonly int g_FogLineFarColor = Shader.PropertyToID("g_FogLineFarColor");
        internal static readonly int g_FoglineDisNearOffset = Shader.PropertyToID("g_FoglineDisNearOffset");
        internal static readonly int g_FoglineDisFarOffset = Shader.PropertyToID("g_FoglineDisFarOffset");
        internal static readonly int g_FoglineDisMin = Shader.PropertyToID("g_FoglineDisMin");
        internal static readonly int g_FogHeightStart = Shader.PropertyToID("g_FogHeightStart");
        internal static readonly int g_FogHeightEnd = Shader.PropertyToID("g_FogHeightEnd");
        internal static readonly int g_FogLineOverrideHeight = Shader.PropertyToID("g_FogLineOverrideHeight");
        internal static readonly int g_FogLineOverrideHeightDisOffset = Shader.PropertyToID("g_FogLineOverrideHeightDisOffset");

        //云投影
        internal static readonly int g_CloudShadow = Shader.PropertyToID("g_CloudShadow");
        internal static readonly int g_CloudShadow_ST = Shader.PropertyToID("g_CloudShadow_ST");
        internal static readonly int g_CloudShadowNearScale = Shader.PropertyToID("g_CloudShadowNearScale");
        internal static readonly int g_CloudShadowFarScale = Shader.PropertyToID("g_CloudShadowFarScale");
        internal static readonly int g_CloudShadowScaleMixStart = Shader.PropertyToID("g_CloudShadowScaleMixStart");
        internal static readonly int g_CloudShadowScaleMixEnd = Shader.PropertyToID("g_CloudShadowScaleMixEnd");
        internal static readonly int g_CloudShadowInt = Shader.PropertyToID("g_CloudShadowInt");
        internal static readonly int g_CloudShadowOverlay = Shader.PropertyToID("g_CloudShadowOverlay");
        internal static readonly int g_CloudShadowOverlayPow = Shader.PropertyToID("g_CloudShadowOverlayPow");
        //世界法线
        internal static readonly int g_TerrainNormal = Shader.PropertyToID("g_TerrainNormal");
        internal static readonly int g_TerrainNormal_ST = Shader.PropertyToID("g_TerrainNormal_ST");
        internal static readonly int g_TerrainNearNormalScale = Shader.PropertyToID("g_TerrainNearNormalScale");
        internal static readonly int g_TerrainFarNormalScale = Shader.PropertyToID("g_TerrainFarNormalScale");
        internal static readonly int g_TerrainNormalScaleMixStart = Shader.PropertyToID("g_TerrainNormalScaleMixStart");
        internal static readonly int g_TerrainNormalScaleMixEnd = Shader.PropertyToID("g_TerrainNormalScaleMixEnd");
        //环境反射
        internal static readonly int g_SpecCube = Shader.PropertyToID("g_SpecCube");
        //风
        // internal static readonly int g_WindNoise = Shader.PropertyToID("g_WindNoise");
        // internal static readonly int g_WindNoise_ST = Shader.PropertyToID("g_WindNoise_ST");
        // internal static readonly int g_WindStrenght = Shader.PropertyToID("g_WindStrenght");
        // internal static readonly int g_WindSpeed = Shader.PropertyToID("g_WindSpeed");
        // internal static readonly int g_Shake = Shader.PropertyToID("g_Shake");
        // internal static readonly int g_WindTurbulence = Shader.PropertyToID("g_WindTurbulence");
        // internal static readonly int g_WindBending = Shader.PropertyToID("g_WindBending");
        // internal static readonly int g_WindDirection = Shader.PropertyToID("g_WindDirection");
        // internal static readonly int g_MBGlobalWindDir = Shader.PropertyToID("g_MBGlobalWindDir");
        //新风
        internal static readonly int g_windPower = Shader.PropertyToID("g_windPower");


        
    }
}
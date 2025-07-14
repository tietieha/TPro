using System;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.HWeather
{
    [ExecuteInEditMode]
    [AddComponentMenu("HWeather/Weather Controller")]
    public class HWeatherController : MonoBehaviour
    {
        // Not included in the build
#if UNITY_EDITOR
        public bool showReferencesHeaderGroup = true;
        public bool showProfilesHeaderGroup = false;
#endif

        ////////////////////////// 静态设置 //////////////////////////
        public Light directionalLight;
        public Texture2D g_CloudShadow;
        public Vector4 g_CloudShadow_ST;
        public Texture2D g_TerrainNormal;
        public Vector4 g_TerrainNormal_ST;
        public float g_TerrainNearNormalScale = 1; //近处 世界法线 强度缩放
        public float g_TerrainFarNormalScale = 0; //远处 世界法线 强度缩放
        public float g_TerrainNormalScaleMixStart = 100; // 混合 开始距离
        public float g_TerrainNormalScaleMixEnd = 500; // 混合 结束距离

        public Cubemap g_SpecCube;

        //////////////////////////////////////////////////////////////////////////////
        // Sky settings
        public HWeatherSettings settings = new HWeatherSettings();
        public float timeOfDay;
        // Profiles
        public HWeatherProfile defaultProfile;
        public HWeatherProfile currentProfile;
        public HWeatherProfile targetProfile;
        // private AzureSkyProfile m_nextDayProfile;

        // Lists
        public List<HWeatherProfile> defaultProfileList = new List<HWeatherProfile>();
        public List<HWeatherGlobalWeather> globalWeatherList = new List<HWeatherGlobalWeather>();
        public List<HWeatherZone> weatherZoneList = new List<HWeatherZone>();

        // Global weather transition
        public float globalWeatherTransitionProgress = 0.0f;
        public float globalWeatherTransitionTime = 15.0f;
        public float globalWeatherStartTransitionTime = 0.0f;
        public float defaultWeatherTransitionTime = 10.0f;
        public int globalWeatherIndex = -1;
        public bool isGlobalWeatherChanging = false;

        // Local weather zones
        public Transform weatherZoneTrigger;
        private Vector3 m_weatherZoneTriggerPosition;
        private Vector3 m_weatherZoneClosestPoint;
        private float m_weatherZoneClosestDistanceSqr;
        private float m_weatherZoneDistance;
        private float m_weatherZoneBlendDistanceSqr;
        private float m_weatherZoneInterpolationFactor;
        private Collider m_weatherZoneCollider;
        public ELandType landType = ELandType.EAll;
        public EWeatherType[] weatherTypes = new EWeatherType[(int) ELandType.EMax];
        public Action<ELandType> ChangeLandType;
        private void OnEnable()
        {
#if ART_PROJECT
                Shader.EnableKeyword("_USE_UNITY_SHADOW");
                Shader.EnableKeyword("_USE_ART_EDITOR");
#endif
            RenderSettings.ambientMode = AmbientMode.Trilight;
        }

        private void Start()
        {
            defaultProfile = defaultProfileList[0];
            currentProfile = defaultProfile;
            targetProfile = defaultProfile;
            // First update of the shader uniforms
            UpdateProfiles();
            UpdateSkySettings();
        }

        private void Update()
        {
            // Editor only
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                defaultProfile = defaultProfileList[0];
                currentProfile = defaultProfile;
            }
#endif
            // Debug.Log("timeOfDay: " + timeOfDay);
            // Update shader uniforms
            UpdateProfiles();

            //设置数据
            UpdateSkySettings();

        }


        public int GetNewWeatherProfile(EWeatherType type)
        {
            for (int i = 0; i < globalWeatherList.Count; i++)
            {
                var name = globalWeatherList[i].profile.name;
                if (HWeatherCommon.GetWeatherType(name) == type)
                {
                    return i;
                }
            }

            return -1;
        }


        /// <summary>
        /// Changes the global weather with a smooth transition.
        /// </summary>
        /// Set the index to -1 if you want to reset the global weather back to the default day profile.
        /// <param name="index">The target profile number in the "global weather profiles" list.</param>
        public void SetNewWeatherProfile(int index)
        {
            switch (index)
            {
                // Back to the default day profile currently in use by sky manager
                case -1:
                    if (defaultProfile)
                    {
                        targetProfile = defaultProfile;
                        globalWeatherTransitionTime = defaultWeatherTransitionTime;
                        globalWeatherIndex = index;
                    }
                    break;

                // Changes the global weather to the corresponding profile index in the global weather list
                default:
                    if (index < 0 || index >= globalWeatherList.Count)
                    {
                        Debug.LogWarningFormat("Weather SetNewWeatherProfile {0} is not legal", index);
                        return;
                    }
                    if (globalWeatherList[index].profile)
                    {
                        targetProfile = globalWeatherList[index].profile;
                        globalWeatherTransitionTime = globalWeatherList[index].transitionTime;
                        globalWeatherIndex = index;
                    }
                    break;
            }
            // Starts the global weather transition progress
            globalWeatherTransitionProgress = 0.0f;
            globalWeatherStartTransitionTime = Time.time;
            isGlobalWeatherChanging = true;
        }

        /// <summary>
        /// Performs the default profile transition when the time changes to the next calendar day at 24 o'clock.
        /// </summary>
        public void PerformDayTransition()
        {

        }

        public void OnDayChange()
        {

        }

        public void UpdateMaterialSettings()
        {

        }

        public void UpdateSkySettings(Material mat)
        {

        }

        public void UpdateSkySettings()
        {
            UpdateStaticData();
            UpdateEnvLight();
            UpdateGlobalShaderSet();
        }
        //设置静态数据
        public void UpdateStaticData()
        {
            // 云投影
            Shader.SetGlobalTexture(HWeatherShaderUniforms.g_CloudShadow, g_CloudShadow);
            Shader.SetGlobalVector(HWeatherShaderUniforms.g_CloudShadow_ST, g_CloudShadow_ST);
            //世界法线
            Shader.SetGlobalTexture(HWeatherShaderUniforms.g_TerrainNormal, g_TerrainNormal);
            Shader.SetGlobalVector(HWeatherShaderUniforms.g_TerrainNormal_ST, g_TerrainNormal_ST);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_TerrainNearNormalScale, g_TerrainNearNormalScale);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_TerrainFarNormalScale, g_TerrainFarNormalScale);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_TerrainNormalScaleMixStart, g_TerrainNormalScaleMixStart);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_TerrainNormalScaleMixEnd, g_TerrainNormalScaleMixEnd);
            //环境反射
            Shader.SetGlobalTexture(HWeatherShaderUniforms.g_SpecCube, g_SpecCube);

            // //风
            // Shader.SetGlobalTexture(HWeatherShaderUniforms.g_WindNoise, g_WindNoise);
            // Shader.SetGlobalVector(HWeatherShaderUniforms.g_WindNoise_ST, g_WindNoise_ST);
        }

        public void UpdateEnvLight()
        {
            // Environment lighting
            directionalLight.intensity = settings.DirectionalLightIntensity;
            directionalLight.color = settings.DirectionalLightColor;
            directionalLight.transform.localRotation = Quaternion.Euler(settings.directionalLightRotationX, settings.directionalLightRotationY, settings.directionalLightRotationZ);
            ;

            RenderSettings.ambientIntensity = settings.EnvironmentIntensity;
            RenderSettings.ambientLight = settings.EnvironmentAmbientColor;
            RenderSettings.ambientSkyColor = settings.EnvironmentAmbientColor;
            RenderSettings.ambientEquatorColor = settings.EnvironmentEquatorColor;
            RenderSettings.ambientGroundColor = settings.EnvironmentGroundColor;
        }


        private void UpdateGlobalShaderSet()
        {
            // 距离雾
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogLineStart, settings.g_FogLineStart);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogLineEnd, settings.g_FogLineEnd);
            Shader.SetGlobalColor(HWeatherShaderUniforms.g_FogLineColor, settings.g_FogLineColor);
            Shader.SetGlobalColor(HWeatherShaderUniforms.g_FogLineFarColor, settings.g_FogLineFarColor);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FoglineDisNearOffset, settings.g_FoglineDisNearOffset);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FoglineDisFarOffset, settings.g_FoglineDisFarOffset);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FoglineDisMin, settings.g_FoglineDisMin);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogHeightStart, settings.g_FogHeightStart);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogHeightEnd, settings.g_FogHeightEnd);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogLineOverrideHeight, settings.g_FogLineOverrideHeight);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogLineOverrideHeightDisOffset, settings.g_FogLineOverrideHeightDisOffset);
            // 云投影
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_CloudShadowNearScale, settings.g_CloudShadowNearScale);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_CloudShadowFarScale, settings.g_CloudShadowFarScale);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_CloudShadowScaleMixStart, settings.g_CloudShadowScaleMixStart);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_CloudShadowScaleMixEnd, settings.g_CloudShadowScaleMixEnd);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_CloudShadowInt, settings.g_CloudShadowInt);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_CloudShadowOverlay, settings.g_CloudShadowOverlay);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_CloudShadowOverlayPow, settings.g_CloudShadowOverlayPow);
            //新风
        }

        /// <summary>
        /// Gets the current sky setting if there is no global weather transition or local weather zone influence.
        /// </summary>
        /// Computes the global weather transition.
        /// <summary>
        /// Computes local weather zones influence.
        /// </summary>
        /// Returns the final sky setting resulting from the profile blending process.
        private void UpdateProfiles()
        {
            if (!isGlobalWeatherChanging)
            {
                // 在没有全球天气转换或本地天气区域影响时获取当前天空设置
                GetDefaultSettings();
            }
            else
            {
                //全局天气过渡
                globalWeatherTransitionProgress = Mathf.Clamp01((Time.time - globalWeatherStartTransitionTime) / globalWeatherTransitionTime);
                // Performs the global weather blend
                ApplyGlobalWeatherTransition(currentProfile, targetProfile, globalWeatherTransitionProgress);
                // Ends the global weather transition
                if (Math.Abs(globalWeatherTransitionProgress - 1.0f) <= 0.0f)
                {
                    isGlobalWeatherChanging = false;
                    globalWeatherTransitionProgress = 0.0f;
                    globalWeatherStartTransitionTime = 0.0f;
                    currentProfile = targetProfile;
                }
            }
            // 计算局部天气系统
            // Based on Unity's Post Processing v2
            if (!weatherZoneTrigger)
                return;

            m_weatherZoneTriggerPosition = weatherZoneTrigger.position;
            var list = new List<ELandType>();
            //遍历“天气区域”列表中的所有天气区域
            foreach (var weatherZone in weatherZoneList)
            {
                // Skip if the list index is null
                if (weatherZone == null)
                    continue;

                if (!weatherZone.isActiveAndEnabled)
                    continue;

                // If weather zone has no collider, skip it as it's useless
                m_weatherZoneCollider = weatherZone.GetComponent<Collider>();
                if (!m_weatherZoneCollider)
                    continue;

                if (!m_weatherZoneCollider.enabled)
                    continue;

                // Find closest distance to weather zone, 0 means it's inside it
                m_weatherZoneClosestDistanceSqr = float.PositiveInfinity;

                m_weatherZoneClosestPoint = m_weatherZoneCollider.ClosestPoint(m_weatherZoneTriggerPosition); // 5.6-only API
                m_weatherZoneDistance = ((m_weatherZoneClosestPoint - m_weatherZoneTriggerPosition) / 2f).sqrMagnitude;

                if (m_weatherZoneDistance < m_weatherZoneClosestDistanceSqr)
                    m_weatherZoneClosestDistanceSqr = m_weatherZoneDistance;

                m_weatherZoneCollider = null;
                m_weatherZoneBlendDistanceSqr = weatherZone.blendDistance * weatherZone.blendDistance;

                // Weather zone has no influence, ignore it
                // Note: Weather zone doesn't do anything when `closestDistanceSqr = blendDistSqr` but
                //       we can't use a >= comparison as blendDistSqr could be set to 0 in which
                //       case weather zone would have total influence
                if (m_weatherZoneClosestDistanceSqr > m_weatherZoneBlendDistanceSqr)
                    continue;

                // Weather zone has influence
                m_weatherZoneInterpolationFactor = 1f;

                if (m_weatherZoneBlendDistanceSqr > 0f)
                    m_weatherZoneInterpolationFactor = 1f - (m_weatherZoneClosestDistanceSqr / m_weatherZoneBlendDistanceSqr);

                // No need to clamp01 the interpolation factor as it'll always be in [0;1[ range
                ApplyWeatherZonesInfluence(weatherZone.profile, m_weatherZoneInterpolationFactor);
                var tmp = HWeatherCommon.GetLandType(weatherZone.name);
                list.Add(tmp);
            }

            //查找有没特殊天气, base是晴天
            var flag = false;
            foreach (var item in list)
            {
                if (item != ELandType.EAll)
                {
                    flag = true;
                    weatherTypes[(int) item] = HWeatherCommon.GetWeatherType(name);
                    if (landType != item)
                    {
                        landType = item;
                        ChangeLandType?.Invoke(item);
                    }
                }
            }

            //当前没特殊天气，那就全都是晴天
            if (!flag)
            {
                for (int i = 0; i < weatherTypes.Length; i++)
                {
                    weatherTypes[i] = EWeatherType.ESunny;
                }

                if (landType != ELandType.EAll)
                {
                    landType = ELandType.EAll;
                    ChangeLandType?.Invoke(landType);
                }
            }
        }

        public void EnableWeatherZone(ELandType type, EWeatherType weatherType, bool flag)
        {
            var weatherZone = GetWeatherZone(type, weatherType);
            if (weatherZone != null)
            {
                var col = weatherZone.GetComponent<Collider>();
                if (col != null)
                {
                    col.enabled = flag;
                }
            }
        }

        private HWeatherZone GetWeatherZone(ELandType type, EWeatherType weatherType)
        {
            foreach (var weatherZone in weatherZoneList)
            {
                if (weatherZone == null || weatherZone.profile == null)
                {
                    continue;
                }

                var name = weatherZone.profile.name;
                if (HWeatherCommon.GetLandType(name) == type && HWeatherCommon.GetWeatherType(name) == weatherType)
                {
                    return weatherZone;
                }
            }

            return null;
        }

        /// <summary>
        /// Update the sky setting when there is no global weather transition or local weather zone influence.
        /// </summary>
        private void GetDefaultSettings()
        {
            if (currentProfile == null)
            {
                // Log.EditorE("HWeather", "GetDefaultSettings currentProfile is null");
                return;
            }
            // Lighting
            settings.DirectionalLightIntensity = currentProfile.directionalLightIntensity.GetValue(timeOfDay);
            settings.DirectionalLightColor = currentProfile.directionalLightColor.GetValue(timeOfDay);
            settings.EnvironmentIntensity = currentProfile.environmentIntensity.GetValue(timeOfDay);
            settings.EnvironmentAmbientColor = currentProfile.environmentAmbientColor.GetValue(timeOfDay);
            settings.EnvironmentEquatorColor = currentProfile.environmentEquatorColor.GetValue(timeOfDay);
            settings.EnvironmentGroundColor = currentProfile.environmentGroundColor.GetValue(timeOfDay);
            settings.directionalLightRotationX = currentProfile.directionalLightRotationX.GetValue(timeOfDay);
            settings.directionalLightRotationY = currentProfile.directionalLightRotationY.GetValue(timeOfDay);
            settings.directionalLightRotationZ = currentProfile.directionalLightRotationZ.GetValue(timeOfDay);

            // Weather
            //雨
            settings.LightRainIntensity = currentProfile.lightRainIntensity.GetValue(timeOfDay);
            settings.MediumRainIntensity = currentProfile.mediumRainIntensity.GetValue(timeOfDay);
            settings.HeavyRainIntensity = currentProfile.heavyRainIntensity.GetValue(timeOfDay);
            settings.RainColor = currentProfile.rainColor.GetValue(timeOfDay);
            //雪
            settings.LightSnowIntensity = currentProfile.lightSnowIntensity.GetValue(timeOfDay);
            settings.MediumSnowIntensity = currentProfile.mediumSnowIntensity.GetValue(timeOfDay);
            settings.HeavySnowIntensity = currentProfile.heavySnowIntensity.GetValue(timeOfDay);
            settings.SnowColor = currentProfile.snowColor.GetValue(timeOfDay);
            //沙尘
            settings.LightSandIntensity = currentProfile.lightSandIntensity.GetValue(timeOfDay);
            settings.MediumSandIntensity = currentProfile.mediumSandIntensity.GetValue(timeOfDay);
            settings.HeavySandIntensity = currentProfile.heavySandIntensity.GetValue(timeOfDay);
            settings.SandColor = currentProfile.sandColor.GetValue(timeOfDay);
            //落叶
            settings.ScreenLeafIntensity = currentProfile.screenLeafIntensity.GetValue(timeOfDay);
            settings.ScreenLeaColor = currentProfile.screenLeafColor.GetValue(timeOfDay);

            //距离雾
            settings.g_FogLineStart = currentProfile.g_FogLineStart.GetValue(timeOfDay);
            settings.g_FogLineEnd = currentProfile.g_FogLineEnd.GetValue(timeOfDay);
            settings.g_FogLineColor = currentProfile.g_FogLineColor.GetValue(timeOfDay);
            settings.g_FogLineFarColor = currentProfile.g_FogLineFarColor.GetValue(timeOfDay);
            settings.g_FoglineDisNearOffset = currentProfile.g_FoglineDisNearOffset.GetValue(timeOfDay);
            settings.g_FoglineDisFarOffset = currentProfile.g_FoglineDisFarOffset.GetValue(timeOfDay);
            settings.g_FoglineDisMin = currentProfile.g_FoglineDisMin.GetValue(timeOfDay);
            settings.g_FogHeightStart = currentProfile.g_FogHeightStart.GetValue(timeOfDay);
            settings.g_FogHeightEnd = currentProfile.g_FogHeightEnd.GetValue(timeOfDay);
            settings.g_FogLineOverrideHeight = currentProfile.g_FogLineOverrideHeight.GetValue(timeOfDay);
            settings.g_FogLineOverrideHeightDisOffset = currentProfile.g_FogLineOverrideHeightDisOffset.GetValue(timeOfDay);
            //云投影
            settings.g_CloudShadowNearScale = currentProfile.g_CloudShadowNearScale.GetValue(timeOfDay);
            settings.g_CloudShadowFarScale = currentProfile.g_CloudShadowFarScale.GetValue(timeOfDay);
            settings.g_CloudShadowScaleMixStart = currentProfile.g_CloudShadowScaleMixStart.GetValue(timeOfDay);
            settings.g_CloudShadowScaleMixEnd = currentProfile.g_CloudShadowScaleMixEnd.GetValue(timeOfDay);
            settings.g_CloudShadowInt = currentProfile.g_CloudShadowInt.GetValue(timeOfDay);
            settings.g_CloudShadowOverlay = currentProfile.g_CloudShadowOverlay.GetValue(timeOfDay);
            settings.g_CloudShadowOverlayPow = currentProfile.g_CloudShadowOverlayPow.GetValue(timeOfDay);
            //新风
            settings.g_windPower = currentProfile.g_windPower.GetValue(timeOfDay);
        }

        /// <summary>
        /// Blends the profiles when there is a global weather transition.
        /// </summary>
        private void ApplyGlobalWeatherTransition(HWeatherProfile from, HWeatherProfile to, float t)
        {
            //强度 交由程序控制
            // //雨强度
            // settings.LightRainIntensity = FloatInterpolation(from.lightRainIntensity.GetValue(timeOfDay), to.lightRainIntensity.GetValue(timeOfDay), t);
            // settings.MediumRainIntensity = FloatInterpolation(from.mediumRainIntensity.GetValue(timeOfDay), to.mediumRainIntensity.GetValue(timeOfDay), t);
            // settings.HeavyRainIntensity = FloatInterpolation(from.heavyRainIntensity.GetValue(timeOfDay), to.heavyRainIntensity.GetValue(timeOfDay), t);
            // //雪强度
            // settings.LightSnowIntensity = FloatInterpolation(from.lightSnowIntensity.GetValue(timeOfDay), to.lightSnowIntensity.GetValue(timeOfDay), t);
            // settings.MediumSnowIntensity = FloatInterpolation(from.mediumSnowIntensity.GetValue(timeOfDay), to.mediumSnowIntensity.GetValue(timeOfDay), t);
            // settings.HeavySnowIntensity = FloatInterpolation(from.heavySnowIntensity.GetValue(timeOfDay), to.heavySnowIntensity.GetValue(timeOfDay), t);
            // //沙尘强度
            // settings.LightSandIntensity = FloatInterpolation(from.lightSandIntensity.GetValue(timeOfDay), to.lightSandIntensity.GetValue(timeOfDay), t);
            // settings.MediumSandIntensity = FloatInterpolation(from.mediumSandIntensity.GetValue(timeOfDay), to.mediumSandIntensity.GetValue(timeOfDay), t);
            // settings.HeavySandIntensity = FloatInterpolation(from.heavySandIntensity.GetValue(timeOfDay), to.heavySandIntensity.GetValue(timeOfDay), t);


            // Lighting
            settings.DirectionalLightIntensity = FloatInterpolation(from.directionalLightIntensity.GetValue(timeOfDay), to.directionalLightIntensity.GetValue(timeOfDay), t);
            settings.DirectionalLightColor = ColorInterpolation(from.directionalLightColor.GetValue(timeOfDay), to.directionalLightColor.GetValue(timeOfDay), t);
            settings.EnvironmentIntensity = FloatInterpolation(from.environmentIntensity.GetValue(timeOfDay), to.environmentIntensity.GetValue(timeOfDay), t);
            settings.EnvironmentAmbientColor = ColorInterpolation(from.environmentAmbientColor.GetValue(timeOfDay), to.environmentAmbientColor.GetValue(timeOfDay), t);
            settings.EnvironmentEquatorColor = ColorInterpolation(from.environmentEquatorColor.GetValue(timeOfDay), to.environmentEquatorColor.GetValue(timeOfDay), t);
            settings.EnvironmentGroundColor = ColorInterpolation(from.environmentGroundColor.GetValue(timeOfDay), to.environmentGroundColor.GetValue(timeOfDay), t);
            settings.directionalLightRotationX = FloatInterpolation(from.directionalLightRotationX.GetValue(timeOfDay), to.directionalLightRotationX.GetValue(timeOfDay), t);
            settings.directionalLightRotationY = FloatInterpolation(from.directionalLightRotationY.GetValue(timeOfDay), to.directionalLightRotationY.GetValue(timeOfDay), t);
            settings.directionalLightRotationZ = FloatInterpolation(from.directionalLightRotationZ.GetValue(timeOfDay), to.directionalLightRotationZ.GetValue(timeOfDay), t);
            //雨
            settings.RainColor = ColorInterpolation(from.rainColor.GetValue(timeOfDay), to.rainColor.GetValue(timeOfDay), t);
            //雪
            settings.SnowColor = ColorInterpolation(from.snowColor.GetValue(timeOfDay), to.snowColor.GetValue(timeOfDay), t);
            //沙尘
            settings.SandColor = ColorInterpolation(from.sandColor.GetValue(timeOfDay), to.sandColor.GetValue(timeOfDay), t);
            // settings.SnowIntensity = FloatInterpolation(from.snowIntensity.GetValue(timeOfDay), to.snowIntensity.GetValue(timeOfDay), t);
            //落叶
            settings.ScreenLeafIntensity = FloatInterpolation(from.screenLeafIntensity.GetValue(timeOfDay), to.screenLeafIntensity.GetValue(timeOfDay), t);
            settings.ScreenLeaColor = ColorInterpolation(from.screenLeafColor.GetValue(timeOfDay), to.screenLeafColor.GetValue(timeOfDay), t);


            //距离雾
            settings.g_FogLineStart = FloatInterpolation(from.g_FogLineStart.GetValue(timeOfDay), to.g_FogLineStart.GetValue(timeOfDay), t);
            settings.g_FogLineEnd = FloatInterpolation(from.g_FogLineEnd.GetValue(timeOfDay), to.g_FogLineEnd.GetValue(timeOfDay), t);
            settings.g_FogLineColor = ColorInterpolation(from.g_FogLineColor.GetValue(timeOfDay), to.g_FogLineColor.GetValue(timeOfDay), t);
            settings.g_FogLineFarColor = ColorInterpolation(from.g_FogLineFarColor.GetValue(timeOfDay), to.g_FogLineFarColor.GetValue(timeOfDay), t);
            settings.g_FoglineDisNearOffset = FloatInterpolation(from.g_FoglineDisNearOffset.GetValue(timeOfDay), to.g_FoglineDisNearOffset.GetValue(timeOfDay), t);
            settings.g_FoglineDisFarOffset = FloatInterpolation(from.g_FoglineDisFarOffset.GetValue(timeOfDay), to.g_FoglineDisFarOffset.GetValue(timeOfDay), t);
            settings.g_FoglineDisMin = FloatInterpolation(from.g_FoglineDisMin.GetValue(timeOfDay), to.g_FoglineDisMin.GetValue(timeOfDay), t);
            settings.g_FogHeightStart = FloatInterpolation(from.g_FogHeightStart.GetValue(timeOfDay), to.g_FogHeightStart.GetValue(timeOfDay), t);
            settings.g_FogHeightEnd = FloatInterpolation(from.g_FogHeightEnd.GetValue(timeOfDay), to.g_FogHeightEnd.GetValue(timeOfDay), t);
            settings.g_FogLineOverrideHeight = FloatInterpolation(from.g_FogLineOverrideHeight.GetValue(timeOfDay), to.g_FogLineOverrideHeight.GetValue(timeOfDay), t);
            settings.g_FogLineOverrideHeightDisOffset = FloatInterpolation(from.g_FogLineOverrideHeightDisOffset.GetValue(timeOfDay), to.g_FogLineOverrideHeightDisOffset.GetValue(timeOfDay), t);
            //云投影
            settings.g_CloudShadowNearScale = FloatInterpolation(from.g_CloudShadowNearScale.GetValue(timeOfDay), to.g_CloudShadowNearScale.GetValue(timeOfDay), t);
            settings.g_CloudShadowFarScale = FloatInterpolation(from.g_CloudShadowFarScale.GetValue(timeOfDay), to.g_CloudShadowFarScale.GetValue(timeOfDay), t);
            settings.g_CloudShadowScaleMixStart = FloatInterpolation(from.g_CloudShadowScaleMixStart.GetValue(timeOfDay), to.g_CloudShadowScaleMixStart.GetValue(timeOfDay), t);
            settings.g_CloudShadowScaleMixEnd = FloatInterpolation(from.g_CloudShadowScaleMixEnd.GetValue(timeOfDay), to.g_CloudShadowScaleMixEnd.GetValue(timeOfDay), t);
            settings.g_CloudShadowInt = FloatInterpolation(from.g_CloudShadowInt.GetValue(timeOfDay), to.g_CloudShadowInt.GetValue(timeOfDay), t);
            settings.g_CloudShadowOverlay = FloatInterpolation(from.g_CloudShadowOverlay.GetValue(timeOfDay), to.g_CloudShadowOverlay.GetValue(timeOfDay), t);
            settings.g_CloudShadowOverlayPow = FloatInterpolation(from.g_CloudShadowOverlayPow.GetValue(timeOfDay), to.g_CloudShadowOverlayPow.GetValue(timeOfDay), t);
            //新风
            settings.g_windPower = FloatInterpolation(from.g_windPower.GetValue(timeOfDay), to.g_windPower.GetValue(timeOfDay), t);
        }






        /// <summary>
        /// Computes local weather zones influence.
        /// </summary>
        private void ApplyWeatherZonesInfluence(HWeatherProfile climateZoneProfile, float t)
        {
            if (climateZoneProfile == null)
            {
                return;
            }



            // Lighting
            settings.DirectionalLightIntensity = FloatInterpolation(settings.DirectionalLightIntensity, climateZoneProfile.directionalLightIntensity.GetValue(timeOfDay), t);
            settings.DirectionalLightColor = ColorInterpolation(settings.DirectionalLightColor, climateZoneProfile.directionalLightColor.GetValue(timeOfDay), t);
            settings.EnvironmentIntensity = FloatInterpolation(settings.EnvironmentIntensity, climateZoneProfile.environmentIntensity.GetValue(timeOfDay), t);
            settings.EnvironmentAmbientColor = ColorInterpolation(settings.EnvironmentAmbientColor, climateZoneProfile.environmentAmbientColor.GetValue(timeOfDay), t);
            settings.EnvironmentEquatorColor = ColorInterpolation(settings.EnvironmentEquatorColor, climateZoneProfile.environmentEquatorColor.GetValue(timeOfDay), t);
            settings.EnvironmentGroundColor = ColorInterpolation(settings.EnvironmentGroundColor, climateZoneProfile.environmentGroundColor.GetValue(timeOfDay), t);
            settings.directionalLightRotationX = FloatInterpolation(settings.directionalLightRotationX, climateZoneProfile.directionalLightRotationX.GetValue(timeOfDay), t);
            settings.directionalLightRotationY = FloatInterpolation(settings.directionalLightRotationY, climateZoneProfile.directionalLightRotationY.GetValue(timeOfDay), t);
            settings.directionalLightRotationZ = FloatInterpolation(settings.directionalLightRotationZ, climateZoneProfile.directionalLightRotationZ.GetValue(timeOfDay), t);
            //雨
            settings.LightRainIntensity = FloatInterpolation(settings.LightRainIntensity, climateZoneProfile.lightRainIntensity.GetValue(timeOfDay), t);
            settings.MediumRainIntensity = FloatInterpolation(settings.MediumRainIntensity, climateZoneProfile.mediumRainIntensity.GetValue(timeOfDay), t);
            settings.HeavyRainIntensity = FloatInterpolation(settings.HeavyRainIntensity, climateZoneProfile.heavyRainIntensity.GetValue(timeOfDay), t);
            settings.RainColor = ColorInterpolation(settings.RainColor, climateZoneProfile.rainColor.GetValue(timeOfDay), t);
            //雪
            settings.LightSnowIntensity = FloatInterpolation(settings.LightSnowIntensity, climateZoneProfile.lightSnowIntensity.GetValue(timeOfDay), t);
            settings.MediumSnowIntensity = FloatInterpolation(settings.MediumSnowIntensity, climateZoneProfile.mediumSnowIntensity.GetValue(timeOfDay), t);
            settings.HeavySnowIntensity = FloatInterpolation(settings.HeavySnowIntensity, climateZoneProfile.heavySnowIntensity.GetValue(timeOfDay), t);
            settings.SnowColor = ColorInterpolation(settings.SnowColor, climateZoneProfile.snowColor.GetValue(timeOfDay), t);
            //沙尘
            settings.LightSandIntensity = FloatInterpolation(settings.LightSandIntensity, climateZoneProfile.lightSandIntensity.GetValue(timeOfDay), t);
            settings.MediumSandIntensity = FloatInterpolation(settings.MediumSandIntensity, climateZoneProfile.mediumSandIntensity.GetValue(timeOfDay), t);
            settings.HeavySandIntensity = FloatInterpolation(settings.HeavySandIntensity, climateZoneProfile.heavySandIntensity.GetValue(timeOfDay), t);
            settings.SandColor = ColorInterpolation(settings.SandColor, climateZoneProfile.sandColor.GetValue(timeOfDay), t);
            //落叶
            settings.ScreenLeafIntensity = FloatInterpolation(settings.ScreenLeafIntensity, climateZoneProfile.screenLeafIntensity.GetValue(timeOfDay), t);
            settings.ScreenLeaColor = ColorInterpolation(settings.ScreenLeaColor, climateZoneProfile.screenLeafColor.GetValue(timeOfDay), t);
            // settings.SnowIntensity = FloatInterpolation(settings.SnowIntensity, climateZoneProfile.snowIntensity.GetValue(timeOfDay), t);
            //距离雾
            settings.g_FogLineStart = FloatInterpolation(settings.g_FogLineStart, climateZoneProfile.g_FogLineStart.GetValue(timeOfDay), t);
            settings.g_FogLineEnd = FloatInterpolation(settings.g_FogLineEnd, climateZoneProfile.g_FogLineEnd.GetValue(timeOfDay), t);
            settings.g_FogLineColor = ColorInterpolation(settings.g_FogLineColor, climateZoneProfile.g_FogLineColor.GetValue(timeOfDay), t);
            settings.g_FogLineFarColor = ColorInterpolation(settings.g_FogLineFarColor, climateZoneProfile.g_FogLineFarColor.GetValue(timeOfDay), t);
            settings.g_FoglineDisNearOffset = FloatInterpolation(settings.g_FoglineDisNearOffset, climateZoneProfile.g_FoglineDisNearOffset.GetValue(timeOfDay), t);
            settings.g_FoglineDisFarOffset = FloatInterpolation(settings.g_FoglineDisFarOffset, climateZoneProfile.g_FoglineDisFarOffset.GetValue(timeOfDay), t);
            settings.g_FoglineDisMin = FloatInterpolation(settings.g_FoglineDisMin, climateZoneProfile.g_FoglineDisMin.GetValue(timeOfDay), t);
            settings.g_FogHeightStart = FloatInterpolation(settings.g_FogHeightStart, climateZoneProfile.g_FogHeightStart.GetValue(timeOfDay), t);
            settings.g_FogHeightEnd = FloatInterpolation(settings.g_FogHeightEnd, climateZoneProfile.g_FogHeightEnd.GetValue(timeOfDay), t);
            settings.g_FogLineOverrideHeight = FloatInterpolation(settings.g_FogLineOverrideHeight, climateZoneProfile.g_FogLineOverrideHeight.GetValue(timeOfDay), t);
            settings.g_FogLineOverrideHeightDisOffset = FloatInterpolation(settings.g_FogLineOverrideHeightDisOffset, climateZoneProfile.g_FogLineOverrideHeightDisOffset.GetValue(timeOfDay), t);
            //云投影
            settings.g_CloudShadowNearScale = FloatInterpolation(settings.g_CloudShadowNearScale, climateZoneProfile.g_CloudShadowNearScale.GetValue(timeOfDay), t);
            settings.g_CloudShadowFarScale = FloatInterpolation(settings.g_CloudShadowFarScale, climateZoneProfile.g_CloudShadowFarScale.GetValue(timeOfDay), t);
            settings.g_CloudShadowScaleMixStart = FloatInterpolation(settings.g_CloudShadowScaleMixStart, climateZoneProfile.g_CloudShadowScaleMixStart.GetValue(timeOfDay), t);
            settings.g_CloudShadowScaleMixEnd = FloatInterpolation(settings.g_CloudShadowScaleMixEnd, climateZoneProfile.g_CloudShadowScaleMixEnd.GetValue(timeOfDay), t);
            settings.g_CloudShadowInt = FloatInterpolation(settings.g_CloudShadowInt, climateZoneProfile.g_CloudShadowInt.GetValue(timeOfDay), t);
            settings.g_CloudShadowOverlay = FloatInterpolation(settings.g_CloudShadowOverlay, climateZoneProfile.g_CloudShadowOverlay.GetValue(timeOfDay), t);
            settings.g_CloudShadowOverlayPow = FloatInterpolation(settings.g_CloudShadowOverlayPow, climateZoneProfile.g_CloudShadowOverlayPow.GetValue(timeOfDay), t);
            //新风
            settings.g_windPower = FloatInterpolation(settings.g_windPower, climateZoneProfile.g_windPower.GetValue(timeOfDay), t);
        }

        /// <summary>
        /// Interpolates between two values given an interpolation factor.
        /// </summary>
        private float FloatInterpolation(float from, float to, float t)
        {
            return from + (to - from) * t;
        }

        // /// <summary>
        // /// Interpolates between two vectors given an interpolation factor.
        // /// </summary>
        // private Vector2 Vector2Interpolation(Vector2 from, Vector2 to, float t)
        // {
        //     Vector2 ret;
        //     ret.x = from.x + (to.x - from.x) * t;
        //     ret.y = from.y + (to.y - from.y) * t;
        //     return ret;
        // }

        // /// <summary>
        // /// Interpolates between two vectors given an interpolation factor.
        // /// </summary>
        // private Vector3 Vector3Interpolation(Vector3 from, Vector3 to, float t)
        // {
        //     Vector3 ret;
        //     ret.x = from.x + (to.x - from.x) * t;
        //     ret.y = from.y + (to.y - from.y) * t;
        //     ret.z = from.z + (to.z - from.z) * t;
        //     return ret;
        // }

        /// <summary>
        /// Interpolates between two colors given an interpolation factor.
        /// </summary>
        private Color ColorInterpolation(Color from, Color to, float t)
        {
            Color ret;
            ret.r = from.r + (to.r - from.r) * t;
            ret.g = from.g + (to.g - from.g) * t;
            ret.b = from.b + (to.b - from.b) * t;
            ret.a = from.a + (to.a - from.a) * t;
            return ret;
        }

        // /// <summary>
        // /// Returns the cloud uv position based on the direction and speed.
        // /// </summary>
        // private Vector2 ComputeCloudPosition()
        // {
        //     float x = m_dynamicCloudDirection.x;
        //     float z = m_dynamicCloudDirection.y;
        //     float windSpeed = settings.DynamicCloudSpeed * 0.05f * Time.deltaTime;

        //     x += windSpeed * Mathf.Sin(0.01745329f * settings.DynamicCloudDirection);
        //     z += windSpeed * Mathf.Cos(0.01745329f * settings.DynamicCloudDirection);

        //     if (x >= 1.0f) x -= 1.0f;
        //     if (z >= 1.0f) z -= 1.0f;

        //     return new Vector2(x, z);
        // }

        // /// <summary>
        // /// Total rayleigh computation.
        // /// </summary>
        // private Vector3 ComputeRayleigh()
        // {
        //     Vector3 rayleigh = Vector3.one;
        //     Vector3 lambda = settings.Wavelength * 1e-9f;
        //     float n = 1.0003f; // Refractive index of air
        //     float pn = 0.035f; // Depolarization factor for standard air.
        //     float n2 = n * n;
        //     //float N = 2.545E25f;
        //     float N = settings.MolecularDensity;
        //     float temp = (8.0f * Mathf.PI * Mathf.PI * Mathf.PI * ((n2 - 1.0f) * (n2 - 1.0f))) / (3.0f * N * 1E25f) * ((6.0f + 3.0f * pn) / (6.0f - 7.0f * pn));

        //     rayleigh.x = temp / Mathf.Pow(lambda.x, 4.0f);
        //     rayleigh.y = temp / Mathf.Pow(lambda.y, 4.0f);
        //     rayleigh.z = temp / Mathf.Pow(lambda.z, 4.0f);

        //     return rayleigh;
        // }

        // /// <summary>
        // /// Total mie computation.
        // /// </summary>
        // private Vector3 ComputeMie()
        // {
        //     Vector3 mie;

        //     //float c = (0.6544f * Turbidity - 0.6510f) * 1e-16f;
        //     float c = (0.6544f * 5.0f - 0.6510f) * 10f * 1e-9f;
        //     Vector3 k = new Vector3(686.0f, 678.0f, 682.0f);

        //     mie.x = (434.0f * c * Mathf.PI * Mathf.Pow((4.0f * Mathf.PI) / settings.Wavelength.x, 2.0f) * k.x);
        //     mie.y = (434.0f * c * Mathf.PI * Mathf.Pow((4.0f * Mathf.PI) / settings.Wavelength.y, 2.0f) * k.y);
        //     mie.z = (434.0f * c * Mathf.PI * Mathf.Pow((4.0f * Mathf.PI) / settings.Wavelength.z, 2.0f) * k.z);

        //     //float c = (6544f * 5.0f - 6510f) * 10.0f * 1.0e-9f;
        //     //mie.x = (0.434f * c * Pi * Mathf.Pow((2.0f * Pi) / settings.Wavelength.x, 2.0f) * settings.K.x) / 3.0f;
        //     //mie.y = (0.434f * c * Pi * Mathf.Pow((2.0f * Pi) / settings.Wavelength.y, 2.0f) * settings.K.y) / 3.0f;
        //     //mie.z = (0.434f * c * Pi * Mathf.Pow((2.0f * Pi) / settings.Wavelength.z, 2.0f) * settings.K.z) / 3.0f;

        //     return mie;
        // }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void ShadowSetting()
        {
            Shader.EnableKeyword("_HGAME_SHADOW_ON");
            Shader.EnableKeyword("_USE_UNITY_SHADOW");

            // 雾，给个默认值，不然预览会很难看
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogLineStart, 10000);
            Shader.SetGlobalFloat(HWeatherShaderUniforms.g_FogLineEnd, 10000);
        }
#endif
    }
}

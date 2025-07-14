using System;

namespace UnityEngine.HWeather
{
    

    
	[ExecuteInEditMode]
	[AddComponentMenu("HWeather/HWeather Effects Controller")]
	public class HWeatherEffectsController : MonoBehaviour
	{
		// Not included in the build
		#if UNITY_EDITOR
		public bool showParticlesHeaderGroup = false;
		public bool showWindHeaderGroup = false;
		#endif
		
		private HWeatherController m_skyController;
		// Particles

		public Transform particleSystemTransform;
		public Material rainMaterial, snowMaterial, rippleMaterial,sandMaterial,screenLeafMaterial;
		public ParticleSystem lightRainParticle,mediumRainParticle,heavyRainParticle;
		public ParticleSystem lightSnowParticle,mediumSnowParticle,heavySnowParticle;
		public ParticleSystem lightSandParticle,mediumSandParticle,heavySandParticle;
		public ParticleSystem  screenLeafParticle;
		// public ParticleSystem  snowParticle;
		public Transform followTarget;

        private ParticleSystem[] m_rainEffectArr;
        private ParticleSystem[] m_sandEffectArr;
        private ParticleSystem[] m_snowEffectArr;
        private float[] m_rateOverTimeArr;
        private HWeatherEffect m_curEffect = new HWeatherEffect();
        private HWeatherEffect m_lastEffect = new HWeatherEffect();
        
        private EWeatherType weatherType = EWeatherType.ENull;

        private EWeatherStrength weatherStrength = EWeatherStrength.ENull;
		//风
		// public Texture2D g_WindNoise;
        // public Vector4 g_WindNoise_ST;
        public Transform g_WindDirection = null;
        
        public Vector3 m_lastCameraPos = Vector3.zero;

		//新风
		public Texture2D noiseTexture;
        public float noiseSize = 50;
        public float noiseSpeed = 1;

        private float noiseDirectionX;
		private float noiseDirectionZ;

		public bool ISWind = false;

        public static bool IsAutoPlay = true;
		private void OnEnable()
		{
			m_skyController = GetComponent<HWeatherController>();
            if (followTarget != null)
            {
                m_lastCameraPos = followTarget.position;
            }
        }
        
        private void Awake()
        {
            if (followTarget != null)
            {
                m_lastCameraPos = followTarget.position;
            }

            if (m_rainEffectArr == null)
            {
                m_rainEffectArr = new []{lightRainParticle, mediumRainParticle, heavyRainParticle};
            }
            if (m_sandEffectArr == null)
            {
                m_sandEffectArr = new []{lightSandParticle, mediumSandParticle, heavySandParticle};
            }
            if (m_snowEffectArr == null)
            {
                m_snowEffectArr = new []{lightSnowParticle, mediumSnowParticle, heavySnowParticle};
            }

            if (m_rateOverTimeArr == null)
            {
                m_rateOverTimeArr = new float[]{400, 800 , 1200};
            }
        }

        private void OnDestroy()
        {
            StopAllEffect();
        }

        private void Start()
		{
            m_skyController = GetComponent<HWeatherController>();

            //避免空
			if(g_WindDirection == null)
			{
				g_WindDirection = gameObject.transform;
				Debug.LogError("风场方向缺失，请指定");
			}
			UpdateStaticData();
			UpdateParticlesMaterials();
			UpdateParticlesPosition();
		}

        private void FixedUpdate()
        {
            m_curEffect.FixedUpdate();
            m_lastEffect.FixedUpdate();
        }
        
		private void Update()
		{
			// Editor only
            #if UNITY_EDITOR
				UpdateStaticData();
            #endif
            UpdateGlobalShaderSet();
            UpdateParticlesPosition();
            // add by grant
            if (followTarget && Vector3.Distance(m_lastCameraPos, followTarget.position) > 0.05f)
            {
                m_lastCameraPos = followTarget.position;
                UpdateParticlesMaterials();
                if (Application.isPlaying && IsAutoPlay && HWeatherCommon.s_openWeather)
                {
					//雨
                    ParticleEffectController(m_skyController.settings.LightRainIntensity ,  400.0f, lightRainParticle);
					ParticleEffectController(m_skyController.settings.MediumRainIntensity ,  800.0f, mediumRainParticle);
					ParticleEffectController(m_skyController.settings.HeavyRainIntensity ,  1200.0f, heavyRainParticle);
					//雪
					ParticleEffectController(m_skyController.settings.LightSnowIntensity ,  400.0f, lightSnowParticle);
					ParticleEffectController(m_skyController.settings.MediumSnowIntensity ,  800.0f, mediumSnowParticle);
					ParticleEffectController(m_skyController.settings.HeavySnowIntensity ,  1200.0f, heavySnowParticle);
					//沙
					ParticleEffectController(m_skyController.settings.LightSandIntensity ,  400.0f, lightSandParticle);
					ParticleEffectController(m_skyController.settings.MediumSandIntensity ,  800.0f, mediumSandParticle);
					ParticleEffectController(m_skyController.settings.HeavySandIntensity ,  1200.0f, heavySandParticle);
					//落叶
					ParticleEffectController(m_skyController.settings.ScreenLeafIntensity ,  20.0f, screenLeafParticle);

                }
            }
        }

        
        private void SetScreenEffect(ParticleSystem particle ,float intensity)
        {
            Transform t_SE = particle.transform.Find("ScreenEffect");
            if( t_SE != null)
            {
                // GameObject g_SE = t_SE.gameObject;
                MeshRenderer MR = t_SE.gameObject.GetComponent<MeshRenderer>();
                if( MR != null)
                {
                    if(intensity > 0 )
                    {
                        if (!MR.enabled){MR.enabled = true;} 
                        var mat = MR.sharedMaterial;
                        if( mat != null)
                        {
                            mat.SetFloat("_ScreenEffectInt", intensity );
                            // Debug.Log("设置 ScreenEffect " + intensity);
                        }
                    }else
                    {
                        if (MR.enabled){MR.enabled = false;} 
                    }
                }
            }else
            {
                // Debug.Log("未找到 ScreenEffect 物体");
            }
        }
		
        /// <summary>
        /// Start and stop the particle effect.
        /// </summary>
        private void ParticleEffectController(float intensity,float RateOverTime, ParticleSystem particle)
        {
            if (particle == null)
            {
                return;
            }
            float setintensity = intensity * RateOverTime;
            var emission = particle.emission;
            emission.rateOverTimeMultiplier = setintensity;
            if (setintensity > 0)
            {
                if (!particle.isPlaying)
                {
                    particle.gameObject.SetActive(true);
                    particle.Play ();
                }
            }
            else
            {
                if (particle.isPlaying)
                {
                    particle.gameObject.SetActive(false);
                    particle.Stop ();
                }
            } 
            //设置屏幕特效
            SetScreenEffect(particle,intensity);
        }
        
        
        public bool PlayEffect( EWeatherType type, EWeatherStrength strength)
        {
            if (!Application.isPlaying || !HWeatherCommon.s_openWeather)
            {
                return false;
            }
            var index = (int) strength;
            if (type == weatherType && weatherStrength == strength)
            {
                return false;
            }
            weatherType = type;
            weatherStrength = strength;
            m_lastEffect.Clone(m_curEffect);
            m_lastEffect.StopEffect();
            switch (type)
            {
                case EWeatherType.ESunny:
                    m_curEffect.curParticle = null;
                    break;
                case EWeatherType.ERain:
                    m_curEffect.PlayEffect(m_rateOverTimeArr[index], m_rainEffectArr[index], true);
                    break;
                case EWeatherType.EDust:
                    m_curEffect.PlayEffect(m_rateOverTimeArr[index], m_sandEffectArr[index], true);
                    break;
                case EWeatherType.ESnow:
                    m_curEffect.PlayEffect(m_rateOverTimeArr[index], m_snowEffectArr[index], true);
                    break;
            }

            return true;
        }


        public void StopCurEffect()
        {
            m_curEffect.StopEffect();
        }
        public void StopAllEffect()
        {
            weatherType = EWeatherType.ENull;
            weatherStrength = EWeatherStrength.ENull;
            m_lastEffect.StopEffectImmediately();
            m_curEffect.StopEffectImmediately();
            if (screenLeafParticle != null )
            {
                screenLeafParticle.gameObject.SetActive(false);
                if (screenLeafParticle.isPlaying)
                {
                    screenLeafParticle.Stop();
                }
            }

            StopEffect(m_rainEffectArr);
            StopEffect(m_snowEffectArr);
            StopEffect(m_sandEffectArr);
        }

        private void StopEffect(ParticleSystem[] arr)
        {
            if (arr == null)
            {
                return;
            }
            
            foreach (var effect in arr)
            {
                if (effect != null )
                {
                    effect.gameObject.SetActive(false);
                    if (effect.IsAlive() && effect.isPlaying)
                    {
                        effect.Stop();
                    }
                }
            }
        }

		/// <summary>
		/// Updates the particle position.
		/// </summary>
		private void UpdateParticlesPosition()
		{
            if (followTarget && Vector3.Distance(particleSystemTransform.position, followTarget.position) > 0.05f)
            {
                particleSystemTransform.position = followTarget.position;
            }
        }

		/// <summary>
		/// Updates the particles color.
		/// </summary>
		private void UpdateParticlesMaterials()
		{
			//暂时避免美术未给资源引发的报错。等待程序确定这部分写法
			if(rainMaterial == null || snowMaterial == null || rippleMaterial == null || sandMaterial == null || screenLeafMaterial == null)
			{
				return;
			}
			rainMaterial.SetColor("_TintColor", m_skyController.settings.RainColor);
			rippleMaterial.SetColor("_TintColor", m_skyController.settings.RainColor);
			snowMaterial.SetColor("_TintColor", m_skyController.settings.SnowColor);
			sandMaterial.SetColor("_TintColor", m_skyController.settings.SandColor);
			screenLeafMaterial.SetColor("_TintColor", m_skyController.settings.ScreenLeaColor);

		}

        //设置静态数据
        public void UpdateStaticData()
        {
			if (ISWind)
            {
                Shader.EnableKeyword("_WIND_ON");
				//风
				// Shader.SetGlobalTexture(HWeatherShaderUniforms.g_WindNoise, g_WindNoise);
				// Shader.SetGlobalVector(HWeatherShaderUniforms.g_WindNoise_ST, g_WindNoise_ST);
				// Shader.SetGlobalVector(HWeatherShaderUniforms.g_WindDirection, g_WindDirection.transform.rotation * Vector3.back);
				// Shader.SetGlobalFloat(HWeatherShaderUniforms.g_MBGlobalWindDir, g_WindDirection.transform.eulerAngles.y); 
				//新风
				// linkNoiseWithMotionDirection
				noiseDirectionX = -g_WindDirection.forward.x;
				noiseDirectionZ = -g_WindDirection.forward.z;
				Shader.SetGlobalTexture("TVE_NoiseTex", noiseTexture);
				Shader.SetGlobalVector("TVE_NoiseSpeed_Vegetation", new Vector2 (noiseSpeed * noiseDirectionX * 0.1f, noiseSpeed * noiseDirectionZ * 0.1f));
				Shader.SetGlobalFloat("TVE_NoiseSize_Vegetation", 1.0f / noiseSize);
            }
            else
            {
                Shader.DisableKeyword("_WIND_ON");
            }
		}
		private void UpdateGlobalShaderSet()
		{
			if (ISWind)
            {
				//新风
            	Shader.SetGlobalVector("TVE_VertexParams", new Vector4(g_WindDirection.forward.x * 0.5f + 0.5f, g_WindDirection.forward.z * 0.5f + 0.5f, m_skyController.settings.g_windPower * 0.5f + 0.5f, 1.0f));
			}
		}

		#if UNITY_EDITOR
			void OnDrawGizmos()
			{
				if(g_WindDirection != null)
				{
					Vector3 dir = (g_WindDirection.transform.position + g_WindDirection.transform.forward).normalized;

					Gizmos.color = Color.green;
					Vector3 up = g_WindDirection.transform.up;
					Vector3 side = g_WindDirection.transform.right;

					float WindGizmo = 2.50f;
					Vector3 end = g_WindDirection.transform.position + g_WindDirection.transform.forward * (WindGizmo * 5f);
					Vector3 mid = g_WindDirection.transform.position + g_WindDirection.transform.forward * (WindGizmo * 2.5f);
					Vector3 start = g_WindDirection.transform.position + g_WindDirection.transform.forward * (WindGizmo * 0f);
					
					float s = WindGizmo;
					Vector3 front = g_WindDirection.transform.forward * WindGizmo;

					Gizmos.DrawLine(start, start - front + up * s);
					Gizmos.DrawLine(start, start - front - up * s);
					Gizmos.DrawLine(start, start - front + side * s);
					Gizmos.DrawLine(start, start - front - side * s);
					Gizmos.DrawLine(start, start - front * 2);

					Gizmos.DrawLine(mid, mid - front + up * s);
					Gizmos.DrawLine(mid, mid - front - up * s);
					Gizmos.DrawLine(mid, mid - front + side * s);
					Gizmos.DrawLine(mid, mid - front - side * s);
					Gizmos.DrawLine(mid, mid - front * 2);

					Gizmos.DrawLine(end, end - front + up * s);
					Gizmos.DrawLine(end, end - front - up * s);
					Gizmos.DrawLine(end, end - front + side * s);
					Gizmos.DrawLine(end, end - front - side * s);
					Gizmos.DrawLine(end, end - front * 2);
				} 
			}
		#endif

	}
}


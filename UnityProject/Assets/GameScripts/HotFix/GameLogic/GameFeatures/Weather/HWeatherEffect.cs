
namespace UnityEngine.HWeather
{
    public class HWeatherEffect
    {
        private const int TimeLen = 3;
        private const float MinIntensity = 0f;
        private const float MaxIntensity = 1f;
        private const int TimeCnt = (int) (TimeLen / 0.033f);
        private const float DeltaIntensity = (MaxIntensity - MinIntensity) / TimeCnt;

        private float m_curIntensity;
        private float m_curDeltaIntensity;
        private float m_rateOverTime = 0f;
        public ParticleSystem curParticle;
        private bool m_startGradual = false; //开始过渡
        private bool m_startOrEnd = true; //开始或结束，true为开始,会从无到有，false为结束，会从有到无。


        public void FixedUpdate()
        {
            if (m_startGradual)
            {
                m_curIntensity += m_curDeltaIntensity;
                Debug.LogFormat("HWeatherEffect FixedUpdate {0} {1} {2}", m_curIntensity, m_curDeltaIntensity, m_startOrEnd);
                if (m_startOrEnd)
                {
                    if (m_curIntensity > MaxIntensity)
                    {
                        m_curIntensity = MaxIntensity;
                        m_startGradual = false;
                        HWeatherCommon.s_reDrawShadow = false;
                    }
                }
                else
                {
                    if (m_curIntensity < MinIntensity)
                    {
                        m_curIntensity = MinIntensity;
                        m_startGradual = false;
                        HWeatherCommon.s_reDrawShadow = false;
                    }
                }

                ParticleEffectController(m_curIntensity, m_rateOverTime, curParticle);
            }
        }

        public void Destroy()
        {
            StopEffectImmediately();
            curParticle = null;
        }

        public void Clone(HWeatherEffect other)
        {
            if (other != null)
            {
                m_curIntensity = other.m_curIntensity;
                m_curDeltaIntensity = other.m_curDeltaIntensity;
                m_rateOverTime = other.m_rateOverTime;
                curParticle = other.curParticle;
                m_startGradual = other.m_startGradual;
                m_startOrEnd = other.m_startOrEnd;
            }
        }

        public void StopEffectImmediately()
        {
            if (curParticle != null)
            {
                curParticle.gameObject.SetActive(false);
                if (curParticle.isPlaying)
                {
                    curParticle.Stop();
                }
            }

            m_startGradual = false;
        }

        public void StopEffect()
        {
            if (curParticle == null)
            {
                return;
            }
            m_startOrEnd = false;
            Debug.Log("HWeatherEffect StopEffect");
            PlayOrStop();
        }

        private void PlayOrStop()
        {
            m_startGradual = true;
            if (m_startOrEnd)
            {
                m_curDeltaIntensity = DeltaIntensity;
                m_curIntensity = DeltaIntensity;
            }
            else
            {
                m_curDeltaIntensity = -DeltaIntensity;
            }

            HWeatherCommon.s_reDrawShadow = true;
            ParticleEffectController(m_curIntensity, m_rateOverTime, curParticle);
        }

        public void PlayEffect(float rateOverTime, ParticleSystem particle, bool startOrEnd)
        {
            m_rateOverTime = rateOverTime;
            curParticle = particle;
            m_startOrEnd = startOrEnd;
            Debug.LogFormat("HWeatherEffect StopEffect {0}", m_startOrEnd);
            PlayOrStop();
        }

        /// <summary>
        /// Start and stop the particle effect.
        /// </summary>
        private void ParticleEffectController(float intensity, float rateOverTime, ParticleSystem particle)
        {
            if (particle == null)
            {
                return;
            }

            Debug.LogFormat("HWeatherEffect ParticleEffectController {0} {1} {2}", m_curIntensity, m_curDeltaIntensity, m_startOrEnd);
            var emission = particle.emission;
            emission.rateOverTimeMultiplier = intensity * rateOverTime;
            if (intensity > 0)
            {
                if (!particle.isPlaying)
                {
                    particle.gameObject.SetActive(true);
                    particle.Play();
                }
            }
            else
            {
                if (particle.isPlaying)
                {
                    particle.gameObject.SetActive(false);
                    particle.Stop();
                }
            }

            //设置屏幕特效
            SetScreenEffect(particle, intensity);
        }



        //设置屏幕面片效果	
        private void SetScreenEffect(ParticleSystem particle, float intensity)
        {
            var effect = particle.transform.Find("ScreenEffect");
            if (effect != null)
            {
                // GameObject g_SE = t_SE.gameObject;
                var mr = effect.gameObject.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    if (intensity > 0)
                    {
                        if (!mr.enabled)
                        { mr.enabled = true; }
                        var mat = mr.sharedMaterial;
                        if (mat != null)
                        {
                            mat.SetFloat("_ScreenEffectInt", intensity);
                            // Debug.Log("设置 ScreenEffect " + intensity);
                        }
                    }
                    else
                    {
                        if (mr.enabled)
                        { mr.enabled = false; }
                    }
                }
            }
            else
            {
                // Debug.Log("未找到 ScreenEffect 物体");
            }
        }
    }
}

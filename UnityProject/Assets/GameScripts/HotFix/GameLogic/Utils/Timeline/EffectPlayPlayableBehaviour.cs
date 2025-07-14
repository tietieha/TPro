using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum EffcetPlayTiming
{
    OnClipPlay = 0,
    OnClipPause = 1,
    OnClipFrame = 2,
    OnGraphStop = 3,
}
public class EffectPlayPlayableBehaviour : PlayableBehaviour
{
    public GameObject m_EffectObj;
    public EffcetPlayTiming m_EffcetPlayTiming;
    public bool m_Active = true;
    public bool m_PlayParticles = false;

    private ParticleSystem[] m_particleSystems;

    public void CommonEffectPlay(EffcetPlayTiming playTiming)
    {
        if (m_EffcetPlayTiming == playTiming)
        {
            if (m_EffectObj != null)
            {
                m_EffectObj.SetActive(m_Active);

                m_particleSystems = m_EffectObj.GetComponentsInChildren<ParticleSystem>();
                if (m_PlayParticles && m_particleSystems != null)
                {
                    foreach (ParticleSystem ps in m_particleSystems)
                    {
                        ps.Play();
                    }
                }
            }
        }
    }

    //Clip内开始时播放
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        CommonEffectPlay(EffcetPlayTiming.OnClipPlay);
    }
    //Clip内停止时播放
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        CommonEffectPlay(EffcetPlayTiming.OnClipPause);
    }
    //Graph停止时播放
    public override void OnGraphStop(Playable playable)
    {
        CommonEffectPlay(EffcetPlayTiming.OnGraphStop);
    }
    //Clip内每帧都播放
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (m_EffcetPlayTiming == EffcetPlayTiming.OnClipFrame)
        {
            GameObject m_gameobject = playerData as GameObject;
            if (m_gameobject != null && m_EffectObj != null)
            {
                m_EffectObj.SetActive(m_Active);

                m_particleSystems = m_EffectObj.GetComponentsInChildren<ParticleSystem>();
                if (m_PlayParticles && m_particleSystems != null)
                {
                    foreach (ParticleSystem ps in m_particleSystems)
                    {
                        ps.Play();
                    }
                }
            }
        }
    }
}

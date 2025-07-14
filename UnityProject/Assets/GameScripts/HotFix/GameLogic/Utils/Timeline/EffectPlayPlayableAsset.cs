using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class EffectPlayPlayableAsset : PlayableAsset
{
    [Header("对应预制")]
    public ExposedReference<GameObject> m_EffectObj;
    [Header("播放时机:Clip内开始时播放/停止时播放/每帧播放-Graph内停止时播放")]
    public EffcetPlayTiming m_EffcetPlayTiming = EffcetPlayTiming.OnClipPlay;
    [Header("预制是否显示激活")]
    public bool m_Active = true;
    [Header("预制是否播放粒子")]
    public bool m_PlayParticles = false;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<EffectPlayPlayableBehaviour>.Create(graph);
        var effcetPlayBehaviour = playable.GetBehaviour();
        effcetPlayBehaviour.m_EffectObj = m_EffectObj.Resolve(graph.GetResolver());
        effcetPlayBehaviour.m_Active = m_Active;
        effcetPlayBehaviour.m_EffcetPlayTiming = m_EffcetPlayTiming;
        effcetPlayBehaviour.m_PlayParticles = m_PlayParticles;

        return playable;
    }
}

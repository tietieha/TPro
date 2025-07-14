using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class EffectPlayPlayableAsset : PlayableAsset
{
    [Header("��ӦԤ��")]
    public ExposedReference<GameObject> m_EffectObj;
    [Header("����ʱ��:Clip�ڿ�ʼʱ����/ֹͣʱ����/ÿ֡����-Graph��ֹͣʱ����")]
    public EffcetPlayTiming m_EffcetPlayTiming = EffcetPlayTiming.OnClipPlay;
    [Header("Ԥ���Ƿ���ʾ����")]
    public bool m_Active = true;
    [Header("Ԥ���Ƿ񲥷�����")]
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

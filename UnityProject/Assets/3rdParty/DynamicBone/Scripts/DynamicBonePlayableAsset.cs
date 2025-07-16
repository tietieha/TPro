using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DynamicBonePlayableAsset : PlayableAsset
{
    public ClipCaps clipCaps = ClipCaps.Blending;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DynamicBonePlayableBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        return playable;
    }
}

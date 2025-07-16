using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1.0f, 0.75f, 0.92f)]
[TrackBindingType(typeof(DynamicBone))]
[TrackClipType(typeof(DynamicBonePlayableAsset))]
public class DynamicBonePlayableTrack : PlayableTrack
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DynamicBonePlayableMixerBehaviour>.Create(graph, inputCount);
    }
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        base.GatherProperties(director, driver);
    }

}

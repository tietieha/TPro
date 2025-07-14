using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


    [TrackClipType(typeof(BulletTimePlayableAsset))]
    [TrackColor(0, 0, 0)]
    public class BulletTimeTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<BulletTimeBehaviour>.Create(graph, inputCount);
        }
    }

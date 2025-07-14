using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


    [System.Serializable]
    public class BulletTimePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public BulletTime template = new BulletTime();
        public ClipCaps clipCaps
        {
            get { return ClipCaps.Extrapolation | ClipCaps.Blending; }
        }
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var scriptPlayable = ScriptPlayable<BulletTime>.Create(graph, template);
            return scriptPlayable;
        }
    }



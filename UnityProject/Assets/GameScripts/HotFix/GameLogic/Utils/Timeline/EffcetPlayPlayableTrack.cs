using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[TrackColor(1.0f, 0.9f, 0.1f)]
[TrackClipType(typeof(EffectPlayPlayableAsset))]
[TrackBindingType(typeof(GameObject))]
public class EffcetPlayPlayableTrack : PlayableTrack { }

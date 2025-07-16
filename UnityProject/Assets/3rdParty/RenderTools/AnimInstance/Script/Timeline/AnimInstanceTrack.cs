// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-10-12 17:51 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace RenderTools.AnimInstance
{
	[TrackClipType(typeof(AnimInstanceClip))]
	[TrackBindingType(typeof(GameObject))]
	[TrackColor(0, 1, 0)]
	public class AnimInstanceTrack : TrackAsset
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			return ScriptPlayable<AnimInstanceMixer>.Create(graph, inputCount);
		}

		protected override void OnCreateClip(TimelineClip clip)
		{
			clip.displayName = "AnimInstance";
			base.OnCreateClip(clip);
		}
	}
}
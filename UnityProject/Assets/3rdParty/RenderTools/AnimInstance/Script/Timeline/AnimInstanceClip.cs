// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-10-12 16:02 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// Clip Asset
// ******************************************************************

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RenderTools.AnimInstance
{
	[Serializable]
	public class AnimInstanceClip : PlayableAsset, ITimelineClipAsset//, ISerializationCallbackReceiver
	{
		public AnimInstanceData template = new AnimInstanceData();
		public float speed = 1;
		public string playName = "idle";

		[NonSerialized] private TimelineClip clip;

		public ClipCaps clipCaps
		{
			get { return ClipCaps.Blending; }
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<AnimInstanceData>.Create(graph, template);
			AnimInstanceData clone = playable.GetBehaviour();
			clone.speed = speed;
			clone.playName = playName;
			return playable;
		}
#if UNITY_EDITOR
		// public void OnBeforeSerialize()
		// {
		// 	if (clip != null)
		// 	{
		// 		if(!clip.displayName.Equals(playName)) 
		// 			clip.displayName = playName;
		// 		return;
		// 	}
		//
		// 	// 获取所有TimeLineAsset
		// 	string[] guids = AssetDatabase.FindAssets("t:TimelineAsset");
		// 	var timelines = guids.Select(id =>
		// 		AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(id), typeof(TimelineAsset)));
		// 	// 找到包含这个PlayableAsset的Clip
		// 	foreach (TimelineAsset timeline in timelines)
		// 	{
		// 		if (timeline)
		// 		{
		// 			foreach (var track in timeline.GetOutputTracks())
		// 			{
		// 				foreach (var clip in track.GetClips())
		// 				{
		// 					if (clip.asset == this)
		// 					{
		// 						if(!clip.displayName.Equals(playName)) 
		// 							clip.displayName = playName;
		// 						this.clip = clip;
		// 						return;
		// 					}
		// 				}
		// 			}
		// 		}
		// 	}
		// }
		//
		// public void OnAfterDeserialize()
		// {
		// }
#endif
	}
}
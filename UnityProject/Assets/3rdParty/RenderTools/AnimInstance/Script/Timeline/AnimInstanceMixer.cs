// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-10-12 16:05 /
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
	public class AnimInstanceMixer : PlayableBehaviour
	{
		private AnimInstance anim;
		private int curIndex = 0;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (anim == null)
			{
				GameObject go = playerData as GameObject;
				if (go == null)
					return;
				if (anim == null)
					anim = go.GetComponent<AnimInstance>();
				if (anim == null)
					anim = go.GetComponentInChildren<AnimInstance>();
			}

			if (anim == null)
				return;

			// curIndex = (int) (playable.GetTime() * 30 * speed);
			// anim.PlayPose(playName, curIndex % anim.GetTotalFrame(playName));
			
			int inputCount = playable.GetInputCount();
			for (int i = 0; i < inputCount; i++)
			{
				float weight =  playable.GetInputWeight(i);
				if(weight <= 0)
					continue;
				ScriptPlayable<AnimInstanceData> playableInput = (ScriptPlayable<AnimInstanceData>)playable.GetInput(i);
				
				AnimInstanceData input = playableInput.GetBehaviour();
				curIndex = (int) (playableInput.GetTime() * 30 * input.speed);
				anim.PlayPose(input.playName, curIndex % anim.GetAnimFrameCount(input.playName));
			}
			
			// TODO 融合
		}
	}
}
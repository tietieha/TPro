// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-08-24 14:27 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************
using UnityEngine;
using System;
using System.Collections.Generic;

namespace RenderTools.AnimInstance
{
    [Serializable]
    public class AnimInstanceFrameInfo
    {
        public string Name;
        public int    StartFrame;
        public int    EndFrame;
        public int    FrameCount;
        public bool   Loop           = true;
        [SerializeField]
        public int AutoTransition = -1;
        [SerializeField]
        public List<EffectBoneData> effectBoneDatas = new List<EffectBoneData>();
        [SerializeField]
        public Dictionary<string, List<TransData>> AnimBoneDic;

        public AnimInstanceFrameInfo(string name, int startFrame, int endFrame, int frameCount, bool loop, int autoTransition, Dictionary<string, List<TransData>> animBone = null)
        {
            Name = name;
            StartFrame = startFrame;
            EndFrame = endFrame;
            FrameCount = frameCount;
            Loop = loop;
            AutoTransition = autoTransition;
            AnimBoneDic = animBone;

            effectBoneDatas.Clear();
            if (animBone != null)
            {
	            foreach (var item in animBone)
	            {
		            EffectBoneData effectBoneData = new EffectBoneData();
		            effectBoneData.boneName = item.Key;
		            effectBoneData.AnimBoneDataList = item.Value;
		            effectBoneDatas.Add(effectBoneData);
	            }
            }
        }
        public AnimInstanceFrameInfo(AnimInstanceFrameInfo info)
        {
            Name = info.Name;
            StartFrame = info.StartFrame;
            EndFrame = info.EndFrame;
            FrameCount = info.FrameCount;
            Loop = info.Loop;
            AutoTransition = info.AutoTransition;
            AnimBoneDic = info.AnimBoneDic;
            effectBoneDatas = info.effectBoneDatas;
        }

        public float GetClipLength()
        {
            if (FrameCount == 0)
                return 0;
            return ((float)FrameCount / 30f);
        }
    }
    [Serializable]
    public class TransData
    {
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
    }
    [Serializable]
    public class EffectBoneData
    {
        public string boneName;
        public List<TransData> AnimBoneDataList;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DynamicBonePlayableBehaviour : PlayableBehaviour
{
    public DynamicBone DynamicBone;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (playerData != null) 
        {
            DynamicBone = playerData as DynamicBone;
        }
        if (DynamicBone == null)
            return;
        DynamicBone.SetTimeLineinfo();
        base.ProcessFrame(playable, info, playerData);
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        if (DynamicBone != null)
        {
            DynamicBone.ActiveBones();
        }
        base.OnPlayableDestroy(playable);
    }
}

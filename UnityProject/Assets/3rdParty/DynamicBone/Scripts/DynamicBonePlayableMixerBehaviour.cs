using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DynamicBonePlayableMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var data = playerData as DynamicBone;
        if (data == null) 
        {
            return;
        }
        data.SetTimeLineinfo();
    }
}
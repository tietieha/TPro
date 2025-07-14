using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[XLua.LuaCallCSharp]
public class AnimatorUtils
{
    public static float GetAnimationClipLength(Animator animator, string clipName)
    {
        if (animator == null || animator.runtimeAnimatorController == null ||
            string.IsNullOrEmpty(clipName))
        {
            return 0f;
        }

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        int count = clips != null ? clips.Length : 0;
        for (int i = 0; i < count; i++)
        {
            if (clips[i].name.Equals(clipName))
                return clips[i].length;
        }

        return 0f;
    }

    /// <summary>
    /// 设置PlayableDirector的extrapolationMode
    /// </summary>
    /// <param name="modeType">
    /// DirectorWrapMode类型对应的int值
    /// 0 - HOLD
    /// 1 - LOOP
    /// 2 - NONE
    /// </param>
    public static void SetPlayableDirectorMode(PlayableDirector playableDirector,
        int modeType)
    {
        if (playableDirector == null || modeType < 0 || modeType > 3)
        {
            return;
        }

        playableDirector.extrapolationMode = (DirectorWrapMode)modeType;
    }

    /// <summary>
    /// 获得Timeline动画时长
    /// </summary>
    public static double GetTimelineAssetDuration(TimelineAsset timelineAsset)
    {
        if (timelineAsset == null)
        {
            return 0;
        }

        double maxDduration = 0;
        foreach (TrackAsset outputTrack in timelineAsset.GetOutputTracks())
        {
            if (outputTrack is AnimationTrack)
            {
                double duration = 0;
                var animationTrack = outputTrack as AnimationTrack;
                foreach (TimelineClip clips in animationTrack.GetClips())
                {
                    double time = clips.start + clips.duration;
                    if (duration < time) duration = time;
                }

                if (maxDduration < duration) maxDduration = duration;
            }
        }

        return maxDduration;
    }
}
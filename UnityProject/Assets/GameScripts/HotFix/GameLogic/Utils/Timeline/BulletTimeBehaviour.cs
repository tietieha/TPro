using UnityEngine;
using UnityEngine.Playables;

    public class BulletTimeBehaviour : PlayableBehaviour
    {
        readonly float defaultTimeScale = 1f;
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();
            float mixedTimeScale = 0f;
            float totalWeight = 0f;
            int currentInputCount = 0;

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight > 0f)
                    currentInputCount++;

                totalWeight += inputWeight;

                ScriptPlayable<BulletTime> playableInput = (ScriptPlayable<BulletTime>)playable.GetInput(i);
                BulletTime input = playableInput.GetBehaviour();
                mixedTimeScale += inputWeight * input._time;
            }
            //Time.timeScale = mixedTimeScale + defaultTimeScale * (1f - totalWeight);
            playable.GetGraph().GetRootPlayable(0).SetSpeed(mixedTimeScale + defaultTimeScale * (1f - totalWeight));
            if (currentInputCount == 0)
            {
                playable.GetGraph().GetRootPlayable(0).SetSpeed(defaultTimeScale);
                //Time.timeScale = defaultTimeScale;
            }
            base.ProcessFrame(playable, info, playerData);
        }
        public override void OnPlayableDestroy(Playable playable)           // 在剪辑销毁时恢复之前的缩放值
        {
            if (playable.GetGraph().GetRootPlayable(0).Equals(Playable.Null))
                return;
            playable.GetGraph().GetRootPlayable(0).SetSpeed(defaultTimeScale);
            base.OnPlayableDestroy(playable);
        }
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            //恢复之前的时间
            playable.GetGraph().GetRootPlayable(0).SetSpeed(defaultTimeScale);
            base.OnBehaviourPause(playable, info);
        }
        public override void OnGraphStop(Playable playable)
        {
            if (playable.GetGraph().GetRootPlayable(0).Equals(Playable.Null))
                return;
            playable.GetGraph().GetRootPlayable(0).SetSpeed(defaultTimeScale);
            base.OnGraphStop(playable);
        }
    }


using System;

namespace UnityEngine.Rendering.Universal
{
    [Serializable, VolumeComponentMenu("Post-processing/扭曲后处理")]
    public sealed class HDistortVolume : VolumeComponent, IPostProcessComponent
    {
        [Tooltip("开启后处理")]
        public BoolParameter isActivePost = new BoolParameter(false);

        public bool IsActive()
        {
            return isActivePost.value;
        }
        
        public bool IsTileCompatible()
        {
            return false;
        }
    }
}

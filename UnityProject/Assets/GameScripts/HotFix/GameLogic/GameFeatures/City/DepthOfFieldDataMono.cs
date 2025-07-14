using Sirenix.OdinInspector;
using UnityEngine;

namespace GameLogic.GameFeatures.City
{
    [SerializeField]
    public class DepthOfFieldDataMono : MonoBehaviour
    {
        [LabelText("到焦点的距离")]
        public float FocusDistance = 0;
        [LabelText("镜头与胶片之间的距离。值越大，景深越浅")]
        public float FocalLength = 0;
        [LabelText("光圈的比率（称为f值或f值）。值越小，景深越浅。")]
        public float Aperture = 0;
    }
}
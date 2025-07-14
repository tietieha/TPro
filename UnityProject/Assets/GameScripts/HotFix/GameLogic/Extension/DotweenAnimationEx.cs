using System;
using DG.Tweening;
using UnityEngine;

namespace GameLogic
{
    public class DotweenAnimationEx : MonoBehaviour
    {
        private Material _mat;
        public Color startColor;
        public Color endColor;
        public float duration;
        public string shaderProperty;

        private int _id;
        private void Awake()
        {
            _id = gameObject.GetInstanceID();
            var render = GetComponent<Renderer>();
            if (render && render.sharedMaterial)
                _mat = render.material;
        }

        private void OnEnable()
        {
            if(_mat)
            {
                DOTween.Kill(_id);
                _mat.SetColor(shaderProperty, startColor);
                _mat.DOColor(endColor, duration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .SetId(_id);
            }
        }

        private void OnDestroy()
        {
            if (_mat && _id != 0)
                DOTween.Kill(_id);
            Destroy(_mat);
        }
    }
}

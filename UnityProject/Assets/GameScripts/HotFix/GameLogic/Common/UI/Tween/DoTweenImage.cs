using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class DoTweenImage : MonoBehaviour
    {
        [Header("每次执行Tween前是否重置属性")]
        public bool isResetProperty = true;
        [Header("初始颜色")]
        public Color startColor = Color.white;
        [Header("目标颜色")]
        public Color toColor = Color.white;
        [Header("动画时间")]
        public float duration = 0.5f;
        private Image _image;
        private Tweener _tweener;
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (isResetProperty)
            {
                _image.color = startColor;
                _tweener = _image.DOColor(toColor,duration);
            }
        }

        private void OnDisable()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class BindSizeToComponent : MonoBehaviour
    {
        public RectTransform bindTarget;
        public Vector2 posOffset;
        public Vector2 sizeOffset;
        
        private Vector2 _targetSize;
        private RectTransform _mRectTransform;
        void Awake()
        {
            _mRectTransform = GetComponent<RectTransform>();
        }
        
        void Update()
        {
            UpdateSize();
        }

        public void UpdateSize()
        {
            if (bindTarget == null)
                return;
            
            _targetSize = bindTarget.sizeDelta + sizeOffset;
            if ( !Mathf.Approximately(_targetSize.x,_mRectTransform.sizeDelta.x) || !Mathf.Approximately(_targetSize.y,_mRectTransform.sizeDelta.y) )
            {
                Vector2 pos = _mRectTransform.anchoredPosition;
                pos.x = bindTarget.anchoredPosition.x + posOffset.x;
                pos.y = bindTarget.anchoredPosition.y + posOffset.y;
                _mRectTransform.anchoredPosition = pos;
                _mRectTransform.sizeDelta = _targetSize;
            }
        }

        [ContextMenu("Test Button")]
        public void TestButton()
        {
            _mRectTransform = GetComponent<RectTransform>();
            UpdateSize();
        }
    }
}

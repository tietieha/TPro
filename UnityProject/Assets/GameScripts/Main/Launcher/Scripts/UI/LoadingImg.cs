using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class LoadingImg : MonoBehaviour
    {
        [SerializeField] private Image loadingImg;
        [SerializeField] private float rotateOffset = 30;
        [SerializeField] private float rotateSecond;

        private float m_curDelta = 0;

        private int m_rotateCount = 0;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            m_curDelta += Time.deltaTime;
            if (m_curDelta >= rotateSecond)
            {
                m_curDelta = 0;
                m_rotateCount++;
                loadingImg.rectTransform.rotation = Quaternion.Euler(0, 0, - rotateOffset * m_rotateCount);
            }
        
        }
    }
}


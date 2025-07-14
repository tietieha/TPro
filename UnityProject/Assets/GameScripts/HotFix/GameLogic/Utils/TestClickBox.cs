using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameLogic
{
    public class TestClickBox : MonoBehaviour
    {
        public bool IsEnable = false;
        
        Camera _camera;

        private bool isScaling;
        // Start is called before the first frame update
        void Start()
        {
            _camera = GetComponent<Camera>();
            isScaling = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (IsEnable && Input.GetMouseButton(0) && isScaling == false)
            {
                var screenPos = Input.mousePosition;
                var myRay = _camera.ScreenPointToRay(screenPos);
                RaycastHit raycastHit;
                
                Debug.DrawRay(myRay.origin, myRay.direction * 700, Color.blue);
                if (Physics.Raycast(myRay, out raycastHit, 700))
                {
                    Debug.DrawLine(myRay.origin, raycastHit.point, Color.red);
                    var originScale = raycastHit.collider.transform.localScale;
                    isScaling = true;
                    raycastHit.collider.transform.DOScale(originScale * 0.8f, 0.5f).OnComplete(() =>
                    {
                        raycastHit.collider.transform.DOScale(originScale, 0.5f).OnComplete(() =>
                        {
                            isScaling = false;
                        });
                    });
                }
            }
        }
    }
}

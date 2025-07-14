using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    public float downScale = 0.85f;

    [SerializeField]
    public float upScale = 1.05f;
    
    public Transform target;
    
    private Vector3 defScale;
    private bool _isLock = false;
    private void Awake()
    {
        defScale = transform.localScale;
        if (target != null)
        {
            defScale = target.localScale;
        }
    }
    
    public void SetLock( bool isLock)
    {
        _isLock = isLock;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isLock)
        {
            if (target != null)
            {
                target.localScale = defScale * downScale;
            }
            else
            {
                transform.localScale = defScale * downScale;
            }
            
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isLock)
        {
            if (target != null)
            {
                target.localScale = defScale*upScale;
            }
            else
            {
                transform.localScale = defScale*upScale;
            }
            Invoke("off",0.1f);
        }
    }

    private void off()
    {
        if (target != null)
        {
            target.localScale = defScale;
        }
        else
        {
            transform.localScale = defScale;
        }
        //if (transform.localScale >= 1)
        //{

        //}
    }


}
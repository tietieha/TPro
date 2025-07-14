using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class UITipChecker : MonoBehaviour
{
    private bool isRaycast = false;
    private Vector3 touchPosition = Vector3.zero;
    private PointerEventData eventData = null;
    private List<RaycastResult> listResult = new List<RaycastResult>();
    private Action handler = null;
    private bool canRaycast = true;
    private Camera mainCamera;

    void Start()
    {
        eventData = new PointerEventData(EventSystem.current);
        // mainCamera = Camera.main;
        mainCamera = SceneCameraManager.Instance.GetCamera(UniversalAdditionalCameraData.CameraViewType.SCENE);
    }

    void Update()
    {
        if (!canRaycast)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            isRaycast = true;
            touchPosition = Input.mousePosition;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isRaycast = true;
                touchPosition = touch.position;
            }
        }
#endif

        if (!isRaycast)
            return;

        // UI检测
        eventData.position = touchPosition;
        listResult.Clear();
        EventSystem.current.RaycastAll(eventData, listResult);

        bool isUIHit = false;
        foreach (var result in listResult)
        {
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform))
            {
                isUIHit = true;
                break;
            }
        }

        if (!isUIHit)
        {
            //mainCamera 有可能为空
            if (mainCamera == null)
            {
                CloseTip();
            }
            else
            {
                // 非UI区域点击，检测3D场景
                Ray ray = mainCamera.ScreenPointToRay(touchPosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    // 命中场景物体，视为点击外部
                    CloseTip();
                }
                else
                {
                    // 没有命中任何物体，视为点击空白
                    CloseTip();
                }
            }
        }

        isRaycast = false;
    }

    private void CloseTip()
    {
        if (handler != null)
            handler();
        else
            gameObject.SetActive(false);
    }

    public void SetCallback(Action cbk)
    {
        handler = cbk;
    }

    public void SetRaycastEnable(bool val)
    {
        canRaycast = val;
    }
}

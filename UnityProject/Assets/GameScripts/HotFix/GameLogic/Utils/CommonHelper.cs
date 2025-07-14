using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TEngine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;
using TMPro;


[CSharpCallLua]
public class CommonHelper
{

    static RaycastHit raycastHit;
    static Vector3 outPos = Vector3.zero;
    static RaycastHit[] m_Hits = new RaycastHit[5];

    public static List<object> luaGetParamsList(params object[] paras)
    {
        List<object> _list = new List<object>();
        var len = paras.Length;
        _list.AddRange(paras);
        return _list;
    }
    /// <summary>
    /// 是否点击到了ui上
    /// </summary>
    /// <returns></returns>
    public static bool isTouchingUI()
    {

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return true;
        }
#else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
#endif
        return false;
    }
    public static Vector3 GetRaycastHitInfo(Ray ray, string layerMaskName, float maxDistance = 100)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, LayerMask.GetMask(layerMaskName)))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public static Vector3 GetRaycastHitInfo(Vector3 origin, Vector3 down, int layerMask, float maxDistance = 200)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, down, out hit, maxDistance, layerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public static Vector2 WorldPosToScreenLocalPos(UnityEngine.Camera camera, UnityEngine.Camera uiCamera, RectTransform rectangle, Vector3 target)
    {
        Vector3 tarPos = target;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, tarPos);
        Vector2 imgPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle, screenPos, uiCamera, out imgPos);
        return new Vector2(imgPos.x, imgPos.y);
    }

    public static bool CameraScreenPointToRay(Camera camera, Vector3 scPos, float distance, raycastHitCall call_back, int layer = -1)
    {
        if (isTouchingUI())
        {
            return false;
        }
        Ray myRay = camera.ScreenPointToRay(scPos);
        var _layer = 1 << layer;
        if (Physics.Raycast(myRay, out raycastHit, distance, _layer))
        {
            Debug.DrawRay(myRay.origin, myRay.direction * 1000, Color.red);
            if (call_back != null)
            {
                call_back(raycastHit);

                Debug.DrawLine(myRay.origin, raycastHit.point, Color.red);
            }
            return true;

        }

        return false;
    }

    /// <summary>
    /// 当 Canvas renderMode 为 RenderMode.ScreenSpaceCamera、RenderMode.WorldSpace 时 uiCamera 不能为空
    /// 当 Canvas renderMode 为 RenderMode.ScreenSpaceOverlay 时 uiCamera 可以为空
    /// </summary>
    /// <param name="rt"></param>
    /// <param name="screenPoint"></param>
    /// <param name="uiCamera"></param>
    /// <returns></returns>
    public static bool ScreenPointToUIWorldPoint(RectTransform rt, Vector2 screenPoint, Camera uiCamera, out Vector3 globalMousePos)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, uiCamera, out globalMousePos))
        {
            return true;
        }
        return false;
    }

    public static Vector3 ScreenPointToWorldPointInRectangle(GameObject target, UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector3 pos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(target.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos))
        {
            return pos;
        }
        return pos;
    }
    /// <summary>
    /// 当 Canvas renderMode 为 RenderMode.ScreenSpaceCamera、RenderMode.WorldSpace 时 uiCamera 不能为空
    /// 当 Canvas renderMode 为 RenderMode.ScreenSpaceOverlay 时 uiCamera 可以为空
    /// </summary>
    /// <param name="rt"></param>
    /// <param name="screenPoint"></param>
    /// <param name="uiCamera"></param>
    /// <returns></returns>
    public static bool ScreenPointToUILocalPoint(RectTransform rt, Vector2 screenPoint, Camera uiCamera, out Vector2 globalMousePos)
    {

        //Vector2 globalMousePos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPoint, null, out globalMousePos))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 判断当前的坐标点是不是在rt大小范围内
    /// </summary>
    /// <param name="_rt">目标区域</param>
    /// <param name="sc">屏幕坐标点</param>
    /// <returns></returns>
    public static bool RectangleContainsScreenPoint(RectTransform _rt, Vector3 sc,Camera camera = null)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_rt, sc, camera);
    }

    public static bool isAnimatorStatus(Animator _animator, string _status)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(_status);
    }

    public static bool IsHitGameObjectBySceenPoint(Camera camera,GameObject gameObject)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    
        int hitResultCount = Physics.RaycastNonAlloc(ray, m_Hits);

        if (hitResultCount <= 0)
            return false;

        foreach (RaycastHit hit in m_Hits)
        {
            if (hit.collider == null)
                continue;
            if (hit.transform.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                return true;
        }
        return false;
    }

    public static bool TestPlanesAABB(Camera camera,Bounds bounds)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
    

    #region  导航网格相关

    public static Vector3 GetNavMeshSamplePosition(Vector3 selfPos, Vector3 targetPos)
    {
        targetPos = NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 10, 1) ? hit.position : selfPos;
        return targetPos;
    }

    public static float GetNavMeshLength(NavMeshAgent agent)
    {
        NavMeshPath path = agent.path;
        if (path.corners.Length < 2)
            return 0;
        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }

    public static float GetNavMeshCalculatePathLength(Vector3 startPos, Vector3 endPos)
    {
        NavMeshPath path = NavMeshCalculatePath(startPos, endPos);
        if (path.corners.Length < 2)
            return 0;
        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }

    public static NavMeshPath NavMeshCalculateSamplePath(Vector3 startPos, Vector3 targetPos)
    {
        var endPos = NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 10, 1) ? hit.position : startPos;

        NavMeshPath path = new NavMeshPath();
        //创建路径
        NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path);
        return path;
    }
    public static NavMeshPath NavMeshCalculatePath(Vector3 startPos, Vector3 endPos)
    {

        NavMeshPath path = new NavMeshPath();
        //创建路径
        NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path);

        // float dis = 0.2f;
        // NavMeshHit hit;
        // //改变路径中每个点的位置，为了不贴边缘走，这样很怪。
        // for (int i = 1; i < path.corners.Length - 2; i++)
        // {
        //     bool result = NavMesh.FindClosestEdge(path.corners[i], out hit, NavMesh.AllAreas);
        //     if (result && hit.distance < dis)
        //         path.corners[i] = hit.position + hit.normal* dis;
        // }
        // Log.Error("path Len " + path.corners.Length);
        // var instance = new GameObject("pointTest");
        // for (int i = 0; i < path.corners.Length; i++)
        // {
        //     GameObject.Instantiate(instance, path.corners[i], Quaternion.identity);
        // }

        return path;
    }

    #endregion

    #region Dotween 拓展

    /// <summary>
    /// int ��ֵ
    /// </summary>
    public static Tween DoMoveInt(int isLoop, LoopType loopType, int curValue, int targetValue, float time, float delayTime, Ease ease, Action<int> onProgress, Action onComplete)
    {
        var doTween = DOTween.To(() => curValue, (x) => curValue = x, targetValue, time).OnUpdate(() =>
        {
            if (onProgress != null)
            {
                onProgress(curValue);
            }
        }).OnComplete(() =>
        {
            if (onComplete != null)
            {
                onComplete();
            }
        }).SetDelay(delayTime).SetEase(ease).SetLoops(isLoop, loopType).SetAutoKill(true);
        return doTween;
    }

    /// <summary>
    /// Float ��ֵ
    /// </summary>
    public static Tween DoMoveFloat(int isLoop, LoopType loopType, float curValue, float targetValue, float time, float delayTime, Ease ease, Action<float> onProgress, Action onComplete, Action onStepComplete)
    {
        var doTween = DOTween.To(() => curValue, (x) => curValue = x, targetValue, time).OnUpdate(() =>
        {
            if (onProgress != null)
            {
                onProgress(curValue);
            }
        }).OnComplete(() =>
        {
            if (onComplete != null)
            {
                onComplete();
            }
        }).OnStepComplete(() =>
        {
            if (onStepComplete != null)
            {
                onStepComplete();
            }
        }).SetDelay(delayTime).SetEase(ease).SetLoops(isLoop, loopType).SetAutoKill(true);
        return doTween;
    }

    /// <summary>
    /// Vector3 ��ֵ
    /// isLoop -1��ѭ��
    /// </summary>
    public static Tween DoMoveVector(int isLoop, LoopType loopType, Vector3 curValue, Vector3 targetValue, float time, float delayTime, Ease ease, Action<Vector3> onProgress, Action onComplete)
    {

        var doTween = DOTween.To(() => curValue, (x) => curValue = x, targetValue, time).OnUpdate(() =>
        {
            if (onProgress != null)
            {
                onProgress(curValue);
            }
        }).OnComplete(() =>
        {
            if (onComplete != null)
            {
                onComplete();
            }
        }).SetDelay(delayTime).SetEase(ease).SetLoops(isLoop, loopType).SetAutoKill(true);
        return doTween;
    }

    /// <summary>
    /// CanvasGroup ��ֵ
    /// </summary>
    public static void DoCanvasGroup(RectTransform rec, float endValue, float endTime)
    {
        //if (rec == null) return;
        //CanvasGroup canvas = rec.GetComponent<CanvasGroup>();
        //if (canvas == null)
        //{
        //    canvas = rec.gameObject.AddComponent<CanvasGroup>();
        //}
        //canvas.DOFade(endValue, endTime);
    }

    /// <summary>
    /// CanvasGroup����Ч��
    /// </summary>
    /// <param name="rec"></param>
    public static void DotweenAllFadeIn(RectTransform rec)
    {
        //if (rec == null)
        //{
        //    return;
        //}
        //CanvasGroup canvasGroup = rec.GetComponent<CanvasGroup>();
        //if (canvasGroup == null)
        //{
        //    canvasGroup = rec.gameObject.AddComponent<CanvasGroup>();
        //}
        //canvasGroup.alpha = 0.3f;
        //canvasGroup.DOKill();
        //canvasGroup.DOFade(1, 1f);
    }

    /// <summary>
    /// CanvasGroup����Ч��
    /// </summary>
    /// <param name="rec"></param>
    public static void DotweenAllFadeOut(RectTransform rec)
    {
        //if (rec == null)
        //{
        //    return;
        //}
        //CanvasGroup canvasGroup = rec.GetComponent<CanvasGroup>();
        //if (canvasGroup == null)
        //{
        //    canvasGroup = rec.gameObject.AddComponent<CanvasGroup>();
        //}
        //canvasGroup.DOFade(0, 0.5f);
    }

    /// <summary>
    /// Scale��ֵ
    /// </summary>
    public static void DotweenScale(RectTransform rec, Vector3 endScale, float time, Action callBack = null)
    {
        if (rec == null)
            return;
        rec.DOScale(endScale, time).OnComplete(() =>
        {
            if (rec != null && callBack != null)
            {
                callBack();
            }
        });
    }

    public static void DotweenScale(RectTransform rec, float endScale, float time, Action callBack = null)
    {
        if (rec == null)
            return;
        rec.DOScale(endScale, time).OnComplete(() =>
        {
            if (rec != null && callBack != null)
            {
                callBack();
            }
        });
    }

    /// <summary>
    /// AnchorPos��ֵ
    /// </summary>
    public static void DOtweenAnchorPos(RectTransform rec, Vector2 endPos, float time, Action callBack = null)
    {
        //if (rec != null)
        //{
        //    rec.DOAnchorPos(endPos, time).OnComplete(() =>
        //     {
        //         rec.anchoredPosition = endPos;
        //         if (callBack != null)
        //         {
        //             callBack();
        //         }
        //     });
        //}
    }

    /// <summary>
    /// AnchorPos ѭ����ֵ
    /// </summary>
    public static void DOtweenAnchorPosLoop(RectTransform rec, Vector2 endPos, float time, int loopCount = 1, Action callBack = null)
    {
        //if (rec != null)
        //{
        //    rec.DOAnchorPos(endPos, time).OnComplete(() =>
        //    {
        //        rec.anchoredPosition = endPos;
        //        if (callBack != null)
        //        {
        //            callBack();
        //        }
        //    }).SetLoops(loopCount, LoopType.Restart).SetEase(Ease.Linear);
        //}
    }

    /// <summary>
    /// AnchorPos��ֵ
    /// </summary>
    public static void DOTweenAnchorPosX(RectTransform rec, float x, float time, Ease ease = Ease.Linear, Action complete = null)
    {
        //if (rec != null)
        //{
        //    rec.DOAnchorPosX(x, time).OnComplete(() => { complete?.Invoke(); }).SetEase(ease);
        //}
    }

    public static void DoRotate(Transform t, Vector3 euler, float duration, System.Action onFinished = null)
    {
        t.DORotate(euler, duration).OnComplete(() =>
        {
            onFinished?.Invoke();
        });
    }

    public static void DoLocalRotate(Transform t, Vector3 euler, float duration, System.Action onFinished = null)
    {
        t.DOLocalRotate(euler, duration).OnComplete(() =>
        {
            onFinished?.Invoke();
        });
    }

    public static void DoLocalRotateLoop(RectTransform rt, int isLoop, Vector3 euler, float duration, System.Action onFinished = null)
    {
        rt.DOLocalRotate(euler, duration).OnComplete(() =>
        {
            onFinished?.Invoke();
        }).SetLoops(isLoop, LoopType.Yoyo).SetAutoKill(true);
    }

    /// <summary>
    /// ����Action��float��ֵ
    /// </summary>
    public static Tweener To(Action<float> setter, float startValue, float endValue, float duration)
    {
        return DOTween.To(v => setter(v), startValue, endValue, duration);
    }

    public static Tween To(Action<float> action, float startValue, float endValue, float duration, int loopTime = -1, Ease easeType = Ease.Linear)
    {
        return DOTween.To(() => startValue, v => { startValue = v; action(v); },
            endValue, duration).SetEase(easeType).SetLoops(loopTime).SetUpdate(UpdateType.Fixed);
    }

    public static Tweener To(Transform target, LuaFunction setter, float startValue, float endValue, float duration)
    {
        var tweener = DOTween.To(v => setter.Call(v), startValue, endValue, duration);
        tweener.SetTarget(target);
        return tweener;
    }

    //public static Tween DoSizeDelta(RectTransform rec, Vector2 end, float time, Action action, Ease ease = Ease.Linear)
    //{
    //    var Tw = rec.DOSizeDelta(end, time);
    //    Tw.OnComplete(() =>
    //    {
    //        action();
    //    }).SetEase(ease);
    //    return Tw;
    //}

    public static void DoCamField(Camera camera, float end, float time, Action action = null)
    {
        if (camera == null)
        {
            Log.Error("δ�ҵ����");
            return;
        }
        camera.DOFieldOfView(end, time).OnComplete(() =>
        {
            if (action != null)
            {
                action();
            }
        });
    }

    public static void DoTransLoacalPos(Transform transform, Vector3 end, float time, Action action = null)
    {
        if (transform == null)
        {
            Log.Error("δ�ҵ�transformλ��");
            return;
        }
        transform.DOLocalMove(end, time).OnComplete(() =>
        {
            action?.Invoke();
        });
    }

    public static void DoTransPos(Transform transform, Vector3 end, float time, Action action = null, Ease ease = Ease.Unset)
    {
        if (transform == null)
        {
            Log.Error("δ�ҵ�transformλ��");
            return;
        }

        transform.DOMove(end, time).OnComplete(() =>
        {
            action?.Invoke();
        }).SetEase(ease);
    }

    //public static Tween DoYoyoFade(Text text, float alpha, float duration)
    //{
    //    if (text == null)
    //        return null;
    //    return text.DOFade(alpha, duration).SetLoops(-1, LoopType.Yoyo);
    //}

    public static void DotweenRestart(RectTransform rec)
    {
        rec.DORestart(rec);
    }

    //public static Tween DOColor(Image image, Color c, float dutation)
    //{
    //    return image.DOColor(c, dutation);
    //}

    public static int Kill(object tweener)
    {
        return DOTween.Kill(tweener);
    }
    #endregion
    public static string GetMd5Hash(string input)
    {
        MD5 md5Hash = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    private static Vector2 _cachedPos;
    public static Vector2 WorldToUI(float x, float y, float z, Camera sceneCamera, Camera uiCamera, RectTransform rectTransform)
    {
        var screenPos = RectTransformUtility.WorldToScreenPoint(sceneCamera, new Vector3(x, y, z));
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, uiCamera, out _cachedPos);
        return _cachedPos;
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using System;
using XLua;

public class FlyModeGroup
{
    public int id = 0;
    public Action<int> m_Action;
    public int count = 0;
    public Action _CallBack;

    public void Callback()
    {
        count--;
        if (count <= 0)
        {
            if (m_Action != null)
            {
                m_Action.Invoke(id);
            }
        }
    }
}


[LuaCallCSharp]
[ExecuteInEditMode]
public class FlyMode : MonoBehaviour
{
    public GameObject m_TargePrefab;
    public TrailRenderer m_trailRenderer;
    public FlyTabelAsset info = null;
    private List<Image> m_objects = new List<Image>();
    public bool m_isPause = false;
    public bool m_isCreatePlay = false;
    private float m_minPoolIndex = 300;
    private float m_InTime = 0.0f;
    private Queue<int> m_IdQueue = new Queue<int>();
    private Queue<FlyModeItem> objectPool = new Queue<FlyModeItem>();
    private Canvas m_canvas = null;
    private List<FlyModeGroup> flyModeGroups = new List<FlyModeGroup>();

    private void Awake()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
#endif
        m_InTime = Time.time;
        m_canvas = GetComponent<Canvas>();
        if (m_canvas == null)
        {
            m_canvas = GetComponentInParent<Canvas>();
        }

        for (int i = 0; i < 100; i++)
        {
            m_IdQueue.Enqueue(i);
        }
    }

    private void Recycle(object sender, GameEventArgs e)
    {
        FlyModeItem item = (FlyModeItem)sender;
        objectPool.Enqueue(item);
        item.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Destory();
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
#endif
    }

    public void SetValue(Vector3 _starPosition, Vector2 _endPosition, Image _icon,
        int _count, GameObject _object, Action _CallBack)
    {
        info.info.m_StartPosition = _starPosition;
        //info.m_CustomEndPoint.m_position = RectTransformUtility.WorldToScreenPoint(SceneCameraManager.Instance.GetCamera(UniversalAdditionalCameraData.CameraViewType.GUICAMERA), _endPosition) ;
        Vector2 endpos = Vector2.zero;
        RectTransform rt = this.transform.parent.parent.parent
            .GetComponentInParent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, _endPosition,
            SceneCameraManager.Instance.GetCamera(UniversalAdditionalCameraData
                .CameraViewType.GUICAMERA), out endpos);
        info.info.m_CustomEndPoint.m_position = new Vector3(endpos.x, endpos.y, 0);
        info.info.m_InstanceCount = _count;
        CreatePlay(_icon, _object, _CallBack);
    }

    public void SetValue(Vector3 _starPosition, Vector2 _endPosition, Image _icon,
        int _count, Action _CallBack)
    {
        info.info.m_StartPosition = _starPosition;
        //info.m_CustomEndPoint.m_position = RectTransformUtility.WorldToScreenPoint(SceneCameraManager.Instance.GetCamera(UniversalAdditionalCameraData.CameraViewType.GUICAMERA), _endPosition) ;
        Vector2 endpos = Vector2.zero;
        RectTransform rt = this.transform.parent.parent.parent
            .GetComponentInParent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, _endPosition,
            SceneCameraManager.Instance.GetCamera(UniversalAdditionalCameraData
                .CameraViewType.GUICAMERA), out endpos);
        info.info.m_CustomEndPoint.m_position = new Vector3(endpos.x, endpos.y, 0);
        info.info.m_InstanceCount = _count;
        CreatePlay(_icon, _CallBack);
    }

    public void SetFlyValue(Vector3 _starPosition, Vector3 _endPosition, Image _icon, int _count, GameObject _object, Action _CallBack)
    {
        info.info.m_StartPosition = _starPosition;
        info.info.m_CustomEndPoint.m_position = _endPosition;
        info.info.m_InstanceCount = _count;
        CreatePlay(_icon, _object, _CallBack);
    }

    public void Play()
    {
        info.info.StartTime = Time.time;
        info.info.m_isRun = true;
        m_isCreatePlay = false;
        IconInstance(null);
    }

    public void CreatePlay(Image _sprite, Action _Callback = null)
    {
        info.info.StartTime = Time.time;
        m_isCreatePlay = true;
        info.info.m_isRun = true;
        IconInstance(_sprite, null, _Callback);
    }

    public void CreatePlay(Image _sprite, GameObject _object, Action _Callback = null)
    {
        info.info.StartTime = Time.time;
        m_isCreatePlay = true;
        info.info.m_isRun = true;
        IconInstance(_sprite, _object, _Callback);
    }

    public void IconInstance(Image _sprite, GameObject _object = null,
        Action _callback = null)
    {
        FlyModeGroup fmg = new FlyModeGroup();
        float deltimes = 0.0f;
        for (int i = 0; i < info.info.m_InstanceCount; i++)
        {
            GameObject go = null;
            Vector3 position = info.info.m_StartPosition;
            if (info.info.m_isInstanceRandomPosition)
            {
                position = new Vector3(
                    UnityEngine.Random.Range(info.info.m_StartPositionMin.x,
                        info.info.m_StartPosition.x),
                    UnityEngine.Random.Range(info.info.m_StartPositionMin.y,
                        info.info.m_StartPosition.y),
                    UnityEngine.Random.Range(info.info.m_StartPositionMin.z,
                        info.info.m_StartPosition.z));
            }

            if (objectPool.Count > 0)
            {
                go = objectPool.Dequeue().gameObject;
                go.transform.position = position;
                go.transform.rotation = Quaternion.identity;
                go.gameObject.SetActive(true);
            }
            else
            {
                if (_object != null)
                {
                    go = GameObject.Instantiate(_object, this.transform);
                    go.transform.position = position;
                }
                else
                {
                    go = Instantiate(m_TargePrefab, position, Quaternion.identity,
                        this.transform);
                }
            }

            if (go != null)
            {
                Image img = go.GetComponent<Image>();

                if (img != null)
                {
                    if (_sprite != null)
                    {
                        img = _sprite;
                    }

                    img.enabled = false;
                    go.AddComponent<FlyModeItem>();
                    if (info.info.m_UseTrail)
                    {
                        TrailRenderer tr = go.GetComponent<TrailRenderer>();
                        if (tr == null)
                        {
                            go.AddComponent<TrailRenderer>();
                            tr = go.GetComponent<TrailRenderer>();
                        }

                        tr.startWidth = m_trailRenderer.startWidth;
                        tr.endWidth = m_trailRenderer.endWidth;
                        tr.widthMultiplier = m_trailRenderer.widthMultiplier;
                        tr.widthCurve = m_trailRenderer.widthCurve;
                        tr.time = m_trailRenderer.time;
                        tr.material = Instantiate(m_trailRenderer.sharedMaterial);
                        if (m_canvas != null)
                        {
                            tr.sortingLayerID = m_canvas.sortingLayerID;
                            tr.sortingLayerName = m_canvas.sortingLayerName;
                            tr.sortingOrder = m_canvas.sortingOrder - 1;
                        }
                    }

                    fmg.count++;
                    if (info.info.m_isInterval)
                    {
                        deltimes += info.info.m_delayTimeActive;
                    }

                    go.GetComponent<FlyModeItem>().Init(info.info, fmg.Callback,
                        fmg.count, deltimes);
                    m_objects.Add(img);
                }
            }
        }

        if (_object != null)
        {
            _object.SetActive(false);
        }

        if (m_IdQueue.Count > 0)
        {
            fmg.id = m_IdQueue.Dequeue();
        }

        fmg.m_Action = QueueCall;
        fmg._CallBack = _callback;
        flyModeGroups.Add(fmg);
    }


    public void QueueCall(int _id)
    {
        for (int i = 0; i < flyModeGroups.Count; i++)
        {
            if (flyModeGroups[i].id == _id)
            {
                if (flyModeGroups[i]._CallBack != null)
                {
                    flyModeGroups[i]._CallBack.Invoke();
                }

                m_IdQueue.Enqueue(_id);
                flyModeGroups.RemoveAt(i);
                break;
            }
        }
    }

    public void NextPlay()
    {
        info.info.m_isRun = true;
        m_isPause = false;
        info.info.StartTime = Time.time - info.info.time;
        for (int i = m_objects.Count - 1; i >= 0; i--)
        {
            m_objects[i].GetComponent<FlyModeItem>().Play();
        }
    }

    public void Pause()
    {
        info.info.m_isRun = false;
        m_isPause = true;
        for (int i = m_objects.Count - 1; i >= 0; i--)
        {
            m_objects[i].GetComponent<FlyModeItem>().Pause();
        }
    }

    public void Stop()
    {
        info.info.time = 0;
        info.info.m_isRun = false;
        m_isPause = false;
        Destory();
    }

    public void Destory()
    {
        objectPool.Clear();
        for (int i = m_objects.Count - 1; i >= 0; i--)
        {
            if (m_objects[i] != null)
            {
                Destroy(m_objects[i].gameObject);
                m_objects[i] = null;
            }
        }

        m_objects.Clear();
    }

    private void Update()
    {
        if (m_isCreatePlay)
            return;
        if (info == null)
            return;
        if (info.info.m_isRun)
        {
            if (info.info.m_isdelayActive)
            {
                info.info.time = (Time.time - info.info.StartTime) /
                                 (info.info.endTime + info.info.m_delayTimeActive);
            }
            else
            {
                info.info.time = (Time.time - info.info.StartTime) / info.info.endTime;
            }

            if (info.info.time >= 1)
            {
                Stop();
            }
        }

        if (m_isPause)
        {
            for (int i = m_objects.Count - 1; i >= 0; i--)
            {
                m_objects[i].GetComponent<FlyModeItem>().setTime(info.info.time);
            }
        }

        PeriodicTreatment();
    }

    private void PeriodicTreatment()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
#endif
        if (Time.time - m_InTime > m_minPoolIndex && objectPool.Count > 10)
        {
            for (int i = 0; i > objectPool.Count - 10; i++)
            {
                FlyModeItem itm = objectPool.Dequeue();
                if (itm != null)
                {
                    Destroy(itm.gameObject);
                }
            }
        }
    }
}

[System.Serializable]
public class FlyModeData
{
    public bool m_Random = false;
    public bool m_Immobilization = false;
    public float m_AllTime = 1.0f;
    public float m_MinDelayTime = 0.0f;
    public float m_delayTime = 0;
    public AnimationCurve m_MoveCurve_X = new AnimationCurve();
    public AnimationCurve m_MoveCurve_Y = new AnimationCurve();
    public AnimationCurve m_SizeCurve_X = new AnimationCurve();
    public AnimationCurve m_SizeCurve_Y = new AnimationCurve();
    public Vector3 m_position = Vector3.zero;
    public Vector3 m_position2 = Vector3.zero;
    public UnityEngine.Gradient m_Color;
    public Vector3 m_StartPointRand = Vector3.zero;
    public bool m_UseTrail = false;
}

[System.Serializable]
public class FlyModeInfo
{
    public bool m_isdelayActive;
    public bool m_isInterval = false;
    public float m_delayTimeActive;
    public int m_InstanceCount = 5;
    public List<FlyModeData> m_TargetPosition = new List<FlyModeData>();
    public bool m_PlayOnAwake = false;
    public bool m_UseCustomEndPoint = true;
    public bool m_isInstanceRandomPosition = false;
    public Vector3 m_StartPosition = Vector3.zero;
    public Vector3 m_StartPositionMin = Vector3.zero;
    public Vector3 m_StartSize = Vector3.one;
    public FlyModeData m_CustomEndPoint = new FlyModeData();
    public float time = 0.0f;
    public float endTime = 1.0f;
    public bool m_isRun;
    public float StartTime = 0.0f;
    public bool m_UseTrail = false;
}
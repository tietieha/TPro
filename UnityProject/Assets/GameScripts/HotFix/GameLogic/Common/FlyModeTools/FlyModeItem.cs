using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FlyModeItem : MonoBehaviour
{
    public FlyModeInfo info = new FlyModeInfo();
    private RectTransform rectTransform = null;
    private Image image = null;
    private float deltime = 0.0f;
    private TrailRenderer tr = null;
    private FlyModeData data = null;
    private Queue<FlyModeData> m_CurrentQueue = new Queue<FlyModeData>();
    private Action m_callback = null;
    private int m_index = 0;
    public void Init(FlyModeInfo _info,Action _callback,int _Index,float _deltime = 0)
    {

        m_index = _Index;
        m_callback = _callback;
        info.m_isInstanceRandomPosition = _info.m_isInstanceRandomPosition;
        info.time = _info.time;
        info.endTime = _info.endTime;
        info.StartTime = _info.StartTime;
        info.m_delayTimeActive = _info.m_delayTimeActive;
        info.m_isdelayActive = _info.m_isdelayActive;
        info.m_CustomEndPoint = new FlyModeData();
        info.m_CustomEndPoint.m_Random = _info.m_CustomEndPoint.m_Random;
        info.m_CustomEndPoint.m_Immobilization = _info.m_CustomEndPoint.m_Immobilization;
        info.m_CustomEndPoint.m_AllTime = _info.m_CustomEndPoint.m_AllTime;
        info.m_CustomEndPoint.m_MinDelayTime = _info.m_CustomEndPoint.m_MinDelayTime;
        info.m_CustomEndPoint.m_delayTime = _info.m_CustomEndPoint.m_delayTime;
        info.m_CustomEndPoint.m_MoveCurve_X = _info.m_CustomEndPoint.m_MoveCurve_X;
        info.m_CustomEndPoint.m_MoveCurve_Y = _info.m_CustomEndPoint.m_MoveCurve_Y;
        info.m_CustomEndPoint.m_SizeCurve_X = _info.m_CustomEndPoint.m_SizeCurve_X;
        info.m_CustomEndPoint.m_SizeCurve_Y = _info.m_CustomEndPoint.m_SizeCurve_Y;
        info.m_CustomEndPoint.m_position = _info.m_CustomEndPoint.m_position;
        info.m_CustomEndPoint.m_position2 = _info.m_CustomEndPoint.m_position2;
        info.m_CustomEndPoint.m_Color = _info.m_CustomEndPoint.m_Color;
        info.m_CustomEndPoint.m_StartPointRand = _info.m_CustomEndPoint.m_StartPointRand;
        info.m_CustomEndPoint.m_UseTrail = _info.m_CustomEndPoint.m_UseTrail;
        info.m_PlayOnAwake = _info.m_PlayOnAwake;
        info.m_TargetPosition = new List<FlyModeData>();
        for (int i = 0; i < _info.m_TargetPosition.Count; i++)
        {
            FlyModeData info_e = new FlyModeData();
            info_e.m_Random = _info.m_TargetPosition[i].m_Random;
            info_e.m_Immobilization = _info.m_TargetPosition[i].m_Immobilization;
            info_e.m_AllTime = _info.m_TargetPosition[i].m_AllTime;
            info_e.m_MinDelayTime = _info.m_TargetPosition[i].m_MinDelayTime;
            info_e.m_delayTime = _info.m_TargetPosition[i].m_delayTime;
            info_e.m_MoveCurve_X = _info.m_TargetPosition[i].m_MoveCurve_X;
            info_e.m_MoveCurve_Y = _info.m_TargetPosition[i].m_MoveCurve_Y;
            info_e.m_SizeCurve_X = _info.m_TargetPosition[i].m_SizeCurve_X;
            info_e.m_SizeCurve_Y = _info.m_TargetPosition[i].m_SizeCurve_Y;
            info_e.m_position = _info.m_TargetPosition[i].m_position;
            info_e.m_position2 = _info.m_TargetPosition[i].m_position2;
            info_e.m_Color = _info.m_TargetPosition[i].m_Color;
            info_e.m_StartPointRand = _info.m_TargetPosition[i].m_StartPointRand;
            info_e.m_UseTrail = _info.m_TargetPosition[i].m_UseTrail;
            info.m_TargetPosition.Add(info_e);
        }
        info.m_StartPosition = this.transform.position;
        info.m_StartPositionMin = _info.m_StartPositionMin;
        info.m_UseCustomEndPoint = _info.m_UseCustomEndPoint;
        info.m_StartSize = _info.m_StartSize;
        info.m_isInterval = _info.m_isInterval;
        info.m_UseTrail = _info.m_UseTrail;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        tr = GetComponent<TrailRenderer>();
        m_CurrentQueue.Clear();
        if (info.m_TargetPosition.Count > 0)
        {
            for (int i = 0; i < info.m_TargetPosition.Count; i++)
            {
                m_CurrentQueue.Enqueue(info.m_TargetPosition[i]);
            }
        }
        m_CurrentQueue.Enqueue(info.m_CustomEndPoint);
        Next();
        if (info.m_isdelayActive && _Index != 1)
        {
            image.enabled = false;
            info.m_isRun = false;
            if (tr != null)
            {
                tr.enabled = false;
            }
            if (info.m_isInterval)
            {
                deltime = _deltime;
            }
            else
            {
                deltime = UnityEngine.Random.Range(0, info.m_delayTimeActive);
            }
            StartCoroutine("DelayTimeActive");
        }
        else
        {
            deltime = 0;
            info.m_isRun = _info.m_isRun;
            image.enabled = true;

        }
    }
    IEnumerator DelayTimeActive()
    {
        yield return new WaitForSecondsRealtime(deltime);
        info.m_isRun = true;
        info.StartTime = Time.time;
        image.color = new Color(0, 0, 0, 0);
        image.enabled = true;
    }
    IEnumerator DelayPlay()
    {
        yield return new WaitForSecondsRealtime(deltime);
        info.time = 0.0f;
        info.m_isRun = true;
        info.StartTime = Time.time;
    }
    private void OnDestroy()
    {
        StopCoroutine("DelayTimeActive");
        StopCoroutine("DelayPlay");
    }

    public void setTime(float _time)
    {
        info.time = _time;

    }
    private void Next()
    {
        if (m_CurrentQueue.Count == 0)
        {
            Stop();
            return;
        }
        data = new FlyModeData();
        FlyModeData ad = m_CurrentQueue.Dequeue();
        data.m_AllTime = ad.m_AllTime ;
        data.m_delayTime = ad.m_delayTime;
        data.m_position = ad.m_position;
        data.m_position2 = ad.m_position2;
        data.m_Random = ad.m_Random;
        data.m_Color = ad.m_Color;
        data.m_MoveCurve_X = ad.m_MoveCurve_X;
        data.m_MoveCurve_Y = ad.m_MoveCurve_Y;
        data.m_SizeCurve_X = ad.m_SizeCurve_X;
        data.m_SizeCurve_Y = ad.m_SizeCurve_Y;
        data.m_Immobilization = ad.m_Immobilization;
        data.m_StartPointRand = ad.m_StartPointRand;
        info.time = 0.0f;
        info.StartTime = Time.time;
        info.endTime = data.m_AllTime;
        info.m_StartPosition = this.rectTransform.anchoredPosition3D;
        if(tr != null)
        {
            tr.enabled = true;
        }

        if (data.m_Random)
        {
            if (!data.m_Immobilization)
            {

                data.m_position = new Vector3(UnityEngine.Random.Range(ad.m_position2.x, ad.m_position.x), UnityEngine.Random.Range(ad.m_position2.y, ad.m_position.y), UnityEngine.Random.Range(ad.m_position2.z, ad.m_position.z)) + info.m_StartPosition;

            }
            else
            {

                Vector3 rad = new Vector3(UnityEngine.Random.Range(-50, 50), 0, 0);
                if (m_index % 2 == 0)
                {
                    info.m_StartPosition = this.rectTransform.anchoredPosition3D + new Vector3(UnityEngine.Random.Range(0, data.m_StartPointRand.x), UnityEngine.Random.Range(0, data.m_StartPointRand.y), UnityEngine.Random.Range(0, data.m_StartPointRand.z));
                    data.m_position = ad.m_position + rad + info.m_StartPosition;
                }
                else
                {
                    info.m_StartPosition = this.rectTransform.anchoredPosition3D + new Vector3(UnityEngine.Random.Range(-data.m_StartPointRand.x, 0), UnityEngine.Random.Range(-data.m_StartPointRand.y, 0), UnityEngine.Random.Range(-data.m_StartPointRand.z, 0));

                    data.m_position = ad.m_position2 + rad + info.m_StartPosition;
                }
            }
        }
        if (ad.m_delayTime > 0)
        {
            info.m_isRun = false;
            deltime = UnityEngine.Random.Range(ad.m_MinDelayTime, ad.m_delayTime);
            StartCoroutine("DelayPlay");
        }
    }
    private void Update()
    {
        if (info.m_isRun)
        {
            info.time = (Time.time - info.StartTime) / info.endTime;
            if (info.m_TargetPosition.Count > 0)
            {
                if (info.time >= 1)
                {
                    if (m_CurrentQueue.Count == 0)
                    {
                        Stop();
                    }
                    else
                    {
                        Next();
                    }
                }
            }
            else
            {
                if (info.time >= 1)
                {
                    Stop();
                }
            }

            this.rectTransform.anchoredPosition3D = info.m_StartPosition + new Vector3(
                (data.m_position.x - info.m_StartPosition.x) * data.m_MoveCurve_X.Evaluate(info.time),
                (data.m_position.y - info.m_StartPosition.y) * data.m_MoveCurve_Y.Evaluate(info.time),
                data.m_position.z - info.m_StartPosition.z);
            // this.rectTransform.localScale = new Vector3(
            //     info.m_StartSize.x * data.m_SizeCurve_X.Evaluate(info.time),
            //     info.m_StartSize.y * data.m_SizeCurve_Y.Evaluate(info.time),
            //     info.m_StartSize.z);
                image.color = data.m_Color.Evaluate(info.time);
            if (tr != null)
            {
                tr.endColor = data.m_Color.Evaluate(info.time) * new Color(1,1,1,0);
                tr.startColor = data.m_Color.Evaluate(info.time);
            }
        }
        else
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                return;
            }
            if (rectTransform == null)
            { return; }
            info.time = info.time - deltime;
            this.rectTransform.anchoredPosition3D = info.m_StartPosition + new Vector3(
               (info.m_CustomEndPoint.m_position.x - info.m_StartPosition.x) * info.m_CustomEndPoint.m_MoveCurve_X.Evaluate(info.time),
               (info.m_CustomEndPoint.m_position.y - info.m_StartPosition.y) * info.m_CustomEndPoint.m_MoveCurve_Y.Evaluate(info.time),
               info.m_CustomEndPoint.m_position.z - info.m_StartPosition.z);
            this.rectTransform.localScale = new Vector3(
                info.m_StartSize.x * info.m_CustomEndPoint.m_SizeCurve_X.Evaluate(info.time),
                info.m_StartSize.y * info.m_CustomEndPoint.m_SizeCurve_Y.Evaluate(info.time),
                info.m_StartSize.z);
            image.color = info.m_CustomEndPoint.m_Color.Evaluate(info.time);
#endif
        }

    }
    public void Stop()
    {
        info.m_isRun = false;
        this.gameObject.SetActive(false);
        m_callback.Invoke();
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
#endif
    }
    public void Pause()
    {
        info.m_isRun = false;
    }
    public void Play()
    {
        info.StartTime = Time.time - info.time;
        info.m_isRun = true;
    }
}

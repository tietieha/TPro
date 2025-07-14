using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class FlyPositionAnimationItem : MonoBehaviour
{
    private AnimationCurve m_posX;
    private AnimationCurve m_posY;
    private AnimationCurve m_posZ;
    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Action m_action = null;
    private Action<GameObject> m_dequeue = null;
    private float m_StartTime;
    private float m_Times;
    private bool m_IsRun;
    private float m_deltime;
    public void Init(AnimationCurve _posx, AnimationCurve _posy, AnimationCurve _posz,Vector3 _startPos,Vector3 _endpos ,float _Time, Action _action ,Action<GameObject> _dequeue,float _deltime) 
    {
        m_posX = _posx;
        m_posY = _posy;
        m_posZ = _posz;
        m_StartPosition = _startPos;
        m_EndPosition = _endpos;
        m_action = _action;
        m_dequeue = _dequeue;
        m_Times = _Time;

        m_deltime = _deltime;
        this.transform.position = m_StartPosition;
        this.gameObject.SetActive(true);
        StartCoroutine("Active");
    }
    IEnumerator Active() 
    {
       yield return new WaitForSecondsRealtime(m_deltime);
        m_StartTime = Time.time;
        GetComponent<Renderer>().enabled = true;
        m_IsRun = true;
    }


    private void Update()
    {
        if (m_IsRun)
        {
            if (Camera.main != null) 
            {
                // this.transform.forward = Camera.main.transform.forward;
            }
            float Duration = (Time.time - m_StartTime) / m_Times;
            this.transform.position = m_StartPosition + new Vector3((m_EndPosition.x - m_StartPosition.x) * m_posX.Evaluate(Duration), (m_EndPosition.y - m_StartPosition.y) * m_posY.Evaluate(Duration), (m_EndPosition.z - m_StartPosition.z) * m_posZ.Evaluate(Duration));
            if (Duration >= 1.0f)
            {
                m_IsRun = false;

                if (m_action != null)
                {
                    m_action.Invoke();
                }
                destory(false, gameObject);
            }
        }

    }
    public  void destory(bool _manager, GameObject _gameObject) 
    {
        if (!_manager)
        {
            if (m_dequeue != null)
            {

                m_dequeue.Invoke(_gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        m_action = null;
        m_dequeue = null;
    }
}

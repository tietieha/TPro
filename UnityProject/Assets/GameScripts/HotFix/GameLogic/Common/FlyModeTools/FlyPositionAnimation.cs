using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using XLua;

[LuaCallCSharp]
public class FlyPositionAnimation : MonoBehaviour
{

    public AnimationCurve m_posX;
    public AnimationCurve m_posY;
    public AnimationCurve m_posZ;
    public float m_Time = 1.0f;
    public float m_delTime = 0.1f;

    private UnityEngine.Pool.ObjectPool<GameObject> m_flyItemTool;
    private UnityEngine.GameObject m_flyItemObject;

    public void Initialized(GameObject _object, int defalutNum, int maxSize)
    {
        m_flyItemObject = _object;
        m_flyItemTool = new ObjectPool<GameObject>(OnItemCreate, OnItemGet, OnItemRelease, OnItemDestroy, true,
            defalutNum, maxSize);
    }

    //需要实例化的对象,起点,终点，数量，完成时的回调
    public void SetFlyMode(GameObject _object,Vector3 _startpostion,Vector3 _ednposition,int _index = 1,Action _action = null) 
    {
        
        for (int i = 0; i < _index; i++)
        {
            GameObject obj = m_flyItemTool.Get();
            FlyPositionAnimationItem fpai= obj.GetComponent<FlyPositionAnimationItem>();
            float time = m_delTime * i;
            fpai.Init(m_posX, m_posY, m_posZ,_startpostion,_ednposition, m_Time, _action, Dequeue, time);
        }
        _object.gameObject.SetActive(false);
    }
    public void Dequeue(GameObject _gameObject) 
    {
        m_flyItemTool.Release(_gameObject);
    }
    public void Clear() 
    {
        if (m_flyItemTool != null)
        {
            m_flyItemTool.Clear();
        }
    }
    private void OnDestroy()
    {
        if (m_flyItemTool != null)
        {
            m_flyItemTool.Clear();
        }

        m_flyItemTool = null;
    }

    private GameObject OnItemCreate()
    {
        GameObject _gameObject = Instantiate(m_flyItemObject, transform);
        return _gameObject;
    }

    private void OnItemGet(GameObject _gameObject)
    {
        _gameObject.AddComponent<FlyPositionAnimationItem>();
        _gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnItemRelease(GameObject _gameObject)
    {
        _gameObject.SetActive(false);
    }

    private void OnItemDestroy(GameObject _gameObject)
    {
        Destroy(_gameObject);
    }
}



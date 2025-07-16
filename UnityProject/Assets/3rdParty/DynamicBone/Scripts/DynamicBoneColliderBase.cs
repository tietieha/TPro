﻿using UnityEngine;

public class DynamicBoneColliderBase : MonoBehaviour
{
    public enum Direction
    {
        X, Y, Z
    }

    public Direction m_Direction = Direction.Y;
    public Vector3 m_Center = Vector3.zero;

    public enum Bound
    {
        Outside,
        Inside
    }
    public Bound m_Bound = Bound.Outside;

    public int PrepareFrame { set; get; }

    public virtual void Start()
    {        
    }

    public virtual void Prepare()
    {
    }

    public virtual bool Collide(ref Vector3 particlePosition, float particleRadius)
    {
        return false;
    }
}

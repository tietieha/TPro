using System;
using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;
using Random = System.Random;
using Vector2 = RVO.Vector2;

public class GameAgent : MonoBehaviour
{
    public int sid = -1;
    public float speed = 1;
    private bool isFirst = true;
    public Vector2 normalized;
    /** Random number generator. */
    private Random m_random = new Random();
    private float commonZValue;

    private Simulator _simulator;
    // Use this for initialization
    void Start()
    {
        commonZValue = -20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (sid >= 0)
        {
            if (!_simulator.IsAgentNo(sid))
                return;
            Vector2 pos = _simulator.GetAgentPosition(sid);
            float x = pos.x();
            if (float.IsNaN(x))
            {
                pos = new Vector2(transform.position.x,transform.position.y);
                _simulator.SetAgentPosition(sid, pos);
            }
            transform.position = new Vector3(pos.x(), pos.y(), commonZValue);

        }
        else
        { return; }
        //var playerPos =  MiniGame.BattleManager.Instance.UnitMgr.GetPlayerPosition();

        Vector2 goalVector = normalized;//new Vector2(playerPos.x, playerPos.y) - Simulator.Instance.getAgentPosition(sid);
        //if (RVOMath.absSq(goalVector) > 1.0f)
        //{
        //    goalVector = RVOMath.normalize(goalVector);
        //}

        _simulator.SetAgentPrefVelocity(sid, goalVector* speed*Time.deltaTime*4);

        /* Perturb a little to avoid deadlocks due to perfect symmetry. */
        //if (!isFirst)
        //    return;
        isFirst = false;
        float angle = (float) m_random.NextDouble()*2.0f*(float) Math.PI;
        float dist = (float) m_random.NextDouble()*0.0001f;

        _simulator.SetAgentPrefVelocity(sid, _simulator.GetAgentPrefVelocity(sid) +
                                                     dist*
                                                     new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)));
    }
}
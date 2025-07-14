using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixPoint;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace M.PathFinding
{

    [Serializable]
    public class UnitGridPos
    {
        public int X = 0;
        public int Y = 0;
    }
    public class BattleNodeInfoDebugger : MonoBehaviour
    {
        private Integer2 _gridPos = new Integer2(0, 0);
        private Unit _unit;
        [SerializeField]
        public UnitGridPos GridPos = new UnitGridPos();

        // [SerializeField] public string UnitStateName;
        [SerializeField] public EUnitState UnitState;
        [SerializeField] public int MoveToTargetId;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_unit == null)
            {
                return;
            }
            if (!Equals(_gridPos, _unit.GetGridPos()))
            {
                _gridPos = _unit.GetGridPos();
                GridPos.X = _gridPos.x;
                GridPos.Y = _gridPos.y;
            }
            UnitState = _unit.UnitState;
            if (UnitState == EUnitState.MoveToTarget)
            {
                var state = _unit.GetCurrentStateObject() as StateMoveToTarget;
                MoveToTargetId = state.GetTargetId();
            }
            else
            {
                MoveToTargetId = 0;
            }
            // var state = _unit.GetCurrentStateObject();
            // var name = state.GetType().Name;
            // if (!string.Equals(name, UnitStateName, comparisonType: StringComparison.Ordinal))
            //     UnitStateName = name;
        }

        public void SetUnit(Unit u)
        {
            _unit = u;
            _gridPos = _unit.GetGridPos();
        }
    }
}

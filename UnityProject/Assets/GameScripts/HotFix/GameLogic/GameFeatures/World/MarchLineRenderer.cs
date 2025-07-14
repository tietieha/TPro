using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace World
{
[LuaCallCSharp]
    public class MarchLineRenderer:MonoBehaviour
    {
        public List<Material> Materials;
        public LineRenderer lr;

        public LayerMask layerMask = new LayerMask();

        private int currentIndex = 0;

        private GameObject _target;
        private Vector3 _lastStartPos;
        private Vector3[] _lineList = new Vector3[2];

        public void SetLineList(Vector3 startPos, Vector3 endPos, GameObject target, int type = -1)
        {
            SetMaterial(type);
            _lastStartPos = startPos;
            lr.positionCount = 2;
            _lineList[1] = startPos;
            _lineList[0] = endPos;
            lr.SetPositions(_lineList);
            SetTarget(target);
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
            if (_target != null)
            {
                _lastStartPos = _target.transform.position;
            }
        }

        public void Update()
        {
        //     if (_target == null || lr == null)
        //         return;
        //     // if (lr.GetPositions())
        //     var targetPos = _target.transform.position;
        //     if (Mathf.Approximately(targetPos.x, _lastStartPos.x) && Mathf.Approximately(targetPos.z, _lastStartPos.z))
        //         return;
        //     _lastStartPos = targetPos;
        //     _lineList[1] = _lastStartPos;
        //     lr.SetPositions(_lineList);
        }

        public void SetMaterial(int index)
        {
            if (index >= 0 && index < Materials.Count)
            {
                lr.material = Materials[index];
            }
        }
    }
}
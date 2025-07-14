using System.Collections.Generic;
using FixPoint;
using Sirenix.OdinInspector;
using XLua;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace M.PathFinding
{
    [LuaCallCSharp]
    public class BattleMapDebugger : MonoBehaviour, IBattleMapDebugger
    {
        private static BattleMapDebugger _instance = null;

        public static BattleMapDebugger GetInstance()
        {
            return _instance;
        }

        private BattleMap _battleMap = null;
        public GameObject _groundCube = null;
        public GameObject _allNode = null;
        public GameObject _virtualHero = null;
        private Transform _virtualSoldiersRoot = null;
        [BlackListAttribute] public GameObject _nodeCubeTemplate = null;
        [BlackListAttribute] public GameObject _virtualHeroTemplate = null;

        private Dictionary<int, GameObject> _nodeCubeDict = new Dictionary<int, GameObject>();

        private Dictionary<int, GameObject> _entityCubeDict = new Dictionary<int, GameObject>();

        public List<int> _currentDebugUnit = new List<int>();

        private Transform _debugRoot = null;

        //当前正在监测的障碍物层
        [OnValueChanged("ChangeType")] public EUnitType _currentUnitType = EUnitType.All;

        [LuaCallCSharp]
        public void Enable(BattleMap map)
        {
            _debugRoot = gameObject.transform.Find("_DebugRoot");
            _virtualSoldiersRoot = _debugRoot.Find("_virtualSoldiersRoot");
            _instance = this;
            RegisterBattleMapDebugger(map);
        }

        public static void Release()
        {
            if (_instance)
            {
                _instance.transform.parent = null;
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        public void ClearAll()
        {
            _nodeCubeDict.Clear();
            _entityCubeDict.Clear();
            //_allNode.transform.DetachChildren();
            List<GameObject> allNodeGo = new List<GameObject>();
            foreach (Transform item in _allNode.transform)
                allNodeGo.Add(item.gameObject);
            foreach (var go in allNodeGo)
            {
                go.transform.parent = null;
                Destroy(go);
            }

            ChangeType();
        }

        public void RegisterBattleMapDebugger(BattleMap map)
        {
            if (_nodeCubeTemplate == null)
            {
                _nodeCubeTemplate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _nodeCubeTemplate.transform.SetParent(_debugRoot);
                _nodeCubeTemplate.transform.localScale = new Vector3(0.1f, 2f, 0.1f);
            }

            if (_virtualHeroTemplate == null)
            {
                _virtualHeroTemplate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _virtualHeroTemplate.transform.SetParent(_debugRoot);
                _virtualHeroTemplate.transform.localScale = new Vector3(0.1f, 2f, 0.1f);
                _virtualHeroTemplate.GetComponent<Renderer>().material.color = Color.yellow;
            }

            Renderer render = _virtualHeroTemplate.GetComponent<Renderer>();
            //调整alpha
            render.material.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
            map.SetDebugger(this);
            _battleMap = map;

            float xOrigin = (float)(_battleMap.GetOrigiPos().x * FixInt2.InverseScale);
            float zOrigin = (float)(_battleMap.GetOrigiPos().y * FixInt2.InverseScale);
            float width = (float)(_battleMap.GetWidth() * _battleMap.GetGridSize() * FixInt2.InverseScale);
            float height = (float)(_battleMap.GetHeight() * _battleMap.GetGridSize() * FixInt2.InverseScale);
            ;
            Vector3 offset = transform.position;
            _groundCube.transform.position =
                offset + new Vector3(xOrigin + width * 0.5f, 0.01f, zOrigin + height * 0.5f);
            _groundCube.transform.localScale = new Vector3(width, 0.2f, height);

            ClearAll();
        }

        //对Grid位置做hash
        private int HashNodeByPos(int x, int y)
        {
            return x * 10000 + y;
        }

        [Button("修改调试单位列表")]
        public void ChangeDebugUnit()
        {
            Utils.SetLogUnitList(_currentDebugUnit);
        }

        public void ChangeType()
        {
            var array = _battleMap.GetNode2D();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    var node = array[i, j];
                    UpdateOneNode(node);
                }
            }
        }

        private void _updateVirtualHero(Unit u)
        {
            if (!u.IsVirtual())
                return;
            if (u.IsDead())
            {
                if (_entityCubeDict.ContainsKey(u.GetId()))
                {
                    var go = _entityCubeDict[u.GetId()];
                    go.SetActive(false);
                }

                return;
            }

            var unitId = u.GetId();
            GameObject heroNodeGo;
            if (!_entityCubeDict.ContainsKey(unitId))
            {
                float gridSize = (float)(_battleMap.GetGridSize() * FixInt2.InverseScale);
                heroNodeGo = Instantiate(_virtualHeroTemplate, _virtualHero.transform);
                heroNodeGo.name = "vh_" + unitId + "_" + u.GetTeamId();
                heroNodeGo.transform.localScale = new Vector3(gridSize * 0.8f, 0.3f, gridSize * 0.8f);
                _entityCubeDict.Add(unitId, heroNodeGo);
            }
            else
            {
                heroNodeGo = _entityCubeDict[unitId];
            }

            heroNodeGo.transform.SetLocalPositionEx((float)(u.WorldPos.x * FixInt2.InverseScale), 0.5f,
                (float)(u.WorldPos.y * FixInt2.InverseScale));
            DrawHollowRectangle(heroNodeGo.transform.position,
                (float)(u.GetGreenCircleRadius().Value * FixInt2.InverseScale), u.GetSide(), _drawLineTime);
        }

        public void DrawHollowRectangle(Vector3 center, float radius, int side, float showTime)
        {
            Vector3 size = new Vector3(radius * 2, 0.1f, radius * 2);
            Vector3 halfSize = size * 0.5f;

            Vector3 topLeft = center + new Vector3(-halfSize.x, 0, halfSize.z);
            Vector3 topRight = center + new Vector3(halfSize.x, 0, halfSize.z);
            Vector3 bottomLeft = center + new Vector3(-halfSize.x, 0, -halfSize.z);
            Vector3 bottomRight = center + new Vector3(halfSize.x, 0, -halfSize.z);

            Color color = side == 1 ? Color.green : Color.blue;

            Debug.DrawLine(topLeft, topRight, color, showTime);
            Debug.DrawLine(topRight, bottomRight, color, showTime);
            Debug.DrawLine(bottomRight, bottomLeft, color, showTime);
            Debug.DrawLine(bottomLeft, topLeft, color, showTime);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateVirtualHero()
        {
            foreach (var item in _battleMap.GetUnitDict())
            {
                var u = item.Value;
                if (u.IsVirtual())
                    _updateVirtualHero(u);
                else
                {
                    _updateSoldier(u);
                }
            }
        }

        private void _updateSoldier(Unit u)
        {
            if (u.IsDead())
            {
                if (_entityCubeDict.ContainsKey(u.GetId()))
                {
                    var go = _entityCubeDict[u.GetId()];
                    go.SetActive(false);
                }

                return;
            }

            var unitId = u.GetId();
            GameObject soldierNode;
            if (!_entityCubeDict.ContainsKey(unitId))
            {
                float gridSize = (float)(_battleMap.GetGridSize() * FixInt2.InverseScale);
                soldierNode = Instantiate(_nodeCubeTemplate, _virtualSoldiersRoot);
                soldierNode.name = u.GetTeamId() + "_" + unitId;
                soldierNode.transform.localScale = new Vector3(gridSize, 0.3f, gridSize);
                Renderer render = soldierNode.GetComponent<Renderer>();
                var baseColor = u.GetSide() == 1 ? Color.green : Color.blue;
                //调整alpha
                render.material.color = baseColor;
                _entityCubeDict.Add(unitId, soldierNode);
                var nodeInfoDebugger = soldierNode.GetComponent<BattleNodeInfoDebugger>();
                if (nodeInfoDebugger == null)
                    nodeInfoDebugger = soldierNode.AddComponent<BattleNodeInfoDebugger>();
                nodeInfoDebugger.SetUnit(u);
            }
            else
            {
                soldierNode = _entityCubeDict[unitId];
            }
            soldierNode.transform.SetLocalPositionEx((float)(u.WorldPos.x * FixInt2.InverseScale), 0.5f, (float)(u.WorldPos.y * FixInt2.InverseScale));
            DrawUnitToTargetLine(u);
            if (IsGameObjectSelected(soldierNode))
            {
                var path = u.PathState.GetCurrentPath();
                DrawPathLine(u, path);
            }
        }

        readonly float _drawLineTime = 0.1f;
        private void DrawUnitToTargetLine(Unit u)
        {
            var uPos = u.GetWorldPos();
            var willAtkTarget = u.WillAttackTarget;
            Vector3 offset = transform.position;

            if (willAtkTarget != null)
            {
                var baseColor = u.GetSide() == 1 ? Color.green : Color.blue;
                var ePos = willAtkTarget.WorldPos;
                float yOff = u.GetSide() == 1 ? 0.1f : 0.2f;
                Debug.DrawLine(
                    offset + new Vector3((float)(uPos.x * FixInt2.InverseScale), yOff, (float)(uPos.y * FixInt2.InverseScale)),
                    offset + new Vector3((float)(ePos.x * FixInt2.InverseScale), yOff, (float)(ePos.y * FixInt2.InverseScale)),
                    baseColor, _drawLineTime);
            }

            if (u.GetCurrentStateObject() is StateMoveToTarget state)
            {
                var targetId = state.GetTargetId();
                if (targetId == 0)
                    return;
                var target = u.GetMap().GetUnit(targetId);
                float yOff = u.GetSide() == 1 ? 0.3f : 0.4f;
                if (target != null)
                {
                    var baseColor = u.GetSide() == 1 ? Color.red : Color.yellow;
                    var tPos = target.WorldPos;
                    Debug.DrawLine(
                        offset + new Vector3((float)(uPos.x * FixInt2.InverseScale), yOff, (float)(uPos.y * FixInt2.InverseScale)),
                        offset + new Vector3((float)(tPos.x * FixInt2.InverseScale), yOff, (float)(tPos.y * FixInt2.InverseScale)),
                        baseColor, _drawLineTime);
                }
            }

        }
        private void DrawPathLine(Unit u, Queue<FixInt2> path)
        {
            if (path.Count <= 1)
                return;
            Vector3 offset = transform.position;
            var color = u.GetSide() == 1 ? Color.green : Color.blue;
            var uPos = u.GetWorldPos();
            //绘制路径
            FixInt2 previousPoint = path.Peek(); // Get the first point without removing it
            Debug.DrawLine(
                offset + new Vector3((float)(uPos.x * FixInt2.InverseScale), 0.3f, (float)(uPos.y * FixInt2.InverseScale)),
                offset + new Vector3((float)(previousPoint.x * FixInt2.InverseScale), 0.3f, (float)(previousPoint.y * FixInt2.InverseScale)),
                color, _drawLineTime);
            foreach (var currentPoint in path)
            {
                if (!currentPoint.Equals(previousPoint))
                {
                    Debug.DrawLine(
                        offset + new Vector3((float)(previousPoint.x * FixInt2.InverseScale), 0.3f, (float)(previousPoint.y * FixInt2.InverseScale)),
                        offset + new Vector3((float)(currentPoint.x * FixInt2.InverseScale), 0.3f, (float)(currentPoint.y * FixInt2.InverseScale)),
                        color, _drawLineTime);
                    previousPoint = currentPoint;
                }
            }
        }

        private bool IsGameObjectSelected(GameObject go)
        {
#if UNITY_EDITOR
            return Selection.activeGameObject == go;
#else
            return false;
#endif
        }

        private void UpdateOneNode(Node node)
        {
            float gridSize = (float)(_battleMap.GetGridSize() * FixInt2.InverseScale);
            var hashValue = HashNodeByPos(node.GetX(), node.GetY());
            //如果不该显示
            if (node._occupyLayerForUnit[(int)_currentUnitType] <= 0 && _nodeCubeDict.ContainsKey(hashValue))
            {
                var go = _nodeCubeDict[hashValue];
                go.SetActive(false);
            }
            if (node._occupyLayerForUnit[(int)_currentUnitType] > 0)
            {
                GameObject go;
                if (!_nodeCubeDict.ContainsKey(hashValue))
                {
                    go = Instantiate(_nodeCubeTemplate, _allNode.transform);
                    var p = node.GetCenterWorldPos();
                    go.transform.SetLocalPositionEx((float)(p.x * FixInt2.InverseScale), 0.2f, (float)(p.y * FixInt2.InverseScale));
                    go.transform.localScale = new Vector3(gridSize * 0.8f, 0.2f, gridSize * 0.8f);

                    _nodeCubeDict.Add(hashValue, go);
                }
                else
                {
                    go = _nodeCubeDict[hashValue];
                    go.SetActive(true);
                }
            }
        }

        //更新Node数据
        public void UpdateNode(HashSet<Node> nodeSet)
        {
            foreach (var node in nodeSet)
            {
                UpdateOneNode(node);
            }
        }
        public void GeneratePath(int unitId, Queue<FixInt2> path)
        {
            if (path.Count <= 1)
                return;
            Vector3 offset = transform.position;
            var color = Color.red;//GetColorForUnitId(unitId);
            //绘制路径
            FixInt2 previousPoint = path.Peek(); // Get the first point without removing it

            foreach (var currentPoint in path)
            {
                if (!currentPoint.Equals(previousPoint))
                {
                    Debug.DrawLine(
                        offset + new Vector3((float)(previousPoint.x * FixInt2.InverseScale), 0.3f, (float)(previousPoint.y * FixInt2.InverseScale)),
                        offset + new Vector3((float)(currentPoint.x * FixInt2.InverseScale), 0.3f, (float)(currentPoint.y * FixInt2.InverseScale)),
                        color);
                    previousPoint = currentPoint;
                }
            }
        }
        private Color GetColorForUnitId(int unitId)
        {
            // Define a method to get a color based on unitId
            // This can be a simple hash function or a predefined dictionary
            return new Color((unitId * 123) % 256 / 255f, (unitId * 456) % 256 / 255f, (unitId * 789) % 256 / 255f);
        }

        public void RemovePath(int unitId)
        {

        }

        public void Awake()
        {
        }

        public void Update()
        {
        }

    }

}
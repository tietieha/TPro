#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
    [ExecuteInEditMode] // 确保脚本在编辑模式下也执行
    public class MapEditorInteractionObject : MonoBehaviour
    {
        public int index;
        public string path;
        public int eventId;

        private Vector3 lastPosition;
        private Vector3 lastRotation;
        private Vector3 lastScale;
        private bool isDragging = false; // 是否正在拖拽
        private Vector3 startPosition; // 拖拽开始时的位置
        private Vector3 startRotation; // 拖拽开始时的旋转
        private Vector3 startScale; // 拖拽开始时的缩放

        private const float rotationThreshold = 1f; // 旋转变化阈值（角度）
        private const float positionThreshold = 0.1f; // 位置变化阈值（单位：米）

        void OnEnable()
        {
            // 监听 Scene 视图的绘制事件
            SceneView.duringSceneGui += OnSceneGUI;
        }

        void OnDisable()
        {
            // 停止监听 Scene 视图事件
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            // 如果正在拖拽，跳过更新
            if (Selection.activeGameObject != gameObject)
            {
                isDragging = false;
                return; // 如果当前选中的物体不是该物体，则跳过
            }

            // 检测 Scene 中的交互事件（比如拖动物体）
            HandleInput();
        }

        void HandleInput()
        {
            // 只有在鼠标按下或触摸时才能开始拖拽
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                isDragging = true;
                startPosition = transform.position;
                startRotation = transform.rotation.eulerAngles;
                startScale = transform.localScale;
            }

            // 鼠标放开时触发
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                if (isDragging)
                {

                    lastPosition = transform.position;
                    lastRotation = transform.rotation.eulerAngles;
                    lastScale = transform.localScale;
                    var param = new MapEditorEvent.InteractionUpdateParam {
                        Index = index,
                        StartPosition = startPosition
                    };
                    MapEditorEventCenter.SendEvent(MapEditorEvent.InteractionUpdateEvent, param);
                    isDragging = false;
                }
            }
        }
    }

    public class MapEditorInteraction : MonoBehaviour
    {
        public MeshFilter _mesh;

        private readonly Dictionary<int, GameObject> _interactionObjects = new();
        // private Dictionary<int, Vector2Int> _oldInteractionPositions = new Dictionary<int, Vector2Int>();
        private void OnEnable()
        {
            RegisterEvents(true);
        }

        private void OnDisable()
        {
            RegisterEvents(false);
        }

        private void RegisterEvents(bool register)
        {
            if (register)
            {
                MapEditorEventCenter.AddListener(MapEditorEvent.InteractionAddEvent,
                    OnInteractionAdd);
                MapEditorEventCenter.AddListener(MapEditorEvent.InteractionRemoveEvent,
                    OnInteractionRemove);
                // MapEditorEventCenter.AddListener(MapEditorEvent.InteractionClearEvent, OnInteractionClear);
                MapEditorEventCenter.AddListener(MapEditorEvent.InteractionReloadEvent,
                    OnInteractionReload);
                MapEditorEventCenter.AddListener(MapEditorEvent.InteractionSyncEvent,
                    OnInteractionSync);
                MapEditorEventCenter.AddListener(MapEditorEvent.InteractionUpdateEvent,
                    OnInteractionUpdate);
                MapEditorEventCenter.AddListener(MapEditorEvent.InteractionEditorUpdateEvent,
                    OnInteractionEditorUpdate);
                MapEditorEventCenter.AddListener(MapEditorEvent.InteractionFocusEvent,
                    OnInteractionFocusUpdate);
            }
            else
            {
                MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionAddEvent,
                    OnInteractionAdd);
                MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionRemoveEvent,
                    OnInteractionRemove);
                // MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionClearEvent, OnInteractionClear);
                MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionReloadEvent,
                    OnInteractionReload);
                MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionSyncEvent,
                    OnInteractionSync);
                MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionUpdateEvent,
                    OnInteractionUpdate);
                MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionEditorUpdateEvent,
                    OnInteractionEditorUpdate);
                MapEditorEventCenter.RemoveListener(MapEditorEvent.InteractionFocusEvent,
                    OnInteractionFocusUpdate);
            }
        }

        private void OnInteractionRemove(int eventId)
        {
            // if (!ValidateInteractionIndex(index, out var interactionInfo)) return;
            var interactionInfo = MapRender.instance.CurrentMapInteractionPoints.Find(info => info.id == eventId);
            if (interactionInfo == null)
                return;
            var interactionIndex = interactionInfo.index;

            if (_interactionObjects.ContainsKey(interactionIndex))
            {
                Destroy(_interactionObjects[interactionIndex]);
                _interactionObjects.Remove(interactionIndex);
            }
        }

        private void OnInteractionAdd(int eventId)
        {
            // if (!ValidateInteractionIndex(index, out var interactionInfo)) return;
            var interactionInfo = MapRender.instance.CurrentMapInteractionPoints.Find(info => info.id == eventId);
            if (interactionInfo == null)
                return;
            var interactionIndex = interactionInfo.index;
            if (_interactionObjects.ContainsKey(interactionIndex))
                Destroy(_interactionObjects[interactionIndex]);

            var interactionObject = InstantiateInteractionObject(interactionInfo);
            if (interactionObject != null)
                _interactionObjects[interactionIndex] = interactionObject;
            // _oldInteractionPositions[interactionInfo.id] = interactionInfo.position;
        }

        private void OnInteractionClear()
        {
            Dictionary<string, GameObject> interactionInfos = new Dictionary<string, GameObject>();
            var childTransforms = transform.GetComponentsInChildren<Transform>(true);
            foreach (var child in childTransforms)
            {
                if (child.name.StartsWith("itc_"))
                {

                    interactionInfos.Add(child.name, child.gameObject);
                }
            }

            foreach (var interactionInfo in interactionInfos)
            {
                Destroy(interactionInfo.Value);
            }
            interactionInfos.Clear();
            _interactionObjects.Clear();
        }

        private void OnInteractionReload()
        {
            OnInteractionClear();
            var interactionInfos = MapRender.instance.CurrentMapInteractionPoints;
            if (interactionInfos == null)
            {
                // Debug.LogError("Interaction points are not initialized for current map");
                return;
            }

            foreach (var interactionPoint in interactionInfos)
            {
                // TODO LZL use AddInteractionPoint instead
                var interactionIndex = interactionPoint.index;
                if (_interactionObjects.ContainsKey(interactionIndex))
                    Destroy(_interactionObjects[interactionIndex]);

                var interactionObject = InstantiateInteractionObject(interactionPoint);
                if (interactionObject != null)
                    _interactionObjects[interactionIndex] = interactionObject;
            }
        }

        private void OnInteractionSync()
        {
            var backupPointInfos = MapRender.instance.ClearCurrentMapInteractionPoints();
            var interactionInfos = MapRender.instance.CurrentMapInteractionPoints;
            var mapId = MapRender.instance.CurrentMapId;
            _interactionObjects.Clear();
            var childTransforms = transform.GetComponentsInChildren<Transform>(true);
            foreach (var child in childTransforms)
                if (child.name.StartsWith("itc_"))
                {

                    var interactionObject = child.gameObject;
                    var interactionObjectComponent = interactionObject.GetComponent<MapEditorInteractionObject>();
                    int index = MapRender.instance.InteractionIndex;
                    if (interactionObjectComponent == null)
                    {
                        interactionObjectComponent = interactionObject.AddComponent<MapEditorInteractionObject>();
                    }
                    interactionObjectComponent.index = index;
                    // var eventId = interactionObjectComponent.eventId;
                    var interactionInfo = new InteractionPoint
                    {
                        index = index,
                        id = int.TryParse(child.name.Split('_')[2], out var id) ? id : 0,
                        type =
                            InteractionTypeExtensions.ToNumericValue(child.name.Split('_')[1]),
                        position = Hex.WorldToOffset(interactionObject.transform.position),
                        rotation = interactionObject.transform.rotation.eulerAngles,
                        prefab = interactionObject,
                        prefabPath = FileHelper.GetPrefabAssetPath(interactionObject, true),
                        scale = interactionObject.transform.localScale,
                        comments = backupPointInfos.ContainsKey(id)
                            ? backupPointInfos[id].Comments
                            : string.Empty,
                        BlockPoints = backupPointInfos.ContainsKey(id)
                            ? backupPointInfos[id].BlockPoints
                            : new List<Vector2Int>(),
                        mapId = mapId
                    };
                    interactionObjectComponent.eventId = interactionInfo.id;

                    if (string.IsNullOrEmpty(interactionInfo.prefabPath))
                    {
                        var interactionObjectScript = child.gameObject.GetComponent<MapEditorInteractionObject>();
                        if (interactionObjectScript != null)
                        {
                            interactionInfo.prefabPath = interactionObjectScript.path;
                        }
                    }
                    interactionInfo.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(interactionInfo.prefabPath);
                    interactionInfos.Add(interactionInfo);

                    interactionObjectComponent.path = interactionInfo.prefabPath;
                    _interactionObjects[interactionInfo.index] = interactionObject;
                }
        }

        private void OnInteractionUpdate(MapEditorEvent.InteractionUpdateParam param)
        {
            var index = param.Index;
            var interactionInfos = MapRender.instance.CurrentMapInteractionPoints;
            if (interactionInfos == null)
            {
                Debug.LogError("Interaction points are not initialized");
                return;
            }

            // 查找InteractionPoints中的每个一个Index 与传入的Index相同的InteractionPoint
            var interactionInfo = interactionInfos.Find(info => info.index == index);
            if (interactionInfo == null)
            {
                // Debug.LogError($"Interaction info is null at index {index}");
                return;
            }

            if (!_interactionObjects.TryGetValue(interactionInfo.index, out var interactionObject))
            {
                // Debug.LogError($"Interaction object is not found at index {index}");
                return;
            }

            var objPosition = interactionObject.transform.position;
            var offset = Hex.WorldToOffset(interactionObject.transform.position);
            if (offset.x != interactionInfo.position.x || offset.y != interactionInfo.position.y)
            {
                interactionInfo.position = offset;
                objPosition = GetInteractionPosition(Hex.OffsetToWorld(interactionInfo.position));
                var offsetStart = Hex.WorldToOffset(param.StartPosition);
                var starPos = GetInteractionPosition(Hex.OffsetToWorld(offsetStart));
                var deltaPos = objPosition - starPos;
                if (deltaPos != Vector3.zero && interactionInfo.BlockPoints != null)
                {
                    for (int i = 0; i < interactionInfo.BlockPoints.Count; i++)
                    {
                        Vector3 worldPos = Hex.OffsetToWorld(interactionInfo.BlockPoints[i]);
                        Vector3 newWorldPos = worldPos + deltaPos;
                        Vector2Int newHex = Hex.WorldToOffset(newWorldPos);
                        interactionInfo.BlockPoints[i] = newHex;
                    }
                }
            }

            interactionObject.transform.position = objPosition;
            interactionInfo.rotation = interactionObject.transform.rotation.eulerAngles;
            interactionInfo.scale = interactionObject.transform.localScale;
        }

        private void OnInteractionEditorUpdate(InteractionPoint interactionInfo)
        {
            // var interactionInfos = MapRender.instance.CurrentMapInteractionPoints;
            // if (interactionInfos == null)
            // {
            //     Debug.LogError("Interaction points are not initialized");
            //     return;
            // }
            //
            // var interactionInfo = interactionInfos[index];
            if (interactionInfo == null)
            {
                // Debug.LogError($"Interaction info is null at index {index}");
                return;
            }

            if (!_interactionObjects.TryGetValue(interactionInfo.index, out var interactionObject))
            {
                // Debug.LogError($"Interaction object is not found at index {index}");
                return;
            }

            interactionObject.transform.position =
                GetInteractionPosition(Hex.OffsetToWorld(interactionInfo.position));
            interactionObject.transform.rotation = Quaternion.Euler(interactionInfo.rotation);
            interactionObject.transform.localScale = interactionInfo.scale;
            interactionObject.name = interactionInfo.id != 0
                ? $"itc_{interactionInfo.type}_{interactionInfo.id}"
                : $"itc_{interactionInfo.type}_{interactionInfo.index}";
            if (interactionObject.GetComponent<MapEditorInteractionObject>() == null)
            {
                var interactionObjectComponent = interactionObject.gameObject
                    .AddComponent<MapEditorInteractionObject>();
                interactionObjectComponent.index = interactionInfo.index;
                interactionObjectComponent.path = interactionInfo.prefabPath;
            }
        }

        private bool ValidateInteractionIndex(int index, out InteractionPoint interactionInfo)
        {
            interactionInfo = null;
            var interactionInfos = MapRender.instance.interactionPoints;
            if (interactionInfos == null)
            {
                Debug.LogError("Interaction points are not initialized");
                return false;
            }

            if (index < 0 || index >= interactionInfos.Count)
            {
                Debug.LogError($"Invalid interaction index {index}");
                return false;
            }
            // int idx = Array.FindIndex(interactionInfos.ToArray(), x => x.index == index);
            // if (idx < 0)
            // {
            //     Debug.LogError($"Invalid interaction index {index}");
            //     return false;
            // }

            interactionInfo = interactionInfos[index];
            if (interactionInfo == null || (interactionInfo.prefab == null &&
                                            string.IsNullOrEmpty(interactionInfo.prefabPath)))
            {
                Debug.LogError($"Interaction info is null at index {index}");
                return false;
            }

            return true;
        }

        private GameObject InstantiateInteractionObject(InteractionPoint interactionInfo)
        {
            GameObject interactionObject = null;
            if (interactionInfo.prefab != null)
            {
                interactionObject = Instantiate(interactionInfo.prefab, transform);
                interactionInfo.prefabPath = FileHelper.GetPrefabAssetPath(interactionInfo.prefab, true);
            }
            else if (!string.IsNullOrEmpty(interactionInfo.prefabPath))
            {
                interactionObject =
                    Instantiate(
                        AssetDatabase.LoadAssetAtPath<GameObject>(interactionInfo.prefabPath),
                        transform);
                interactionInfo.prefab =
                    AssetDatabase.LoadAssetAtPath<GameObject>(interactionInfo.prefabPath);
            }

            if (interactionObject != null)
            {
                interactionObject.transform.position =
                    GetInteractionPosition(Hex.OffsetToWorld(interactionInfo.position));
                interactionObject.transform.rotation = Quaternion.Euler(interactionInfo.rotation);
                interactionObject.transform.localScale = interactionInfo.scale;
                interactionObject.name = interactionInfo.id != 0
                    ? $"itc_{interactionInfo.type}_{interactionInfo.id}"
                    : $"itc_{interactionInfo.type}_{interactionInfo.index}";
                if (interactionObject.GetComponent<MapEditorInteractionObject>() == null)
                {
                    var interactionObjectComponent = interactionObject.gameObject
                        .AddComponent<MapEditorInteractionObject>();
                    interactionObjectComponent.index = interactionInfo.index;
                    interactionObjectComponent.eventId = interactionInfo.id;
                    interactionObjectComponent.path = interactionInfo.prefabPath;
                }
            }

            return interactionObject;
        }

        public Vector3 GetInteractionPosition(Vector3 position)
        {
            RaycastHit hit;

            // 构建射线，从目标的 X 坐标发射
            Vector3 rayOrigin = new Vector3(position.x, 100f, position.z);

            // 发射射线向下查找
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity,
                    LayerMask.GetMask("ObjRaycast")))
            {
                // 获取碰撞点的世界坐标 y 值
                Debug.Log("Hit height at x = " + position.x + " is: " + hit.point.y);
                rayOrigin.y = hit.point.y + 0.01f;
            }
            else
            {
                Debug.Log("No hit at x = " + position.x);
                rayOrigin.y = 0;
            }

            return rayOrigin;
        }

        public void OnInteractionFocusUpdate(int eventId)
        {
            // if (!ValidateInteractionIndex(eventId, out var interactionInfo)) return;
            var interactionInfo = MapRender.instance.CurrentMapInteractionPoints.Find(info => info.id == eventId);
            var interactionIndex = interactionInfo.index;
            if (_interactionObjects.ContainsKey(interactionIndex))
            {
                var interactionObject = _interactionObjects[interactionIndex];
                if (interactionObject != null)
                {
                    Selection.activeGameObject = interactionObject;
                    SceneView.lastActiveSceneView.FrameSelected();
                    SceneView.RepaintAll();
                }
            }
        }
    }
}
#endif
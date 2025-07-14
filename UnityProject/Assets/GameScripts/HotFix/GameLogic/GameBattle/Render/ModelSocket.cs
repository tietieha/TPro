using System.Collections.Generic;
using UnityEngine;
using TEngine;
using XLua;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

namespace M.Battle.Render
{
    [LuaCallCSharp]
    [ExecuteAlways]
    [System.Serializable]
    public class SocketInfo
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [ReadOnly]
        [HideLabel]
#endif
        [SerializeField]
        private string SocketName;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HideLabel]
#endif
        [SerializeField]
        string ParentName;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HideLabel]
#endif
        [SerializeField]
        Transform socket;

        public SocketInfo(string socketName, string parentName, Transform socket)
        {
            SocketName = socketName;
            ParentName = parentName;
            this.socket = socket;
        }

        public Transform GetSocket()
        {
            return socket;
        }

        public string GetSocketName()
        {
            return SocketName;
        }

        public string GetParentName()
        {
            return ParentName;
        }

        public void SetSocket(Transform socketTrans)
        {
            socket = socketTrans;
        }
    }

    public class ModelSocket : MonoBehaviour
    {
        public static List<Vector3[]> positions = new List<Vector3[]>();

#if ODIN_INSPECTOR && UNITY_EDITOR
        [ListDrawerSettings(OnBeginListElementGUI = "BeginDrawElement", OnEndListElementGUI = "EndDrawElement",
            DraggableItems = false, HideRemoveButton = true, HideAddButton = true)]
#endif
        [SerializeField]
        private List<SocketInfo> socketInfos = new List<SocketInfo>()
        {
            new SocketInfo("Root", "root_s", null),
            new SocketInfo("Chest", "Bip001 Spine1", null),
            new SocketInfo("Head", "Bip001 Head", null),
            new SocketInfo("Hand_L", "Bip001 L Hand", null),
            new SocketInfo("Hand_R", "Bip001 R Hand", null),
            new SocketInfo("Weapon01", "", null),
            new SocketInfo("Weapon02", "", null),
        };


        public Transform GetSocketByName(string key)
        {
            foreach (var socketInfo in socketInfos)
            {
                if (socketInfo.GetSocketName() == key)
                {
                    return socketInfo.GetSocket();
                }
            }

            return this.transform;
        }


#if UNITY_EDITOR
        private void Awake()
        {
            if (Application.isPlaying)
            {
                return;
            }

            CreateAndGetSockets();
        }

        [Button("创建挂点且获取引用")]
        public void CreateAndGetSockets()
        {
            bool isDirty = false;

            for (int i = 0; i < socketInfos.Count; i++)
            {
                var socketInfo = socketInfos[i];
                string socketParentName = socketInfo.GetParentName();
                string socketName = socketInfo.GetSocketName();
                if (this.transform.FindChildByName(socketName))
                {
                    socketInfo.SetSocket(this.transform.FindChildByName(socketName));
                    isDirty = true;
                    continue;
                }

                if (socketParentName == "")
                {
                    continue;
                }

                var child = this.transform.FindChildByName(socketParentName);
                if (child == null)
                {
                    Debug.LogError("没有找到路径" + socketParentName);
                    continue;
                }

                if (i == 0)
                {
                    var rootS = this.transform.FindChildByName(socketInfo.GetParentName());
                    child = rootS.parent;
                }

                var socket = new GameObject(socketName);
                socket.transform.SetParent(child);
                socket.transform.localPosition = Vector3.zero;
                socket.transform.localRotation = Quaternion.identity;
                socket.transform.localScale = Vector3.one;
                socketInfo.SetSocket(socket.transform);
                isDirty = true;
            }

            if (isDirty)
            {
                Save();
            }
        }


        [Button("复制数据")]
        private void Copy()
        {
            positions.Clear();
            foreach (var sockets in socketInfos)
            {
                if (sockets.GetSocket() == null)
                {
                    positions.Add(new Vector3[] { Vector3.zero, Vector3.zero, Vector3.one });
                }
                else
                {
                    var socket = sockets.GetSocket();
                    positions.Add(
                        new Vector3[] { socket.localPosition, socket.localEulerAngles, socket.localScale });
                }
            }
        }

        [Button("粘贴数据")]
        private void Paste()
        {
            if (positions.Count == 0) return;
            for (int i = 1; i < socketInfos.Count; i++)
            {
                var sockets = socketInfos[i];
                var socketTransform = sockets.GetSocket();
                if (socketTransform != null)
                {
                    var info = positions[i];
                    socketTransform.SetLocalPositionEx(info[0].x, info[0].y, info[0].z);
                    socketTransform.SetLocalRotationEx(info[1].x, info[1].y, info[1].z);
                    socketTransform.SetLocalScaleEx(info[2].x, info[2].y, info[2].z);
                }
            }

            Save();
        }

        private void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void BeginDrawElement(int index)
        {
            GUILayout.BeginHorizontal();
            if (index == 0)
            {
                GUI.enabled = false;
            }
        }

        void EndDrawElement(int index)
        {
            if (index == 0)
            {
                GUI.enabled = true;
            }

            GUILayout.EndHorizontal();
        }

        public List<Transform> GetAllSockets()
        {
            List<Transform> sockets = new List<Transform>();
            foreach (var socketInfo in socketInfos)
            {
                if (socketInfo.GetSocket() != null)
                {
                    sockets.Add(socketInfo.GetSocket());
                }
            }

            return sockets;
        }
#endif
    }
}
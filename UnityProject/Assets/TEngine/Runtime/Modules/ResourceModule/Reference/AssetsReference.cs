using System;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector;
#endif

namespace TEngine
{
    [Serializable]
    public struct AssetsRefInfo
    {
        public int instanceId;

        public Object refAsset;

        public AssetsRefInfo(Object refAsset)
        {
            this.refAsset = refAsset;
            instanceId = this.refAsset.GetInstanceID();
        }
    }

    public sealed class AssetsReference : MonoBehaviour
    {
        [SerializeField] private GameObject _sourceGameObject;

        [SerializeField] private List<AssetsRefInfo> _refAssetInfoList;

        private IResourceManager _resourceManager;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Space]
        [ShowInInspector]
        [ReadOnly]
        [InfoBox("true: 资源管理器管理\nfalse:自己管理", InfoMessageType.Info)]
#endif
        private bool _isRefAsset;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [Button("复制名字+GotoPool")]
        private void GotoPool()
        {
            if (_sourceGameObject != null)
            {
                if (GameModule.ObjectPool != null)
                {
                    Selection.activeObject = GameModule.ObjectPool.gameObject;
                    EditorGUIUtility.systemCopyBuffer = _sourceGameObject.name;
                }
            }
        }
#endif

        private void Awake()
        {
            _sourceGameObject = null;
            _refAssetInfoList = null;
            _resourceManager = null;
            _isRefAsset = false;
        }

        private void OnDestroy()
        {
            if (_resourceManager == null)
            {
                _resourceManager = ModuleImpSystem.GetModule<IResourceManager>();
            }

            if (_resourceManager == null)
            {
                throw new GameFrameworkException($"ResourceManager is null.");
            }

            if (!_isRefAsset)
            {
                return;
            }

            if (_sourceGameObject != null)
            {
                _resourceManager.UnloadAsset(_sourceGameObject);
            }

            if (_refAssetInfoList != null)
            {
                foreach (var refInfo in _refAssetInfoList)
                {
                    _resourceManager.UnloadAsset(refInfo.refAsset);
                }

                _refAssetInfoList.Clear();
            }
        }

        public AssetsReference Ref(GameObject source, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            _isRefAsset = true;
            _resourceManager = resourceManager;
            _sourceGameObject = source;
            return this;
        }

        public AssetsReference Ref<T>(T source, IResourceManager resourceManager = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            _resourceManager = resourceManager;
            _refAssetInfoList = new List<AssetsRefInfo>();
            _refAssetInfoList.Add(new AssetsRefInfo(source));
            return this;
        }

        public static AssetsReference Instantiate(GameObject source, Transform parent = null, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            GameObject instance = Object.Instantiate(source, parent);
            return instance.AddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref(GameObject source, GameObject instance, IResourceManager resourceManager = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }

        public static AssetsReference Ref<T>(T source, GameObject instance, IResourceManager resourceManager = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            return instance.GetOrAddComponent<AssetsReference>().Ref(source, resourceManager);
        }
    }
}
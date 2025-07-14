using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M.Battle.Pool;
using XLua;

namespace M.Battle
{
    [LuaCallCSharp]
    public class BattlePoolManager
    {
        private static BattlePoolManager s_Manager;
        private Dictionary<int, string> m_InstanceIdMap = new Dictionary<int, string>(10);

        private Dictionary<string, Dictionary<string, GameObjectPool>> m_allPoos =
            new Dictionary<string, Dictionary<string, GameObjectPool>>(10);

        private Dictionary<string, InstanceAsset> m_AllAssets = new Dictionary<string, InstanceAsset>(10);

        public static BattlePoolManager Instance
        {
            get
            {
                if (s_Manager == null)
                {
                    s_Manager = new BattlePoolManager();
                }

                return s_Manager;
            }
        }

        public static void ReleaseBattlePoolManager()
        {
            if (s_Manager != null)
            {
                s_Manager.Release();
                s_Manager = null;
            }
        }

        private const string AssetName = ".asset";
        private const string PrefabName = ".prefab";

        public void AddPoolItem(string poolName, string assetPath, Object loadObj, int prepareNum, System.Action cbk)
        {
            var go = (GameObject)loadObj;
            if (!m_allPoos.TryGetValue(poolName, out Dictionary<string, GameObjectPool> pools))
            {
                pools = new Dictionary<string, GameObjectPool>(10);
                m_allPoos.Add(poolName, pools);
            }

            if (!pools.TryGetValue(assetPath, out GameObjectPool pool))
            {
                pool = new GameObjectPool(go);
                pool.AssetPath = assetPath;
                pool.PrepareAsync(prepareNum, "BattlePooling", cbk);
                pools.Add(assetPath, pool);
            }
            else
            {
                cbk?.Invoke();
            }
        }

        public bool HasPool(string poolName, string assetPath)
        {
            if (m_allPoos.TryGetValue(poolName, out Dictionary<string, GameObjectPool> pools))
            {
                return pools.ContainsKey(assetPath);
            }

            return false;
        }

        public InstanceAsset GetInstanceAsset(string assetPath)
        {
            if (m_AllAssets.TryGetValue(assetPath, out InstanceAsset asset))
            {
                return asset;
            }

            return null;
        }

        public GameObject GetItemFromPool(string poolName, string assetPath)
        {
            if (assetPath == null)
                return null;
            if (m_allPoos.TryGetValue(poolName, out Dictionary<string, GameObjectPool> pools))
            {
                if (pools.TryGetValue(assetPath, out GameObjectPool pool))
                {
                    var go = pool.Get();
                    if (!m_InstanceIdMap.ContainsKey(go.GetInstanceID()))
                    {
                        m_InstanceIdMap.Add(go.GetInstanceID(), assetPath);
                    }

                    return go;
                }
            }

            return null;
        }

        public bool BackItemToPool(string poolName, GameObject go)
        {
            if (go == null)
                return false;
            if (m_allPoos.TryGetValue(poolName, out Dictionary<string, GameObjectPool> pools))
            {
                if (m_InstanceIdMap.TryGetValue(go.GetInstanceID(), out string assetPath))
                {
                    if (pools.TryGetValue(assetPath, out GameObjectPool pool))
                    {
                        pool.Put(go);
                        return true;
                    }
                }
            }

            return false;
        }

        public void ReleasePoolByPath(string poolName, string assetPath)
        {
            if (m_allPoos.TryGetValue(poolName, out Dictionary<string, GameObjectPool> pools))
            {
                if (pools.TryGetValue(assetPath, out GameObjectPool pool))
                {
                    pool.ReleasePool();
                    pools.Remove(assetPath);
                }
            }
        }

        public void ReleaseAllPoos(string poolName)
        {
            if (m_allPoos.TryGetValue(poolName, out Dictionary<string, GameObjectPool> pools))
            {
                foreach (var pool in pools)
                {
                    pool.Value.ReleasePool();
                }

                pools.Clear();
                m_allPoos.Remove(poolName);
            }
        }

        private void Release()
        {
            foreach (var pools in m_allPoos)
            {
                foreach (var pool in pools.Value)
                {
                    pool.Value.ReleasePool();
                }
                pools.Value.Clear();
            }

            m_allPoos.Clear();
            m_InstanceIdMap.Clear();
            m_AllAssets.Clear();
        }
    }
}
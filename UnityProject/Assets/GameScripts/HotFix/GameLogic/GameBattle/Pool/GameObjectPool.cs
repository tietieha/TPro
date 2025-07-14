/// <summary>
/// GameObjectPool.cs
/// Author: zhanghuayu@topjoy.com
/// Date:   2019-05-16 16:37:17
/// Desc:
/// </summary>

#define POOL_DEBUG

using System.Collections.Generic;
using UnityEngine;

namespace M.Battle.Pool
{

    public class GameObjectPool
    {
        private readonly Stack<GameObject> m_Stack;
        private readonly HashSet<GameObject> m_inPoolHashSet;
        private readonly System.Action<GameObject> m_ActionOnGet;
        private readonly System.Action<GameObject> m_ActionOnRelease;
        private readonly System.Action<GameObject> m_ActionOnNew;
        public System.Action<GameObject> internalActionOnNew;

        GameObject mPrefab;

        internal GameObject Prefab
        {
            get
            {
                return mPrefab;
            }

        }
        int mCountCreate;
        int mMaxActive;

        bool mUsePoolRoot;
        bool mWorldPositionStays;
        bool mActiveInPool = false;
        Transform mPoolRoot;

        public Transform PoolRoot
        {
            get
            {
                if (mUsePoolRoot)
                {
                    if (mPoolRoot == null)
                    {
                        var poolRootGo = new GameObject(mPrefab.name + "Pool");
                        // if (Application.isPlaying)
                        //     GameObject.DontDestroyOnLoad(poolRootGo);
                        mPoolRoot = poolRootGo.transform;
                    }
                    return mPoolRoot;
                }
                else
                    return null;
            }
        }

        public int CountActive { get { return mCountCreate - CountInactive; } }
        public int CountInactive { get { return m_Stack.Count; } }

        public int CountCreate
        {
            get
            {
                return mCountCreate;
            }
        }

        public string AssetPath;

        public GameObjectPool(GameObject prefab, System.Action<GameObject> actionOnNew = null, System.Action<GameObject> actionOnGet = null, System.Action<GameObject> actionOnRelease = null, bool usePoolRoot = true, bool activeInPool = false)
        {
            if (prefab == null)
                throw new System.ArgumentNullException("prefab", "Can not create a GameObjectInstPool with null prefab");

            this.mPrefab = prefab;
            this.mActiveInPool = activeInPool;

            m_Stack = new Stack<GameObject>(10);
            m_inPoolHashSet = new HashSet<GameObject>();

            m_ActionOnNew = actionOnNew;
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;

            mUsePoolRoot = usePoolRoot;

#if POOL_DEBUG
            // GameObjectPoolProfiler.Instance.AddPool(this);
#endif
        }

        public GameObjectPool(GameObject prefab, int capacity, System.Action<GameObject> actionOnNew = null, System.Action<GameObject> actionOnGet = null, System.Action<GameObject> actionOnRelease = null, bool usePoolRoot = true)
        {
            if (prefab == null)
                throw new System.ArgumentNullException("prefab", "Can not create a GameObjectInstPool with null prefab");

            this.mPrefab = prefab;

            m_Stack = new Stack<GameObject>(capacity);
            m_inPoolHashSet = new HashSet<GameObject>();

            m_ActionOnNew = actionOnNew;
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;

            mUsePoolRoot = usePoolRoot;

#if POOL_DEBUG
            // GameObjectPoolProfiler.Instance.AddPool(this);
#endif
        }

        /// <summary>
        /// 设置池子根结点到工作模式
        /// 为了避免特效因为绑点放大
        /// </summary>
        /// <param name="usePoolRoot">将所有池子对象返回时是否集中到统一到池子节点下</param>
        /// <param name="worldPositionStays">使用PoolRoot时是否保持世界坐标(position,rotation,scale)</param>
        public void SetPoolRootMode(bool usePoolRoot, bool worldPositionStays)
        {
            mUsePoolRoot = usePoolRoot;
            mWorldPositionStays = worldPositionStays;
        }

        GameObject CreateInstance(GameObject prefab)
        {
            var element = GameObject.Instantiate(prefab) as GameObject;
            mCountCreate++;

            InvokeCallbackSafe(m_ActionOnNew, element);
            InvokeCallbackSafe(internalActionOnNew, element);
            return element;
        }

        GameObject CreateInstanceByResourceModule()
        {
            var element = TEngine.GameModule.Resource.LoadGameObject(AssetPath);
            mCountCreate++;

            InvokeCallbackSafe(m_ActionOnNew, element);
            InvokeCallbackSafe(internalActionOnNew, element);
            return element;
        }

        public void Recycle(GameObject element)
        {
            mCountCreate++;

            InvokeCallbackSafe(m_ActionOnNew, element);
            InvokeCallbackSafe(internalActionOnNew, element);

            Put(element);
        }

        public GameObject Get(Vector3 worldPosition, Quaternion worldRotation, Transform parent, bool elementState = true)
        {
            GameObject element;
            if (m_Stack.Count == 0)
            {
                element = CreateInstance(mPrefab);
            }
            else
            {
                //实际上有一种可能,GameObject是可以被外部销毁的,所以这里可能获取的是null
                do
                {
                    element = m_Stack.Pop();
                    if (element == null)
                    {
                        mCountCreate--;
                    }
                    else
                        break;
                }
                while (m_Stack.Count > 0);

                //如果此时element仍旧是null
                if (element == null)
                    element = CreateInstance(mPrefab);
            }

            element.transform.position = worldPosition;
            element.transform.rotation = worldRotation;

            if (parent)
                element.transform.SetParent(parent);
            else if (element.transform.parent == mPoolRoot)
                element.transform.SetParent(null);

            element.SetActive(elementState);
            m_inPoolHashSet.Remove(element);

            InvokeCallbackSafe(m_ActionOnGet, element);
            UpdateMaxActive();
            // Debug.Log("Get ["+element+"]");
            return element;
        }
        public GameObject Get(bool elementState = true)
        {
            GameObject element;
            if (m_Stack.Count == 0)
            {
                element = CreateInstance(mPrefab);
                // element = CreateInstanceByResourceModule();
            }
            else
            {
                //实际上有一种可能,GameObject是可以被外部销毁的,所以这里可能获取的是null
                do
                {
                    element = m_Stack.Pop();
                    if (element == null)
                    {
                        mCountCreate--;
                    }
                    else
                        break;
                }
                while (m_Stack.Count > 0);

                //如果此时element仍旧是null
                if (element == null)
                    element = CreateInstance(mPrefab);
            }

            if (element.transform.parent == mPoolRoot)
                element.transform.SetParent(null);

            element.SetActive(elementState);
            m_inPoolHashSet.Remove(element);

            InvokeCallbackSafe(m_ActionOnGet, element);
            UpdateMaxActive();
            // Debug.Log("Get ["+element+"]");
            return element;
        }

        public GameObject Get(System.Action<GameObject> actionOnGet)
        {
            GameObject go = Get();

            InvokeCallbackSafe(actionOnGet, go);
            return go;
        }

        public bool IsInPool(GameObject element)
        {
            return m_inPoolHashSet.Contains(element);
        }

        public void Put(GameObject element)
        {
            //如果对象已经是null,直接跳过
            if (element == null)
            {
                //看起来很奇怪,但是这里有一个<null>和null的区别……
                element = null;
                return;
            }
            // Debug.Log("Release ["+element+"]");
            if (m_inPoolHashSet.Contains(element))
            {
                UnityEngine.Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
                element = null;
                return;
            }
            InvokeCallbackSafe(m_ActionOnRelease, element);

            if (PoolRoot)
            {
                element.transform.SetParent(PoolRoot, mWorldPositionStays);
            }
            element.SetActive(mActiveInPool);
            m_Stack.Push(element);
            m_inPoolHashSet.Add(element);

            element = null;
        }

        int m_PrepareCount = 0;
        int m_PrepareNum = 0;

        System.Action m_PrepareCallback;

        public bool IsPreparing
        {
            get
            {
                return m_PrepareCount != m_PrepareNum;
            }
        }
        public void Prepare(int createNum)
        {
            for (int i = 0; i < createNum; ++i)
            {
                PrepareOne();
            }
        }
        public void PrepareAsync(int createNum, string asyncTag, System.Action callback = null)
        {
            if (IsPreparing)
            {
                Debug.LogError("Pool[" + Prefab + "] is runing prepare![" + m_PrepareCount + "/" + m_PrepareNum + "]");
                return;
            }
            m_PrepareCount = 0;
            m_PrepareNum = createNum;
            m_PrepareCallback = callback;
            for (int i = 0; i < createNum; ++i)
            {
                // Performance.PerformanceSmoother.Instance.AddPendingAction(PrepareOne, asyncTag);
            }
        }

        void PrepareOne()
        {
            Put(CreateInstance(mPrefab));
            m_PrepareCount++;
            if (m_PrepareCount == m_PrepareNum)
            {
                if (m_PrepareCallback != null)
                {
                    m_PrepareCallback();
                    m_PrepareCallback = null;
                }
            }
        }
        /// <summary>
        /// 清空池子，被从池子取出的都不管啦
        /// </summary>
        public void ReleasePool()
        {
            while (m_Stack.Count > 0)
            {
                var go = m_Stack.Pop();
                if (go != null)
                {
                    if (Application.isPlaying)
                    {
                        GameObject.Destroy(go);
                    }
                    else
                        GameObject.DestroyImmediate(go);
                }

                mCountCreate--;
            }
            m_inPoolHashSet.Clear();

            //销毁池子根节点
            if (mPoolRoot != null)
            {
                if (Application.isPlaying)
                {
                    GameObject.Destroy(mPoolRoot.gameObject);
                }
                else
                    GameObject.DestroyImmediate(mPoolRoot.gameObject);
            }
            // Debug.Log("ReleasePool [" + mPrefab.name + "]");
            mPrefab = null;

#if POOL_DEBUG
            // GameObjectPoolProfiler.Instance.RemovePool(this);
#endif

        }

        public bool IsPoolNotUsed()
        {
            return m_Stack.Count == mCountCreate;
        }

        void InvokeCallbackSafe(System.Action<GameObject> callback, GameObject element)
        {
            if (callback != null)
            {
                try
                {
                    callback(element);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }

        #region test code

#if UNITY_EDITOR
        public Transform PoolRootTest
        {
            get
            {
                return mPoolRoot;
            }
        }

#endif
        public int MaxActive { get => mMaxActive; }

        void UpdateMaxActive()
        {
            if (CountActive > mMaxActive)
            {
                mMaxActive = CountActive;
            }
        }
        #endregion
    }
}
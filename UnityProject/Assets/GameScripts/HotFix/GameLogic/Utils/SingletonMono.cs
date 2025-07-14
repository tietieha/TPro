using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 继承自 MonoBehaviour 的单例基类
    /// 使用方式：public class MyClass : SingletonMono<MyClass> { ... }
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        // 单例实例
        private static T instance;
        
        // 线程锁
        private static readonly object lockObj = new object();
        
        // 是否已销毁标志
        private static bool isDestroyed = false;

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (isDestroyed)
                {
                    Debug.LogWarning($"[Singleton] 单例 '{typeof(T)}' 已被销毁，返回 null。");
                    return null;
                }

                lock (lockObj)
                {
                    if (instance == null)
                    {
                        // 查找场景中已存在的实例
                        instance = FindObjectOfType<T>();
                        
                        if (instance == null)
                        {
                            // 创建新的 GameObject 并添加组件
                            var singletonObject = new GameObject();
                            instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"{typeof(T)} (Singleton)";
                            
                            // 标记为不销毁
                            DontDestroyOnLoad(singletonObject);
                            
                            Debug.Log($"[Singleton] 创建新的单例实例: {typeof(T)}");
                        }
                        else
                        {
                            // 确保场景中已存在的实例不会被销毁
                            DontDestroyOnLoad(instance.gameObject);
                        }
                    }
                    
                    return instance;
                }
            }
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        protected virtual void Awake()
        {
            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = this as T;
                    DontDestroyOnLoad(gameObject);
                }
                else if (instance != this)
                {
                    Debug.LogWarning($"[Singleton] 场景中存在多个 '{typeof(T)}' 实例，销毁额外的实例。");
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        /// 销毁时处理
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                isDestroyed = true;
                instance = null;
            }
        }

        /// <summary>
        /// 应用退出时处理
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            isDestroyed = true;
            instance = null;
        }
    }
}

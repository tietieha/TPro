using System;

using UnityEngine;

namespace SDK.BaseCore
{
    public class SDKMono : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
#if UNITY_EDITOR
            return;
#endif
            SDKInit();
        }

        // 各SDK初始化代码在这里添加
        private void SDKInit()
        {
            try{
                SDKManager.Instance.InitSDKInstance();
                
            }catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
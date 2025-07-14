using System;
using UnityEngine;

namespace Platform.Api
{
    public class ResultCallback : MonoBehaviour
    {
        private const string CbObj = "SDKPlatform";

        private static ResultCallback _instance;

        private static readonly object Lock = new object();

        //初始化回调对象
        public static void InitCallback()
        {
            lock (Lock)
            {
                if (_instance != null) return;
                var callback = GameObject.Find(CbObj);
                if (callback == null)
                {
                    callback = new GameObject(CbObj);
                    DontDestroyOnLoad(callback);
                    _instance = callback.AddComponent<ResultCallback>();
                }
                else
                {
                    _instance = callback.GetComponent<ResultCallback>();
                }
            }
        }

        public static ResultCallback Instance
        {
            get
            {
                if (_instance == null)
                {
                    InitCallback();
                }

                return _instance;
            }
        }

        /*
         * 接收Android、iOS原生回调信息
         */
        public void onResult(string jsonParam)
        {
            if (SdkPlatform.api().Callback != null)
            {
                SdkPlatform.api().Callback(jsonParam);
            }
            else
            {
                throw new Exception("Callback is null");
            }
        }
    }
}
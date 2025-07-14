#if UNITY_OPENHARMONY

using System;
using Platform.Api;
using UnityEngine;

namespace Platform.Impl
{
    public class ParamImplHarmony : ParamInterface
    {
        private readonly OpenHarmonyJSObject _openHarmonyJSObject =
            new OpenHarmonyJSObject("UnityParamsBridge");

        private T SDKStaticCall<T>(string method, params object[] param)
        {
            try
            {
                return _openHarmonyJSObject.Call<T>(method, param);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return default(T);
        }

        private void SDKStaticCall(string method, params object[] param)
        {
            try
            {
                _openHarmonyJSObject.Call(method, param);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

         public override string area()
        {
            return SDKStaticCall<string>("area");
        }

        public override int appId()
        {
            return SDKStaticCall<int>("appId");
        }

        public override string channelId()
        {
            return SDKStaticCall<string>("channelId");
        }

        public override string deviceId()
        {
            return SDKStaticCall<string>("deviceId");
        }

        public override string trackId()
        {
            return SDKStaticCall<string>("trackId");
        }

        public override string sdkHost()
        {
            return SDKStaticCall<string>("sdkHost");
        }

        public override string advertisingId()
        {
            return SDKStaticCall<string>("advertisingId");
        }

        public override long freeSpace()
        {
            return SDKStaticCall<long>("freeSpace");
        }

        public override string language()
        {
            return SDKStaticCall<string>("language");
        }

        public override string versionName()
        {
            return SDKStaticCall<string>("versionName");
        }

        public override int versionCode()
        {
            return SDKStaticCall<int>("versionCode");
        }

        public override bool appReview()
        {
            return SDKStaticCall<bool>("appReview");
        }

        public override bool appDebug()
        {
            return SDKStaticCall<bool>("appDebug");
        }
    }
}
#endif
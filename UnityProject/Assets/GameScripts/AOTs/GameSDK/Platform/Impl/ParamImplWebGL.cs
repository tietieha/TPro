#if UNITY_WEBGL

using System;
using Platform.Api;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Platform.Impl
{
    public class ParamImplWebGL : ParamInterface
    {

        [DllImport("__Internal")]
        private static extern string AREA();
        [DllImport("__Internal")]
        private static extern int APPID();
        [DllImport("__Internal")]
        private static extern string CHANNELID();
        [DllImport("__Internal")]
        private static extern string DEVICEID();
        [DllImport("__Internal")]
        private static extern string TRACKID();
        [DllImport("__Internal")]
        private static extern string SDKHOST();
        [DllImport("__Internal")]
        private static extern string ADVERTISINGID();
        [DllImport("__Internal")]
        private static extern long FREESPACE();
        [DllImport("__Internal")]
        private static extern string LANGUAGE();
        [DllImport("__Internal")]
        private static extern string VERSIONNAME();
        [DllImport("__Internal")]
        private static extern int VERSIONCODE();
        [DllImport("__Internal")]
        private static extern bool APPREVIEW();
        [DllImport("__Internal")]
        private static extern bool APPDEBUG();

        public override string area()
        {
            return AREA();
        }

        public override int appId()
        {
            return APPID();
        }

        public override string channelId()
        {
            return CHANNELID();
        }

        public override string deviceId()
        {
            return DEVICEID();
        }

        public override string trackId()
        {
            return TRACKID();
        }

        public override string sdkHost()
        {
            Debug.Log("call sdk host webGL");
            return SDKHOST();
        }

        public override string advertisingId()
        {
            return ADVERTISINGID();
        }

        public override long freeSpace()
        {
            return FREESPACE();
        }

        public override string language()
        {
            return LANGUAGE();
        }

        public override string versionName()
        {
            return VERSIONNAME();
        }

        public override int versionCode()
        {
            return VERSIONCODE();
        }

        public override bool appReview()
        {
            return APPREVIEW();
        }

        public override bool appDebug()
        {
            return APPDEBUG();
        }
    }
}
#endif
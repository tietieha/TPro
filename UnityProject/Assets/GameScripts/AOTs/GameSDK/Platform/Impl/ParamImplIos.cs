#if UNITY_IOS

using System.Runtime.InteropServices;
using Platform.Api;

namespace Platform.Impl
{
    public class ParamImplIos : ParamInterface
    {
        [DllImport("__Internal")]
        private static extern string _area();

        [DllImport("__Internal")]
        private static extern int _appId();

        [DllImport("__Internal")]
        private static extern string _channelId();

        [DllImport("__Internal")]
        private static extern string _deviceId();

        [DllImport("__Internal")]
        private static extern string _trackId();

        [DllImport("__Internal")]
        private static extern string _sdkHost();

        [DllImport("__Internal")]
        private static extern string _advertisingId();

        [DllImport("__Internal")]
        private static extern long _freeSpace();

        [DllImport("__Internal")]
        private static extern string _language();

        [DllImport("__Internal")]
        private static extern string _versionName();

        [DllImport("__Internal")]
        private static extern int _versionCode();

        [DllImport("__Internal")]
        private static extern bool _appReview();

        [DllImport("__Internal")]
        private static extern bool _appDebug();


        public override string area()
        {
            return _area();
        }

        public override int appId()
        {
            return _appId();
        }

        public override string channelId()
        {
            return _channelId();
        }

        public override string deviceId()
        {
            return _deviceId();
        }

        public override string trackId()
        {
            return _trackId();
        }

        public override string sdkHost()
        {
            return _sdkHost();
        }

        public override string advertisingId()
        {
            return _advertisingId();
        }

        public override long freeSpace()
        {
            return _freeSpace();
        }

        public override string language()
        {
            return _language();
        }

        public override string versionName()
        {
            return _versionName();
        }

        public override int versionCode()
        {
            return _versionCode();
        }

        public override bool appReview()
        {
            return _appReview();
        }

        public override bool appDebug()
        {
            return _appDebug();
        }
    }
}

#endif
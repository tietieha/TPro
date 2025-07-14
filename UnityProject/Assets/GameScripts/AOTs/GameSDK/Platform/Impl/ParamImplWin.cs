#if   UNITY_STANDLONE || UNITY_STANDALONE_WIN
using Platform.Api;
using UnityBridge = Platform.Other.UnityBridge;

namespace Platform.Impl
{
    public class ParamImplWin : ParamInterface
    {
        public override string area()
        {
            return UnityBridge.Area();
        }

        public override int appId()
        {
            return UnityBridge.AppId();
        }

        public override string channelId()
        {
            return UnityBridge.ChannelId();
        }

        public override string deviceId()
        {
            return UnityBridge.DeviceId();
        }

        public override string trackId()
        {
            return UnityBridge.TrackId();
        }

        public override string sdkHost()
        {
            return UnityBridge.SdkHost();
        }

        public override string advertisingId()
        {
            return UnityBridge.AdvertisingId();
        }

        public override long freeSpace()
        {
            return UnityBridge.FreeSpace();
        }

        public override string language()
        {
            return UnityBridge.Language();
        }

        public override string versionName()
        {
            return UnityBridge.VersionName();
        }

        public override int versionCode()
        {
            return UnityBridge.VersionCode();
        }

        public override bool appReview()
        {
            return UnityBridge.AppReview();
        }

        public override bool appDebug()
        {
            return UnityBridge.AppDebug();
        }
    }
}
#endif
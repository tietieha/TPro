namespace Platform.Api
{
    public abstract class ParamInterface
    {
        /**
         * @return 地区
         */
        public abstract string area();

        /**
         * @return 应用ID
         */
        public abstract int appId();

        /**
         * @return 渠道ID
         */
        public abstract string channelId();

        /**
         * @return 设备ID
         */
        public abstract string deviceId();

        /**
         * @return 追踪ID
         */
        public abstract string trackId();

        /**
         * @return SDK的url
         */
        public abstract string sdkHost();

        /**
         * @return 广告ID
         */
        public abstract string advertisingId();

        /**
         * @return 手机剩余空间
         */
        public abstract long freeSpace();

        /**
         * @return 手机语言
         */
        public abstract string language();

        /**
         * @return 显示版本
         */
        public abstract string versionName();

        /**
         * @return 商店版本
         */
        public abstract int versionCode();

        /**
         * @return 提审模式
         */
        public abstract bool appReview();

        /**
         * @return 测试模式
         */
        public abstract bool appDebug();
    }
}
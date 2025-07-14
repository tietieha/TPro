using Platform.Api;
using Platform.Impl;

namespace Platform
{
    public static class SdkPlatform
    {
        private static ApiInterface _apiInterface;
        private static ParamInterface _paramInterface;

        public static ApiInterface api()
        {
            if (_apiInterface == null)
            {
#if  UNITY_STANDLONE
                _apiInterface = new ApiImplWin();
#elif UNITY_ANDROID
                _apiInterface = new ApiImplAndroid();
#elif UNITY_IOS
                _apiInterface = new ApiImplIos();
#elif UNITY_OPENHARMONY
                _apiInterface = new ApiImplHarmony();
#elif UNITY_STANDALONE_WIN
                _apiInterface = new ApiImplWin();
#elif UNITY_WEBGL
                _apiInterface = new ApiImplWebGL();
#else
                 throw new ApplicationException("No api implementation exists for this platform.");
#endif
            }

            return _apiInterface;
        }

        public static ParamInterface param()
        {
            if (_paramInterface == null)
            {
#if  UNITY_STANDLONE
                _paramInterface = new ParamImplWin();
#elif UNITY_ANDROID
                _paramInterface = new ParamImplAndroid();
#elif UNITY_IOS
                _paramInterface = new ParamImplIos();
#elif UNITY_OPENHARMONY
                _paramInterface = new ParamImplHarmony();
#elif UNITY_STANDALONE_WIN
                _paramInterface = new ParamImplWin();
#elif UNITY_WEBGL
                _paramInterface = new ParamImplWebGL();
#else
                 throw new ApplicationException("No param implementation exists for this platform.");
#endif
            }

            return _paramInterface;
        }
    }
}
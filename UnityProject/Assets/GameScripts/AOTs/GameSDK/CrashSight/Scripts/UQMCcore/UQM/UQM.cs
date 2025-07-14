using UnityEngine;

namespace GCloud.UQM
{
    #region UQM
	/// <summary>
	/// 回调的范型
	/// </summary>
	public delegate void OnUQMRetEventHandler<T> (T ret);
	public delegate string OnUQMStringRetEventHandler<T> (T ret, T crashType);

    public delegate string OnUQMStringRetSetLogPathEventHandler<T>(T ret, T crashType);
    public delegate void OnUQMRetLogUploadEventHandler<T>(T ret, T crashType, T result);
    

    public class UQM
    {
#if UNITY_ANDROID
        public const string LibName = "CrashSight";
#elif UNITY_IOS
        public const string LibName = "__Internal";
#elif UNITY_STANDALONE_WIN
#if UNITY_64//win64
        public const string LibName = "CrashSight64";
#else//win32
        public const string LibName = "CrashSight";
#endif
#elif UNITY_XBOXONE
        public const string LibName = "CrashSightXbox";
#elif UNITY_PS4 || UNITY_PS5
        public const string LibName = "libcs";
#elif UNITY_SWITCH
        public const string LibName = "__Internal";
#elif UNITY_STANDALONE_LINUX
        public const string LibName = "CrashSight";
#elif UNITY_ANDROID || UNITY_OPENHARMONY
		public const string LibName = "CrashSight";
#else
        public const string LibName = "__Internal";
#endif
        private static bool initialized;

        public static bool isDebug = true;

		/// <summary>
		/// UQM init，游戏开始的时候设置
		/// </summary>
        public static void Init()
		{
            if (initialized) return;
            initialized = true;
            if (isDebug)
                UQMLog.SetLevel(UQMLog.Level.Log);
            else
                UQMLog.SetLevel(UQMLog.Level.Error);
			UQMLog.Log ("UQM initialed !");
        }
    }

    #endregion
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class PlatformUtils
{
    public static bool isEditor => Application.isEditor;
    public static bool isAndroid => Application.platform == RuntimePlatform.Android;
    public static bool isIOS => Application.platform == RuntimePlatform.IPhonePlayer;

    /// <summary>
    /// 内网
    /// </summary>
    public static bool isNetPrivate
    {
        get { return SettingsUtils.GetGameNetType() == GameNetType.Internal; }
    }

    /// <summary>
    /// 外网
    /// </summary>
    public static bool isOnlineTest
    {
        get { return SettingsUtils.GetGameNetType() == GameNetType.Master; }
    }

    public static bool isOnline
    {
        get { return SettingsUtils.GetGameNetType() == GameNetType.Release; }
    }

    public static GameNetType GameNetType
    {
        get { return SettingsUtils.GetGameNetType(); }
    }

    /// <summary>
    /// GM开启
    /// </summary>
    public static bool isDebug
    {
        get
        {
#if GAME_DEBUG
            return true;
#else
            return PlatformDebug.IsDebugEnabled();
#endif
        }
    }

    private static bool _isLogEnable = true;

    public static bool isLogEnable
    {
        get
        {
#if ENABLE_LOG
            return _isLogEnable;
#else
            return PlatformDebug.IsLogEnabled();
#endif
        }

        set
        {
#if ENABLE_LOG
            _isLogEnable = value;
#else
		    _isLogEnable = false;
#endif
        }
    }

    public const string DEVICE_ID = "DEVICE_ID";
    private static string s_DeviceUuid;

    public static string deviceUuid
    {
        get
        {
#if UNITY_EDITOR
            //编辑器环境随机生成一个uuid存起来，游戏状态下可以开始新账号，不缓存s_DeviceUuid
            string deviceID = PlayerPrefs.GetString(DEVICE_ID, "");
            if (string.IsNullOrEmpty(deviceID))
            {
                s_DeviceUuid = SystemInfo.deviceUniqueIdentifier + UnityEngine.Random.Range(0, 1000000);
                PlayerPrefs.SetString(DEVICE_ID, s_DeviceUuid);
            }
            else
            {
                s_DeviceUuid = deviceID;
            }
#else
            if (s_DeviceUuid != null)
            {
                return s_DeviceUuid;
            }
            // s_DeviceUuid = deviceImpl.GetDeviceUuid();
             s_DeviceUuid = SystemInfo.deviceUniqueIdentifier;
#endif
            return s_DeviceUuid;
        }
    }

    public static bool IsAndroidPlatform()
    {
#if UNITY_ANDROID
        return true;
#endif
        return false;
    }

    public static bool IsIOSPlatform()
    {
#if UNITY_IOS
        return true;
#endif
        return false;
    }

    public static bool IsEditor()
    {
#if UNITY_EDITOR
        return true;
#endif
#pragma warning disable CS0162 // 检测到无法访问的代码
        return false;
#pragma warning restore CS0162 // 检测到无法访问的代码
    }

    public static bool IsSimulateAssetBundleInEditor()
    {
        return false;
    }

    public static string GetStreamingAssetsOrCacheUrl(string fileName)
    {
        string filePath =
#if UNITY_STANDLONE_WIN || UNITY_EDITOR
            string.Format("file://{0}/DataTable/{1}", Application.dataPath, fileName);
#elif UNITY_ANDROID
        string.Format("{0}/{1}",Application.streamingAssetsPath, fileName);
#elif UNITY_IPHONE
        string.Format("file://{0}/{1}",Application.streamingAssetsPath, fileName);
#else
        string.Empty;
#endif
        return filePath;
    }

    public static string GetStreamingAssetsUrl(string fileName)
    {
        string filePath =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            string.Format("file://{0}/{1}", Application.streamingAssetsPath, fileName);
#elif UNITY_ANDROID
        string.Format("{0}/{1}",Application.streamingAssetsPath, fileName);
#elif UNITY_IPHONE
        string.Format("file://{0}/{1}",Application.streamingAssetsPath, fileName);
#else
        string.Empty;
#endif
        return filePath;
    }

    public static int GetGameNetValue()
    {
        return (int)GameNetType;
    }

    public static string GetLocalIP()
    {
        string ipAddress = "";
        try
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("IP 获取失败");
        }
        return ipAddress;
    }

    // IPV6
    public static string GetLocalIPV6()
    {
        string ipAddress = "";
        try
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ipAddress = ip.ToString();
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("IP 获取失败");
        }
        return ipAddress;
    }

    public static string GetGLRender()
    {
        return SystemInfo.graphicsDeviceType.ToString();
    }

    public class PlatformDebug
    {
        private const string DebugFile = "PlatformDebug.ini";
        // 配置缓存
        private static Dictionary<string, string> _configCache;

        /// <summary>
        /// 检查Debug模式是否开启
        /// </summary>
        public static bool IsDebugEnabled()
        {
            LoadConfigIfNeeded();
            return GetBool("debug", Application.isEditor);
        }

        public static bool IsLogEnabled()
        {
            LoadConfigIfNeeded();
            return GetBool("log", false);
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值或默认值</returns>
        public static string GetConfig(string key, string defaultValue = "")
        {
            LoadConfigIfNeeded();
            return _configCache.TryGetValue(key, out var value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取布尔型配置值
        /// </summary>
        public static bool GetBool(string key, bool defaultValue = false)
        {
            string value = GetConfig(key, defaultValue.ToString());
            return value.Trim().ToLower() switch
            {
                "true" or "1" or "yes" or "on" => true,
                "false" or "0" or "no" or "off" => false,
                _ => defaultValue
            };
        }

        /// <summary>
        /// 获取整数型配置值
        /// </summary>
        public static int GetInt(string key, int defaultValue = 0)
        {
            return int.TryParse(GetConfig(key), out int result) ? result : defaultValue;
        }

        /// <summary>
        /// 获取浮点型配置值
        /// </summary>
        public static float GetFloat(string key, float defaultValue = 0f)
        {
            return float.TryParse(GetConfig(key), out float result) ? result : defaultValue;
        }

        /// <summary>
        /// 首次使用时加载配置
        /// </summary>
        private static void LoadConfigIfNeeded()
        {
            if (_configCache != null) return;

            _configCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string configPath = Path.Combine(Application.persistentDataPath, DebugFile);

            if (!File.Exists(configPath))
            {
                Debug.Log("Config file not found, using defaults");
                return;
            }

            try
            {
                using (var reader = new StreamReader(configPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string trimmed = line.Trim();

                        // 跳过空行和注释
                        if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                            continue;

                        // 解析键值对
                        int equalsIndex = trimmed.IndexOf('=');
                        if (equalsIndex > 0)
                        {
                            string key = trimmed.Substring(0, equalsIndex).Trim();
                            string value = trimmed.Substring(equalsIndex + 1).Trim();

                            if (!string.IsNullOrEmpty(key))
                            {
                                _configCache[key] = value;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read config");
            }
        }
    }
}
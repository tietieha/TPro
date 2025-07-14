using System;
using System.Collections.Generic;
using System.IO;
using TEngine;
using UnityEngine;
using Version = TEngine.Version;

public static class SettingsUtils
{
    private static readonly string GlobalSettingsPath = $"TEngineGlobalSettings";
    private static TEngineSettings _globalSettings;

    public static TEngineSettings GlobalSettings
    {
        get
        {
            if (_globalSettings == null)
            {
                _globalSettings = GetSingletonAssetsByResources<TEngineSettings>(GlobalSettingsPath);
            }

            return _globalSettings;
        }
    }

    public static FrameworkGlobalSettings FrameworkGlobalSettings => GlobalSettings.FrameworkGlobalSettings;

    public static HybridCLRCustomGlobalSettings HybridCLRCustomGlobalSettings => GlobalSettings.BybridCLRCustomGlobalSettings;

    public static void SetHybridCLRHotUpdateAssemblies(List<string> hotUpdateAssemblies)
    {
        HybridCLRCustomGlobalSettings.HotUpdateAssemblies.Clear();
        HybridCLRCustomGlobalSettings.HotUpdateAssemblies.AddRange(hotUpdateAssemblies);
    }

    public static void SetHybridCLRAOTMetaAssemblies(List<string> aOTMetaAssemblies)
    {
        HybridCLRCustomGlobalSettings.AOTMetaAssemblies.Clear();
        HybridCLRCustomGlobalSettings.AOTMetaAssemblies.AddRange(aOTMetaAssemblies);
    }

    public static bool EnableUpdateRes()
    {
        return FrameworkGlobalSettings.EnableUpdateRes;
    }

    public static bool EnableUpdateConfig()
    {
#if UNITY_EDITOR
        return UnityEditor.EditorPrefs.GetInt("EnableUpdateConfig", 1) == 1;
#endif
        return FrameworkGlobalSettings.EnableUpdateConfig;
    }

    private static string FindConfigServerURL(GameNetType gameNetType)
    {
        var updateInfo = FindUpdateInfo(gameNetType);
        return updateInfo?.configURL;
    }
    /// <summary>
    /// 获取配置表热更地址
    /// </summary>
    /// <returns>cdnUrl/Internal/Config</returns>
    public static string GetConfigServerUrl()
    {
        if (IsConfigUpdateFromGm())
        {
            var gameNetType = GetGameNetType();
            var configURL = FindConfigServerURL(gameNetType);
            if (string.IsNullOrEmpty(configURL))
            {
                Log.Error($"Could not found ConfigServerUrl by GameNetType:{gameNetType}");
                return string.Empty;
            }
            return configURL;
        }
        return Path.Combine(GetResServerUrl(), "Config").Replace("\\", "/");
    }

    public static string GetConfigCheckVersionURL()
    {
        if (IsConfigUpdateFromGm())
        {
            return $"{GetConfigServerUrl()}/game/version/config_show";
        }

        return $"{GetConfigServerUrl()}/{ConfigVersionFileName}";
    }

    public static string GetConfigFileListURL(string updateConfigVersion)
    {
        if (IsConfigUpdateFromGm())
        {
            return $"{GetConfigServerUrl()}/admin/download?name=Tables/ConfigFiles.txt";
        }

        return $"{GetConfigServerUrl()}/ConfigFiles_{updateConfigVersion}.txt";
    }

    public static string GetConfigDownloadURL(string fileName, string path, string name)
    {
        if (IsConfigUpdateFromGm())
        {
            return $"{GetConfigServerUrl()}/admin/download?name={path}/{name}";
        }

        return $"{GetConfigServerUrl()}/{fileName}";
    }

    public static bool IsConfigUpdateFromGm()
    {
        var gameNetType = GetGameNetType();
        return gameNetType == GameNetType.Internal || gameNetType == GameNetType.Master || gameNetType == GameNetType.Release;
    }


    /// <summary>
    /// 获取热更地址
    /// </summary>
    /// <returns> http://xxx.xxx.xxx:端口/Internal </returns>
    public static string GetResServerUrl()
    {
        // 如果有覆盖的UpdateInfo，则使用覆盖的resURL
        if (_overrideUpdateInfo != null && !string.IsNullOrEmpty(_overrideUpdateInfo.resURL))
        {
            return _overrideUpdateInfo.resURL;
        }

        var gameNetType = GetGameNetType();
        var updateInfo = FindUpdateInfo(gameNetType);
        var url = updateInfo?.resURL;
        if (url.IsNullOrEmpty())
        {
            Log.Error($"Could not found ResServerUrl by GameNetType:{gameNetType}");
            return string.Empty;
        }
        return url;
    }

    public static string GetResVersionServerUrl()
    {
        var gameNetType = GetGameNetType();
        var updateInfo = FindUpdateInfo(gameNetType);
        var url = updateInfo?.resVersionURL;
        if (url.IsNullOrEmpty())
        {
            Log.Error($"Could not found ResVersionServerUrl by GameNetType:{gameNetType}");
            return string.Empty;
        }
        return url;
    }

    public static string GetResCheckVersionURL()
    {
        // http://xxx.xxx.xxx:端口/内网/Android/1.0.0/UpdateData.json
        return Path.Combine(GetResServerUrl(), GetPlatformName(), Version.GameVersion, "UpdateData.json").Replace("\\", "/");
    }

    public static string GetResDownLoadPath(string fileName = "")
    {
        // http://xxx.xxx.xxx:端口/内网/Android/1.0.0
        return Path.Combine(GetResServerUrl(), GetPlatformName(), Version.GameVersion, fileName).Replace("\\", "/");
    }

    private static UpdateInfo FindUpdateInfo(GameNetType gameNetType)
    {
        foreach (var configServerInfo in FrameworkGlobalSettings.UpdateInfos)
        {
            if (configServerInfo.gameNetType == gameNetType)
            {
                return configServerInfo;
            }
        }

        return null;
    }

    #region 包名 打包用

    public static string GetConfigPackageNeme()
    {
        return FindConfigPackageName(FrameworkGlobalSettings.GameNetType);
    }
    private static string FindConfigPackageName(GameNetType gameNetType)
    {
        var updateInfo = FindUpdateInfo(gameNetType);
        if (updateInfo == null)
        {
            return "com.tencent.newheroes.test";
        }
        else
        {
            return updateInfo.packageName;
        }
    }
    #endregion

    private static T GetSingletonAssetsByResources<T>(string assetsPath) where T : ScriptableObject, new()
    {
        string assetType = typeof(T).Name;
#if UNITY_EDITOR
        string[] globalAssetPaths = UnityEditor.AssetDatabase.FindAssets($"t:{assetType}");
        if (globalAssetPaths.Length > 1)
        {
            foreach (var assetPath in globalAssetPaths)
            {
                Debug.LogError($"Could not had Multiple {assetType}. Repeated Path: {UnityEditor.AssetDatabase.GUIDToAssetPath(assetPath)}");
            }

            throw new Exception($"Could not had Multiple {assetType}");
        }
#endif
        T customGlobalSettings = Resources.Load<T>(assetsPath);
        if (customGlobalSettings == null)
        {
            Log.Error($"Could not found {assetType} asset，so auto create:{assetsPath}.");
            return null;
        }

        return customGlobalSettings;
    }

    /// <summary>
    /// 平台名字
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
#if UNITY_ANDROID
        return "Android";
#elif UNITY_IOS
        return "IOS";
#elif UNITY_WEBGL
        return "WebGL";
#else
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                return "Windows64";
            case RuntimePlatform.WindowsPlayer:
                return "Windows64";

            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "MacOS";

            case RuntimePlatform.IPhonePlayer:
                return "IOS";

            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.WebGLPlayer:
                return "WebGL";

            case RuntimePlatform.PS5:
                return "PS5";
            default:
                throw new NotSupportedException($"Platform '{Application.platform.ToString()}' is not supported.");
        }
#endif
    }

    public static List<ScriptGenerateRuler> GetScriptGenerateRule()
    {
        return FrameworkGlobalSettings.ScriptGenerateRule;
    }

    public static string GetUINameSpace()
    {
        return FrameworkGlobalSettings.NameSpace;
    }

    #region Config
    public static string ConfigSandBoxFolder = Path.Combine(Application.persistentDataPath, "Config");
    public static string ConfigStreamingAssetsFolder = Path.Combine(Application.streamingAssetsPath, "Config");
    public static string ConfigEditorFolder = "Assets/GameAssets/Config/Tables";

    public static string ConfigSandBoxFolder_Tables = Path.Combine(Application.persistentDataPath, "Config/Tables");
    public static string ConfigSandBoxFolder_Tables_Enum = Path.Combine(Application.persistentDataPath, "Config/Tables/Enum");
    public static string ConfigSandBoxFolder_Tables_Module = Path.Combine(Application.persistentDataPath, "Config/Tables/Module");


    public static string ConfigVersionFile = "Tables/VERSION.txt";
    public static string ConfigFileList = "Tables/ConfigFiles.txt";
    public static string ConfigVersionFileName = "VERSION.txt";
    public static string ConfigFileListName = "ConfigFiles.txt";

    public static string ConfigDownloadFolder = Path.Combine(Application.persistentDataPath, "Tmp");
    public static string ConfigZipName = Path.Combine(ConfigDownloadFolder, "Config.zip");

    #endregion

    #region Build Func

    public static void SetEnableUpdateConfig(bool enable)
    {
        FrameworkGlobalSettings.EnableUpdateConfig = enable;
    }

    public static void SetEnableUpdateRes(bool enable)
    {
        FrameworkGlobalSettings.EnableUpdateRes = enable;
    }

    public static void SetGameNetType(GameNetType type)
    {
        FrameworkGlobalSettings.GameNetType = type;
    }

    public static GameNetType GetGameNetType()
    {
#if UNITY_EDITOR
        return (GameNetType)UnityEditor.EditorPrefs.GetInt("EditorGameNetType", 1);
#endif
        return FrameworkGlobalSettings.GameNetType;
    }

    #endregion

    #region Runtime Override UpdateInfo
    private static UpdateInfo _overrideUpdateInfo;
    private static string _overrideFetchServerUrl = string.Empty;
    private static int _overrideDefaultServerId;
    public static UpdateInfo OverrideUpdateInfo => _overrideUpdateInfo;
    public static string OverrideFetchServerUrl => _overrideFetchServerUrl;
    public static int OverrideDefaultServerId => _overrideDefaultServerId;
    public static string OverrideNoticeUrl;
    public static void SetOverrideUpdateInfo(string configURL, string resURL, string fetchServerUrl, int dataDefaultServerId)
    {
        Log.Debug($"SetOverrideUpdateInfo configURL:{configURL}, resURL:{resURL}, fetchServerUrl:{fetchServerUrl}, dataDefaultServerId:{dataDefaultServerId}");
        if (_overrideUpdateInfo == null)
            _overrideUpdateInfo = new UpdateInfo();

        _overrideUpdateInfo.configURL = configURL;
        _overrideUpdateInfo.resURL = resURL;

        _overrideFetchServerUrl = fetchServerUrl;
        _overrideDefaultServerId = dataDefaultServerId;
    }

    /// <summary>
    /// 走sdk情况下 需要复写预设值的参数 目前有配置表更新url，热更资源url，
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static bool IsOverrideUpdateInfo()
    {
        var gameNetType = GetGameNetType();
        if (gameNetType > GameNetType.Internal)
        {
#if ONCE_SDK
            return true;
#endif
        }

        return false;
    }
    #endregion


}
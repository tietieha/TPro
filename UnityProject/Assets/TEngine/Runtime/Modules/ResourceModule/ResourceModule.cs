using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using YooAsset;
using Object = UnityEngine.Object;

namespace TEngine
{
    /// <summary>
    /// 资源组件。
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceModule : Module
    {
        #region Propreties

        private const int DefaultPriority = 0;

        private IResourceManager m_ResourceManager;

        private bool m_ForceUnloadUnusedAssets = false;

        private bool m_PreorderUnloadUnusedAssets = false;

        private bool m_PerformGCCollect = false;

        private AsyncOperation m_AsyncOperation = null;

        private float m_LastUnloadUnusedAssetsOperationElapseSeconds = 0f;

        [SerializeField] private float m_MinUnloadUnusedAssetsInterval = 60f;

        [SerializeField] private float m_MaxUnloadUnusedAssetsInterval = 300f;

        [SerializeField] private bool m_UseSystemUnloadUnusedAssets = true;

        [SerializeField] private float m_MaxCarrierDataNetworkSizeMb = 50f;  // 50MB
        /// <summary>
        /// 当前最新的包裹版本。
        /// </summary>
        public string PackageVersion { set; get; }

        /// <summary>
        /// 远程配置表版本。
        /// </summary>
        public string RemoteConfigVersion { set; get; }

        public string ConfigVersionStr => ConfigVersion.VersionStr;

        /// <summary>
        /// 当前最新的配置表版本。
        /// </summary>
        public ConfigVersion ConfigVersion
        {
            get
            {
                return m_ConfigVersion;
            }
            set
            {
                m_ConfigVersion = value;
            }
        }
        private ConfigVersion m_ConfigVersion;

        public ConfigVersion ConfigPackageVersion { get; private set; }
        public ConfigVersion ConfigSandboxVersion { get; private set; }

        /// <summary>
        /// 配置表空间
        /// </summary>
        public ConfigMode ConfigMode
        {
            get => m_ConfigMode;
            set => m_ConfigMode = value;
        }

        private ConfigMode m_ConfigMode = ConfigMode.Package;

        /// <summary>
        /// 资源包名称。
        /// </summary>
        [SerializeField] private string packageName = "DefaultPackage";

        /// <summary>
        /// 资源包名称。
        /// </summary>
        public string PackageName
        {
            get => packageName;
            set => packageName = value;
        }

        /// <summary>
        /// 资源系统运行模式。
        /// </summary>
        [SerializeField] private EPlayMode playMode = EPlayMode.EditorSimulateMode;

        /// <summary>
        /// 资源系统运行模式。
        /// <remarks>编辑器内优先使用。</remarks>
        /// </summary>
        public EPlayMode PlayMode
        {
            get
            {
#if UNITY_EDITOR
                //编辑器模式使用。
                return (EPlayMode)UnityEditor.EditorPrefs.GetInt("EditorPlayMode");
#else
                if (playMode == EPlayMode.EditorSimulateMode)
                {
                    playMode = EPlayMode.OfflinePlayMode;
                }
                //运行时使用。
                return playMode;
#endif
            }
            set
            {
#if UNITY_EDITOR
                playMode = value;
#endif
            }
        }

        /// <summary>
        /// 是否支持边玩边下载。
        /// </summary>
        [SerializeField] private bool m_UpdatableWhilePlaying = false;

        /// <summary>
        /// 是否支持边玩边下载。
        /// </summary>
        public bool UpdatableWhilePlaying => m_UpdatableWhilePlaying;

        /// <summary>
        /// 下载文件校验等级。
        /// </summary>
        public EVerifyLevel VerifyLevel = EVerifyLevel.Middle;

        [SerializeField] private ReadWritePathType m_ReadWritePathType = ReadWritePathType.Unspecified;

        /// <summary>
        /// 设置异步系统参数，每帧执行消耗的最大时间切片（单位：毫秒）
        /// </summary>
        [SerializeField] public long Milliseconds = 30;

        public int m_DownloadingMaxNum = 10;

        /// <summary>
        /// 获取或设置同时最大下载数目。
        /// </summary>
        public int DownloadingMaxNum
        {
            get => m_DownloadingMaxNum;
            set => m_DownloadingMaxNum = value;
        }

        public int m_FailedTryAgain = 3;

        public int FailedTryAgain
        {
            get => m_FailedTryAgain;
            set => m_FailedTryAgain = value;
        }

        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        public string ApplicableGameVersion => m_ResourceManager.ApplicableGameVersion;

        /// <summary>
        /// 获取当前内部资源版本号。
        /// </summary>
        public int InternalResourceVersion => m_ResourceManager.InternalResourceVersion;

        /// <summary>
        /// 获取资源读写路径类型。
        /// </summary>
        public ReadWritePathType ReadWritePathType => m_ReadWritePathType;

        /// <summary>
        /// 获取或设置无用资源释放的最小间隔时间，以秒为单位。
        /// </summary>
        public float MinUnloadUnusedAssetsInterval
        {
            get => m_MinUnloadUnusedAssetsInterval;
            set => m_MinUnloadUnusedAssetsInterval = value;
        }

        /// <summary>
        /// 获取或设置无用资源释放的最大间隔时间，以秒为单位。
        /// </summary>
        public float MaxUnloadUnusedAssetsInterval
        {
            get => m_MaxUnloadUnusedAssetsInterval;
            set => m_MaxUnloadUnusedAssetsInterval = value;
        }

        /// <summary>
        /// 使用系统释放无用资源策略。
        /// </summary>
        public bool UseSystemUnloadUnusedAssets
        {
            get => m_UseSystemUnloadUnusedAssets;
            set => m_UseSystemUnloadUnusedAssets = value;
        }

        /// <summary>
        /// 流量下载的最大资源包大小，以MB为单位。
        /// </summary>
        public float MaxCarrierDataNetworkSizeMb
        {
            get => m_MaxCarrierDataNetworkSizeMb;
            set => m_MaxCarrierDataNetworkSizeMb = value;
        }

        /// <summary>
        /// 获取无用资源释放的等待时长，以秒为单位。
        /// </summary>
        public float LastUnloadUnusedAssetsOperationElapseSeconds => m_LastUnloadUnusedAssetsOperationElapseSeconds;

        /// <summary>
        /// 获取资源只读路径。
        /// </summary>
        public string ReadOnlyPath => m_ResourceManager.ReadOnlyPath;

        /// <summary>
        /// 获取资源读写路径。
        /// </summary>
        public string ReadWritePath => m_ResourceManager.ReadWritePath;

        [SerializeField]
        private float m_AssetAutoReleaseInterval = 60f;

        [SerializeField]
        private int m_AssetCapacity = 64;

        [SerializeField]
        private float m_AssetExpireTime = 60f;

        [SerializeField]
        private int m_AssetPriority = 0;

        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        public float AssetAutoReleaseInterval
        {
            get
            {
                return m_ResourceManager.AssetAutoReleaseInterval;
            }
            set
            {
                m_ResourceManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        public int AssetCapacity
        {
            get
            {
                return m_ResourceManager.AssetCapacity;
            }
            set
            {
                m_ResourceManager.AssetCapacity = m_AssetCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        public float AssetExpireTime
        {
            get
            {
                return m_ResourceManager.AssetExpireTime;
            }
            set
            {
                m_ResourceManager.AssetExpireTime = m_AssetExpireTime = value;
            }
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        public int AssetPriority
        {
            get
            {
                return m_ResourceManager.AssetPriority;
            }
            set
            {
                m_ResourceManager.AssetPriority = m_AssetPriority = value;
            }
        }
        #endregion

        public override void Init()
        {
            base.Init();

            RootModule rootModule = ModuleSystem.GetModule<RootModule>();
            if (rootModule == null)
            {
                Log.Fatal("Root module is invalid.");
                return;
            }

            m_ResourceManager = ModuleImpSystem.GetModule<IResourceManager>();
            if (m_ResourceManager == null)
            {
                Log.Fatal("Resource module is invalid.");
                return;
            }

            if (PlayMode == EPlayMode.EditorSimulateMode)
            {
                Log.Info("During this run, Game Framework will use editor resource files, which you should validate first.");
#if !UNITY_EDITOR
                PlayMode = EPlayMode.OfflinePlayMode;
#endif
            }

            m_ResourceManager.SetReadOnlyPath(Application.streamingAssetsPath);
            if (m_ReadWritePathType == ReadWritePathType.TemporaryCache)
            {
                m_ResourceManager.SetReadWritePath(Application.temporaryCachePath);
            }
            else
            {
                if (m_ReadWritePathType == ReadWritePathType.Unspecified)
                {
                    m_ReadWritePathType = ReadWritePathType.PersistentData;
                }

                m_ResourceManager.SetReadWritePath(Application.persistentDataPath);
            }

            m_ResourceManager.DefaultPackageName = PackageName;
            m_ResourceManager.PlayMode = PlayMode;
            m_ResourceManager.VerifyLevel = VerifyLevel;
            m_ResourceManager.Milliseconds = Milliseconds;
            m_ResourceManager.InstanceRoot = transform;
            m_ResourceManager.HostServerURL = SettingsUtils.GetResDownLoadPath();
            m_ResourceManager.Initialize();
            m_ResourceManager.AssetAutoReleaseInterval = m_AssetAutoReleaseInterval;
            m_ResourceManager.AssetCapacity = m_AssetCapacity;
            m_ResourceManager.AssetExpireTime = m_AssetExpireTime;
            m_ResourceManager.AssetPriority = m_AssetPriority;
            Log.Info($"ResourceComponent Run Mode：{PlayMode}");

            // 配置表
            // InitConfig();
        }

        /// <summary>
        /// 初始化操作。
        /// </summary>
        /// <returns></returns>
        public async UniTask<InitializationOperation> InitPackage(string packageName = "")
        {
            if (m_ResourceManager == null)
            {
                Log.Fatal("Resource component is invalid.");
                return null;
            }

            return await m_ResourceManager.InitPackage(string.IsNullOrEmpty(packageName) ? PackageName:packageName);
        }

        #region 版本更新
        /// <summary>
        /// 获取当前资源包版本。
        /// </summary>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源包版本。</returns>
        public string GetPackageVersion(string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(PackageName)
                : YooAssets.GetPackage(customPackageName);
            if (package == null)
            {
                return string.Empty;
            }

            return package.GetPackageVersion();
        }

        /// <summary>
        /// 异步更新最新包的版本。
        /// </summary>
        /// <param name="appendTimeTicks">请求URL是否需要带时间戳。</param>
        /// <param name="timeout">超时时间。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>请求远端包裹的最新版本操作句柄。</returns>
        public UpdatePackageVersionOperation UpdatePackageVersionAsync(bool appendTimeTicks = false, int timeout = 60,
            string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(PackageName)
                : YooAssets.GetPackage(customPackageName);
            return package.UpdatePackageVersionAsync(appendTimeTicks, timeout);
        }

        /// <summary>
        /// 向网络端请求并更新清单
        /// </summary>
        /// <param name="packageVersion">更新的包裹版本</param>
        /// <param name="autoSaveVersion">更新成功后自动保存版本号，作为下次初始化的版本。</param>
        /// <param name="timeout">超时时间（默认值：60秒）</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        public UpdatePackageManifestOperation UpdatePackageManifestAsync(string packageVersion,
            bool autoSaveVersion = true, int timeout = 60, string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(PackageName)
                : YooAssets.GetPackage(customPackageName);
            return package.UpdatePackageManifestAsync(packageVersion, autoSaveVersion, timeout);
        }

        /// <summary>
        /// 确认过流量下载
        /// </summary>
        public bool ConfirmedCarrierDataNetwork { get; set; }

        /// <summary>
        /// 资源下载器，用于下载当前资源版本所有的资源包文件。
        /// </summary>
        public ResourceDownloaderOperation Downloader { get; set; }

        /// <summary>
        /// 创建资源下载器，用于下载当前资源版本所有的资源包文件。
        /// </summary>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        public ResourceDownloaderOperation CreateResourceDownloader(string customPackageName = "")
        {
            if (string.IsNullOrEmpty(customPackageName))
            {
                var package = YooAssets.GetPackage(PackageName);
                Downloader = package.CreateResourceDownloader(DownloadingMaxNum, FailedTryAgain);
                return Downloader;
            }
            else
            {
                var package = YooAssets.GetPackage(customPackageName);
                Downloader = package.CreateResourceDownloader(DownloadingMaxNum, FailedTryAgain);
                return Downloader;
            }
        }

        /// <summary>
        /// 清理包裹未使用的缓存文件。
        /// </summary>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        public ClearUnusedCacheFilesOperation ClearUnusedCacheFilesAsync(string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(PackageName)
                : YooAssets.GetPackage(customPackageName);
            return package.ClearUnusedCacheFilesAsync();
        }

        /// <summary>
        /// 清理沙盒路径。
        /// </summary>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        public void ClearSandbox(string customPackageName = "")
        {
            var package = string.IsNullOrEmpty(customPackageName)
                ? YooAssets.GetPackage(PackageName)
                : YooAssets.GetPackage(customPackageName);
            package.ClearPackageSandbox();
        }
        #endregion

        #region 配置表更新
        /// <summary>
        /// 获取沙盒配置表版本。
        /// </summary>
        /// <returns>配置表版本。</returns>
        public ConfigVersion GetSandboxConfigVersion()
        {
            string configVerFile = Path.Combine(SettingsUtils.ConfigSandBoxFolder_Tables, SettingsUtils.ConfigVersionFileName);
            if (Utility.File.ReadPersistentFile(configVerFile, out string configVersionStr, false))
            {
                if (configVersionStr != null)
                {
                    return new ConfigVersion(configVersionStr);
                }
            }

            return new ConfigVersion();
        }

        /// <summary>
        /// 获取包内配置版本号。
        /// </summary>
        /// <returns></returns>
        public ConfigVersion GetPackageConfigVersion()
        {
#if UNITY_EDITOR
            string configVersionStr = Utility.File.ReadAssetsText($"{SettingsUtils.ConfigEditorFolder}/{SettingsUtils.ConfigVersionFileName}");
            Log.Info($"Editor Config Version {configVersionStr}");
#else
            var configVersionStr = Utility.File.ReadStreamingAssetsText($"{SettingsUtils.ConfigStreamingAssetsFolder}/{SettingsUtils.ConfigVersionFile}");
            Log.Info($"Package Config Version {configVersionStr}");
#endif
            if (!string.IsNullOrEmpty(configVersionStr))
            {
                return new ConfigVersion(configVersionStr);
            }

            return new ConfigVersion();
        }

        /// <summary>
        /// 比较两个版本号
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>v1 > v2</returns>
        public bool CompareVersion(string v1, string v2)
        {
            if (string.IsNullOrEmpty(v1) || string.IsNullOrEmpty(v2))
            {
                return false;
            }

            var v1Ver = v1.Split(";");
            var v2Ver = v2.Split(";");

            var v1Param = v1Ver[0].Split(".");
            var v2Param = v2Ver[0].Split(".");
            var v1GMVer = v1Ver[1];
            var v2GMVer = v2Ver[1];
            if (v1Param.Length != v2Param.Length)
            {
                return false;
            }
            for (int i = 0; i < v1Param.Length; i++)
            {
                if (int.Parse(v1Param[i]) > int.Parse(v2Param[i]))
                {
                    return true;
                }
            }

            if (int.Parse(v1GMVer) > int.Parse(v2GMVer))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取沙盒配置表列表。
        /// </summary>
        /// <returns>配置表版本。</returns>
        public string GetSandboxConfigFileList()
        {
            string configFileList = $"{SettingsUtils.ConfigSandBoxFolder_Tables}/{SettingsUtils.ConfigFileListName}";
            if (Utility.File.ReadPersistentFile(configFileList, out string configFileListStr, false))
            {
                if (configFileListStr != null)
                {
                    return configFileListStr;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取包内配置版本号。
        /// </summary>
        /// <returns></returns>
        public string GetPackageConfigFileList()
        {
#if UNITY_EDITOR
            string configFileList = Utility.File.ReadAssetsText($"{SettingsUtils.ConfigEditorFolder}/{SettingsUtils.ConfigFileListName}");
#else
            var configFileList = Utility.File.ReadStreamingAssetsText($"{SettingsUtils.ConfigStreamingAssetsFolder}/{SettingsUtils.ConfigFileList}");
#endif
            if (!string.IsNullOrEmpty(configFileList))
            {
                return configFileList;
            }

            return string.Empty;
        }

        public void SaveConfigVersion(string version)
        {
            string configVerFile = Path.Combine(SettingsUtils.ConfigSandBoxFolder_Tables, SettingsUtils.ConfigVersionFileName);
            Utility.File.WritePersistentFile(configVerFile, version);
        }

        public void InitConfig()
        {
            ConfigPackageVersion = GameModule.Resource.GetPackageConfigVersion();
            m_ConfigMode = ConfigMode.Package;
            m_ConfigVersion = ConfigPackageVersion;
            if (SettingsUtils.EnableUpdateConfig())
            {
                ConfigSandboxVersion = GameModule.Resource.GetSandboxConfigVersion();
                if (ConfigSandboxVersion.IsValid)
                {
                    if (ConfigVersion.Compare(ConfigPackageVersion, ConfigSandboxVersion))
                    {
                        Log.Info("ProcedureUpdateConfig Delete 沙盒配置:", SettingsUtils.ConfigSandBoxFolder);
                        Utility.File.DeleteFolder(SettingsUtils.ConfigSandBoxFolder);
                    }
                    else
                    {
                        m_ConfigMode = ConfigMode.Sandbox;
                        m_ConfigVersion = ConfigSandboxVersion;
                    }
                }
            }

            Log.Info("Config Mode:{0} Version:{1}", m_ConfigMode, m_ConfigVersion.VersionStr);
        }

        #endregion

        #region 获取资源
        /// <summary>
        /// 检查资源是否存在。
        /// </summary>
        /// <param name="location">要检查资源的名称。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>检查资源是否存在的结果。</returns>
        public HasAssetResult HasAsset(string location, string customPackageName = "")
        {
            return m_ResourceManager.HasAsset(location, packageName: customPackageName);
        }

        /// <summary>
        /// 检查资源定位地址是否有效。
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        public bool CheckLocationValid(string location, string customPackageName = "")
        {
            return m_ResourceManager.CheckLocationValid(location, packageName: customPackageName);
        }

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="resTag">资源标签。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息列表。</returns>
        public AssetInfo[] GetAssetInfos(string resTag, string customPackageName = "")
        {
            return m_ResourceManager.GetAssetInfos(resTag, packageName: customPackageName);
        }

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="tags">资源标签列表。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息列表。</returns>
        public AssetInfo[] GetAssetInfos(string[] tags, string customPackageName = "")
        {
            return m_ResourceManager.GetAssetInfos(tags, packageName: customPackageName);
        }

        /// <summary>
        /// 获取资源信息。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息。</returns>
        public AssetInfo GetAssetInfo(string location, string customPackageName = "")
        {
            return m_ResourceManager.GetAssetInfo(location, packageName: customPackageName);
        }
        #endregion

        #region 加载资源

        #region Lua加载资源
        public delegate void LuaLoadAssetSuccessCbk(UnityEngine.Object obj, string assetName, int guid);
        public delegate void LuaLoadAssetFailureCbk(string assetName, int guid, bool isCancelled);
        /// <summary>
        /// lua用的加载句柄
        /// </summary>
        private static LuaLoadAssetSuccessCbk _luaLoadSuccessAction = null;
        private static LuaLoadAssetFailureCbk _luaLoadFailureAction = null;
        private static Dictionary<int, CancellationTokenSource> _activeLoads =
            new Dictionary<int, CancellationTokenSource>();

        private static LoadAssetSuccessCallback _successCbk = (assetName, asset, guid, userData) =>
        {
            if (_luaLoadSuccessAction != null)
            {
                var ass = asset as UnityEngine.Object;
                _luaLoadSuccessAction(ass, assetName, guid);
            }
        };
        private static LoadAssetFailureCallback _failureCbk = (assetName, status, errorMessage, guid, userData) =>
        {
            Log.Error(errorMessage);
            if (_luaLoadFailureAction != null)
            {
                _luaLoadFailureAction(assetName, guid, false);
            }
        };
        private LoadAssetCallbacks _loadAssetCallbacks = new LoadAssetCallbacks(_successCbk, _failureCbk);

        public void SetLuaLoad(LuaLoadAssetSuccessCbk loadSuccessCbk, LuaLoadAssetFailureCbk loadFailureCbk)
        {
            if (_luaLoadSuccessAction == null)
            {
                _luaLoadSuccessAction = loadSuccessCbk;
            }
            else
            {
                Debug.LogError("已经设置过lua加载回调了！！！");
            }

            if (_luaLoadFailureAction == null)
            {
                _luaLoadFailureAction = loadFailureCbk;
            }
            else
            {
                Debug.LogError("已经设置过lua加载回调了！！！");
            }
        }
        /// <summary>
        /// 异步加载资源Lua
        /// </summary>
        public void LoadAsyncLua(string assetName, Type assetType, int guid)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("Asset name is invalid.");
                return;
            }

            // 检查是否已有相同guid的加载任务
            if (_activeLoads.ContainsKey(guid))
            {
                Log.Warning($"Load task with guid {guid} is already in progress.");
                return;
            }

            var cts = new CancellationTokenSource();
            _activeLoads.Add(guid, cts); // 先占位

            if (assetType == typeof(GameObject))
            {
                LoadGameObjectAsync(assetName, guid, cts.Token);
            }
            else
            {
                LoadAssetAsync(assetName, assetType, guid, cts.Token);
            }
        }

        /// <summary>
        /// 异步加载游戏对象
        /// </summary>
        private async void LoadGameObjectAsync(string assetName, int guid, CancellationToken cancellationToken)
        {
            try
            {
                // 检查是否已被取消
                cancellationToken.ThrowIfCancellationRequested();

                GameObject gameObject = await m_ResourceManager.LoadGameObjectAsync(
                    assetName,
                    null,
                    cancellationToken,
                    packageName: null
                );

                // 再次检查是否已被取消
                if (cancellationToken.IsCancellationRequested)
                {
                    Destroy(gameObject);
                    return;
                }

                if (gameObject != null)
                {
                    _luaLoadSuccessAction?.Invoke(gameObject, assetName, guid);
                }
                else
                {
                    _luaLoadFailureAction?.Invoke(assetName, guid, false);
                }
            }
            catch (OperationCanceledException)
            {
                // 取消操作已处理，不需要额外操作
            }
            catch (Exception e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Log.Error($"LoadGameObjectAsync error: {e.Message}");
                    _luaLoadFailureAction?.Invoke(assetName, guid, false);
                }
            }
            finally
            {
                _activeLoads.Remove(guid);
            }
        }

        /// <summary>
        /// 异步加载普通资源
        /// </summary>
        private async void LoadAssetAsync(string assetName, Type assetType, int guid, CancellationToken cancellationToken)
        {
            try
            {
                // 检查是否已被取消
                cancellationToken.ThrowIfCancellationRequested();

                m_ResourceManager.LoadAssetAsync(assetName, assetType, DefaultPriority, _loadAssetCallbacks, cancellationToken, guid);

            }
            catch (OperationCanceledException)
            {
                // 取消操作已处理，不需要额外操作
            }
            catch (Exception e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Log.Error($"LoadAssetAsync error: {e.Message}");
                    _luaLoadFailureAction?.Invoke(assetName, guid, false);
                }
            }
            finally
            {
                _activeLoads.Remove(guid);
            }
        }

        public void CancelLoad(int guid)
        {
            if (_activeLoads.TryGetValue(guid, out var load))
            {
                load.Cancel(); // 触发取消
                _activeLoads.Remove(guid);
                _luaLoadFailureAction?.Invoke("", guid, true); // 通知Lua取消
            }
        }

        /// <summary>
        /// 同步加载资源。Lua
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="assetType"></param>
        /// <param name="guid"></param>
        public void LoadSyncLua(string assetName, Type assetType, int guid)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("Asset name is invalid.");
                return;
            }

            Object obj = null;
            if (assetType == typeof(GameObject))
            {
                obj = m_ResourceManager.LoadGameObject(assetName, null, packageName: null);
            }
            else
            {
                obj = m_ResourceManager.LoadAsset(assetName, assetType, packageName: null);
            }

            if (obj != null) _luaLoadSuccessAction(obj, assetName, guid);
        }
        #endregion
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        public void LoadAssetAsync(string location, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData = null, string packageName = "")
        {
            LoadAssetAsync(location, assetType, DefaultPriority, loadAssetCallbacks, userData, packageName);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="priority">加载资源的优先级。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        public void LoadAssetAsync(string location, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                Log.Error("Asset name is invalid.");
                return;
            }

            m_ResourceManager.LoadAssetAsync(location, assetType, priority, loadAssetCallbacks, -1, userData, packageName);
        }

        /// <summary>
        /// 同步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>资源实例。</returns>
        public T LoadAsset<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                Log.Error("Asset name is invalid.");
                return null;
            }

            return m_ResourceManager.LoadAsset<T>(location, packageName);
        }

        /// <summary>
        /// 同步加载游戏物体并实例化。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="parent">资源实例父节点。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        /// <returns>资源实例。</returns>
        public GameObject LoadGameObject(string location, Transform parent = null, string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                Log.Error("Asset name is invalid.");
                return null;
            }

            return m_ResourceManager.LoadGameObject(location, parent, packageName);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="callback">回调函数。</param>
        /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        public void LoadAsset<T>(string location, Action<T> callback, string customPackageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                Log.Error("Asset name is invalid.");
                return;
            }

            m_ResourceManager.LoadAsset<T>(location, callback, packageName: customPackageName);
        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="cancellationToken">取消操作Token。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        /// <typeparam name="T">要加载资源的类型。</typeparam>
        /// <returns>异步资源实例。</returns>
        public async UniTask<T> LoadAssetAsync<T>(string location, CancellationToken cancellationToken = default,
            string packageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(location))
            {
                Log.Error("Asset name is invalid.");
                return null;
            }

            return await m_ResourceManager.LoadAssetAsync<T>(location, cancellationToken, packageName);
        }

        /// <summary>
        /// 异步加载游戏物体并实例化。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="parent">资源实例父节点。</param>
        /// <param name="cancellationToken">取消操作Token。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
        /// <returns>异步游戏物体实例。</returns>
        public async UniTask<GameObject> LoadGameObjectAsync(string location, Transform parent = null, CancellationToken cancellationToken = default,
            string packageName = "")
        {
            if (string.IsNullOrEmpty(location))
            {
                Log.Error("Asset name is invalid.");
                return null;
            }

            return await m_ResourceManager.LoadGameObjectAsync(location, parent, cancellationToken, packageName);
        }

        internal AssetHandle LoadAssetGetOperation<T>(string location,
            string packageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.LoadAssetSync<T>(location);
            }

            var package = YooAssets.GetPackage(packageName);
            return package.LoadAssetSync<T>(location);
        }

        internal AssetHandle LoadAssetAsyncHandle<T>(string location, string packageName = "") where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return YooAssets.LoadAssetAsync<T>(location);
            }

            var package = YooAssets.GetPackage(packageName);
            return package.LoadAssetAsync<T>(location);
        }
        #endregion

        #region 卸载资源

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        public void UnloadAsset(object asset)
        {
            if (asset == null)
            {
                return;
            }
            m_ResourceManager.UnloadAsset(asset);
        }

        #endregion

        #region 释放资源

        /// <summary>
        /// 强制执行释放未被使用的资源。
        /// </summary>
        /// <param name="performGCCollect">是否使用垃圾回收。</param>
        public void ForceUnloadUnusedAssets(bool performGCCollect)
        {
            m_ForceUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                m_PerformGCCollect = true;
            }
        }

        /// <summary>
        /// 预订执行释放未被使用的资源。
        /// </summary>
        /// <param name="performGCCollect">是否使用垃圾回收。</param>
        public void UnloadUnusedAssets(bool performGCCollect)
        {
            m_PreorderUnloadUnusedAssets = true;
            if (performGCCollect)
            {
                m_PerformGCCollect = true;
            }
        }

        private void Update()
        {
            m_LastUnloadUnusedAssetsOperationElapseSeconds += Time.unscaledDeltaTime;
            if (m_AsyncOperation == null && (m_ForceUnloadUnusedAssets || m_LastUnloadUnusedAssetsOperationElapseSeconds >= m_MaxUnloadUnusedAssetsInterval ||
                                             m_PreorderUnloadUnusedAssets && m_LastUnloadUnusedAssetsOperationElapseSeconds >= m_MinUnloadUnusedAssetsInterval))
            {
                Log.Info("Unload unused assets...");
                m_ForceUnloadUnusedAssets = false;
                m_PreorderUnloadUnusedAssets = false;
                m_LastUnloadUnusedAssetsOperationElapseSeconds = 0f;
                m_AsyncOperation = Resources.UnloadUnusedAssets();
                if (m_UseSystemUnloadUnusedAssets)
                {
                    m_ResourceManager.UnloadUnusedAssets();
                }
            }

            if (m_AsyncOperation is { isDone: true })
            {
                m_AsyncOperation = null;
                if (m_PerformGCCollect)
                {
                    Log.Info("GC.Collect...");
                    m_PerformGCCollect = false;
                    GC.Collect();
                }
            }
        }

        #endregion
    }
}
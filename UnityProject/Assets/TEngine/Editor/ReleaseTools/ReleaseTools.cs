using System;
using System.IO;
using System.Linq;
using System.Xml;
using AssetPreprocessor.Scripts.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

namespace TEngine.Editor
{
    /// <summary>
    /// 打包工具类。
    /// <remarks>通过CommandLineReader可以不前台开启Unity实现静默打包以及CLI工作流，详见CommandLineReader.cs example1</remarks>
    /// </summary>
    public static class ReleaseTools
    {
        private static void SetDefaultArgs()
        {
            CommandLineReader.SetDefaultCustomArgument("version"        , "1.0.0");
            CommandLineReader.SetDefaultCustomArgument("buildNumber"    , "1");

            CommandLineReader.SetDefaultCustomArgument("serverTypeEnum" , "1");

            CommandLineReader.SetDefaultCustomArgument("enableUpdateRes", "false");
            CommandLineReader.SetDefaultCustomArgument("enableHybrid"   , "false");
            CommandLineReader.SetDefaultCustomArgument("development"    , "false");
            CommandLineReader.SetDefaultCustomArgument("cleanBuildCache", "false");
            CommandLineReader.SetDefaultCustomArgument("onlyBundle"     , "false");

            CommandLineReader.SetDefaultCustomArgument("gameDebug"      , "true");
            CommandLineReader.SetDefaultCustomArgument("enableLog"      , "true");
            CommandLineReader.SetDefaultCustomArgument("platformSymbol" , "NONE_SDK");
        }

        public static void BuildDll()
        {
            string platform = CommandLineReader.GetCustomArgument("platform");
            if (string.IsNullOrEmpty(platform))
            {
                Debug.LogError($"Build Asset Bundle Error！platform is null");
                return;
            }

            BuildTarget target = GetBuildTarget(platform);
            
            BuildDLLCommand.BuildAndCopyDlls(target);
        }

        public static void BuildAssetBundle()
        {
            string outputRoot = CommandLineReader.GetCustomArgument("outputRoot");
            if (string.IsNullOrEmpty(outputRoot))
            {
                Debug.LogError($"Build Asset Bundle Error！outputRoot is null");
                return;
            }

            string packageVersion = CommandLineReader.GetCustomArgument("packageVersion");
            if (string.IsNullOrEmpty(packageVersion))
            {
                Debug.LogError($"Build Asset Bundle Error！packageVersion is null");
                return;
            }

            string platform = CommandLineReader.GetCustomArgument("platform");
            if (string.IsNullOrEmpty(platform))
            {
                Debug.LogError($"Build Asset Bundle Error！platform is null");
                return;
            }

            BuildTarget target = GetBuildTarget(platform);
            BuildInternal(target, outputRoot);
            Debug.LogWarning($"Start BuildPackage BuildTarget:{target} outputPath:{outputRoot}");
        }
        
        [MenuItem("TEngine/Quick Build/一键打包AssetBundle")]
        public static void BuildCurrentPlatformAB()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            BuildInternal(target, Application.dataPath + "/../Builds/", packageVersion: GetBuildPackageVersion());
            AssetDatabase.Refresh();
        }

        private static BuildTarget GetBuildTarget(string platform)
        {
            BuildTarget target = BuildTarget.NoTarget;
            switch (platform)
            {
                case "Android":
                    target = BuildTarget.Android;
                    break;
                case "IOS":
                    target = BuildTarget.iOS;
                    break;
                case "Windows":
                    target = BuildTarget.StandaloneWindows64;
                    break;
                case "MacOS":
                    target = BuildTarget.StandaloneOSX;
                    break;
                case "Linux":
                    target = BuildTarget.StandaloneLinux64;
                    break;
                case "WebGL":
                    target = BuildTarget.WebGL;
                    break;
                case "Switch":
                    target = BuildTarget.Switch;
                    break;
                case "PS4":
                    target = BuildTarget.PS4;
                    break;
                case "PS5":
                    target = BuildTarget.PS5;
                    break;
            }

            return target;
        }

        private static bool BuildInternal(BuildTarget buildTarget, string outputRoot, string packageVersion = "1.0",
            EBuildPipeline buildPipeline = EBuildPipeline.ScriptableBuildPipeline)
        {
            Debug.Log($"开始构建 : {buildTarget}");
            SpriteProcessor.GenerateSpriteAtlases();

            IBuildPipeline pipeline = null;
            BuildParameters buildParameters = null;
            
            if (buildPipeline == EBuildPipeline.BuiltinBuildPipeline)
            {
                // 构建参数
                BuiltinBuildParameters builtinBuildParameters = new BuiltinBuildParameters();
                
                // 执行构建
                pipeline = new BuiltinBuildPipeline();
                buildParameters = builtinBuildParameters;
                
                builtinBuildParameters.CompressOption = ECompressOption.LZ4;
            }
            else
            {
                ScriptableBuildParameters scriptableBuildParameters = new ScriptableBuildParameters();
                
                // 执行构建
                pipeline = new ScriptableBuildPipeline();
                buildParameters = scriptableBuildParameters;
                
                scriptableBuildParameters.CompressOption = ECompressOption.LZ4;
            }
            
            buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
            buildParameters.BuildPipeline = buildPipeline.ToString();
            buildParameters.BuildTarget = buildTarget;
            buildParameters.BuildMode = EBuildMode.IncrementalBuild;
            buildParameters.PackageName = "DefaultPackage";
            buildParameters.PackageVersion = packageVersion;
            buildParameters.VerifyBuildingResult = true;
            buildParameters.FileNameStyle =  EFileNameStyle.HashName;
            buildParameters.BuildinFileCopyOption = EBuildinFileCopyOption.ClearAndCopyAll;
            buildParameters.BuildinFileCopyParams = string.Empty;
            buildParameters.EncryptionServices = CreateEncryptionInstance("DefaultPackage",buildPipeline);
            // 启用共享资源打包
            buildParameters.EnableSharePackRule = true;
            
            var buildResult = pipeline.Run(buildParameters, true);
            if (buildResult.Success)
            {
                Debug.Log($"构建成功 : {buildResult.OutputPackageDirectory}");
            }
            else
            {
                Debug.LogError($"构建失败 : {buildResult.ErrorInfo}");
            }

            return buildResult.Success;
        }
        
        /// <summary>
        /// 创建加密类实例
        /// </summary>
        private static IEncryptionServices CreateEncryptionInstance(string packageName, EBuildPipeline buildPipeline)
        {
            var encryptionClassName = AssetBundleBuilderSetting.GetPackageEncyptionClassName(packageName, buildPipeline);
            var encryptionClassTypes = EditorTools.GetAssignableTypes(typeof(IEncryptionServices));
            var classType = encryptionClassTypes.Find(x => x.FullName != null && x.FullName.Equals(encryptionClassName));
            if (classType != null)
            {
                Debug.Log($"Use Encryption {classType}");
                return (IEncryptionServices)Activator.CreateInstance(classType);
            }
            else
            {
                return null;
            }
        }

        [MenuItem("TEngine/Quick Build/一键打包Window", false, 90)]
        public static void AutomationBuild()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            BuildDLLCommand.BuildAndCopyDlls(target);
            AssetDatabase.Refresh();
            if (!BuildInternal(target, Application.dataPath + "/../Builds/Windows", packageVersion: GetBuildPackageVersion()))
                return;
            AssetDatabase.Refresh();
            BuildImp(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64, $"{Application.dataPath}/../Builds/Windows/Release_Windows.exe");
        }

        // 构建版本相关
        private static string GetBuildPackageVersion()
        {
            var buildNumber = CommandLineReader.GetCustomArgument("buildNumber");
            int buildNumberInt = buildNumber.ToInt();

            // return buildNumber;
            // return DateTime.Now.ToString("yyyyMMdd");
            return $"{Application.version.Replace(".", "")}{string.Format("{0:D4}", buildNumberInt)}";
        }

        private static void  PreBuild()
        {
            SetDefaultArgs();

            var version = CommandLineReader.GetCustomArgument("version");
            PlayerSettings.bundleVersion = version;

            var buildNumber = CommandLineReader.GetCustomArgument("buildNumber");
            int buildNumberInt = buildNumber.ToInt();
            #if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode = buildNumberInt;
            #elif UNITY_IOS
            PlayerSettings.iOS.buildNumber = buildNumber;
            #endif

            var serverTypeEnum = CommandLineReader.GetCustomArgument("serverTypeEnum");
            var serverTypeEnumValue = (GameNetType)(serverTypeEnum.ToInt());
            SettingsUtils.SetGameNetType(serverTypeEnumValue);

            // 需要在servertype设置完
            var packageName = SettingsUtils.GetConfigPackageNeme();
            if (!string.IsNullOrEmpty(packageName))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, packageName);
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, packageName);
                ChangeManifest();
            }

            // 渠道宏
            var platformSymbol = CommandLineReader.GetCustomArgument("platformSymbol");
            ScriptingDefineSymbols.SetScriptingDefineSymbolsState(platformSymbol, true);

            var enableLog = CommandLineReader.GetCustomArgument("enableLog").Equals("true");
            if (enableLog)
                LogScriptingDefineSymbols.EnableAllLogs();
            else
                LogScriptingDefineSymbols.EnableInfoAndAboveLogs();

            var gameDebug = CommandLineReader.GetCustomArgument("gameDebug").Equals("true");
            ScriptingDefineSymbols.SetScriptingDefineSymbolsState("GAME_DEBUG", gameDebug);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var enableUpdateRes = CommandLineReader.GetCustomArgument("enableUpdateRes").Equals("true");
            SettingsUtils.SetEnableUpdateRes(enableUpdateRes);

            var enableHybrid = CommandLineReader.GetCustomArgument("enableHybrid");
            if (enableHybrid.Equals("true"))
            {
                BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
                BuildDLLCommand.Enable();
                PrebuildCommand.GenerateAll();
                BuildDLLCommand.CopyAOTHotUpdateDlls(target);
                Debug.Log($"[构建]: PreBuild-Hybrid enableHybrid:{HybridCLR.Editor.SettingsUtil.Enable}");
            }
            else
            {
                BuildDLLCommand.Disable();
            }

        }



        // [MenuItem("TEngine/Quick Build/修改AndroidManifest", false, 90)]
        public static void ChangeManifest()
        {
            // TODO ：根据不同渠道替换对应的Manifest
            string manifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
            if (File.Exists(manifestPath))
            {
                ModifyPackageNameInManifest(manifestPath);
            }
        }

        private static void ModifyPackageNameInManifest(string manifestPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(manifestPath);
            XmlNode manifestNode = xmlDoc.SelectSingleNode("/manifest");
            if (manifestNode != null)
            {
                XmlAttribute packageAttr = manifestNode.Attributes["package"];
                if (packageAttr != null)
                {
                    packageAttr.Value = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android); // 替换为你的包名
                    xmlDoc.Save(manifestPath);
                }
            }
        }

        [MenuItem("TEngine/Quick Build/一键打包Android", false, 90)]
        public static void AutomationBuildAndroid()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;
            AssetDatabase.Refresh();
            PreBuild();
            AssetDatabase.Refresh();
            if (!BuildInternal(BuildTarget.Android, outputRoot: Application.dataPath + "/../Bundles", packageVersion: GetBuildPackageVersion()))
                return;
            AssetDatabase.Refresh();
            var buildNumber = CommandLineReader.GetCustomArgument("buildNumber");
            var gameNetType = SettingsUtils.FrameworkGlobalSettings.GameNetType;
            BuildImp(BuildTargetGroup.Android, BuildTarget.Android, $"{Application.dataPath}/../Build/Android/ProNeoH_{Enum.GetName(typeof(GameNetType), gameNetType)}_{Application.version}_{DateTime.Now.ToString("yyyyMMdd")}_{buildNumber}.apk");
            // BuildImp(BuildTargetGroup.Android, BuildTarget.Android, $"{Application.dataPath}/../Build/Android/Android.apk");
        }

        [MenuItem("TEngine/Quick Build/一键打包IOS", false, 90)]
        public static void AutomationBuildIOS()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            BuildDLLCommand.BuildAndCopyDlls(target);
            AssetDatabase.Refresh();
            if (!BuildInternal(target, outputRoot: Application.dataPath + "/../Bundles", packageVersion: GetBuildPackageVersion()))
                return;
            AssetDatabase.Refresh();
            BuildImp(BuildTargetGroup.iOS, BuildTarget.iOS, $"{Application.dataPath}/../Build/IOS/XCode_Project");
        }
        
        // [MenuItem("TEngine/Quick Build/一键打包WebGL", false, 91)]
        // public static void AutomationBuildWebGL()
        // {
        //     BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        //     BuildDLLCommand.BuildAndCopyDlls(target);
        //     AssetDatabase.Refresh();
        //     BuildInternal(target, Application.dataPath + "/../Builds/WebGL", packageVersion: GetBuildPackageVersion());
        //     AssetDatabase.Refresh();
        //     BuildImp(BuildTargetGroup.WebGL, BuildTarget.WebGL, $"{Application.dataPath}/../Builds/WebGL");
        // }

        public static void BuildImp(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, string locationPathName)
        {
            var onlyBundle = CommandLineReader.GetCustomArgument("onlyBundle").Equals("true");
            if (onlyBundle)
            {
                Debug.Log($"[构建]: 仅打包AssetBundle");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return;
            }

            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
                locationPathName = locationPathName,
                targetGroup = buildTargetGroup,
                target = buildTarget,
                options = BuildOptions.None
            };

            var development = CommandLineReader.GetCustomArgument("development").Equals("true");
            if (development)
            {
                buildPlayerOptions.options |= BuildOptions.Development;
                buildPlayerOptions.options |= BuildOptions.ConnectWithProfiler;
                buildPlayerOptions.options |= BuildOptions.AllowDebugging;
            }

            var cleanBuildCache = CommandLineReader.GetCustomArgument("cleanBuildCache").Equals("true");
            if (cleanBuildCache)
            {
                buildPlayerOptions.options |= BuildOptions.CleanBuildCache;
            }

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build success: {summary.totalSize / 1024 / 1024} MB");
            }
            else
            {
                Debug.Log($"Build Failed" + summary.result);
            }
        }
    }
}
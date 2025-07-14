namespace GameMain
{
    public enum ProcedureEnum
    {
        None,
        Launch,                     // 启动流程
        Splash,                     // 启动界面
        InitPackage,                // 初始化包
        UpdateVersion,              // 更新版本
        UpdateManifest,             // 更新清单
        CreateDownloader,           // 创建下载器
        DownloadFile,               // 下载文件
        DownloadOver,               // 下载完成
        ClearCache,                 // 清理缓存
        InitResources,              // 初始化资源
        UpdateConfig,               // 更新配置
        Preload,                    // 预加载

        LoadAssembly,               // 加载程序集
        StartGame,                  // 开始游戏
        ReloadGame,                 // 重载游戏
        LogoutGame,                 // 登出游戏
    }
}
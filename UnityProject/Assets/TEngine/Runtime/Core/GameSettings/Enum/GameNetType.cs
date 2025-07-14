using UnityEngine;

/// <summary>
/// 游戏服务器类型
/// ServerTypeEnum 是资源相关的服务器类型，GameNetType 是游戏业务服务器类型
/// </summary>
public enum GameNetType
{
    [Header("无")]
    None = 0,

    [Header("内网")]
    Internal = 1,

    [Header("外网测试服")]
    Master = 2,

    [Header("外网服")]
    Release = 3,

    [Header("外网QA")]
    PreOnlineQA = 4,

    [Header("外网预发布")]
    PreOnline = 5,

    [Header("外网线上服")]
    Online = 6
}
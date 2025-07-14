// GMLogConstant.cs
// Created by nancheng.
// DateTime: 2024年2月27日 17:40:25
// Desc: GM打点常量

public class GmLogConstant
{
    // 上传地址
    public static readonly string POST_URL_JSON = "https://openapi.im30app.com/openapi/v1/data/create";

    public static readonly string LOCAL_CACHE_DIR = "RecordCache";
    // 本地缓存文件
    public static readonly string LOCAL_CACHE_LOG_FILE = "gm_log_event_cache.txt";
    
    // 定期缓存时间(s)
    public static readonly float SAVE_CACHE_TIMING_SECOND = 20;
    // 最大打点数量
    public static readonly int MAX_RECORD_COUNT_FRAME = 3;
}
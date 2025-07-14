
public class GameDefines
{
    public static class SettingKeys
    {
        // 支付相关
        public const string UwCurrencyCodeKey = "uw_currency_code";
        public const string UwCacheItemID = "uw_cache_item_id";
        public const string UwPurchaseKey = "UW_PURCHASE_KEY";
        public const string UwPriceExchangeRate = "UW_PRICE_EXCHANGE_RATE";
        public const string UwPriceSymbol = "UW_PRICE_SYMBOL";
        public const string UwPriceSymbolBack = "UW_PRICE_SYMBOL_BACK";
        
        public const string GAME_UID = "Setting.GAME_UID";                                // 用户id（使用这个）
        public const string SERVER_ZONE = "SERVER_ZONE";

        /// 账号相关
        public const string ACCOUNT_LIST = "Setting.ACCOUNT_LIST";                              // 登录过的账号列表
        public const string IM30_ACCOUNT = "Setting.IM30_ACCOUNT";                              // IM30账号，本地缓存
        public const string IM30_PWD = "Setting.IM30_PWD";                                      // IM30密码，本地缓存
        
        public const string CATCH_ITEM_ID = "Setting.CATCH_ITEM_ID";

        public const string EFFECT_MUSIC_ON = "isEffectMusicOn";                                // 音效
        public const string BG_MUSIC_ON = "isBGMusicOn";                                        // 音乐
        
        public const string USER_LANGUAGE = "Setting.USER_LANGUAGE";                            // 用户自定义语言
        
        
        //聊天相关
        public const string FastChatServerInfo = "FastChatServerInfo";//网络环境最好的聊天服ip
        public const string FastChatServerInfoDebug = "FastChatServerInfoDebug";//网络环境最好的聊天服ip
    }

    public class SoundAssets
    {
        public const string Music_Sfx_logo_loading = "battle_of_lepanto.ogg";//播放登录LOGO背景音乐
        public const string Music_Sfx_scene_change = "world_cloud.ogg";//点击后出现云的音效
        public const string Music_Sfx_click_tabs = "ui_tabs.ogg";   //页签音效
    }

    public class SoundGround
    {
        public const string Music = "Music";
        public const string Sound = "Sound";
        public const string Effect = "Effect";
        public const string Dub = "Dub";
    }
    
    public static readonly string UITextAssetPath = "Assets/GameAssets/SoAsset/TextAsset/UITextStyleAsset.asset";
}


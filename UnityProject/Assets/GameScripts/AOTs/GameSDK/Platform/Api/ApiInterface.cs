namespace Platform.Api
{
    public delegate void PlatformCallback(string jsonParam);

    public abstract class ApiInterface
    {
        public PlatformCallback Callback { protected set; get; }


        /**
        * 初始化接口
        *
        * @param callback 各个操作回调
        */
        public abstract void initSDK(PlatformCallback callback);

        /**
         * 登录
         */
        public abstract void login();


        /**
         * 服务器列表
         */
        public abstract void servers();

        /**
         * 获取服务器
         * 调用位置：玩家选择该服务器需要进入时，用于获取IP端口等信息
         */
        public abstract void server(int serverId);

        /**
         * 角色列表
         */
        public abstract void roles();

        /**
         * 游戏开始
         * 调用位置：登录完账号，游戏进入主场景时（也可以在拿到服务器角色信息读条时）
         *
         * @param newRole        是否为角色
         * @param roleId         角色ID
         * @param roleName       角色名称
         * @param roleLevel      角色等级
         * @param vipLevel       vip等级
         * @param roleCreateTime 角色创建时间（时间戳：秒）
         * @param roleData       角色数据，将在角色列表中回传，大小限制1024个字符
         */
        public abstract void gameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel,
            string roleCreateTime, string roleData);


        /**
         * 角色更新
         * 调用位置：以下角色任一数据发生更新时调用，未变化的参数也需要传入
         *
         * @param roleName  角色名称
         * @param roleLevel 角色等级
         * @param vipLevel  vip等级
         * @param roleData  角色数据，将在角色列表中回传，大小限制1024个字符
         */
        public abstract void roleUpdate(string roleName, int roleLevel, int vipLevel, string roleData);

        /**
         * 支付
         *
         * @param data 游戏服务器在平台下单后获取的 platformOrderData
         *             字段最大长度 1024个字符
         */
        public abstract void pay(string data);

        /**
         * 恢复购买，海外游戏专用
         */
        public abstract void renewPay();


        /**
         * 商品列表
         */
        public abstract void products();

        /**
         * 登出账号
         */
        public abstract void logout();

        /**
         * 退出游戏
         */
        public abstract void exitGame();

        /**
         * 绑定账户
         */
        public abstract void bind();

        public abstract bool bindAvailable();

        /**
         * 删除账户
         */
        public abstract void del();

        public abstract bool delAvailable();

        /**
         * 客服
         */
        public abstract void customer(string param);

        public abstract bool customerAvailable();

        /**
         * 公告
         */
        public abstract void placard();

        public abstract bool placardAvailable();

        public abstract bool placardUpdated();

        /**
         * 礼包码
         */
        public abstract void cdKey();

        public abstract bool cdKeyAvailable();

        /**
         * 调查问卷
         */
        public abstract void survey();

        public abstract bool surveyAvailable();

        /**
         * 分享
         *
         * @param method 分享方式
         * @param json   分享的数据json
         */
        public abstract void share(string method, string json);

        /**
         * 分享方式列表
         *
         * @return 用, 分割分享列表
         */
        public abstract string shareMethods();

        public abstract bool shareAvailable();

        /**
         * 显示条款
         *
         * @param type 条款类型
         */
        public abstract void showTerms(string type);

        /**
         * 杀死游戏
         */
        public abstract void killGame();

        /**
         * 显示吐司
         */
        public abstract void showToast(string text);

        /**
         * 客户端事件打点
         */
        public abstract void eventClient(string eventKey, string eventValue);

        /**
         * 三方事件打点
         */
        public abstract void eventTripartite(string eventKey, string eventValue);

        /**
         * 程序日志
         *
         * @param level 日志等级，支持debug,info,warn,error
         * @param tag   日志标识
         * @param log   日志内容
         */
        public abstract void logger(string level, string tag, string log);

        /**
        * 打开网页
        *
        * @param url    网页地址
        * @param param 网页参数
        */
        public abstract void webview(string url, string param);

        /**
         * 网址是否可见
         *
         * @param url 网页地址
         * @return 可见
         */
        public abstract bool webviewAvailable(string url);


        /**
         * 扫码登录
         */
        public abstract void qrLogin();

        /**
         * 扫码是否可见
         */
        public abstract bool qrLoginAvailable();
    }
}
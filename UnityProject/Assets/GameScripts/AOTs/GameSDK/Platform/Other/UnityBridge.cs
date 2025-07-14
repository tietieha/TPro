#if UNITY_STANDLONE || UNITY_STANDALONE_WIN
using System;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Platform.desktop.api;
using Platform.desktop.common;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using Timer = System.Timers.Timer;

namespace Platform.Other
{
    public static class UnityBridge
    {
        private const int BufferSize = 40960;
        private static  Timer _timer;
        private static JObject _obj = new JObject();
        private static Process _process;
        private static Socket _socket;

        public static void Kill()
        {
            try
            {
                _process?.Kill();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
            }
        }

        private static void Quit(object sender, EventArgs e)
        {
            try
            {
                _obj = null;
                _timer.Stop();
                _socket.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Error killing child process: {ex.Message}");
                Debug.WriteLine($@"Error killing child process: {ex.Message}");
            }
        }

        /**
        * 初始化接口
        *
        * @param callback 各个操作回调
        */
        public static async void InitSdk(OnPlatformCallback callback)
        {
            try
            {
                Application.quitting += () =>
                {
                    Quit(null, null);
                };
                _timer = new Timer();
                _timer.Interval = 1000;
                _timer.AutoReset = true;
                _timer.Elapsed += (sender, e) =>
                {
                    var memoryMappedJObject = MemoryMappedJObject();
                    if (null != memoryMappedJObject)
                    {
                        _obj = memoryMappedJObject;
                    }
                };
                _timer.Start();
                //启动socket服务器
                var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var port = BindPort(serverSocket, 11100);
                //启动进程
                var pid = await UnityProcess.RunPlatformView(port);
                _process = Process.GetProcessById(pid);
                Console.WriteLine(@"Waiting for a connection , port : " + port);
                // 接受客户端的连接，此方法会阻塞，直到有客户端连接为止
                await Task.Run(() =>
                {
                    _socket = serverSocket.Accept();
                    Console.WriteLine(@"Socket accepted." + _socket.RemoteEndPoint);
                    Debug.WriteLine(@"Socket accepted." + _socket.RemoteEndPoint);
                    HandleMessageReceived(callback);
                    var jObject = new JObject { { "opcode", "InitSdk" } };
                    SendMessage(jObject);
                    // ReSharper disable once FunctionNeverReturns
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
            }
        }

        private static int BindPort(Socket socket, int port)
        {
            try
            {
                var ipAddress = IPAddress.Parse("127.0.0.1");
                var localEndPoint = new IPEndPoint(ipAddress, port);
                socket.Bind(localEndPoint);
                socket.Listen(1);
                return port;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
                return BindPort(socket, port + 1);
            }
        }


        private static void HandleMessageReceived(OnPlatformCallback callback)
        {
            Task.Run(() =>
            {
                while (null != _obj)
                {
                    var bytes = new byte[BufferSize];
                    var bytesRec = _socket.Receive(bytes);
                    var message = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    var decode = Hex.Decode(message);
                    PlatformLog.D($"Client Received: {message} , Decode: {decode}");
                    callback.Invoke(decode);
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }

        private static void SendMessage(JObject jObject)
        {
            try
            {
                var message = jObject.ToString(Formatting.None);
                var encode = Hex.Encode(message);
                PlatformLog.D($"Client sent: {message} , Encode: {encode}");
                var bytes = Encoding.UTF8.GetBytes(encode);
                _socket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.WriteLine(e);
            }
        }

        /**
         * 登录
         */
        public static void Login()
        {
            SendMessage(new JObject { { "opcode", "Login" } });
        }


        /**
         * 服务器列表
         */
        public static void Servers()
        {
            SendMessage(new JObject { { "opcode", "Servers" } });
        }

        /**
         * 获取服务器
         * 调用位置：玩家选择该服务器需要进入时，用于获取IP端口等信息
         */
        public static void Server(int serverId)
        {
            SendMessage(new JObject
            {
                { "opcode", "Server" },
                { "serverId", serverId }
            });
        }

        /**
         * 角色列表
         */
        public static void Roles()
        {
            SendMessage(new JObject { { "opcode", "Roles" } });
        }

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
         */
        public static void GameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel,
            string roleCreateTime)
        {
            SendMessage(new JObject
            {
                { "opcode", "GameStarted" },
                { "newRole", newRole },
                { "roleId", roleId },
                { "roleName", roleName },
                { "roleLevel", roleLevel },
                { "vipLevel", vipLevel },
                { "roleCreateTime", roleCreateTime },
            });
        }


        /**
         * 角色更新
         * 调用位置：以下角色任一数据发生更新时调用，未变化的参数也需要传入
         *
         * @param roleName  角色名称
         * @param roleLevel 角色等级
         * @param vipLevel  vip等级
         */
        public static void RoleUpdate(string roleName, int roleLevel, int vipLevel)
        {
            SendMessage(new JObject
            {
                { "opcode", "RoleUpdate" },
                { "roleName", roleName },
                { "roleLevel", roleLevel },
                { "vipLevel", vipLevel },
            });
        }

        /**
         * 支付
         *
         * @param data 游戏服务器在平台下单后获取的 platformOrderData
         *             字段最大长度 1024个字符
         */
        public static void Pay(string data)
        {
            SendMessage(new JObject
            {
                { "opcode", "Pay" },
                { "data", data },
            });
        }

        /**
         * 恢复购买，海外游戏专用
         */
        public static void RenewPay()
        {
            SendMessage(new JObject { { "opcode", "RenewPay" } });
        }


        /**
         * 商品列表
         */
        public static void Products()
        {
            SendMessage(new JObject { { "opcode", "Products" } });
        }

        /**
         * 登出账号
         */
        public static void Logout()
        {
            SendMessage(new JObject { { "opcode", "Logout" } });
        }

        /**
         * 退出游戏
         */
        public static void ExitGame()
        {
            SendMessage(new JObject { { "opcode", "ExitGame" } });
        }

        /**
         * 绑定账户
         */
        public static void Bind()
        {
            SendMessage(new JObject { { "opcode", "Bind" } });
        }

        public static bool BindAvailable()
        {
            return _obj.ContainsKey("bindAvailable") && (bool)_obj["bindAvailable"];
        }

        /**
         * 删除账户
         */
        public static void Del()
        {
            SendMessage(new JObject { { "opcode", "Del" } });
        }

        public static bool DelAvailable()
        {
            return _obj.ContainsKey("delAvailable") && (bool)_obj["delAvailable"];
        }

        /**
         * 客服
         */
        public static void Customer(string param)
        {
            SendMessage(new JObject
            {
                { "opcode", "Customer" },
                { "param", param },
            });
        }

        public static bool CustomerAvailable()
        {
            return _obj.ContainsKey("customerAvailable") && (bool)_obj["customerAvailable"];
        }

        /**
         * 公告
         */
        public static void Placard()
        {
            SendMessage(new JObject { { "opcode", "Placard" } });
        }

        public static bool PlacardAvailable()
        {
            return _obj.ContainsKey("placardAvailable") && (bool)_obj["placardAvailable"];
        }

        public static bool PlacardUpdated()
        {
            return _obj.ContainsKey("placardUpdated") && (bool)_obj["placardUpdated"];
        }

        /**
         * 礼包码
         */
        public static void CdKey()
        {
            SendMessage(new JObject { { "opcode", "CdKey" } });
        }

        public static bool CdKeyAvailable()
        {
            return _obj.ContainsKey("cdKeyAvailable") && (bool)_obj["cdKeyAvailable"];
        }

        /**
         * 调查问卷
         */
        public static void Survey()
        {
            SendMessage(new JObject { { "opcode", "Survey" } });
        }

        public static bool SurveyAvailable()
        {
            return _obj.ContainsKey("surveyAvailable") && (bool)_obj["surveyAvailable"];
        }

        /**
         * 分享
         *
         * @param method 分享方式
         * @param json   分享的数据json
         */
        public static void Share(string method, string json)
        {
            SendMessage(new JObject
            {
                { "opcode", "Share" },
                { "method", method },
                { "json", json },
            });
        }

        /**
         * 分享方式列表
         *
         * @return 用, 分割分享列表
         */
        public static string ShareMethods()
        {
            return (string)(null == _obj ? "" : _obj["shareMethods"]);
        }

        public static bool ShareAvailable()
        {
            return _obj.ContainsKey("shareAvailable") && (bool)_obj["shareAvailable"];
        }

        /**
         * 显示条款
         *
         * @param type 条款类型
         */
        public static void ShowTerms(string type)
        {
            SendMessage(new JObject
            {
                { "opcode", "ShowTerms" },
                { "type", type },
            });
        }

        /**
         * 杀死游戏
         */
        public static void KillGame()
        {
            SendMessage(new JObject { { "opcode", "KillGame" } });
        }

        /**
         * 显示吐司
         */
        public static void ShowToast(string text)
        {
            SendMessage(new JObject
            {
                { "opcode", "ShowToast" },
                { "text", text },
            });
        }

        /**
         * 客户端事件打点
         */
        public static void EventClient(string eventKey, string eventValue)
        {
            SendMessage(new JObject
            {
                { "opcode", "EventClient" },
                { "eventKey", eventKey },
                { "eventValue", eventValue },
            });
        }

        /**
         * 三方事件打点
         */
        public static void EventTripartite(string eventKey, string eventValue)
        {
            SendMessage(new JObject
            {
                { "opcode", "EventTripartite" },
                { "eventKey", eventKey },
                { "eventValue", eventValue },
            });
        }

        /**
         * 程序日志
         *
         * @param level 日志等级，支持debug,info,warn,error
         * @param tag   日志标识
         * @param log   日志内容
         */
        public static void Logger(string level, string tag, string log)
        {
            SendMessage(new JObject
            {
                { "opcode", "Logger" },
                { "level", level },
                { "tag", tag },
                { "log", log },
            });
        }

        /**
         * 打开网页
         *
         * @param url   网页地址
         * @param param 网页参数
         */
        public static void Webview(string url, string param)
        {
            SendMessage(new JObject
            {
                { "opcode", "Webview" },
                { "url", url },
                { "param", param },
            });
        }

        /**
         * 网址是否可见
         *
         * @param url 网页地址
         * @return 可见
         */
        public static bool WebviewAvailable(string url)
        {
            if (url.StartsWith("https://") || url.StartsWith("http://")) return true;
            var key = "webviewAvailable-" + url;
            return _obj.ContainsKey(key) && (bool)_obj[key];
        }

        /**
         * 扫码登录
         */
        public static void QrLogin()
        {
            SendMessage(new JObject { { "opcode", "QrLogin" } });
        }

        /**
         * 扫码是否可见
         */
        public static bool QrLoginAvailable()
        {
            return _obj.ContainsKey("qrLoginAvailable") && (bool)_obj["qrLoginAvailable"];
        }


        /**
     * @return 地区
     */
        public static string Area()
        {
            return (string)(null == _obj ? "" : _obj["area"]);
        }

        /**
         * @return 应用ID
         */
        public static int AppId()
        {
            return (int)(null == _obj ? 0 : _obj["appId"]);
        }

        /**
         * @return 渠道ID
         */
        public static string ChannelId()
        {
            return (string)(null == _obj ? "" : _obj["channelId"]);
        }

        /**
         * @return 设备ID
         */
        public static string DeviceId()
        {
            return (string)(null == _obj ? "" : _obj["deviceId"]);
        }

        /**
         * @return 追踪ID
         */
        public static string TrackId()
        {
            return (string)(null == _obj ? "" : _obj["trackId"]);
        }

        /**
         * @return SDK的url
         */
        public static string SdkHost()
        {
            return (string)(null == _obj ? "" : _obj["sdkHost"]);
        }

        /**
         * @return 广告ID
         */
        public static string AdvertisingId()
        {
            return (string)(null == _obj ? "" : _obj["advertisingId"]);
        }

        /**
         * @return 手机剩余空间
         */
        public static long FreeSpace()
        {
            return (long)(null == _obj ? 0 : _obj["freeSpace"]);
        }

        /**
         * @return 手机语言
         */
        public static string Language()
        {
            return (string)(null == _obj ? "" : _obj["language"]);
        }

        /**
         * @return 显示版本
         */
        public static string VersionName()
        {
            return (string)(null == _obj ? "" : _obj["versionName"]);
        }

        /**
         * @return 商店版本
         */
        public static int VersionCode()
        {
            return (int)(null == _obj ? 0 : _obj["versionCode"]);
        }

        /**
         * @return 提审模式
         */
        public static bool AppReview()
        {
            return (bool)(null == _obj ? false : _obj["appReview"]);
        }

        /**
         * @return 测试模式
         */
        public static bool AppDebug()
        {
            return (bool)(null == _obj ? false : _obj["appDebug"]);
        }


        private static JObject MemoryMappedJObject()
        {
            try
            {
                using var mmf = MemoryMappedFile.CreateOrOpen("SdkPlatform", 10240);
                using var accessor = mmf.CreateViewAccessor();
                // 读取字符
                var strLength = accessor.ReadInt32(0);
                var charsInMMf = new char[strLength];
                accessor.ReadArray(4, charsInMMf, 0, strLength);
                var s = new string(charsInMMf);
                return JObject.Parse(s);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
#endif
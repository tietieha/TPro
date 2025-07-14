#if UNITY_IOS

using System.Runtime.InteropServices;
using Platform.Api;

namespace Platform.Impl
{
    public class ApiImplIos : ApiInterface
    {
        [DllImport("__Internal")]
        private static extern void _initSDK();

        [DllImport("__Internal")]
        private static extern void _login();

        [DllImport("__Internal")]
        private static extern void _servers();

        [DllImport("__Internal")]
        private static extern void _server(int serverId);

        [DllImport("__Internal")]
        private static extern void _roles();

        [DllImport("__Internal")]
        private static extern void _gameStarted(bool newRole, string roleId, string roleName, int roleLevel,
            int vipLevel, string roleCreateTime, string roleData);

        [DllImport("__Internal")]
        private static extern void _roleUpdate(string roleName, int roleLevel, int vipLevel, string roleData);

        [DllImport("__Internal")]
        private static extern void _pay(string data);

        [DllImport("__Internal")]
        private static extern void _renewPay();

        [DllImport("__Internal")]
        private static extern void _products();

        [DllImport("__Internal")]
        private static extern void _logout();

        [DllImport("__Internal")]
        private static extern void _exitGame();

        [DllImport("__Internal")]
        private static extern void _bind();

        [DllImport("__Internal")]
        private static extern bool _bindAvailable();

        [DllImport("__Internal")]
        private static extern void _del();

        [DllImport("__Internal")]
        private static extern bool _delAvailable();

        [DllImport("__Internal")]
        private static extern void _customer(string param);

        [DllImport("__Internal")]
        private static extern bool _customerAvailable();

        [DllImport("__Internal")]
        private static extern void _placard();

        [DllImport("__Internal")]
        private static extern bool _placardAvailable();

        [DllImport("__Internal")]
        private static extern bool _placardUpdated();

        [DllImport("__Internal")]
        private static extern void _cdKey();

        [DllImport("__Internal")]
        private static extern bool _cdKeyAvailable();

        [DllImport("__Internal")]
        private static extern void _survey();

        [DllImport("__Internal")]
        private static extern bool _surveyAvailable();

        [DllImport("__Internal")]
        private static extern void _share(string method, string json);       
        [DllImport("__Internal")]
        private static extern string _shareMethods();

        [DllImport("__Internal")]
        private static extern bool _shareAvailable();

        [DllImport("__Internal")]
        private static extern void _showTerms(string type);

        [DllImport("__Internal")]
        private static extern void _killGame();

        [DllImport("__Internal")]
        private static extern void _showToast(string text);

        [DllImport("__Internal")]
        private static extern void _eventClient(string eventKey, string eventValue);

        [DllImport("__Internal")]
        private static extern void _eventTripartite(string eventKey, string eventValue);

        [DllImport("__Internal")]
        private static extern void _logger(string level, string tag, string log);

        [DllImport("__Internal")]
        private static extern void _webview(string url, string param);

        [DllImport("__Internal")]
        private static extern bool _webviewAvailable(string url);
        [DllImport("__Internal")]
        private static extern void _qrLogin();
        
        [DllImport("__Internal")]
        private static extern bool _qrLoginAvailable();

        public override void initSDK(PlatformCallback platformCallback)
        {
            Callback = platformCallback;
            ResultCallback.InitCallback();
            _initSDK();
        }

        public override void login()
        {
            _login();
        }

        public override void servers()
        {
            _servers();
        }

        public override void server(int serverId)
        {
            _server(serverId);
        }

        public override void roles()
        {
            _roles();
        }

        public override void gameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel,
            string roleCreateTime, string roleData)
        {
            _gameStarted(newRole, roleId, roleName, roleLevel, vipLevel, roleCreateTime, roleData);
        }

        public override void roleUpdate(string roleName, int roleLevel, int vipLevel, string roleData)
        {
            _roleUpdate(roleName, roleLevel, vipLevel, roleData);
        }

        public override void pay(string data)
        {
            _pay(data);
        }

        public override void renewPay()
        {
            _renewPay();
        }

        public override void products()
        {
            _products();
        }

        public override void logout()
        {
            _logout();
        }

        public override void exitGame()
        {
            _exitGame();
        }

        public override void bind()
        {
            _bind();
        }

        public override bool bindAvailable()
        {
            return _bindAvailable();
        }

        public override void del()
        {
            _del();
        }

        public override bool delAvailable()
        {
            return _delAvailable();
        }

        public override void customer(string param)
        {
            _customer();
        }

        public override bool customerAvailable()
        {
            return _customerAvailable();
        }

        public override void placard()
        {
            _placard();
        }

        public override bool placardAvailable()
        {
            return _placardAvailable();
        }

        public override bool placardUpdated()
        {
            return _placardUpdated();
        }

        public override void cdKey()
        {
            _cdKey();
        }

        public override bool cdKeyAvailable()
        {
            return _cdKeyAvailable();
        }

        public override void survey()
        {
            _survey();
        }

        public override bool surveyAvailable()
        {
            return _surveyAvailable();
        }

        public override void share(string method, string json)
        {
            _share(method, json);
        }

        public override string shareMethods()
        {
            return _shareMethods();
        }

        public override bool shareAvailable()
        {
            return _shareAvailable();
        }

        public override void showTerms(string type)
        {
            _showTerms(type);
        }

        public override void killGame()
        {
            _killGame();
        }

        public override void showToast(string text)
        {
            _showToast(text);
        }

        public override void eventClient(string eventKey, string eventValue)
        {
            _eventClient(eventKey, eventValue);
        }

        public override void eventTripartite(string eventKey, string eventValue)
        {
            _eventTripartite(eventKey, eventValue);
        }

        public override void logger(string level, string tag, string log)
        {
            _logger(level, tag, log);
        }

        public override void webview(string url, string param)
        {
            _webview(url, param);
        }

        public override bool webviewAvailable(string url)
        {
            return _webviewAvailable(url);
        }

        public override void qrLogin()
        {
            _qrLogin();
        }

        public override bool qrLoginAvailable()
        {
            return _qrLoginAvailable();
        }
    }
}

#endif
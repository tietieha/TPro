#if UNITY_STANDLONE || UNITY_STANDALONE_WIN

using System;
using Platform.Api;
using Platform.Other;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityBridge = Platform.Other.UnityBridge;

namespace Platform.Impl
{
    public class ApiImplWin : ApiInterface
    {
        private static readonly PlatformMainThreadDispatcher Dispatcher =
            new GameObject("PlatformMainThreadDispatcher").AddComponent<PlatformMainThreadDispatcher>();

        public ApiImplWin()
        {
            Environment.SetEnvironmentVariable("SDK_PLATFORM_ENV",
                Application.isEditor ? "UNITY_EDITOR" : "UNITY_RUNTIME");
        }

        public override void initSDK(PlatformCallback platformCallback)
        {
            Callback = platformCallback;
            ResultCallback.InitCallback();
            UnityBridge.InitSdk(jsonResult =>
            {
                Dispatcher.Enqueue(() => { ResultCallback.Instance.onResult(jsonResult); });
            });
        }

        public override void login()
        {
            UnityBridge.Login();
        }

        public override void servers()
        {
            UnityBridge.Servers();
        }

        public override void server(int serverId)
        {
            UnityBridge.Server(serverId);
        }

        public override void roles()
        {
            UnityBridge.Roles();
        }

        public override void gameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel,
            string roleCreateTime, string roleData)
        {
            UnityBridge.GameStarted(newRole, roleId, roleName, roleLevel, vipLevel, roleCreateTime);
        }

        public override void roleUpdate(string roleName, int roleLevel, int vipLevel, string roleData)
        {
            UnityBridge.RoleUpdate(roleName, roleLevel, vipLevel);
        }

        public override void pay(string data)
        {
            UnityBridge.Pay(data);
        }

        public override void renewPay()
        {
            UnityBridge.RenewPay();
        }

        public override void products()
        {
            UnityBridge.Products();
        }

        public override void logout()
        {
            UnityBridge.Logout();
        }

        public override void exitGame()
        {
            UnityBridge.ExitGame();
        }

        public override void bind()
        {
            UnityBridge.Bind();
        }

        public override bool bindAvailable()
        {
            return UnityBridge.BindAvailable();
        }

        public override void del()
        {
            UnityBridge.Del();
        }

        public override bool delAvailable()
        {
            return UnityBridge.DelAvailable();
        }

        public override void customer(string param)
        {
            UnityBridge.Customer(param);
        }

        public override bool customerAvailable()
        {
            return UnityBridge.CustomerAvailable();
        }

        public override void placard()
        {
            UnityBridge.Placard();
        }

        public override bool placardAvailable()
        {
            return UnityBridge.PlacardAvailable();
        }

        public override bool placardUpdated()
        {
            return UnityBridge.PlacardUpdated();
        }

        public override void cdKey()
        {
            UnityBridge.CdKey();
        }

        public override bool cdKeyAvailable()
        {
            return UnityBridge.CdKeyAvailable();
        }

        public override void survey()
        {
            UnityBridge.Survey();
        }

        public override bool surveyAvailable()
        {
            return UnityBridge.SurveyAvailable();
        }

        public override void share(string method, string json)
        {
            Debug.Log("分享：" + method + json);
            UnityBridge.Share(method, json);
        }

        public override string shareMethods()
        {
            return UnityBridge.ShareMethods();
        }

        public override bool shareAvailable()
        {
            return UnityBridge.ShareAvailable();
        }

        public override void showTerms(string type)
        {
            UnityBridge.ShowTerms(type);
        }

        public override void killGame()
        {
            UnityBridge.KillGame();
        }

        public override void showToast(string text)
        {
            UnityBridge.ShowToast(text);
        }

        public override void eventClient(string eventKey, string eventValue)
        {
            UnityBridge.EventClient(eventKey, eventValue);
        }

        public override void eventTripartite(string eventKey, string eventValue)
        {
            UnityBridge.EventTripartite(eventKey, eventValue);
        }

        public override void logger(string level, string tag, string log)
        {
            UnityBridge.Logger(level, tag, log);
        }

        public override void webview(string url, string param)
        {
            UnityBridge.Webview(url, param);
        }

        public override bool webviewAvailable(string url)
        {
            return UnityBridge.WebviewAvailable(url);
        }

        public override void qrLogin()
        {
            UnityBridge.QrLogin();
        }

        public override bool qrLoginAvailable()
        {
            return UnityBridge.QrLoginAvailable();
        }
    }
}
#endif
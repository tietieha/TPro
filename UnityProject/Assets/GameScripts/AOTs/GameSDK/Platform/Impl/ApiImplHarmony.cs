#if UNITY_OPENHARMONY

using System;
using System.Reflection;
using Platform.Api;
using UnityEngine;

namespace Platform.Impl
{
    public class ApiImplHarmony : ApiInterface
    {

        private readonly OpenHarmonyJSObject _openHarmonyJSObject =
            new OpenHarmonyJSObject("UnityApiBridge");

        private T SDKStaticCall<T>(string method, params object[] param)
        {
            try
            {
                return _openHarmonyJSObject.Call<T>(method, param);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return default(T);
        }

        private void SDKStaticCall(string method, params object[] param)
        {
            try
            {
                _openHarmonyJSObject.Call(method, param);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public override void initSDK(PlatformCallback resultCallback)
        {
            Callback = resultCallback;
            ResultCallback.InitCallback();
            SDKStaticCall("initSDK");
        }

        public override void login()
        {
            SDKStaticCall("login");
        }

        public override void logout()
        {
            SDKStaticCall("logout");
        }

        public override void exitGame()
        {
            SDKStaticCall("exitGame");
        }

        public override void servers()
        {
            SDKStaticCall("servers");
        }

        public override void server(int serverId)
        {
            SDKStaticCall("server", serverId);
        }

        public override void roles()
        {
            SDKStaticCall("roles");
        }

        public override void gameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel,
            string roleCreateTime, string roleData)
        {
            SDKStaticCall("gameStarted", newRole, roleId, roleName, roleLevel, vipLevel, roleCreateTime, roleData);
        }

        public override void roleUpdate(string roleName, int roleLevel, int vipLevel, string roleData)
        {
            SDKStaticCall("roleUpdate", roleName, roleLevel, vipLevel, roleData);
        }

        public override void pay(string data)
        {
            SDKStaticCall("pay", data);
        }


        public override void renewPay()
        {
            SDKStaticCall("renewPay");
        }

        public override void products()
        {
            SDKStaticCall("products");
        }

        public override void bind()
        {
            SDKStaticCall("bind");
        }

        public override bool bindAvailable()
        {
            return SDKStaticCall<bool>("bindAvailable");
        }

        public override void del()
        {
            SDKStaticCall("del");
        }

        public override bool delAvailable()
        {
            return SDKStaticCall<bool>("delAvailable");
        }

        public override void customer(string param)
        {
            SDKStaticCall("customer");
        }

        public override bool customerAvailable()
        {
            return SDKStaticCall<bool>("customerAvailable");
        }

        public override void placard()
        {
            SDKStaticCall("placard");
        }

        public override bool placardAvailable()
        {
            return SDKStaticCall<bool>("placardAvailable");
        }

        public override bool placardUpdated()
        {
            return SDKStaticCall<bool>("placardUpdated");
        }

        public override void cdKey()
        {
            SDKStaticCall("cdKey");
        }

        public override bool cdKeyAvailable()
        {
            return SDKStaticCall<bool>("cdKeyAvailable");
        }

        public override void survey()
        {
            SDKStaticCall("survey");
        }

        public override bool surveyAvailable()
        {
            return SDKStaticCall<bool>("surveyAvailable");
        }

        public override void share(string method, string json)
        {
            SDKStaticCall("share", method, json);
        }

        public override string shareMethods()
        {
            return SDKStaticCall<string>("shareMethods");
        }

        public override bool shareAvailable()
        {
            return SDKStaticCall<bool>("shareAvailable");
        }

        public override void showTerms(string type)
        {
            SDKStaticCall("showTerms", type);
        }

        public override void killGame()
        {
            SDKStaticCall("killGame");
        }

        public override void showToast(string text)
        {
            SDKStaticCall("showToast", text);
        }

        public override void eventClient(string eventKey, string eventValue)
        {
            SDKStaticCall("eventClient", eventKey, eventValue);
        }

        public override void eventTripartite(string eventKey, string eventValue)
        {
            SDKStaticCall("eventTripartite", eventKey, eventValue);
        }

        public override void logger(string level, string tag, string log)
        {
            SDKStaticCall("logger", level, tag, log);
        }

        public override void webview(string url, string param)
        {
            SDKStaticCall("webview", url ,param);
        }

        public override bool webviewAvailable(string url)
        {
            return SDKStaticCall<bool>("webviewAvailable", url);
        }

        public override void qrLogin()
        {
            SDKStaticCall("qrLogin");
        }

        public override bool qrLoginAvailable()
        {
            return SDKStaticCall<bool>("qrLoginAvailable");
        }
    }
}

#endif
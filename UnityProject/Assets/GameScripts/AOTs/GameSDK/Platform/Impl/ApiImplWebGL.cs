#if UNITY_WEBGL

using System;
using System.Reflection;
using Platform.Api;
using UnityEngine;
using System.Runtime.InteropServices;


namespace Platform.Impl
{
    public class ApiImplWebGL : ApiInterface
    {
        [DllImport("__Internal")]
        private static extern void SetUUID(string str);
        [DllImport("__Internal")]
        private static extern string GetUUID();

        [DllImport("__Internal")]
        private static extern void INITSDK();
        [DllImport("__Internal")]
        private static extern void LOGIN();
        [DllImport("__Internal")]
        private static extern void LOGOUT();
        [DllImport("__Internal")]
        private static extern void SERVERS();
        [DllImport("__Internal")]
        private static extern void SERVER(int serverId);
        [DllImport("__Internal")]
        private static extern void ROLES();
        [DllImport("__Internal")]
        private static extern void STARTGAME(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel,
            string roleCreateTime, string roleData);
        [DllImport("__Internal")]
        private static extern void UPDATEROLE(string roleName, int roleLevel, int vipLevel, string roleData);
        [DllImport("__Internal")]
        private static extern void PRODUCTS();
        [DllImport("__Internal")]
        private static extern void PAY(string data);
        [DllImport("__Internal")]
        private static extern void EXITGAME();
        [DllImport("__Internal")]
        private static extern void ACCOUNTDELETE();
        [DllImport("__Internal")]
        private static extern void CUSTOMER();
        [DllImport("__Internal")]
        private static extern void SHOWTERMS(string type);

        [DllImport("__Internal")]
        private static extern void EVENTCLIENT(string eventKey,string eventValue);
        [DllImport("__Internal")]
        private static extern void EVENTTRIPARTITE(string eventKey, string eventValue);

        [DllImport("__Internal")]
        private static extern void CDKEY();
        [DllImport("__Internal")]// cdkeyAvailable
        private static extern bool CDKEYAVAILABLE();
        [DllImport("__Internal")]
        private static extern void SHOWTOAST(string msg);




        private void wxcakkback(string jsonParam)
        {
            Debug.Log("Unity get callback :"+ jsonParam);
            SdkPlatform.api().Callback(jsonParam);
        }


        public override void initSDK(PlatformCallback resultCallback)
        {
            Callback = resultCallback;
            ResultCallback.InitCallback();
            INITSDK();

        }

        public override void login()
        {
            LOGIN();
        }

        public override void logout()
        {
            LOGOUT();
        }

        public override void exitGame()
        {
            EXITGAME();
        }

        public override void servers()
        {
            SERVERS();
        }

        public override void server(int serverId)
        {
            SERVER(serverId);
        }

        public override void roles()
        {
            ROLES();
        }

        public override void gameStarted(bool newRole, string roleId, string roleName, int roleLevel, int vipLevel,
            string roleCreateTime, string roleData)
        {
            STARTGAME(newRole, roleId, roleName, roleLevel, vipLevel, roleCreateTime, roleData);
        }

        public override void roleUpdate(string roleName, int roleLevel, int vipLevel, string roleData)
        {
            UPDATEROLE(roleName, roleLevel, vipLevel, roleData);
        }

        public override void pay(string data)
        {
            PAY(data);   
        }


        public override void renewPay()
        {

        }

        public override void products()
        {
            PRODUCTS();
        }

        public override void bind()
        {

        }

        public override bool bindAvailable()
        {
            return false;
        }

        public override void del()
        {
            ACCOUNTDELETE();
        }

        public override bool delAvailable()
        {
            return true;
        }

        public override void customer(string param)
        {
            CUSTOMER();
        }

        public override bool customerAvailable()
        {
            return true;
        }

        public override void placard()
        {

        }

        public override bool placardAvailable()
        {
            return false;
        }

        public override bool placardUpdated()
        {
            return false;
        }

        public override void cdKey()
        {
            CDKEY();
        }

        public override bool cdKeyAvailable()
        {
            return CDKEYAVAILABLE();
            return false;
        }

        public override void survey()
        {

        }

        public override bool surveyAvailable()
        {
            return false;
        }

        public override void share(string method, string json)
        {

        }

        public override string shareMethods()
        {
            return "";
        }

        public override bool shareAvailable()
        {
            return false;
        }

        public override void showTerms(string type)
        {
            SHOWTERMS(type);
        }

        public override void killGame()
        {

        }

        public override void showToast(string text)
        {
            SHOWTOAST(text);
        }

        public override void eventClient(string eventKey, string eventValue)
        {
            EVENTCLIENT(eventKey, eventValue);
        }

        public override void eventTripartite(string eventKey, string eventValue)
        {
            EVENTTRIPARTITE(eventKey, eventValue);
        }

        public override void logger(string level, string tag, string log)
        {

        }

        public override void webview(string url, string param)
        {

        }

        public override bool webviewAvailable(string url)
        {
            return false;
        }

        public override void qrLogin()
        {
        }

        public override bool qrLoginAvailable()
        {
            return false;
        }
    }
}

#endif
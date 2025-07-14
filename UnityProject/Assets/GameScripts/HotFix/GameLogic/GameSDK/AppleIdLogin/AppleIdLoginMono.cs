using System;
using UnityEngine;

namespace SDK.AppleIdLogin
{
    public class AppleIdLoginMono : MonoBehaviour
    {
        public static AppleIdLoginMono Instance;

        // private IAppleAuthManager _appleAuthManager;

        private static string _tag = "[AppleIdLogin]";

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // if (AppleAuthManager.IsCurrentPlatformSupported == false)
            //     return;
            //
            // var deserializer = new PayloadDeserializer();
            // _appleAuthManager = new AppleAuthManager(deserializer);

            InitializeLoginMenu();
        }

        private void Update()
        {
            // _appleAuthManager?.Update();
        }

        private void InitializeLoginMenu()
        {
            // if (_appleAuthManager == null)
            // {
            //     Debug.Log(_tag + "unsupported Platform !");
            //     return;
            // }
            //
            // _appleAuthManager.SetCredentialsRevokedCallback(result =>
            // {
            //     Debug.Log(_tag + "Received revoked callback " + result);
            // });
        }

        public void SignInWithApple(Action<bool, string> callback)
        {
            // if (AppleAuthManager.IsCurrentPlatformSupported == false)
            // {
            //     Debug.LogError(_tag + "SignInWithApple Error, Current Platform Not Supported");
            //     return;
            // }
            //
            // var loginArgs = new AppleAuthLoginArgs(LoginOptions.None);
            //
            // _appleAuthManager.LoginWithAppleId(
            //     loginArgs,
            //     credential => { callback(true, credential.User); },
            //     error =>
            //     {
            //         var authorizationErrorCode = error.GetAuthorizationErrorCode();
            //         Debug.LogError(_tag + $"authorizationErrorCode {authorizationErrorCode}");
            //         callback(false, "");
            //     });
        }
    }
}
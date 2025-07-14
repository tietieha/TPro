// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-06-04       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using System.Reflection;
using UnityEngine;

namespace TEngine
{
    public class SDKModule : Module
    {
        [Header("是否开启错误上报")]
        public bool OpenExceptionUpLoad = false;

        private SDKModuleImp _sdkModuleImp;
        internal SDKModuleImp SdkModuleImp
        {
            get
            {
                if (_sdkModuleImp == null)
                {
                    Log.Error("SDK ModuleImp is invalid.");
                    return null;
                }
                return _sdkModuleImp;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            _sdkModuleImp = ModuleImpSystem.GetModule<SDKModuleImp>();
            if (_sdkModuleImp == null)
            {
                Log.Fatal("SDK ModuleImp is invalid.");
                return;
            }
        }

        public override void Init()
        {
            base.Init();
            InitSDK();
        }

        public void InitSDK()
        {
            try
            {
                _sdkModuleImp.Init();
            }
            catch (Exception e)
            {
                Log.Error($"SDK Module: InitSDK failed with exception: {e.Message}");
                throw;
            }

        }

        public bool IsInitSuccess()
        {
            return _sdkModuleImp.IsInitSuccess;
        }


    }
}
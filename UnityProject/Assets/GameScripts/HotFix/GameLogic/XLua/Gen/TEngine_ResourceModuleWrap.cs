#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLuaBase.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class TEngineResourceModuleWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(TEngine.ResourceModule);
			Utils.BeginObjectRegister(type, L, translator, 0, 29, 30, 20);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitPackage", _m_InitPackage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPackageVersion", _m_GetPackageVersion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdatePackageVersionAsync", _m_UpdatePackageVersionAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdatePackageManifestAsync", _m_UpdatePackageManifestAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateResourceDownloader", _m_CreateResourceDownloader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearUnusedCacheFilesAsync", _m_ClearUnusedCacheFilesAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearSandbox", _m_ClearSandbox);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSandboxConfigVersion", _m_GetSandboxConfigVersion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPackageConfigVersion", _m_GetPackageConfigVersion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CompareVersion", _m_CompareVersion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSandboxConfigFileList", _m_GetSandboxConfigFileList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPackageConfigFileList", _m_GetPackageConfigFileList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SaveConfigVersion", _m_SaveConfigVersion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitConfig", _m_InitConfig);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasAsset", _m_HasAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckLocationValid", _m_CheckLocationValid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAssetInfos", _m_GetAssetInfos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAssetInfo", _m_GetAssetInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaLoad", _m_SetLuaLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAsyncLua", _m_LoadAsyncLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CancelLoad", _m_CancelLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSyncLua", _m_LoadSyncLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAssetAsync", _m_LoadAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadGameObject", _m_LoadGameObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadGameObjectAsync", _m_LoadGameObjectAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAsset", _m_UnloadAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceUnloadUnusedAssets", _m_ForceUnloadUnusedAssets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadUnusedAssets", _m_UnloadUnusedAssets);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "PackageVersion", _g_get_PackageVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RemoteConfigVersion", _g_get_RemoteConfigVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ConfigVersionStr", _g_get_ConfigVersionStr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ConfigVersion", _g_get_ConfigVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ConfigPackageVersion", _g_get_ConfigPackageVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ConfigSandboxVersion", _g_get_ConfigSandboxVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ConfigMode", _g_get_ConfigMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PackageName", _g_get_PackageName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlayMode", _g_get_PlayMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UpdatableWhilePlaying", _g_get_UpdatableWhilePlaying);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DownloadingMaxNum", _g_get_DownloadingMaxNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FailedTryAgain", _g_get_FailedTryAgain);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ApplicableGameVersion", _g_get_ApplicableGameVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InternalResourceVersion", _g_get_InternalResourceVersion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReadWritePathType", _g_get_ReadWritePathType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MinUnloadUnusedAssetsInterval", _g_get_MinUnloadUnusedAssetsInterval);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxUnloadUnusedAssetsInterval", _g_get_MaxUnloadUnusedAssetsInterval);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseSystemUnloadUnusedAssets", _g_get_UseSystemUnloadUnusedAssets);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LastUnloadUnusedAssetsOperationElapseSeconds", _g_get_LastUnloadUnusedAssetsOperationElapseSeconds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReadOnlyPath", _g_get_ReadOnlyPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReadWritePath", _g_get_ReadWritePath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetAutoReleaseInterval", _g_get_AssetAutoReleaseInterval);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetCapacity", _g_get_AssetCapacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetExpireTime", _g_get_AssetExpireTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetPriority", _g_get_AssetPriority);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Downloader", _g_get_Downloader);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VerifyLevel", _g_get_VerifyLevel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Milliseconds", _g_get_Milliseconds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_DownloadingMaxNum", _g_get_m_DownloadingMaxNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_FailedTryAgain", _g_get_m_FailedTryAgain);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "PackageVersion", _s_set_PackageVersion);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RemoteConfigVersion", _s_set_RemoteConfigVersion);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ConfigVersion", _s_set_ConfigVersion);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ConfigMode", _s_set_ConfigMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PackageName", _s_set_PackageName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PlayMode", _s_set_PlayMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DownloadingMaxNum", _s_set_DownloadingMaxNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FailedTryAgain", _s_set_FailedTryAgain);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MinUnloadUnusedAssetsInterval", _s_set_MinUnloadUnusedAssetsInterval);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaxUnloadUnusedAssetsInterval", _s_set_MaxUnloadUnusedAssetsInterval);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseSystemUnloadUnusedAssets", _s_set_UseSystemUnloadUnusedAssets);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetAutoReleaseInterval", _s_set_AssetAutoReleaseInterval);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetCapacity", _s_set_AssetCapacity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetExpireTime", _s_set_AssetExpireTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetPriority", _s_set_AssetPriority);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Downloader", _s_set_Downloader);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "VerifyLevel", _s_set_VerifyLevel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Milliseconds", _s_set_Milliseconds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_DownloadingMaxNum", _s_set_m_DownloadingMaxNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_FailedTryAgain", _s_set_m_FailedTryAgain);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new TEngine.ResourceModule();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitPackage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _packageName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.InitPackage( _packageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.InitPackage(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.InitPackage!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPackageVersion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _customPackageName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPackageVersion( _customPackageName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.GetPackageVersion(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.GetPackageVersion!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdatePackageVersionAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    bool _appendTimeTicks = LuaAPI.lua_toboolean(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    string _customPackageName = LuaAPI.lua_tostring(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageVersionAsync( _appendTimeTicks, _timeout, _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    bool _appendTimeTicks = LuaAPI.lua_toboolean(L, 2);
                    int _timeout = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageVersionAsync( _appendTimeTicks, _timeout );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _appendTimeTicks = LuaAPI.lua_toboolean(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageVersionAsync( _appendTimeTicks );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageVersionAsync(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.UpdatePackageVersionAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdatePackageManifestAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    string _packageVersion = LuaAPI.lua_tostring(L, 2);
                    bool _autoSaveVersion = LuaAPI.lua_toboolean(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    string _customPackageName = LuaAPI.lua_tostring(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageManifestAsync( _packageVersion, _autoSaveVersion, _timeout, _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _packageVersion = LuaAPI.lua_tostring(L, 2);
                    bool _autoSaveVersion = LuaAPI.lua_toboolean(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageManifestAsync( _packageVersion, _autoSaveVersion, _timeout );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _packageVersion = LuaAPI.lua_tostring(L, 2);
                    bool _autoSaveVersion = LuaAPI.lua_toboolean(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageManifestAsync( _packageVersion, _autoSaveVersion );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _packageVersion = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.UpdatePackageManifestAsync( _packageVersion );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.UpdatePackageManifestAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateResourceDownloader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _customPackageName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.CreateResourceDownloader( _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.CreateResourceDownloader(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.CreateResourceDownloader!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearUnusedCacheFilesAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _customPackageName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.ClearUnusedCacheFilesAsync( _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        var gen_ret = gen_to_be_invoked.ClearUnusedCacheFilesAsync(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.ClearUnusedCacheFilesAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearSandbox(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _customPackageName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ClearSandbox( _customPackageName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.ClearSandbox(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.ClearSandbox!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSandboxConfigVersion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetSandboxConfigVersion(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPackageConfigVersion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetPackageConfigVersion(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CompareVersion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _v1 = LuaAPI.lua_tostring(L, 2);
                    string _v2 = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.CompareVersion( _v1, _v2 );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSandboxConfigFileList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetSandboxConfigFileList(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPackageConfigFileList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetPackageConfigFileList(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveConfigVersion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _version = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SaveConfigVersion( _version );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitConfig(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitConfig(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    string _customPackageName = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.HasAsset( _location, _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.HasAsset( _location );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.HasAsset!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckLocationValid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    string _customPackageName = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.CheckLocationValid( _location, _customPackageName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.CheckLocationValid( _location );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.CheckLocationValid!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetInfos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _resTag = LuaAPI.lua_tostring(L, 2);
                    string _customPackageName = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetAssetInfos( _resTag, _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _resTag = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAssetInfos( _resTag );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<string[]>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string[] _tags = (string[])translator.GetObject(L, 2, typeof(string[]));
                    string _customPackageName = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetAssetInfos( _tags, _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<string[]>(L, 2)) 
                {
                    string[] _tags = (string[])translator.GetObject(L, 2, typeof(string[]));
                    
                        var gen_ret = gen_to_be_invoked.GetAssetInfos( _tags );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.GetAssetInfos!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    string _customPackageName = LuaAPI.lua_tostring(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetAssetInfo( _location, _customPackageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAssetInfo( _location );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.GetAssetInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    TEngine.ResourceModule.LuaLoadAssetSuccessCbk _loadSuccessCbk = translator.GetDelegate<TEngine.ResourceModule.LuaLoadAssetSuccessCbk>(L, 2);
                    TEngine.ResourceModule.LuaLoadAssetFailureCbk _loadFailureCbk = translator.GetDelegate<TEngine.ResourceModule.LuaLoadAssetFailureCbk>(L, 3);
                    
                    gen_to_be_invoked.SetLuaLoad( _loadSuccessCbk, _loadFailureCbk );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsyncLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetName = LuaAPI.lua_tostring(L, 2);
                    System.Type _assetType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    int _guid = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.LoadAsyncLua( _assetName, _assetType, _guid );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CancelLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _guid = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.CancelLoad( _guid );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSyncLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetName = LuaAPI.lua_tostring(L, 2);
                    System.Type _assetType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    int _guid = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.LoadSyncLua( _assetName, _assetType, _guid );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& translator.Assignable<TEngine.LoadAssetCallbacks>(L, 4)&& translator.Assignable<object>(L, 5)&& (LuaAPI.lua_isnil(L, 6) || LuaAPI.lua_type(L, 6) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    System.Type _assetType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    TEngine.LoadAssetCallbacks _loadAssetCallbacks = (TEngine.LoadAssetCallbacks)translator.GetObject(L, 4, typeof(TEngine.LoadAssetCallbacks));
                    object _userData = translator.GetObject(L, 5, typeof(object));
                    string _packageName = LuaAPI.lua_tostring(L, 6);
                    
                    gen_to_be_invoked.LoadAssetAsync( _location, _assetType, _loadAssetCallbacks, _userData, _packageName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& translator.Assignable<TEngine.LoadAssetCallbacks>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    System.Type _assetType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    TEngine.LoadAssetCallbacks _loadAssetCallbacks = (TEngine.LoadAssetCallbacks)translator.GetObject(L, 4, typeof(TEngine.LoadAssetCallbacks));
                    object _userData = translator.GetObject(L, 5, typeof(object));
                    
                    gen_to_be_invoked.LoadAssetAsync( _location, _assetType, _loadAssetCallbacks, _userData );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& translator.Assignable<TEngine.LoadAssetCallbacks>(L, 4)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    System.Type _assetType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    TEngine.LoadAssetCallbacks _loadAssetCallbacks = (TEngine.LoadAssetCallbacks)translator.GetObject(L, 4, typeof(TEngine.LoadAssetCallbacks));
                    
                    gen_to_be_invoked.LoadAssetAsync( _location, _assetType, _loadAssetCallbacks );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<TEngine.LoadAssetCallbacks>(L, 5)&& translator.Assignable<object>(L, 6)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    System.Type _assetType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    int _priority = LuaAPI.xlua_tointeger(L, 4);
                    TEngine.LoadAssetCallbacks _loadAssetCallbacks = (TEngine.LoadAssetCallbacks)translator.GetObject(L, 5, typeof(TEngine.LoadAssetCallbacks));
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    string _packageName = LuaAPI.lua_tostring(L, 7);
                    
                    gen_to_be_invoked.LoadAssetAsync( _location, _assetType, _priority, _loadAssetCallbacks, _userData, _packageName );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<TEngine.LoadAssetCallbacks>(L, 5)&& translator.Assignable<object>(L, 6)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    System.Type _assetType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    int _priority = LuaAPI.xlua_tointeger(L, 4);
                    TEngine.LoadAssetCallbacks _loadAssetCallbacks = (TEngine.LoadAssetCallbacks)translator.GetObject(L, 5, typeof(TEngine.LoadAssetCallbacks));
                    object _userData = translator.GetObject(L, 6, typeof(object));
                    
                    gen_to_be_invoked.LoadAssetAsync( _location, _assetType, _priority, _loadAssetCallbacks, _userData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.LoadAssetAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadGameObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    string _packageName = LuaAPI.lua_tostring(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.LoadGameObject( _location, _parent, _packageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        var gen_ret = gen_to_be_invoked.LoadGameObject( _location, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.LoadGameObject( _location );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.LoadGameObject!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadGameObjectAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& translator.Assignable<System.Threading.CancellationToken>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    System.Threading.CancellationToken _cancellationToken;translator.Get(L, 4, out _cancellationToken);
                    string _packageName = LuaAPI.lua_tostring(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.LoadGameObjectAsync( _location, _parent, _cancellationToken, _packageName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& translator.Assignable<System.Threading.CancellationToken>(L, 4)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    System.Threading.CancellationToken _cancellationToken;translator.Get(L, 4, out _cancellationToken);
                    
                        var gen_ret = gen_to_be_invoked.LoadGameObjectAsync( _location, _parent, _cancellationToken );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        var gen_ret = gen_to_be_invoked.LoadGameObjectAsync( _location, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _location = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.LoadGameObjectAsync( _location );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to TEngine.ResourceModule.LoadGameObjectAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _asset = translator.GetObject(L, 2, typeof(object));
                    
                    gen_to_be_invoked.UnloadAsset( _asset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceUnloadUnusedAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _performGCCollect = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.ForceUnloadUnusedAssets( _performGCCollect );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadUnusedAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _performGCCollect = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.UnloadUnusedAssets( _performGCCollect );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PackageVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.PackageVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RemoteConfigVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.RemoteConfigVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ConfigVersionStr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ConfigVersionStr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ConfigVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ConfigVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ConfigPackageVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ConfigPackageVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ConfigSandboxVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ConfigSandboxVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ConfigMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ConfigMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PackageName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.PackageName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PlayMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UpdatableWhilePlaying(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UpdatableWhilePlaying);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DownloadingMaxNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DownloadingMaxNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FailedTryAgain(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.FailedTryAgain);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ApplicableGameVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ApplicableGameVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InternalResourceVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.InternalResourceVersion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReadWritePathType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ReadWritePathType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MinUnloadUnusedAssetsInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MinUnloadUnusedAssetsInterval);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxUnloadUnusedAssetsInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MaxUnloadUnusedAssetsInterval);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseSystemUnloadUnusedAssets(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseSystemUnloadUnusedAssets);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LastUnloadUnusedAssetsOperationElapseSeconds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LastUnloadUnusedAssetsOperationElapseSeconds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReadOnlyPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ReadOnlyPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReadWritePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ReadWritePath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetAutoReleaseInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.AssetAutoReleaseInterval);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.AssetCapacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetExpireTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.AssetExpireTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetPriority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.AssetPriority);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Downloader(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Downloader);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VerifyLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.VerifyLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Milliseconds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, gen_to_be_invoked.Milliseconds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_DownloadingMaxNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.m_DownloadingMaxNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_FailedTryAgain(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.m_FailedTryAgain);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PackageVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PackageVersion = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RemoteConfigVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RemoteConfigVersion = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ConfigVersion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ConfigVersion = (TEngine.ConfigVersion)translator.GetObject(L, 2, typeof(TEngine.ConfigVersion));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ConfigMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                TEngine.ConfigMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.ConfigMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PackageName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PackageName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PlayMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                YooAsset.EPlayMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.PlayMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DownloadingMaxNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DownloadingMaxNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FailedTryAgain(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FailedTryAgain = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MinUnloadUnusedAssetsInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MinUnloadUnusedAssetsInterval = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaxUnloadUnusedAssetsInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaxUnloadUnusedAssetsInterval = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseSystemUnloadUnusedAssets(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseSystemUnloadUnusedAssets = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetAutoReleaseInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetAutoReleaseInterval = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetCapacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetCapacity = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetExpireTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetExpireTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetPriority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetPriority = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Downloader(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Downloader = (YooAsset.ResourceDownloaderOperation)translator.GetObject(L, 2, typeof(YooAsset.ResourceDownloaderOperation));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_VerifyLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                YooAsset.EVerifyLevel gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.VerifyLevel = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Milliseconds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Milliseconds = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_DownloadingMaxNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_DownloadingMaxNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_FailedTryAgain(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                TEngine.ResourceModule gen_to_be_invoked = (TEngine.ResourceModule)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_FailedTryAgain = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

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
    public class HttpRequestUtilsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HttpRequestUtils);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 9, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRequestFormData", _m_GetRequestFormData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Get", _m_Get_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PostByFormData", _m_PostByFormData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PostByRawData", _m_PostByRawData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PostByJson", _m_PostByJson_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HttpDownLoad", _m_HttpDownLoad_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HttpDownLoadImage", _m_HttpDownLoadImage_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HttpUpLoad", _m_HttpUpLoad_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new HttpRequestUtils();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HttpRequestUtils constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRequestFormData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = HttpRequestUtils.GetRequestFormData(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    RequestDataType _dataType;translator.Get(L, 2, out _dataType);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 3);
                    
                    HttpRequestUtils.Get( _url, _dataType, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostByFormData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    RequestDataType _dataType;translator.Get(L, 2, out _dataType);
                    RequestFormData _formData = (RequestFormData)translator.GetObject(L, 3, typeof(RequestFormData));
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 4);
                    
                    HttpRequestUtils.PostByFormData( _url, _dataType, _formData, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostByRawData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    RequestDataType _dataType;translator.Get(L, 2, out _dataType);
                    byte[] _rawData = LuaAPI.lua_tobytes(L, 3);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 4);
                    
                    HttpRequestUtils.PostByRawData( _url, _dataType, _rawData, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostByJson_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    string _jsonData = LuaAPI.lua_tostring(L, 2);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 3);
                    object _flag = translator.GetObject(L, 4, typeof(object));
                    
                    HttpRequestUtils.PostByJson( _url, _jsonData, _callback, _flag );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpDownLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    string _savePath = LuaAPI.lua_tostring(L, 2);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 3);
                    BestHttpManager.OnDownLoadProgress _onProgress = translator.GetDelegate<BestHttpManager.OnDownLoadProgress>(L, 4);
                    
                    HttpRequestUtils.HttpDownLoad( _url, _savePath, _callback, _onProgress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpDownLoadImage_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 2);
                    object _flag = translator.GetObject(L, 3, typeof(object));
                    
                    HttpRequestUtils.HttpDownLoadImage( _url, _callback, _flag );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpUpLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 1);
                    string _filePath = LuaAPI.lua_tostring(L, 2);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 3);
                    BestHttpManager.OnUpLoadProgress _onProgress = translator.GetDelegate<BestHttpManager.OnUpLoadProgress>(L, 4);
                    
                    HttpRequestUtils.HttpUpLoad( _url, _filePath, _callback, _onProgress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

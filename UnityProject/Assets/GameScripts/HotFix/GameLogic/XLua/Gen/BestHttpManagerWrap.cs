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
    public class BestHttpManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BestHttpManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Get", _m_Get);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PostByFormData", _m_PostByFormData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PostByRawData", _m_PostByRawData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PostByJson", _m_PostByJson);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HttpDownLoad", _m_HttpDownLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HttpDownLoadImage", _m_HttpDownLoadImage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HttpUpLoad", _m_HttpUpLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Release", _m_Release);
			
			
			
			
			
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
					
					var gen_ret = new BestHttpManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BestHttpManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    RequestDataType _dataType;translator.Get(L, 3, out _dataType);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 4);
                    
                    gen_to_be_invoked.Get( _url, _dataType, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostByFormData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    RequestDataType _dataType;translator.Get(L, 3, out _dataType);
                    RequestFormData _formData = (RequestFormData)translator.GetObject(L, 4, typeof(RequestFormData));
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 5);
                    
                    gen_to_be_invoked.PostByFormData( _url, _dataType, _formData, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostByRawData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    RequestDataType _dataType;translator.Get(L, 3, out _dataType);
                    byte[] _rawData = LuaAPI.lua_tobytes(L, 4);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 5);
                    
                    gen_to_be_invoked.PostByRawData( _url, _dataType, _rawData, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostByJson(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _jsonData = LuaAPI.lua_tostring(L, 3);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 4);
                    object _flag = translator.GetObject(L, 5, typeof(object));
                    
                    gen_to_be_invoked.PostByJson( _url, _jsonData, _callback, _flag );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpDownLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _savePath = LuaAPI.lua_tostring(L, 3);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 4);
                    BestHttpManager.OnDownLoadProgress _onProgress = translator.GetDelegate<BestHttpManager.OnDownLoadProgress>(L, 5);
                    
                    gen_to_be_invoked.HttpDownLoad( _url, _savePath, _callback, _onProgress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpDownLoadImage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 3);
                    object _flag = translator.GetObject(L, 4, typeof(object));
                    
                    gen_to_be_invoked.HttpDownLoadImage( _url, _callback, _flag );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpUpLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _filePath = LuaAPI.lua_tostring(L, 3);
                    BestHttpManager.RequestCallback _callback = translator.GetDelegate<BestHttpManager.RequestCallback>(L, 4);
                    BestHttpManager.OnUpLoadProgress _onProgress = translator.GetDelegate<BestHttpManager.OnUpLoadProgress>(L, 5);
                    
                    gen_to_be_invoked.HttpUpLoad( _url, _filePath, _callback, _onProgress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Release(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHttpManager gen_to_be_invoked = (BestHttpManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

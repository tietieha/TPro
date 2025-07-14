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
    public class BestHTTPWebSocketWebSocketWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BestHTTP.WebSocket.WebSocket);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 16, 10);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Open", _m_Open);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Send", _m_Send);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Close", _m_Close);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "State", _g_get_State);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsOpen", _g_get_IsOpen);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BufferedAmount", _g_get_BufferedAmount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StartPingThread", _g_get_StartPingThread);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PingFrequency", _g_get_PingFrequency);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CloseAfterNoMesssage", _g_get_CloseAfterNoMesssage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InternalRequest", _g_get_InternalRequest);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Extensions", _g_get_Extensions);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Latency", _g_get_Latency);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnOpen", _g_get_OnOpen);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnMessage", _g_get_OnMessage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnBinary", _g_get_OnBinary);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnClosed", _g_get_OnClosed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnError", _g_get_OnError);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnErrorDesc", _g_get_OnErrorDesc);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnIncompleteFrame", _g_get_OnIncompleteFrame);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "StartPingThread", _s_set_StartPingThread);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PingFrequency", _s_set_PingFrequency);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CloseAfterNoMesssage", _s_set_CloseAfterNoMesssage);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnOpen", _s_set_OnOpen);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnMessage", _s_set_OnMessage);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnBinary", _s_set_OnBinary);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnClosed", _s_set_OnClosed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnError", _s_set_OnError);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnErrorDesc", _s_set_OnErrorDesc);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnIncompleteFrame", _s_set_OnIncompleteFrame);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "EncodeCloseData", _m_EncodeCloseData_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<System.Uri>(L, 2))
				{
					System.Uri _uri = (System.Uri)translator.GetObject(L, 2, typeof(System.Uri));
					
					var gen_ret = new BestHTTP.WebSocket.WebSocket(_uri);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) >= 4 && translator.Assignable<System.Uri>(L, 2) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 5) || translator.Assignable<BestHTTP.WebSocket.Extensions.IExtension>(L, 5)))
				{
					System.Uri _uri = (System.Uri)translator.GetObject(L, 2, typeof(System.Uri));
					string _origin = LuaAPI.lua_tostring(L, 3);
					string _protocol = LuaAPI.lua_tostring(L, 4);
					BestHTTP.WebSocket.Extensions.IExtension[] _extensions = translator.GetParams<BestHTTP.WebSocket.Extensions.IExtension>(L, 5);
					
					var gen_ret = new BestHTTP.WebSocket.WebSocket(_uri, _origin, _protocol, _extensions);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to BestHTTP.WebSocket.WebSocket constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Open(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Open(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Send(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.Send( _message );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    gen_to_be_invoked.Send( _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<BestHTTP.WebSocket.Frames.WebSocketFrame>(L, 2)) 
                {
                    BestHTTP.WebSocket.Frames.WebSocketFrame _frame = (BestHTTP.WebSocket.Frames.WebSocketFrame)translator.GetObject(L, 2, typeof(BestHTTP.WebSocket.Frames.WebSocketFrame));
                    
                    gen_to_be_invoked.Send( _frame );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) || LuaAPI.lua_isuint64(L, 3))&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4) || LuaAPI.lua_isuint64(L, 4))) 
                {
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    ulong _offset = LuaAPI.lua_touint64(L, 3);
                    ulong _count = LuaAPI.lua_touint64(L, 4);
                    
                    gen_to_be_invoked.Send( _buffer, _offset, _count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BestHTTP.WebSocket.WebSocket.Send!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Close(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.Close(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    ushort _code = (ushort)LuaAPI.xlua_tointeger(L, 2);
                    string _message = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.Close( _code, _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to BestHTTP.WebSocket.WebSocket.Close!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EncodeCloseData_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    ushort _code = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = BestHTTP.WebSocket.WebSocket.EncodeCloseData( _code, _message );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_State(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.State);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsOpen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsOpen);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BufferedAmount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BufferedAmount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StartPingThread(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.StartPingThread);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PingFrequency(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.PingFrequency);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CloseAfterNoMesssage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CloseAfterNoMesssage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InternalRequest(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.InternalRequest);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Extensions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Extensions);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Latency(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Latency);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnOpen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnOpen);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnMessage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnMessage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnBinary(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnBinary);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnClosed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnClosed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnError(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnError);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnErrorDesc(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnErrorDesc);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnIncompleteFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnIncompleteFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StartPingThread(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StartPingThread = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PingFrequency(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PingFrequency = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CloseAfterNoMesssage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                System.TimeSpan gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.CloseAfterNoMesssage = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnOpen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnOpen = translator.GetDelegate<BestHTTP.WebSocket.OnWebSocketOpenDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnMessage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnMessage = translator.GetDelegate<BestHTTP.WebSocket.OnWebSocketMessageDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnBinary(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnBinary = translator.GetDelegate<BestHTTP.WebSocket.OnWebSocketBinaryDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnClosed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnClosed = translator.GetDelegate<BestHTTP.WebSocket.OnWebSocketClosedDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnError(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnError = translator.GetDelegate<BestHTTP.WebSocket.OnWebSocketErrorDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnErrorDesc(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnErrorDesc = translator.GetDelegate<BestHTTP.WebSocket.OnWebSocketErrorDescriptionDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnIncompleteFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BestHTTP.WebSocket.WebSocket gen_to_be_invoked = (BestHTTP.WebSocket.WebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnIncompleteFrame = translator.GetDelegate<BestHTTP.WebSocket.OnWebSocketIncompleteFrameDelegate>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

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
    public class PVEFogRenderWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(PVEFogRender);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCamera", _m_SetCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCameraVisibleRect", _m_SetCameraVisibleRect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetData", _m_SetData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFogOfWarType", _m_SetFogOfWarType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnlockFog", _m_UnlockFog);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Draw", _m_Draw);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "renderLayer", _g_get_renderLayer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockWCount", _g_get_BlockWCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockHCount", _g_get_BlockHCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NormalRenderUnitRes", _g_get_NormalRenderUnitRes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FadeRenderUnitRes", _g_get_FadeRenderUnitRes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockDatas", _g_get_BlockDatas);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "renderLayer", _s_set_renderLayer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockWCount", _s_set_BlockWCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockHCount", _s_set_BlockHCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "NormalRenderUnitRes", _s_set_NormalRenderUnitRes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FadeRenderUnitRes", _s_set_FadeRenderUnitRes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockDatas", _s_set_BlockDatas);
            
			
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
					
					var gen_ret = new PVEFogRender();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to PVEFogRender constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                    gen_to_be_invoked.SetCamera( _camera );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCameraVisibleRect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    CameraVisibleRect _cameraVisibleRect = (CameraVisibleRect)translator.GetObject(L, 2, typeof(CameraVisibleRect));
                    
                    gen_to_be_invoked.SetCameraVisibleRect( _cameraVisibleRect );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _gridWidth = LuaAPI.xlua_tointeger(L, 2);
                    int _gridHeight = LuaAPI.xlua_tointeger(L, 3);
                    float _hexRadius = (float)LuaAPI.lua_tonumber(L, 4);
                    int[] _data = (int[])translator.GetObject(L, 5, typeof(int[]));
                    
                    gen_to_be_invoked.SetData( _gridWidth, _gridHeight, _hexRadius, _data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFogOfWarType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _fogId = LuaAPI.xlua_tointeger(L, 2);
                    bool _isUnlock = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetFogOfWarType( _fogId, _isUnlock );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnlockFog(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _fogId = LuaAPI.xlua_tointeger(L, 2);
                    float _fadeTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.UnlockFog( _fogId, _fadeTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Draw(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Draw(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_renderLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.renderLayer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockWCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BlockWCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockHCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BlockHCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NormalRenderUnitRes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.NormalRenderUnitRes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FadeRenderUnitRes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.FadeRenderUnitRes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockDatas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BlockDatas);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_renderLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                UnityEngine.LayerMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.renderLayer = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockWCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockWCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockHCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockHCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NormalRenderUnitRes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.NormalRenderUnitRes = (RenderUnitRes)translator.GetObject(L, 2, typeof(RenderUnitRes));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FadeRenderUnitRes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FadeRenderUnitRes = (RenderUnitRes)translator.GetObject(L, 2, typeof(RenderUnitRes));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockDatas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                PVEFogRender gen_to_be_invoked = (PVEFogRender)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockDatas = (BlockData[,])translator.GetObject(L, 2, typeof(BlockData[,]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

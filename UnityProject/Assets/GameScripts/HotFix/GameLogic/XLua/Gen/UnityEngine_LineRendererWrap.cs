﻿#if USE_UNI_LUA
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
using DG.Tweening;

namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class UnityEngineLineRendererWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.LineRenderer);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 18, 18);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPosition", _m_SetPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPosition", _m_GetPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Simplify", _m_Simplify);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BakeMesh", _m_BakeMesh);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPositions", _m_GetPositions);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPositions", _m_SetPositions);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DOColor", _m_DOColor);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "startWidth", _g_get_startWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "endWidth", _g_get_endWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "widthMultiplier", _g_get_widthMultiplier);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "numCornerVertices", _g_get_numCornerVertices);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "numCapVertices", _g_get_numCapVertices);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useWorldSpace", _g_get_useWorldSpace);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "loop", _g_get_loop);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "startColor", _g_get_startColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "endColor", _g_get_endColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "positionCount", _g_get_positionCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "textureScale", _g_get_textureScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "shadowBias", _g_get_shadowBias);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "generateLightingData", _g_get_generateLightingData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "textureMode", _g_get_textureMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "alignment", _g_get_alignment);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maskInteraction", _g_get_maskInteraction);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "widthCurve", _g_get_widthCurve);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "colorGradient", _g_get_colorGradient);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "startWidth", _s_set_startWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "endWidth", _s_set_endWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "widthMultiplier", _s_set_widthMultiplier);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "numCornerVertices", _s_set_numCornerVertices);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "numCapVertices", _s_set_numCapVertices);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useWorldSpace", _s_set_useWorldSpace);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "loop", _s_set_loop);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "startColor", _s_set_startColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "endColor", _s_set_endColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "positionCount", _s_set_positionCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "textureScale", _s_set_textureScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "shadowBias", _s_set_shadowBias);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "generateLightingData", _s_set_generateLightingData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "textureMode", _s_set_textureMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "alignment", _s_set_alignment);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maskInteraction", _s_set_maskInteraction);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "widthCurve", _s_set_widthCurve);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "colorGradient", _s_set_colorGradient);
            
			
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
					
					var gen_ret = new UnityEngine.LineRenderer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.LineRenderer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _position;translator.Get(L, 3, out _position);
                    
                    gen_to_be_invoked.SetPosition( _index, _position );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPosition( _index );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Simplify(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _tolerance = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.Simplify( _tolerance );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BakeMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Mesh>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 2, typeof(UnityEngine.Mesh));
                    bool _useTransform = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.BakeMesh( _mesh, _useTransform );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Mesh>(L, 2)) 
                {
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 2, typeof(UnityEngine.Mesh));
                    
                    gen_to_be_invoked.BakeMesh( _mesh );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Mesh>(L, 2)&& translator.Assignable<UnityEngine.Camera>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 2, typeof(UnityEngine.Mesh));
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    bool _useTransform = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.BakeMesh( _mesh, _camera, _useTransform );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Mesh>(L, 2)&& translator.Assignable<UnityEngine.Camera>(L, 3)) 
                {
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 2, typeof(UnityEngine.Mesh));
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    
                    gen_to_be_invoked.BakeMesh( _mesh, _camera );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.LineRenderer.BakeMesh!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPositions(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3[]>(L, 2)) 
                {
                    UnityEngine.Vector3[] _positions = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
                    
                        var gen_ret = gen_to_be_invoked.GetPositions( _positions );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeArray<UnityEngine.Vector3>>(L, 2)) 
                {
                    Unity.Collections.NativeArray<UnityEngine.Vector3> _positions;translator.Get(L, 2, out _positions);
                    
                        var gen_ret = gen_to_be_invoked.GetPositions( _positions );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeSlice<UnityEngine.Vector3>>(L, 2)) 
                {
                    Unity.Collections.NativeSlice<UnityEngine.Vector3> _positions;translator.Get(L, 2, out _positions);
                    
                        var gen_ret = gen_to_be_invoked.GetPositions( _positions );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.LineRenderer.GetPositions!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPositions(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3[]>(L, 2)) 
                {
                    UnityEngine.Vector3[] _positions = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
                    
                    gen_to_be_invoked.SetPositions( _positions );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeArray<UnityEngine.Vector3>>(L, 2)) 
                {
                    Unity.Collections.NativeArray<UnityEngine.Vector3> _positions;translator.Get(L, 2, out _positions);
                    
                    gen_to_be_invoked.SetPositions( _positions );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Unity.Collections.NativeSlice<UnityEngine.Vector3>>(L, 2)) 
                {
                    Unity.Collections.NativeSlice<UnityEngine.Vector3> _positions;translator.Get(L, 2, out _positions);
                    
                    gen_to_be_invoked.SetPositions( _positions );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.LineRenderer.SetPositions!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOColor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    DG.Tweening.Color2 _startValue;translator.Get(L, 2, out _startValue);
                    DG.Tweening.Color2 _endValue;translator.Get(L, 3, out _endValue);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.DOColor( _startValue, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_startWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.startWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_endWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.endWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_widthMultiplier(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.widthMultiplier);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_numCornerVertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.numCornerVertices);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_numCapVertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.numCapVertices);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useWorldSpace(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useWorldSpace);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.loop);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_startColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.startColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_endColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.endColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_positionCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.positionCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textureScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.textureScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_shadowBias(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.shadowBias);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_generateLightingData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.generateLightingData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_textureMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.textureMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_alignment(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.alignment);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maskInteraction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.maskInteraction);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_widthCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.widthCurve);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_colorGradient(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.colorGradient);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_startWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.startWidth = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_endWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.endWidth = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_widthMultiplier(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.widthMultiplier = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_numCornerVertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.numCornerVertices = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_numCapVertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.numCapVertices = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useWorldSpace(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useWorldSpace = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.loop = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_startColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.startColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_endColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.endColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_positionCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.positionCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_textureScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.textureScale = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_shadowBias(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.shadowBias = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_generateLightingData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.generateLightingData = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_textureMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.LineTextureMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.textureMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_alignment(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.LineAlignment gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.alignment = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maskInteraction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.SpriteMaskInteraction gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.maskInteraction = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_widthCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.widthCurve = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_colorGradient(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.LineRenderer gen_to_be_invoked = (UnityEngine.LineRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.colorGradient = (UnityEngine.Gradient)translator.GetObject(L, 2, typeof(UnityEngine.Gradient));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

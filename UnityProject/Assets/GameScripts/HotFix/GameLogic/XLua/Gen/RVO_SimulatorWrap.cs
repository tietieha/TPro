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
    public class RVOSimulatorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(RVO.Simulator);
			Utils.BeginObjectRegister(type, L, translator, 0, 74, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AgentRevive", _m_AgentRevive);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DelAgent", _m_DelAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddAgent", _m_AddAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addAgent_Radius_Lua", _m_addAgent_Radius_Lua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentPosition", _m_SetAgentPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDeadAgentRevive", _m_OnDeadAgentRevive);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addAgent", _m_addAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddVirtualHeroAgent", _m_AddVirtualHeroAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddSoliderAgent", _m_AddSoliderAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BuildAgentTree", _m_BuildAgentTree);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BuildDeadAgentTree", _m_BuildDeadAgentTree);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentSpeed", _m_SetAgentSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentAvoidRatio", _m_SetAgentAvoidRatio);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddObstacle", _m_AddObstacle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DoStep", _m_DoStep);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentAgentNeighbor", _m_GetAgentAgentNeighbor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentMaxNeighbors", _m_GetAgentMaxNeighbors);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentMaxSpeed", _m_GetAgentMaxSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentNeighborDist", _m_GetAgentNeighborDist);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentNumAgentNeighbors", _m_GetAgentNumAgentNeighbors);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentNumObstacleNeighbors", _m_GetAgentNumObstacleNeighbors);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentObstacleNeighbor", _m_GetAgentObstacleNeighbor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentOrcaLines", _m_GetAgentOrcaLines);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsAgentNo", _m_IsAgentNo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentPosition", _m_GetAgentPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentStopDis", _m_GetAgentStopDis);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getAgentPosition_Lua", _m_getAgentPosition_Lua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentPrefVelocity", _m_GetAgentPrefVelocity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentRadius", _m_GetAgentRadius);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentTimeHorizon", _m_GetAgentTimeHorizon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentTimeHorizonObst", _m_GetAgentTimeHorizonObst);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentVelocity", _m_GetAgentVelocity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentVelocityToNormal", _m_GetAgentVelocityToNormal);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGlobalTime", _m_GetGlobalTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNumAgents", _m_GetNumAgents);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNumObstacleVertices", _m_GetNumObstacleVertices);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNumWorkers", _m_GetNumWorkers);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getObstacleVertex", _m_getObstacleVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNextObstacleVertexNo", _m_GetNextObstacleVertexNo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPrevObstacleVertexNo", _m_GetPrevObstacleVertexNo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTimeStep", _m_GetTimeStep);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ProcessObstacles", _m_ProcessObstacles);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QueryVisibility", _m_QueryVisibility);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "QueryNearAgent", _m_QueryNearAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentDefaults", _m_SetAgentDefaults);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentMaxNeighbors", _m_SetAgentMaxNeighbors);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentMaxSpeed", _m_SetAgentMaxSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentNeighborDist", _m_SetAgentNeighborDist);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "setAgentPosition_Lua", _m_setAgentPosition_Lua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentPrefVelocity", _m_SetAgentPrefVelocity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "setAgentPrefVelocity_Lua", _m_setAgentPrefVelocity_Lua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAgentPrefVelocity", _m_StopAgentPrefVelocity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentRadius", _m_SetAgentRadius);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentTimeHorizon", _m_SetAgentTimeHorizon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentTimeHorizonObst", _m_SetAgentTimeHorizonObst);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentVelocity", _m_SetAgentVelocity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetGlobalTime", _m_SetGlobalTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetNumWorkers", _m_SetNumWorkers);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTimeStep", _m_SetTimeStep);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTargetId", _m_SetTargetId);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Release", _m_Release);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentIdsInRange", _m_GetAgentIdsInRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentsInCircle", _m_GetAgentsInCircle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentsInRotatedRect", _m_GetAgentsInRotatedRect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgentsInRotatedSector", _m_GetAgentsInRotatedSector);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAttackRange", _m_SetAttackRange);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetStopDis", _m_SetStopDis);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAgentTargetPosition", _m_SetAgentTargetPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAgent", _m_GetAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsAgentValid", _m_IsAgentValid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAIType", _m_SetAIType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsAgentEncircle", _m_IsAgentEncircle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NeedChangeNearestAgent", _m_NeedChangeNearestAgent);
			
			
			
			
			
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
					
					var gen_ret = new RVO.Simulator();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Simulator constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AgentRevive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.AgentRevive( _agentNo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DelAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.DelAgent( _agentNo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    RVO.Vector2 _position;translator.Get(L, 2, out _position);
                    
                        var gen_ret = gen_to_be_invoked.AddAgent( _position );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addAgent_Radius_Lua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _avoidanceRatio = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.addAgent_Radius_Lua( _x, _y, _radius, _avoidanceRatio );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetAgentPosition( _agentNo, _x, _y );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<RVO.Vector2>(L, 3)) 
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    RVO.Vector2 _position;translator.Get(L, 3, out _position);
                    
                    gen_to_be_invoked.SetAgentPosition( _agentNo, _position );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Simulator.SetAgentPosition!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDeadAgentRevive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnDeadAgentRevive(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    RVO.Vector2 _position;translator.Get(L, 2, out _position);
                    float _neighborDist = (float)LuaAPI.lua_tonumber(L, 3);
                    int _maxNeighbors = LuaAPI.xlua_tointeger(L, 4);
                    float _timeHorizon = (float)LuaAPI.lua_tonumber(L, 5);
                    float _timeHorizonObst = (float)LuaAPI.lua_tonumber(L, 6);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 7);
                    float _maxSpeed = (float)LuaAPI.lua_tonumber(L, 8);
                    RVO.Vector2 _velocity;translator.Get(L, 9, out _velocity);
                    
                        var gen_ret = gen_to_be_invoked.addAgent( _position, _neighborDist, _maxNeighbors, _timeHorizon, _timeHorizonObst, _radius, _maxSpeed, _velocity );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddVirtualHeroAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    int _teamId = LuaAPI.xlua_tointeger(L, 5);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    float _maxSpeed = (float)LuaAPI.lua_tonumber(L, 8);
                    float _attackRange = (float)LuaAPI.lua_tonumber(L, 9);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 10);
                    bool _isMelee = LuaAPI.lua_toboolean(L, 11);
                    float _stopDis = (float)LuaAPI.lua_tonumber(L, 12);
                    float _avoidanceRatio = (float)LuaAPI.lua_tonumber(L, 13);
                    LuaArrAccess _luaArray = (LuaArrAccess)translator.GetObject(L, 14, typeof(LuaArrAccess));
                    RVO.AgentType _agentType;translator.Get(L, 15, out _agentType);
                    float _neighborDist = (float)LuaAPI.lua_tonumber(L, 16);
                    int _maxNeighbors = LuaAPI.xlua_tointeger(L, 17);
                    float _timeHorizon = (float)LuaAPI.lua_tonumber(L, 18);
                    
                    gen_to_be_invoked.AddVirtualHeroAgent( _id, _x, _y, _teamId, _radius, _side, _maxSpeed, _attackRange, _isVirtual, _isMelee, _stopDis, _avoidanceRatio, _luaArray, _agentType, _neighborDist, _maxNeighbors, _timeHorizon );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddSoliderAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    int _teamId = LuaAPI.xlua_tointeger(L, 5);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    float _maxSpeed = (float)LuaAPI.lua_tonumber(L, 8);
                    float _attackRange = (float)LuaAPI.lua_tonumber(L, 9);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 10);
                    bool _isMelee = LuaAPI.lua_toboolean(L, 11);
                    float _stopDis = (float)LuaAPI.lua_tonumber(L, 12);
                    float _avoidanceRatio = (float)LuaAPI.lua_tonumber(L, 13);
                    int _groupId = LuaAPI.xlua_tointeger(L, 14);
                    LuaArrAccess _luaArray = (LuaArrAccess)translator.GetObject(L, 15, typeof(LuaArrAccess));
                    RVO.AgentType _agentType;translator.Get(L, 16, out _agentType);
                    
                    gen_to_be_invoked.AddSoliderAgent( _id, _x, _y, _teamId, _radius, _side, _maxSpeed, _attackRange, _isVirtual, _isMelee, _stopDis, _avoidanceRatio, _groupId, _luaArray, _agentType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BuildAgentTree(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BuildAgentTree(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BuildDeadAgentTree(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BuildDeadAgentTree(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _speed = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAgentSpeed( _id, _speed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentAvoidRatio(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    float _avoidRatio = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAgentAvoidRatio( _id, _avoidRatio );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddObstacle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.IList<RVO.Vector2> _vertices = (System.Collections.Generic.IList<RVO.Vector2>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IList<RVO.Vector2>));
                    
                        var gen_ret = gen_to_be_invoked.AddObstacle( _vertices );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoStep(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.DoStep(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentAgentNeighbor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    int _neighborNo = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentAgentNeighbor( _agentNo, _neighborNo );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentMaxNeighbors(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentMaxNeighbors( _agentNo );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentMaxSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentMaxSpeed( _agentNo );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentNeighborDist(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentNeighborDist( _agentNo );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentNumAgentNeighbors(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentNumAgentNeighbors( _agentNo );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentNumObstacleNeighbors(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentNumObstacleNeighbors( _agentNo );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentObstacleNeighbor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    int _neighborNo = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentObstacleNeighbor( _agentNo, _neighborNo );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentOrcaLines(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentOrcaLines( _agentNo );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsAgentNo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.IsAgentNo( _agentNo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentPosition( _agentNo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentStopDis(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentStopDis( _agentNo );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getAgentPosition_Lua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _x;
                    float _y;
                    
                    gen_to_be_invoked.getAgentPosition_Lua( _agentNo, out _x, out _y );
                    LuaAPI.lua_pushnumber(L, _x);
                        
                    LuaAPI.lua_pushnumber(L, _y);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentPrefVelocity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentPrefVelocity( _agentNo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentRadius(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentRadius( _agentNo );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentTimeHorizon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentTimeHorizon( _agentNo );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentTimeHorizonObst(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentTimeHorizonObst( _agentNo );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentVelocity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentVelocity( _agentNo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentVelocityToNormal(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentVelocityToNormal( _agentNo );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGlobalTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetGlobalTime(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNumAgents(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetNumAgents(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNumObstacleVertices(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetNumObstacleVertices(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNumWorkers(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetNumWorkers(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getObstacleVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _vertexNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.getObstacleVertex( _vertexNo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNextObstacleVertexNo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _vertexNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetNextObstacleVertexNo( _vertexNo );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPrevObstacleVertexNo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _vertexNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetPrevObstacleVertexNo( _vertexNo );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTimeStep(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetTimeStep(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ProcessObstacles(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ProcessObstacles(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryVisibility(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    RVO.Vector2 _point1;translator.Get(L, 2, out _point1);
                    RVO.Vector2 _point2;translator.Get(L, 3, out _point2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.QueryVisibility( _point1, _point2, _radius );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QueryNearAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    RVO.Vector2 _point;translator.Get(L, 2, out _point);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.QueryNearAgent( _point, _radius );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentDefaults(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _neighborDist = (float)LuaAPI.lua_tonumber(L, 2);
                    int _maxNeighbors = LuaAPI.xlua_tointeger(L, 3);
                    float _timeHorizon = (float)LuaAPI.lua_tonumber(L, 4);
                    float _timeHorizonObst = (float)LuaAPI.lua_tonumber(L, 5);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 6);
                    float _maxSpeed = (float)LuaAPI.lua_tonumber(L, 7);
                    RVO.Vector2 _velocity;translator.Get(L, 8, out _velocity);
                    
                    gen_to_be_invoked.SetAgentDefaults( _neighborDist, _maxNeighbors, _timeHorizon, _timeHorizonObst, _radius, _maxSpeed, _velocity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentMaxNeighbors(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    int _maxNeighbors = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetAgentMaxNeighbors( _agentNo, _maxNeighbors );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentMaxSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _maxSpeed = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAgentMaxSpeed( _agentNo, _maxSpeed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentNeighborDist(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _neighborDist = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAgentNeighborDist( _agentNo, _neighborDist );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_setAgentPosition_Lua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.setAgentPosition_Lua( _agentNo, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentPrefVelocity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    RVO.Vector2 _prefVelocity;translator.Get(L, 3, out _prefVelocity);
                    
                    gen_to_be_invoked.SetAgentPrefVelocity( _agentNo, _prefVelocity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_setAgentPrefVelocity_Lua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.setAgentPrefVelocity_Lua( _agentNo, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAgentPrefVelocity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.StopAgentPrefVelocity( _agentNo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentRadius(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAgentRadius( _agentNo, _radius );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentTimeHorizon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _timeHorizon = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAgentTimeHorizon( _agentNo, _timeHorizon );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentTimeHorizonObst(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    float _timeHorizonObst = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAgentTimeHorizonObst( _agentNo, _timeHorizonObst );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentVelocity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    RVO.Vector2 _velocity;translator.Get(L, 3, out _velocity);
                    
                    gen_to_be_invoked.SetAgentVelocity( _agentNo, _velocity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGlobalTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _globalTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.SetGlobalTime( _globalTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetNumWorkers(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _numWorkers = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetNumWorkers( _numWorkers );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTimeStep(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _timeStep = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.SetTimeStep( _timeStep );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTargetId(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    bool _isFollow = LuaAPI.lua_toboolean(L, 4);
                    bool _isFirst = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.SetTargetId( _agentNo, _targetId, _isFollow, _isFirst );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    bool _isFollow = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.SetTargetId( _agentNo, _targetId, _isFollow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetTargetId( _agentNo, _targetId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Simulator.SetTargetId!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Release(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentIdsInRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentIdsInRange( _agentId, _radius, _isVirtual );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentIdsInRange( _agentId, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Simulator.GetAgentIdsInRange!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentsInCircle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 6);
                    bool _containDead = LuaAPI.lua_toboolean(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInCircle( _x, _y, _radius, _side, _isVirtual, _containDead );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInCircle( _x, _y, _radius, _side, _isVirtual );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    int _side = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInCircle( _x, _y, _radius, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInCircle( _x, _y, _radius );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Simulator.GetAgentsInCircle!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentsInRotatedRect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 9&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    bool _containDead = LuaAPI.lua_toboolean(L, 9);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedRect( _x, _y, _width, _height, _angleRad, _side, _isVirtual, _containDead );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedRect( _x, _y, _width, _height, _angleRad, _side, _isVirtual );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedRect( _x, _y, _width, _height, _angleRad, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _width = (float)LuaAPI.lua_tonumber(L, 4);
                    float _height = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedRect( _x, _y, _width, _height, _angleRad );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Simulator.GetAgentsInRotatedRect!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgentsInRotatedSector(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 9&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    bool _containDead = LuaAPI.lua_toboolean(L, 9);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedSector( _x, _y, _radius, _angle, _angleRad, _side, _isVirtual, _containDead );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    bool _isVirtual = LuaAPI.lua_toboolean(L, 8);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedSector( _x, _y, _radius, _angle, _angleRad, _side, _isVirtual );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    int _side = LuaAPI.xlua_tointeger(L, 7);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedSector( _x, _y, _radius, _angle, _angleRad, _side );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _radius = (float)LuaAPI.lua_tonumber(L, 4);
                    float _angle = (float)LuaAPI.lua_tonumber(L, 5);
                    float _angleRad = (float)LuaAPI.lua_tonumber(L, 6);
                    
                        var gen_ret = gen_to_be_invoked.GetAgentsInRotatedSector( _x, _y, _radius, _angle, _angleRad );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to RVO.Simulator.GetAgentsInRotatedSector!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAttackRange(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _attackRange = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAttackRange( _agentId, _attackRange );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetStopDis(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _stopDis = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetStopDis( _agentId, _stopDis );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAgentTargetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _x = (float)LuaAPI.lua_tonumber(L, 3);
                    float _y = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.SetAgentTargetPosition( _agentId, _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.GetAgent( _agentNo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsAgentValid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentNo = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.IsAgentValid( _agentNo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAIType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    int _aiType = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetAIType( _agentId, _aiType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsAgentEncircle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    float _attackRange = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.IsAgentEncircle( _agentId, _attackRange );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NeedChangeNearestAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                RVO.Simulator gen_to_be_invoked = (RVO.Simulator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _agentId = LuaAPI.xlua_tointeger(L, 2);
                    int _targetId = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.NeedChangeNearestAgent( _agentId, _targetId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

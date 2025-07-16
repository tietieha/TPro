// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-11-11 14:19 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GEngine.MapEditor
{
	public class MapEditorEventCenter
	{
		private static MapEditorEventCenter mInstance;

		public static MapEditorEventCenter Instance
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = new MapEditorEventCenter();
				}

				return mInstance;
			}
		}

		private List<IGameEventAgent> gameEventAgentList = new List<IGameEventAgent>(30); //根据项目中的事件数量调整

		private MapEditorEventCenter()
		{
		}

		public void AddGameEventAgent(IGameEventAgent agent)
		{
			gameEventAgentList.Add(agent);
		}

		//----------------------------------------无参--------------------------------------------
		public static void AddListener(string eventName, Action action)
		{
			MapEditorEventAgent.Instance.AddListener(eventName, action);
		}

		public static void AddListener(MapEditorEvents.Event gameEvent, Action action)
		{
			MapEditorEventAgent.Instance.AddListener(gameEvent.eventName, action);
		}

		public static void SendEvent(MapEditorEvents.Event gameEvent)
		{
			MapEditorEventAgent.Instance.Invoke(gameEvent.eventName);
		}

		public static void RemoveListener(string eventName, Action action)
		{
			MapEditorEventAgent.Instance.RemoveListener(eventName, action);
		}

		public static void RemoveListener(MapEditorEvents.Event gameEvent, Action action)
		{
			MapEditorEventAgent.Instance.RemoveListener(gameEvent.eventName, action);
		}

		public static bool CheckHaveListener(MapEditorEvents.Event gameEvent)
		{
			return MapEditorEventAgent.Instance.CheckHaveListener(gameEvent.eventName);
		}

		//--------------------------------------------------------------------------------------

		//----------------------------------T参数-----------------------------------------------
		public static void AddListener<T>(string eventName, Action<T> action)
		{
			GameEventAgent<T>.Instance.AddListener(eventName, action);
		}

		public static void AddListener<T>(MapEditorEvents.Event<T> gameEvent, Action<T> action)
		{
			GameEventAgent<T>.Instance.AddListener(gameEvent.eventName, action);
		}

		public static void SendEvent<T>(MapEditorEvents.Event<T> gameEvent, T param)
		{
			GameEventAgent<T>.Instance.Invoke(gameEvent.eventName, param);
		}

		public static void RemoveListener<T>(string eventName, Action<T> action)
		{
			GameEventAgent<T>.Instance.RemoveListener(eventName, action);
		}

		public static void RemoveListener<T>(MapEditorEvents.Event<T> gameEvent, Action<T> action)
		{
			GameEventAgent<T>.Instance.RemoveListener(gameEvent.eventName, action);
		}

		public static bool CheckHaveListener<T>(MapEditorEvents.Event<T> gameEvent)
		{
			return GameEventAgent<T>.Instance.CheckHaveListener(gameEvent.eventName);
		}

		//--------------------------------------------------------------------------------------

		//-----------------------------T1 T2参数-------------------------------------------------
		public static void AddListener<T1, T2>(string eventName, Action<T1, T2> action)
		{
			GameEventAgent<T1, T2>.Instance.AddListener(eventName, action);
		}

		public static void AddListener<T1, T2>(MapEditorEvents.Event<T1, T2> gameEvent, Action<T1, T2> action)
		{
			GameEventAgent<T1, T2>.Instance.AddListener(gameEvent.eventName, action);
		}

		public static void SendEvent<T1, T2>(MapEditorEvents.Event<T1, T2> gameEvent, T1 param1, T2 param2)
		{
			GameEventAgent<T1, T2>.Instance.Invoke(gameEvent.eventName, param1, param2);
		}

		public static void RemoveListener<T1, T2>(string eventName, Action<T1, T2> action)
		{
			GameEventAgent<T1, T2>.Instance.RemoveListener(eventName, action);
		}

		public static void RemoveListener<T1, T2>(MapEditorEvents.Event<T1, T2> gameEvent, Action<T1, T2> action)
		{
			GameEventAgent<T1, T2>.Instance.RemoveListener(gameEvent.eventName, action);
		}

		public static bool CheckHaveListener<T1, T2>(MapEditorEvents.Event<T1, T2> gameEvent)
		{
			return GameEventAgent<T1, T2>.Instance.CheckHaveListener(gameEvent.eventName);
		}

		//--------------------------------------------------------------------------------------

		//--------------------------------------------------------------------------------------
		public static void AddListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
		{
			GameEventAgent<T1, T2, T3>.Instance.AddListener(eventName, action);
		}

		public static void AddListener<T1, T2, T3>(MapEditorEvents.Event<T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
		{
			GameEventAgent<T1, T2, T3>.Instance.AddListener(gameEvent.eventName, action);
		}

		public static void SendEvent<T1, T2, T3>(MapEditorEvents.Event<T1, T2, T3> gameEvent, T1 param1, T2 param2,
			T3 param3)
		{
			GameEventAgent<T1, T2, T3>.Instance.Invoke(gameEvent.eventName, param1, param2, param3);
		}

		public static void RemoveListener<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
		{
			GameEventAgent<T1, T2, T3>.Instance.RemoveListener(eventName, action);
		}

		public static void RemoveListener<T1, T2, T3>(MapEditorEvents.Event<T1, T2, T3> gameEvent, Action<T1, T2, T3> action)
		{
			GameEventAgent<T1, T2, T3>.Instance.RemoveListener(gameEvent.eventName, action);
		}

		public static bool CheckHaveListener<T1, T2, T3>(MapEditorEvents.Event<T1, T2, T3> gameEvent)
		{
			return GameEventAgent<T1, T2, T3>.Instance.CheckHaveListener(gameEvent.eventName);
		}

		//--------------------------------------------------------------------------------------

		//-----------------------------------T1T2T3T4参数----------------------------------------
		public static void AddListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
		{
			GameEventAgent<T1, T2, T3, T4>.Instance.AddListener(eventName, action);
		}

		public static void AddListener<T1, T2, T3, T4>(MapEditorEvents.Event<T1, T2, T3, T4> gameEvent,
			Action<T1, T2, T3, T4> action)
		{
			GameEventAgent<T1, T2, T3, T4>.Instance.AddListener(gameEvent.eventName, action);
		}

		public static void SendEvent<T1, T2, T3, T4>(MapEditorEvents.Event<T1, T2, T3, T4> gameEvent, T1 param1, T2 param2,
			T3 param3, T4 param4)
		{
			GameEventAgent<T1, T2, T3, T4>.Instance.Invoke(gameEvent.eventName, param1, param2, param3, param4);
		}

		public static void RemoveListener<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
		{
			GameEventAgent<T1, T2, T3, T4>.Instance.RemoveListener(eventName, action);
		}

		public static void RemoveListener<T1, T2, T3, T4>(MapEditorEvents.Event<T1, T2, T3, T4> gameEvent,
			Action<T1, T2, T3, T4> action)
		{
			GameEventAgent<T1, T2, T3, T4>.Instance.RemoveListener(gameEvent.eventName, action);
		}

		public static bool CheckHaveListener<T1, T2, T3, T4>(MapEditorEvents.Event<T1, T2, T3, T4> gameEvent)
		{
			return GameEventAgent<T1, T2, T3, T4>.Instance.CheckHaveListener(gameEvent.eventName);
		}

		//--------------------------------------------------------------------------------------
	}
}
#endif
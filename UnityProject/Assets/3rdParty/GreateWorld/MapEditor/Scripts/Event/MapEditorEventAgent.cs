﻿// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-11-11 14:16 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System;
namespace GEngine.MapEditor
{
	/// <summary>
	/// 事件系统底层类
	/// </summary>

	//无参
	public class MapEditorEventAgent : IGameEventAgent
	{
		private static MapEditorEventAgent mInatance;

		public static MapEditorEventAgent Instance
		{
			get
			{
				if (mInatance == null)
				{
					mInatance = new MapEditorEventAgent();
				}

				return mInatance;
			}
		}

		private class GameEventDelegate
		{
			Action mAction;
			bool needUpdate;
			private Delegate[] delegateList;

			public void Add(Action action)
			{
				mAction -= action;
				mAction += action;
				needUpdate = true;
			}

			public void Remove(Action action)
			{
				mAction -= action;
				needUpdate = true;
			}

			public void Remove(object listener)
			{
				CheckUpdate();
				if (delegateList == null)
					return;
				for (int i = 0; i < delegateList.Length; i++)
				{
					Delegate mDelegate = delegateList[i];
					if (mDelegate.Target == listener)
					{
						Remove(mDelegate as Action);
					}
				}
			}

			public void Invoke()
			{
				if (mAction == null)
					return;
				CheckUpdate();
				if (delegateList == null)
					return;

				for (int i = 0; i < delegateList.Length; i++)
				{
					try
					{
						Action mDeleate = delegateList[i] as Action;
						mDeleate();
					}
					catch (Exception e)
					{
						Debug.Log(e);
					}
				}
			}

			public Delegate[] GetInvokeList()
			{
				CheckUpdate();
				return delegateList;
			}

			private void CheckUpdate()
			{
				if (needUpdate)
				{
					needUpdate = false;
					if (mAction != null)
					{
						delegateList = mAction.GetInvocationList(); //得到mAction链中所有方法并存入delegateList
					}
					else
					{
						delegateList = null;
					}
				}
			}
		}

		Dictionary<string, GameEventDelegate> eventList = new Dictionary<string, GameEventDelegate>();

		private MapEditorEventAgent()
		{
		}

		public void AddListener(string gameEvent, Action action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate) == false)
			{
				gameEventDelegate = new GameEventDelegate();
				eventList.Add(gameEvent, gameEventDelegate);
			}

			gameEventDelegate.Add(action);
		}

		public void Invoke(string gameEvent)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Invoke();
			}
		}

		public void RemoveListener(string gameEvent, Action action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Remove(action);
			}
		}

		private void RemoveListener(string gameEvent)
		{
			if (CheckHaveListener(gameEvent))
			{
				eventList.Remove(gameEvent);
			}
		}

		public bool CheckHaveListener(string gameEvent)
		{
			GameEventDelegate eventDelegate;
			if (eventList.TryGetValue(gameEvent, out eventDelegate))
			{
				Delegate[] delegateList = eventDelegate.GetInvokeList();
				if (delegateList != null && delegateList.Length > 0)
					return true;
			}

			return false;
		}

		private void RemoveListener(object listener)
		{
			Dictionary<string, GameEventDelegate>.Enumerator
				tor = eventList.GetEnumerator(); //返回实例的枚举数 就是返回集的中所有元素一个一个列出来
			while (tor.MoveNext())
			{
				tor.Current.Value.Remove(listener);
			}
		}
	}

	/// <summary>
	/// T1参数
	/// </summary>
	public class GameEventAgent<T> : IGameEventAgent
	{
		private static GameEventAgent<T> mInatance;

		public static GameEventAgent<T> Instance
		{
			get
			{
				if (mInatance == null)
				{
					mInatance = new GameEventAgent<T>();
				}

				return mInatance;
			}
		}

		private class GameEventDelegate
		{
			Action<T> mAction;
			bool needUpdate;
			private Delegate[] delegateList;

			public void Add(Action<T> action)
			{
				mAction -= action;
				mAction += action;
				needUpdate = true;
			}

			public void Remove(Action<T> action)
			{
				mAction -= action;
				needUpdate = true;
			}

			public void Remove(object listener)
			{
				CheckUpdate();
				if (delegateList == null)
					return;
				for (int i = 0; i < delegateList.Length; i++)
				{
					Delegate mDelegate = delegateList[i];
					if (mDelegate.Target == listener)
					{
						Remove(mDelegate as Action<T>);
					}
				}
			}

			public void Invoke(T param)
			{
				if (mAction == null)
					return;
				CheckUpdate();
				if (delegateList == null)
					return;

				for (int i = 0; i < delegateList.Length; i++)
				{
					try
					{
						Action<T> mDeleate = delegateList[i] as Action<T>;
						mDeleate(param);
					}
					catch (Exception e)
					{
						Debug.Log(e);
					}
				}
			}

			public Delegate[] GetInvokeList()
			{
				CheckUpdate();
				return delegateList;
			}

			private void CheckUpdate()
			{
				if (needUpdate)
				{
					needUpdate = false;
					if (mAction != null)
					{
						delegateList = mAction.GetInvocationList(); //得到mAction链中所有方法并存入delegateList
					}
					else
					{
						delegateList = null;
					}
				}
			}
		}

		Dictionary<string, GameEventDelegate> eventList = new Dictionary<string, GameEventDelegate>();

		private GameEventAgent()
		{
		}

		public void AddListener(string gameEvent, Action<T> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate) == false)
			{
				gameEventDelegate = new GameEventDelegate();
				eventList.Add(gameEvent, gameEventDelegate);
			}

			gameEventDelegate.Add(action);
		}

		public void Invoke(string gameEvent, T param)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Invoke(param);
			}
		}

		public void RemoveListener(string gameEvent, Action<T> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Remove(action);
			}
		}

		private void RemoveListener(string gameEvent)
		{
			if (CheckHaveListener(gameEvent))
			{
				eventList.Remove(gameEvent);
			}
		}

		public bool CheckHaveListener(string gameEvent)
		{
			GameEventDelegate eventDelegate;
			if (eventList.TryGetValue(gameEvent, out eventDelegate))
			{
				Delegate[] delegateList = eventDelegate.GetInvokeList();
				if (delegateList != null && delegateList.Length > 0)
					return true;
			}

			return false;
		}

		private void RemoveListener(object listener)
		{
			Dictionary<string, GameEventDelegate>.Enumerator
				tor = eventList.GetEnumerator(); //返回实例的枚举数 就是返回集的中所有元素一个一个列出来
			while (tor.MoveNext())
			{
				tor.Current.Value.Remove(listener);
			}
		}
	}


	/// <summary>
	/// T1 T2参数
	/// </summary>
	public class GameEventAgent<T1, T2> : IGameEventAgent
	{
		private static GameEventAgent<T1, T2> mInatance;

		public static GameEventAgent<T1, T2> Instance
		{
			get
			{
				if (mInatance == null)
				{
					mInatance = new GameEventAgent<T1, T2>();
				}

				return mInatance;
			}
		}

		private class GameEventDelegate
		{
			Action<T1, T2> mAction;
			bool needUpdate;
			private Delegate[] delegateList;

			public void Add(Action<T1, T2> action)
			{
				mAction -= action;
				mAction += action;
				needUpdate = true;
			}

			public void Remove(Action<T1, T2> action)
			{
				mAction -= action;
				needUpdate = true;
			}

			public void Remove(object listener)
			{
				CheckUpdate();
				if (delegateList == null)
					return;
				for (int i = 0; i < delegateList.Length; i++)
				{
					Delegate mDelegate = delegateList[i];
					if (mDelegate.Target == listener)
					{
						Remove(mDelegate as Action<T1, T2>);
					}
				}
			}

			public void Invoke(T1 param1, T2 param2)
			{
				if (mAction == null)
					return;
				CheckUpdate();
				if (delegateList == null)
					return;

				for (int i = 0; i < delegateList.Length; i++)
				{
					try
					{
						Action<T1, T2> mDeleate = delegateList[i] as Action<T1, T2>;
						mDeleate(param1, param2);
					}
					catch (Exception e)
					{
						Debug.Log(e);
					}
				}
			}

			public Delegate[] GetInvokeList()
			{
				CheckUpdate();
				return delegateList;
			}

			private void CheckUpdate()
			{
				if (needUpdate)
				{
					needUpdate = false;
					if (mAction != null)
					{
						delegateList = mAction.GetInvocationList(); //得到mAction链中所有方法并存入delegateList
					}
					else
					{
						delegateList = null;
					}
				}
			}
		}

		Dictionary<string, GameEventDelegate> eventList = new Dictionary<string, GameEventDelegate>();

		private GameEventAgent()
		{
		}

		public void AddListener(string gameEvent, Action<T1, T2> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate) == false)
			{
				gameEventDelegate = new GameEventDelegate();
				eventList.Add(gameEvent, gameEventDelegate);
			}

			gameEventDelegate.Add(action);
		}

		public void Invoke(string gameEvent, T1 param1, T2 param2)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Invoke(param1, param2);
			}
		}

		public void RemoveListener(string gameEvent, Action<T1, T2> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Remove(action);
			}
		}

		private void RemoveListener(string gameEvent)
		{
			if (CheckHaveListener(gameEvent))
			{
				eventList.Remove(gameEvent);
			}
		}

		public bool CheckHaveListener(string gameEvent)
		{
			GameEventDelegate eventDelegate;
			if (eventList.TryGetValue(gameEvent, out eventDelegate))
			{
				Delegate[] delegateList = eventDelegate.GetInvokeList();
				if (delegateList != null && delegateList.Length > 0)
					return true;
			}

			return false;
		}

		private void RemoveListener(object listener)
		{
			Dictionary<string, GameEventDelegate>.Enumerator
				tor = eventList.GetEnumerator(); //返回实例的枚举数 就是返回集的中所有元素一个一个列出来
			while (tor.MoveNext())
			{
				tor.Current.Value.Remove(listener);
			}
		}
	}

	/// <summary>
	/// T1 T2 T3参数
	/// </summary>
	public class GameEventAgent<T1, T2, T3> : IGameEventAgent
	{
		private static GameEventAgent<T1, T2, T3> mInatance;

		public static GameEventAgent<T1, T2, T3> Instance
		{
			get
			{
				if (mInatance == null)
				{
					mInatance = new GameEventAgent<T1, T2, T3>();
				}

				return mInatance;
			}
		}

		private class GameEventDelegate
		{
			Action<T1, T2, T3> mAction;
			bool needUpdate;
			private Delegate[] delegateList;

			public void Add(Action<T1, T2, T3> action)
			{
				mAction -= action;
				mAction += action;
				needUpdate = true;
			}

			public void Remove(Action<T1, T2, T3> action)
			{
				mAction -= action;
				needUpdate = true;
			}

			public void Remove(object listener)
			{
				CheckUpdate();
				if (delegateList == null)
					return;
				for (int i = 0; i < delegateList.Length; i++)
				{
					Delegate mDelegate = delegateList[i];
					if (mDelegate.Target == listener)
					{
						Remove(mDelegate as Action<T1, T2, T3>);
					}
				}
			}

			public void Invoke(T1 param1, T2 param2, T3 param3)
			{
				if (mAction == null)
					return;
				CheckUpdate();
				if (delegateList == null)
					return;

				for (int i = 0; i < delegateList.Length; i++)
				{
					try
					{
						Action<T1, T2, T3> mDeleate = delegateList[i] as Action<T1, T2, T3>;
						mDeleate(param1, param2, param3);
					}
					catch (Exception e)
					{
						Debug.Log(e);
					}
				}
			}

			public Delegate[] GetInvokeList()
			{
				CheckUpdate();
				return delegateList;
			}

			private void CheckUpdate()
			{
				if (needUpdate)
				{
					needUpdate = false;
					if (mAction != null)
					{
						delegateList = mAction.GetInvocationList(); //得到mAction链中所有方法并存入delegateList
					}
					else
					{
						delegateList = null;
					}
				}
			}
		}

		Dictionary<string, GameEventDelegate> eventList = new Dictionary<string, GameEventDelegate>();

		private GameEventAgent()
		{
		}

		public void AddListener(string gameEvent, Action<T1, T2, T3> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate) == false)
			{
				gameEventDelegate = new GameEventDelegate();
				eventList.Add(gameEvent, gameEventDelegate);
			}

			gameEventDelegate.Add(action);
		}

		public void Invoke(string gameEvent, T1 param1, T2 param2, T3 param3)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Invoke(param1, param2, param3);
			}
		}

		public void RemoveListener(string gameEvent, Action<T1, T2, T3> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Remove(action);
			}
		}

		private void RemoveListener(string gameEvent)
		{
			if (CheckHaveListener(gameEvent))
			{
				eventList.Remove(gameEvent);
			}
		}

		public bool CheckHaveListener(string gameEvent)
		{
			GameEventDelegate eventDelegate;
			if (eventList.TryGetValue(gameEvent, out eventDelegate))
			{
				Delegate[] delegateList = eventDelegate.GetInvokeList();
				if (delegateList != null && delegateList.Length > 0)
					return true;
			}

			return false;
		}

		private void RemoveListener(object listener)
		{
			Dictionary<string, GameEventDelegate>.Enumerator
				tor = eventList.GetEnumerator(); //返回实例的枚举数 就是返回集的中所有元素一个一个列出来
			while (tor.MoveNext())
			{
				tor.Current.Value.Remove(listener);
			}
		}
	}

	/// <summary>
	/// T1 T2 T3 T4参数
	/// </summary>
	public class GameEventAgent<T1, T2, T3, T4> : IGameEventAgent
	{
		private static GameEventAgent<T1, T2, T3, T4> mInatance;

		public static GameEventAgent<T1, T2, T3, T4> Instance
		{
			get
			{
				if (mInatance == null)
				{
					mInatance = new GameEventAgent<T1, T2, T3, T4>();
				}

				return mInatance;
			}
		}

		private class GameEventDelegate
		{
			Action<T1, T2, T3, T4> mAction;
			bool needUpdate;
			private Delegate[] delegateList;

			public void Add(Action<T1, T2, T3, T4> action)
			{
				mAction -= action;
				mAction += action;
				needUpdate = true;
			}

			public void Remove(Action<T1, T2, T3, T4> action)
			{
				mAction -= action;
				needUpdate = true;
			}

			public void Remove(object listener)
			{
				CheckUpdate();
				if (delegateList == null)
					return;
				for (int i = 0; i < delegateList.Length; i++)
				{
					Delegate mDelegate = delegateList[i];
					if (mDelegate.Target == listener)
					{
						Remove(mDelegate as Action<T1, T2, T3, T4>);
					}
				}
			}

			public void Invoke(T1 param1, T2 param2, T3 param3, T4 param4)
			{
				if (mAction == null)
					return;
				CheckUpdate();
				if (delegateList == null)
					return;

				for (int i = 0; i < delegateList.Length; i++)
				{
					try
					{
						Action<T1, T2, T3, T4> mDeleate = delegateList[i] as Action<T1, T2, T3, T4>;
						mDeleate(param1, param2, param3, param4);
					}
					catch (Exception e)
					{
						Debug.Log(e);
					}
				}
			}

			public Delegate[] GetInvokeList()
			{
				CheckUpdate();
				return delegateList;
			}

			private void CheckUpdate()
			{
				if (needUpdate)
				{
					needUpdate = false;
					if (mAction != null)
					{
						delegateList = mAction.GetInvocationList(); //得到mAction链中所有方法并存入delegateList
					}
					else
					{
						delegateList = null;
					}
				}
			}
		}

		Dictionary<string, GameEventDelegate> eventList = new Dictionary<string, GameEventDelegate>();

		private GameEventAgent()
		{
		}

		public void AddListener(string gameEvent, Action<T1, T2, T3, T4> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate) == false)
			{
				gameEventDelegate = new GameEventDelegate();
				eventList.Add(gameEvent, gameEventDelegate);
			}

			gameEventDelegate.Add(action);
		}

		public void Invoke(string gameEvent, T1 param1, T2 param2, T3 param3, T4 param4)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Invoke(param1, param2, param3, param4);
			}
		}

		public void RemoveListener(string gameEvent, Action<T1, T2, T3, T4> action)
		{
			GameEventDelegate gameEventDelegate;
			if (eventList.TryGetValue(gameEvent, out gameEventDelegate))
			{
				gameEventDelegate.Remove(action);
			}
		}

		private void RemoveListener(string gameEvent)
		{
			if (CheckHaveListener(gameEvent))
			{
				eventList.Remove(gameEvent);
			}
		}

		public bool CheckHaveListener(string gameEvent)
		{
			GameEventDelegate eventDelegate;
			if (eventList.TryGetValue(gameEvent, out eventDelegate))
			{
				Delegate[] delegateList = eventDelegate.GetInvokeList();
				if (delegateList != null && delegateList.Length > 0)
					return true;
			}

			return false;
		}

		private void RemoveListener(object listener)
		{
			Dictionary<string, GameEventDelegate>.Enumerator
				tor = eventList.GetEnumerator(); //返回实例的枚举数 就是返回集的中所有元素一个一个列出来
			while (tor.MoveNext())
			{
				tor.Current.Value.Remove(listener);
			}
		}
	}
}

#endif
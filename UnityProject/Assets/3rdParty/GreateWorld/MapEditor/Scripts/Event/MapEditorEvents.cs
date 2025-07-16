// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-11-11 14:20 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace GEngine.MapEditor
{
	/// <summary>
	/// 定义事件
	/// </summary>
	public class MapEditorEvents
	{
		public class Event
		{
			public readonly string eventName;

			public Event(string mEventname)
			{
				eventName = mEventname;
			}
		}

		public class Event<T>
		{
			public readonly string eventName;

			public Event(string mEventname)
			{
				eventName = mEventname;
			}
		}

		public class Event<T1, T2>
		{
			public readonly string eventName;

			public Event(string mEventname)
			{
				eventName = mEventname;
			}
		}

		public class Event<T1, T2, T3>
		{
			public readonly string eventName;

			public Event(string mEventname)
			{
				eventName = mEventname;
			}
		}

		public class Event<T1, T2, T3, T4>
		{
			public readonly string eventName;

			public Event(string mEventname)
			{
				eventName = mEventname;
			}
		}
	}
}
#endif
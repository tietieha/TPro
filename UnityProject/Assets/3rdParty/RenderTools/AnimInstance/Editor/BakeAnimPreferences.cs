// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-09-01 10:59 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RenderTools.AnimInstance
{
	[Serializable]
	public class BakeAnimPreferences : ScriptableObject
	{
		public DefaultAsset outputFolder;
		public BakeAnimStateDic bakeAnimStates = new BakeAnimStateDic();
		public List<AnimationClip> customClips = new List<AnimationClip>();
	}
	
	[Serializable]
	public class BakeAnimState
	{
		public bool isBake;
		public int frame;
	}
	
	[Serializable]
	public class BakeAnimStateDic : SerializedDictionary<string, BakeAnimState>
	{
		public BakeAnimState this[string name]
		{
			get
			{
				if(dictionary.TryGetValue(name, out BakeAnimState state))
				{
					return state;
				}
				return null;
			}
			set
			{
				if (!dictionary.ContainsKey(name))
				{
					dictionary.Add(name, value);
				}
				else
				{
					dictionary[name] = value;
				}
			}
		}

		public void Clear()
		{
			dictionary.Clear();
		}
	}
}
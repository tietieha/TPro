// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-08-08 16:31 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenderTools.AnimInstance
{
	[System.Serializable]
	public class AnimBaker : ScriptableObject
	{
		[Serializable]
		public class AnimTexs : SerializedDictionary<string, Texture2D>
		{
			public Texture2D this[string name]
			{
				get
				{
					if(dictionary.TryGetValue(name, out Texture2D tex))
					{
						return tex;
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
		
		public string AnimName;
		public float Length;
		public WrapMode WrapMode;
		public AnimTexs AnimTex = new AnimTexs();
		
		public bool IsName(string animName)
		{
			if (animName.Length != this.AnimName.Length)
				return false;
			return string.CompareOrdinal(animName, this.AnimName) == 0;
		}
	}
}
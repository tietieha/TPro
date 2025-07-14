// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-12-06 15:33 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using UnityEngine;

namespace BlurEffect
{
	[Serializable]
	public class BlurEffectParam
	{
		public float blur_size = 0.5f;
		[Range(1, 4)] 
		public int blur_iteration = 1;
		[Range(1, 4)]
		public int blur_down_sample = 2;
		public float blur_spread = 1.0f;
		[Range(0, 1)] 
		public float color_to_dark = 1.0f;
		[Range(0, 1)] 
		public float color_saturate = 1.0f;
	}
}
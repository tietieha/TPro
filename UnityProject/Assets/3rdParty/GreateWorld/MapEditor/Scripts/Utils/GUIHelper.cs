#if UNITY_EDITOR
// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-07-04 21:18 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************
using UnityEngine;

namespace GEngine.MapEditor
{
	public class GUIHelper
	{
		public static bool Button(string label, Color color, params GUILayoutOption[] options)
		{
			Color preColor = GUI.color;
			GUI.color = color;
			if (GUILayout.Button(label, options))
			{
				return true;
			}

			GUI.color = preColor;
			return false;
		}
	}
}
#endif
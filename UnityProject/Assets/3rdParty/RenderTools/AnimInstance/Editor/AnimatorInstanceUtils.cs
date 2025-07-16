// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-08-02 11:41 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System.IO;
using UnityEditor;

namespace RenderTools.AnimInstance
{
	public class AnimatorInstanceUtils
	{
		public static string[] GetTargetsPath(string targetFilePath, string searchType)
		{
			string[] pathArr = {};
			if (Directory.Exists(targetFilePath))
			{
				string[] guids = AssetDatabase.FindAssets($"t:{searchType}", new[] {targetFilePath});
				pathArr = new string[guids.Length];
				for (int i = 0; i < guids.Length; i++)
				{
					pathArr[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
				}
			}

			return pathArr;
		}
	}
}
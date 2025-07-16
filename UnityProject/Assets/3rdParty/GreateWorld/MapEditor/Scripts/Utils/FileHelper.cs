// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-19 17:29 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class FileHelper
	{
		private static string _defaultDir = "";

		static void SaveDefaultDirectory(string fileName)
		{
			FileInfo fi = new FileInfo(fileName);
			_defaultDir = fi.Directory.FullName;
			PlayerPrefs.SetString("defaultDir", _defaultDir);
			PlayerPrefs.Save();
		}

		public static void OpenFileDialog(string title, System.Action<string> callback, string[] filters)
		{
			if (string.IsNullOrEmpty(_defaultDir))
			{
				_defaultDir = PlayerPrefs.GetString("defaultDir", Application.dataPath);
			}

			var fileName = EditorUtility.OpenFilePanelWithFilters(title, _defaultDir, filters);
			if (!string.IsNullOrEmpty(fileName))
			{
				SaveDefaultDirectory(fileName);
				callback?.Invoke(fileName);
			}
		}
		
		/// <summary>
		/// 获取预制体资源路径。
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		//[调整下面的代码，根据参数判定是获取资源路径还是GUID]
		public static string GetPrefabAssetPath(GameObject gameObject, bool isPath = false)
		{
#if UNITY_EDITOR
			// Project中的Prefab是Asset不是Instance
			if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
			{

				// 预制体资源就是自身
				return isPath ? AssetDatabase.GetAssetPath(gameObject) : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(gameObject));
			}

			// Scene中的Prefab Instance是Instance不是Asset
			if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
			{
				// 获取预制体资源
				var prefabAsset = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
				return isPath ? AssetDatabase.GetAssetPath(prefabAsset) : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefabAsset));
			}
#endif

			// 不是预制体
			return null;
		}

	}
}
#endif
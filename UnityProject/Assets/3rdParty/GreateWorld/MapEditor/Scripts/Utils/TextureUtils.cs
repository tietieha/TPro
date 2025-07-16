#if UNITY_EDITOR
// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-06-28 9:56 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System.IO;
using UnityEditor;
using UnityEngine;


namespace GEngine.MapEditor
{
	public class TextureUtils
	{
		[MenuItem("Assets/美术工具/设置Texture最大尺寸/1024", false, 9999)]
		private static void SetTextureMaxSize1024()
		{
			SetTexturesMaxSize(1024);
		}

		[MenuItem("Assets/美术工具/设置Texture最大尺寸/512", false, 9998)]
		private static void SetTextureMaxSize512()
		{
			SetTexturesMaxSize(512);
		}

		[MenuItem("Assets/美术工具/设置Texture最大尺寸/256", false, 9997)]
		private static void SetTextureMaxSize256()
		{
			SetTexturesMaxSize(256);
		}

		[MenuItem("Assets/美术工具/设置Texture最大尺寸/128", false, 9996)]
		private static void SetTextureMaxSize128()
		{
			SetTexturesMaxSize(128);
		}
		
		[MenuItem("Assets/美术工具/设置Texture最大尺寸/64", false, 9995)]
		private static void SetTextureMaxSize64()
		{
			SetTexturesMaxSize(64);
		}
		
		[MenuItem("Assets/美术工具/设置Texture最大尺寸/32", false, 9994)]
		private static void SetTextureMaxSize32()
		{
			SetTexturesMaxSize(32);
		}

		private static void SetTexturesMaxSize(int maxSize)
		{
			if (!EditorUtility.DisplayDialog("提示", $"是否将所选图片的最大尺寸设为{maxSize}", "OK"))
				return;

			var guids = Selection.assetGUIDs;
			if (guids == null || guids.Length < 1)
			{
				EditorUtility.DisplayDialog("提示", "请选中包含纹理的文件或者文件夹!", "OK");
				return;
			}

			int index = 0;
			foreach (var guid in guids)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);
				if (string.IsNullOrEmpty(assetPath))
					continue;

				if (EditorUtility.DisplayCancelableProgressBar("正在设置纹理的Max Size",
					    string.Format("已完成：{0}/{1}", index + 1, guids.Length),
					    1.0f * (index + 1) / guids.Length))
					break;
				index++;

				if (Directory.Exists(assetPath))
				{
					// 如果是文件夹
					int subIndex = 0;
					string[] subGuids = AssetDatabase.FindAssets("t:Texture2D", new string[] {assetPath});
					foreach (var subGuid in subGuids)
					{
						if (EditorUtility.DisplayCancelableProgressBar("正在设置纹理的Max Size",
							    string.Format("已完成：{0}/{1}", subIndex + 1, subGuids.Length),
							    1.0f * (subIndex + 1) / subGuids.Length))
							break;

						subIndex++;

						var subAssetPath = AssetDatabase.GUIDToAssetPath(subGuid);
						if (string.IsNullOrEmpty(subAssetPath))
							continue;

						SetTextureMaxSize(subAssetPath, maxSize);
					}
				}
				else
				{
					// 如果是文件
					SetTextureMaxSize(assetPath, maxSize);
				}
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			EditorUtility.ClearProgressBar();
			EditorUtility.DisplayDialog("提示", "处理完毕", "OK");
		}

		private static void SetTextureMaxSize(string assetPath, int maxSize)
		{
			TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			if (null == importer)
				return;

			SetTextureMaxSize(importer, maxSize);
		}

		private static void SetTextureMaxSize(TextureImporter importer, int maxSize)
		{
			bool changed = false;
			if (importer.maxTextureSize != maxSize)
			{
				importer.maxTextureSize = maxSize;
				changed = true;
			}

			changed |= SetTextureMaxSize(importer, maxSize, "Android");
			changed |= SetTextureMaxSize(importer, maxSize, "iPhone");

			if (changed) importer.SaveAndReimport();
		}

		private static bool SetTextureMaxSize(TextureImporter importer, int maxSize, string platform)
		{
			bool changed = false;
			TextureImporterPlatformSettings platformSettings = importer.GetPlatformTextureSettings(platform);
			if (!platformSettings.overridden)
			{
				platformSettings.overridden = true;
				changed = true;
			}

			if (platformSettings.maxTextureSize != maxSize)
			{
				platformSettings.maxTextureSize = maxSize;
				importer.SetPlatformTextureSettings(platformSettings);
				changed = true;
			}
			

			return changed;
		}
		
		public static bool CheckTexReadable(Texture2D tex, bool justCheck = false)
		{
			bool ret = false;
			if (tex != null)
			{
				string p = AssetDatabase.GetAssetPath(tex);
				TextureImporter ti = AssetImporter.GetAtPath(p) as TextureImporter;
				if (ti != null)
				{
					if(!ti.isReadable && !justCheck)
					{
						ti.isReadable = true;
						AssetDatabase.ImportAsset(p, ImportAssetOptions.ForceUpdate);
					}
					ret = ti.isReadable;
				}
			}

			return ret;
		}

		public static Texture2D LoadByIO(string url)
		{
			//创建文件读取流
			FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
			//创建文件长度缓冲区
			byte[] bytes = new byte[fileStream.Length];
			//读取文件
			fileStream.Read(bytes, 0, (int)fileStream.Length);

			//释放文件读取流
			fileStream.Close();
			//释放本机屏幕资源
			fileStream.Dispose();
			fileStream = null;

			//创建Texture
			Texture2D texture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.LoadImage(bytes);

			return texture;
		}
	}
}
#endif
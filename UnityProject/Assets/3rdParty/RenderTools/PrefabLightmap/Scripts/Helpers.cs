#if UNITY_EDITOR
// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-02-28 21:09 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using TEngine;
using UnityEditor;
using UnityEngine;

namespace RenderTools.PrefabLightmap
{
	public static class Helpers
	{
		public static Texture2D CopyTextureToFolder(Texture2D tex, string folder)
		{
			if (tex == null)
				return null;
			string srcPath = AssetDatabase.GetAssetPath(tex);
			string newPath = srcPath.Replace(srcPath.GetDirectoryName(), folder);
			if (AssetDatabase.CopyAsset(srcPath, newPath))
			{
				return AssetDatabase.LoadAssetAtPath<Texture2D>(newPath);
			}

			return null;
		}

		public static Texture2D CopyTexture(Texture2D texture)
		{
			if (texture == null)
				return null;
			// 创建一个与纹理大小相同的临时 RenderTexture
			RenderTexture tmp = RenderTexture.GetTemporary(
				texture.width,
				texture.height,
				0,
				RenderTextureFormat.Default,
				RenderTextureReadWrite.Linear);
						
			Graphics.Blit(texture, tmp);
			RenderTexture previous = RenderTexture.active;
			RenderTexture.active = tmp;
			Texture2D tmpTexture2D = new Texture2D(texture.width, texture.height);
			tmpTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
			tmpTexture2D.Apply();

			RenderTexture.active = previous;
			RenderTexture.ReleaseTemporary(tmp);
			return tmpTexture2D;
		}
	}
}
#endif
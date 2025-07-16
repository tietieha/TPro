using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SpineExWindow : EditorWindow
{
    [MenuItem("Assets/Spine Helper/检测atlas是否有效")]
    private static void ValidateAtlasFile()
    {
        var guids = Selection.assetGUIDs;
        if (guids == null || guids.Length < 1)
        {
            EditorUtility.DisplayDialog("spine检测", "请选中文件（支持多选）", "好的");
            return;
        }

        EditorUtility.DisplayProgressBar("spine检测", "文件检测中...", 1f);
        List<string> invalidList = new List<string>();
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid); //通过GUID获取路径
            if (string.IsNullOrEmpty(assetPath))
            {
                continue;
            }

            bool isFolder = Directory.Exists(assetPath);
            if (isFolder)
            {
                SpineExUtility.ValidateSpineAtlasFileFormat(assetPath, true, ref invalidList);
                continue;
            }

            if (SpineExUtility.ValidateSpineAtlasFileFormat(assetPath) != SpineExUtility.ValidateSpineAtlasFileFormatResult.Invalid)
            {
                continue;
            }
            
            invalidList.Add(assetPath);
        }

        foreach (var path in invalidList)
        {
            Debug.LogError($"spine文件需要为Unity重新导出 : {path}");
        }
        Debug.Log("spine文件检测结束");
        EditorUtility.ClearProgressBar();
    }
}
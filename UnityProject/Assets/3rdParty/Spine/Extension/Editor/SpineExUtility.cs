using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SpineExUtility
{
    public enum ValidateSpineAtlasFileFormatResult
    {
        NotAtlasFile,
        Invalid,
        Valid
    }
    
    public static void ValidateSpineAtlasFileFormat(string folderPath, bool validateSubFolder, ref List<string> invalidList)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            return;
        }

        if (!Directory.Exists(folderPath))
        {
            return;
        }

        var files = Directory.GetFiles(folderPath);
        foreach (var file in files)
        {
            if (ValidateSpineAtlasFileFormat(file) == ValidateSpineAtlasFileFormatResult.Invalid)
            {
                invalidList.Add(file);
            }
        }

        if (validateSubFolder)
        {
            var directories = Directory.GetDirectories(folderPath);
            foreach (var directory in directories)
            {
                ValidateSpineAtlasFileFormat(directory, true, ref invalidList);
            }
        }
    }
    
    public static ValidateSpineAtlasFileFormatResult ValidateSpineAtlasFileFormat(string atlasFilePath)
    {
        if (string.IsNullOrEmpty(atlasFilePath) || !atlasFilePath.EndsWith(".atlas.txt"))
        {
            return ValidateSpineAtlasFileFormatResult.NotAtlasFile;
        }
        
        TextAsset atlasText = (TextAsset)AssetDatabase.LoadAssetAtPath(atlasFilePath, typeof(TextAsset));
        if (atlasText == null)
        {
            return ValidateSpineAtlasFileFormatResult.NotAtlasFile;
        }

        string atlasStr = atlasText.text;
        atlasStr = atlasStr.Replace("\r", "");
        string[] atlasLines = atlasStr.Split('\n');
        for (int i = 0; i < atlasLines.Length - 1; i++)
        {
            if (atlasLines[i].Trim().Length == 0)
            {
                return ValidateSpineAtlasFileFormatResult.Valid;
            }
        }
        
        return ValidateSpineAtlasFileFormatResult.Invalid;
    }
}
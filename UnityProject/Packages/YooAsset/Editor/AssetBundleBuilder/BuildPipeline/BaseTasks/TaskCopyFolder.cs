// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-01-20       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System.IO;
using UnityEditor;

namespace YooAsset.Editor
{
    public class TaskCopyFolder
    {
        /// <summary>
        /// 拷贝目录
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        internal void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                BuildLogger.Error($"Source folder not exist: {sourceFolder}");
                return;
            }

            if (Directory.Exists(destFolder))
            {
                Directory.Delete(destFolder, true);
            }

            CopyDirectory(sourceFolder, destFolder);

            AssetDatabase.Refresh();
            BuildLogger.Log($"Copy folder complete: {sourceFolder} -> {destFolder}");
        }

        /// <summary>
        /// 拷贝目录
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        internal void CopyDirectory(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            string[] files = Directory.GetFileSystemEntries(sourceFolder);

            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    CopyDirectory(file, Path.Combine(destFolder, Path.GetFileName(file)));
                }
                else
                {
                    if (!file.EndsWith(".meta"))
                    {
                        File.Copy(file, Path.Combine(destFolder, Path.GetFileName(file)), true);
                    }
                }
            }
        }
    }
}
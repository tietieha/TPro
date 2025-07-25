﻿using System;
using System.IO;
using System.IO.Compression;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;

namespace TEngine
{
    /// <summary>
    /// Unity平台路径类型。
    /// </summary>
    public enum UnityPlatformPathType : int
    {
        dataPath = 0,
        streamingAssetsPath,
        persistentDataPath,
        temporaryCachePath,
    }

    public static partial class Utility
    {
        /// <summary>
        /// 文件相关的实用函数。
        /// </summary>
        public static class File
        {
            public static bool CreateFile(string filePath, bool isCreateDir = true)
            {
                if (!System.IO.File.Exists(filePath))
                {
                    string dir = System.IO.Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(dir))
                    {
                        if (isCreateDir)
                        {
                            Directory.CreateDirectory(dir);
                        }
                        else
                        {
                            Log.Error("文件夹不存在 Path=" + dir);
                            return false;
                        }
                    }

                    System.IO.File.Create(filePath);
                }

                return true;
            }

            public static bool CreateFile(string filePath, string info, bool isCreateDir = true)
            {
                StreamWriter sw;
                FileInfo t = new FileInfo(filePath);
                if (!t.Exists)
                {
                    string dir = System.IO.Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(dir))
                    {
                        if (isCreateDir)
                        {
                            Directory.CreateDirectory(dir);
                        }
                        else
                        {
#if UNITY_EDITOR
                            EditorUtility.DisplayDialog("Tips", "文件夹不存在", "CANCEL");
#endif
                            Log.Error("文件夹不存在 Path=" + dir);
                            return false;
                        }
                    }

                    sw = t.CreateText();
                }
                else
                {
                    sw = t.AppendText();
                }

                sw.WriteLine(info);
                sw.Close();
                sw.Dispose();
                return true;
            }

            public static string GetPersistentDataPlatformPath(string filePath)
            {
                filePath =
#if UNITY_ANDROID && !UNITY_EDITOR
             Application.dataPath + "!assets" + "/" + filePath;
#else
                    Application.streamingAssetsPath + "/" + filePath;
#endif
                return filePath;
            }

            public static string GetPath(string path)
            {
                return path.Replace("\\", "/");
            }

            public static bool ReadPersistentFile(string path, out string result, bool log = true)
            {
                result = null;
                string fullPath_p = path.Contains(Application.persistentDataPath) ? path : Application.persistentDataPath + "/" + path;
                bool exist = true;
                if (System.IO.File.Exists(fullPath_p))
                {
                    result = System.IO.File.ReadAllText(fullPath_p);
                }
                else
                {
                    exist = false;
                    if (log)
                    {
                        Debug.LogError($"Persistent load failed :{fullPath_p}");
                    }
                }

                return exist;
            }

            public static byte[] ReadPersistentBytes(string path)
            {
                byte[] result = null;
                string fullPath = Application.persistentDataPath + "/" + path;
                if (System.IO.File.Exists(fullPath))
                {
                    result = System.IO.File.ReadAllBytes(fullPath);
                }
                else
                {
                    Debug.LogError($"Persistent read bytes failed :{fullPath}");
                }

                return result;
            }

            public static void WritePersistentFile(string path, string info)
            {
                System.IO.File.WriteAllText(path, info);
            }

            public static string ReadStreamingAssetsText(string path)
            {
                string reslut = null;
                string fullPath = path.Contains(Application.streamingAssetsPath) ? path : Application.streamingAssetsPath + "/" + path;
#if UNITY_EDITOR
                if (System.IO.File.Exists(fullPath))
                {
                    StreamReader sr = System.IO.File.OpenText(fullPath);
                    reslut = sr.ReadToEnd();
                    sr.Close();
                }

#elif UNITY_ANDROID && !UNITY_EDITOR
                UnityWebRequest unityWebRequest = UnityWebRequest.Get(fullPath);
                unityWebRequest.SendWebRequest();
                while (unityWebRequest.error == null)
                {
                    if (unityWebRequest.downloadHandler.isDone)
                    {
                        reslut = unityWebRequest.downloadHandler.text;
                        break;
                    }
                }
                unityWebRequest.Dispose();
                if (reslut == null)
                {
                    Debug.LogError($"{fullPath} 加载失败");
                }
#elif UNITY_IOS && !UNITY_EDITOR
                if (System.IO.File.Exists(fullPath))
                {
                    StreamReader sr = System.IO.File.OpenText(fullPath);
                    reslut = sr.ReadToEnd();
                    sr.Close();
                }
#endif
                return reslut;
            }

            public static byte[] ReadStreamingAssetsBytes(string path)
            {
                byte[] reslut = null;
                string fullPath = path.Contains(Application.streamingAssetsPath) ? path : Application.streamingAssetsPath + "/" + path;
#if UNITY_EDITOR
                if (System.IO.File.Exists(fullPath))
                {
                    reslut = System.IO.File.ReadAllBytes(fullPath);
                }

#elif UNITY_ANDROID && !UNITY_EDITOR
                UnityWebRequest unityWebRequest = UnityWebRequest.Get(fullPath);
                unityWebRequest.SendWebRequest();
                while (unityWebRequest.error == null)
                {
                    if (unityWebRequest.downloadHandler.isDone)
                    {
                        reslut = unityWebRequest.downloadHandler.data;
                        break;
                    }
                }
                unityWebRequest.Dispose();
                if (reslut == null)
                {
                    Debug.LogError($"{fullPath} 加载失败");
                }
#elif UNITY_IOS && !UNITY_EDITOR
                if (System.IO.File.Exists(fullPath))
                {
                    reslut = System.IO.File.ReadAllBytes(fullPath);
                }
#endif
                return reslut;
            }

            public static string ReadAssetsText(string path)
            {
                string reslut = null;
                string fullPath = $"{Application.dataPath.Replace("/Assets", "")}/{path}";
#if UNITY_EDITOR
                if (System.IO.File.Exists(fullPath))
                {
                    StreamReader sr = System.IO.File.OpenText(fullPath);
                    reslut = sr.ReadToEnd();
                    sr.Close();
                }
#endif
                return reslut;
            }

            public static byte[] ReadAssetsBytes(string path)
            {
                byte[] reslut = null;
                string fullPath = $"{Application.dataPath.Replace("/Assets", "")}/{path}";
#if UNITY_EDITOR
                if (System.IO.File.Exists(fullPath))
                {
                    reslut = System.IO.File.ReadAllBytes(fullPath);
                }
#endif
                return reslut;
            }

            public static string Md5ByPathName(string pathName)
            {
                try
                {
                    FileStream file = new FileStream(pathName, FileMode.Open);
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }

                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    Log.Error("to md5 fail,error:" + ex.Message);
                    return "Error";
                }
            }

            public static string GetLengthString(long length)
            {
                if (length < 1024)
                {
                    return $"{length.ToString()} Bytes";
                }

                if (length < 1024 * 1024)
                {
                    return $"{(length / 1024f):F2} KB";
                }

                return length < 1024 * 1024 * 1024
                    ? $"{(length / 1024f / 1024f):F2} MB"
                    : $"{(length / 1024f / 1024f / 1024f):F2} GB";
            }

            public static string GetByteLengthString(long byteLength)
            {
                if (byteLength < 1024L) // 2 ^ 10
                {
                    return Utility.Text.Format("{0} Bytes", byteLength.ToString());
                }

                if (byteLength < 1048576L) // 2 ^ 20
                {
                    return Utility.Text.Format("{0} KB", (byteLength / 1024f).ToString("F2"));
                }

                if (byteLength < 1073741824L) // 2 ^ 30
                {
                    return Utility.Text.Format("{0} MB", (byteLength / 1048576f).ToString("F2"));
                }

                if (byteLength < 1099511627776L) // 2 ^ 40
                {
                    return Utility.Text.Format("{0} GB", (byteLength / 1073741824f).ToString("F2"));
                }

                if (byteLength < 1125899906842624L) // 2 ^ 50
                {
                    return Utility.Text.Format("{0} TB", (byteLength / 1099511627776f).ToString("F2"));
                }

                if (byteLength < 1152921504606846976L) // 2 ^ 60
                {
                    return Utility.Text.Format("{0} PB", (byteLength / 1125899906842624f).ToString("F2"));
                }

                return Utility.Text.Format("{0} EB", (byteLength / 1152921504606846976f).ToString("F2"));
            }

            public static string BinToUtf8(byte[] total)
            {
                byte[] result = total;
                if (total[0] == 0xef && total[1] == 0xbb && total[2] == 0xbf)
                {
                    // utf8文件的前三个字节为特殊占位符，要跳过
                    result = new byte[total.Length - 3];
                    System.Array.Copy(total, 3, result, 0, total.Length - 3);
                }

                string utf8string = System.Text.Encoding.UTF8.GetString(result);
                return utf8string;
            }

            /// <summary>
            /// 数据格式转换
            /// </summary>
            /// <param name="data">数据</param>
            /// <returns></returns>
            public static string FormatData(long data)
            {
                string result = "";
                if (data < 0)
                    data = 0;

                if (data > 1024 * 1024)
                {
                    result = ((int)(data / (1024 * 1024))).ToString() + "MB";
                }
                else if (data > 1024)
                {
                    result = ((int)(data / 1024)).ToString() + "KB";
                }
                else
                {
                    result = data + "B";
                }

                return result;
            }

            /// <summary>
            /// 获取文件大小
            /// </summary>
            /// <param name="path">文件路径</param>
            /// <returns></returns>
            public static long GetFileSize(string path)
            {
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return file.Length;
                }
            }

            /// <summary>
            /// 删除文件夹
            /// </summary>
            /// <param name="pathFolder"></param>
            public static void DeleteFolder(string pathFolder)
            {
                if (Directory.Exists(pathFolder))
                {
                    Directory.Delete(pathFolder, true);
                }
            }

            public static void DeleteFolderUnderSandbox()
            {

            }

            public static bool CheckFolder(string pathFolder, bool isCreate)
            {
                if (!string.IsNullOrEmpty(pathFolder))
                {
                    if (!Directory.Exists(pathFolder))
                    {
                        if (isCreate)
                        {
                            Directory.CreateDirectory(pathFolder);
                        }

                        return false;
                    }

                    return true;
                }

                return false;
            }

            public static void UnZip(string zipPath, string outPath)
            {
                DeleteFolder(outPath);
                CheckFolder(outPath, true);
                ZipFile.ExtractToDirectory(zipPath, outPath);
            }
        }
    }
}
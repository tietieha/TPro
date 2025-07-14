using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace TEngine.Editor
{
    /// <summary>
    /// 日志重定向相关的实用函数。
    /// </summary>
    internal static class LogRedirection
    {
        [OnOpenAsset(0)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            if (line <= 0)
            {
                return false;
            }

            // 获取资源路径
            string assetPath = AssetDatabase.GetAssetPath(instanceID);

            // 判断资源类型
            if (!assetPath.EndsWith(".cs"))
            {
                return false;
            }

            bool autoFirstMatch = assetPath.Contains("Logger.cs") ||
                                  assetPath.Contains("DefaultLogHelper.cs") ||
                                  assetPath.Contains("GameFrameworkLog.cs") ||
                                  assetPath.Contains("AssetsLogger.cs") ||
                                  assetPath.Contains("Log.cs") || 
                                  assetPath.Contains("LuaEnv.cs") ||
                                  assetPath.Contains("UnityEngine_DebugWrap.cs");

            var stackTrace = GetStackTrace();
            if (!string.IsNullOrEmpty(stackTrace) && (stackTrace.Contains("[Debug]") ||
                                                      stackTrace.Contains("[INFO]") ||
                                                      stackTrace.Contains("[ASSERT]") ||
                                                      stackTrace.Contains("[WARNING]") ||
                                                      stackTrace.Contains("[ERROR]") ||
                                                      stackTrace.Contains("[EXCEPTION]") ||
                                                      stackTrace.Contains(".lua.txt")))

            {
                if (!autoFirstMatch)
                {
                    var fullPath = UnityEngine.Application.dataPath.Substring(0,
                        UnityEngine.Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal));
                    fullPath = $"{fullPath}{assetPath}";
                    // 跳转到目标代码的特定行
                    InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                    return true;
                }

                if (!stackTrace.Contains(".lua.txt"))
                {
                    // 使用正则表达式匹配at的哪个脚本的哪一行
                    var matches = Regex.Match(stackTrace, @"\(at (.+)\)",
                        RegexOptions.IgnoreCase);
                    while (matches.Success)
                    {
                        var pathLine = matches.Groups[1].Value;

                        if (!pathLine.Contains("Logger.cs") &&
                            !pathLine.Contains("DefaultLogHelper.cs") &&
                            !pathLine.Contains("GameFrameworkLog.cs") &&
                            !pathLine.Contains("AssetsLogger.cs") &&
                            !pathLine.Contains("Log.cs"))
                        {
                            var splitIndex = pathLine.LastIndexOf(":", StringComparison.Ordinal);
                            // 脚本路径
                            var path = pathLine.Substring(0, splitIndex);
                            // 行号
                            line = Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                            var fullPath = UnityEngine.Application.dataPath.Substring(0,
                                UnityEngine.Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal));
                            fullPath = $"{fullPath}{path}";
                            // 跳转到目标代码的特定行
                            InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                            break;
                        }

                        matches = matches.NextMatch();
                    }
                }
                else
                {
                    var matches2 = Regex.Match(stackTrace, @"(.+\.lua\.txt):(\d+)", RegexOptions.IgnoreCase);
                    while (matches2.Success)
                    {
                        var pathLine = matches2.Groups[0].Value;

                        if (pathLine.Contains("LuaException"))
                        {
                            // 使用正则表达式提取 href 和 line 的值
                            string pattern = @"<a href=""([^""]+)"" line=""(\d+)"">";
                            Match match = Regex.Match(pathLine, pattern);

                            if (match.Success)
                            {
                                // 提取 href 和 line 的值
                                string path = match.Groups[1].Value; // 第一个捕获组对应 href
                                line = int.Parse(match.Groups[2].Value); // 第二个捕获组对应 line
                                var fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal));
                            
                                fullPath = $"{fullPath}{path}";
                                fullPath = fullPath.Replace('/', '\\');
              
                                InternalEditorUtility.OpenFileAtLineExternal(fullPath, line);
                            }

                            return true;
                        }
                        
                        if (pathLine.Contains(".lua.txt"))
                        {
                            var splitIndex = pathLine.LastIndexOf(":", StringComparison.Ordinal);

                            Match match = Regex.Match(pathLine, @"Assets.*?\.lua\.txt");
                            var path = match.Value;
                            
                            // 行号
                            line = Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                            var fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal));
                            
                            fullPath = $"{fullPath}{path}";
                            fullPath = fullPath.Replace('/', '\\');
              
                            InternalEditorUtility.OpenFileAtLineExternal(fullPath, line);
                            return true;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取当前日志窗口选中的日志的堆栈信息。
        /// </summary>
        /// <returns>选中日志的堆栈信息实例。</returns>
        private static string GetStackTrace()
        {
            // 通过反射获取ConsoleWindow类
            var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            // 获取窗口实例
            var fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow",
                BindingFlags.Static |
                BindingFlags.NonPublic);
            if (fieldInfo != null)
            {
                var consoleInstance = fieldInfo.GetValue(null);
                if (consoleInstance != null)
                    if (EditorWindow.focusedWindow == (EditorWindow)consoleInstance)
                    {
                        // 获取m_ActiveText成员
                        fieldInfo = consoleWindowType.GetField("m_ActiveText",
                            BindingFlags.Instance |
                            BindingFlags.NonPublic);
                        // 获取m_ActiveText的值
                        if (fieldInfo != null)
                        {
                            var activeText = fieldInfo.GetValue(consoleInstance).ToString();
                            return activeText;
                        }
                    }
            }

            return null;
        }
    }
}
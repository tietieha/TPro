using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UnityToolbarExtender
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle HighlightStyle;
        public static readonly GUIStyle ButtonStyle;
        public static readonly GUIStyle OSXStyle;
        public static readonly GUIStyle WarnningStyle;

        static ToolbarStyles()
        {
            ButtonStyle = new GUIStyle("U2D.createRect")
            {
                fixedHeight = 23,
                padding     = new RectOffset(10,10,2,0),
                margin      = new RectOffset(0,0,0,0),
                alignment   = TextAnchor.UpperCenter,
                fontStyle   = FontStyle.Bold,
                fontSize    = 14
            };
            ButtonStyle.normal.textColor  = new Color(0.4f, 1f, 0.4f);

            HighlightStyle = new GUIStyle("SelectionRect")
            {
                fixedHeight = 23,
                padding     = new RectOffset(10,10,2,0),
                alignment   = TextAnchor.UpperCenter,
                fontStyle   = FontStyle.Bold,
                fontSize    = 14
            };
            HighlightStyle.normal.textColor  = new Color(0.4f, 0.8f, 1f);
            
            WarnningStyle = new GUIStyle("SelectionRect")
            {
                fixedHeight = 23,
                padding     = new RectOffset(10,10,2,0),
                alignment   = TextAnchor.UpperCenter,
                fontStyle   = FontStyle.Bold,
                fontSize    = 14
            };
            WarnningStyle.normal.textColor  = new Color(1f, 0f, 0f);
            
            OSXStyle = new GUIStyle("Command")
            {
                fixedHeight   = 23,
                padding       = new RectOffset(10,10,2,0),
                alignment     = TextAnchor.UpperCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle     = FontStyle.Bold,
                fontSize      = 14,
                fixedWidth    = 0
            };
        }
    }

    [InitializeOnLoad]
    public class GitTool
    {
        static GitTool()
        {

            ToolbarExtender.LeftToolbarGUI.Add(RepositoryInfo);
            // ToolbarExtender.LeftToolbarGUI.Add(ProjectInfo);

            
            // ToolbarExtender.RightToolbarGUI.Add(DataTableInfo);
        }

        private static string _git_path;

        static string GetGitInstallPath()
        {
            if(string.IsNullOrEmpty(_git_path))
            {
                string sPath = System.Environment.GetEnvironmentVariable("Path");
                var result = sPath.Split(';');
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].Contains(@"\Git\cmd") || result[i].Contains(@"\Git\bin"))
                    {
                        _git_path = result[i];
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(_git_path))
            {
                if (File.Exists("C:/Program Files/Git/bin/git.exe"))
                    _git_path = "C:/Program Files/Git/bin";
                else if(File.Exists("C:/Program Files (x86)/Git/bin/git.exe"))
                    _git_path = "C:/Program Files (x86)/Git/bin";
                else if (File.Exists("D:/Program Files (x86)/Git/bin/git.exe"))
                    _git_path = "D:/Program Files (x86)/Git/bin";
                else if (File.Exists("D:/Program Files/Git/bin/git.exe"))
                    _git_path = "D:/Program Files/Git/bin";
            }

            return _git_path;
        }

        static void RepositoryInfo()
        {
#if UNITY_EDITOR_WIN
            // string gitFile = GetGitExecuteFile();
            // if (string.IsNullOrEmpty(gitFile))
            //     return;

            // 获得git根目录
            string projectPath = Application.streamingAssetsPath.Substring(0, Application.streamingAssetsPath.IndexOf("UnityProject"));
            // string branchName = GetGitBranchName(projectPath, gitFile);

            string buttonName = string.Format("目录：{0}", GetGitRepositoryName(true));

            float width = ToolbarStyles.ButtonStyle.CalcSize(new GUIContent(buttonName)).x;
            if (GUILayout.Button(new GUIContent(buttonName, "打开根目录"), ToolbarStyles.ButtonStyle,GUILayout.Width(width + 30)))
            {
                InternalOpenFolder(projectPath);
            }
#endif
#if UNITY_EDITOR_OSX
            string gitFile = GetGitExecuteFile();
            if (string.IsNullOrEmpty(gitFile))
                return;
            // 获得git根目录
            string projectPath = Application.streamingAssetsPath.Substring(0, Application.streamingAssetsPath.IndexOf("UnityProject"));
            string branchName = GetGitBranchName(projectPath, gitFile);

            string buttonName = string.Format("仓库：{0}", GetGitRepositoryName());

            float width = ToolbarStyles.ButtonStyle.CalcSize(new GUIContent(buttonName)).x;
            
            if (GUILayout.Button(new GUIContent(buttonName, "打开根目录"), ToolbarStyles.ButtonStyle,GUILayout.Width(width + 30)))
            {
                InternalOpenFolder(projectPath);
            }
#endif
        }

        static void ProjectInfo()
        {
            if (!EditorPrefs.GetBool("EnableGitToolBar"))
            {
                return;
            }
            
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX
            string gitFile = GetGitExecuteFile();
            if (string.IsNullOrEmpty(gitFile))
                return;

            // 获得git根目录
            string projectPath = Application.streamingAssetsPath.Substring(0,Application.streamingAssetsPath.IndexOf("UnityProject"));
            string branchName = GetGitBranchName(projectPath, gitFile);
            string buttonName = string.Format("分支：{0}", branchName);
            
            float width = ToolbarStyles.ButtonStyle.CalcSize(new GUIContent(buttonName)).x;
            if (GUILayout.Button(new GUIContent(buttonName), ToolbarStyles.HighlightStyle, GUILayout.Width(width)))
            {
                // ShowBranchMenu(projectPath, gitFile, true);
            }
#endif
        }
        
        static void ShowBranchMenu(string projectPath, string gitPath, bool isCode = false)
        {
            List<string> branches = GetBranches(projectPath, gitPath);

            GUIContent[] branchContents = new GUIContent[branches.Count];
            for (int i = 0; i < branches.Count; i++)
            {
                branchContents[i] = new GUIContent(branches[i].Replace("origin/", "").Trim());
            }
            EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 0, 0), branchContents, -1, isCode? CodeBranchMenuSelected : SubmoduleBranchMenuSelected, projectPath);
        }

        static void CodeBranchMenuSelected(object userData, string[] options, int selected)
        {
            string gitFile = GetGitExecuteFile();
            if (string.IsNullOrEmpty(gitFile))
                return;
            string submodulePath = userData as string;
            string selectedBranch = options[selected];
            SwitchBranch(submodulePath, gitFile, selectedBranch, true);
        }

        static void SubmoduleBranchMenuSelected(object userData, string[] options, int selected)
        {
            string gitFile = GetGitExecuteFile();
            if (string.IsNullOrEmpty(gitFile))
                return;
            string submodulePath = userData as string;
            string selectedBranch = options[selected];
            SwitchBranch(submodulePath, gitFile, selectedBranch);
        }

        static List<string> GetBranches(string projectPath, string gitPath)
        {
            string submoduleFullPath = Path.Combine(Application.dataPath, projectPath);
            string fetchCommand = "fetch --prune --all --verbose";
            string branchesCommand = "branch -r";
            RunShellCommand(gitPath, submoduleFullPath, fetchCommand);
            return RunShellCommand(gitPath, submoduleFullPath, branchesCommand);
        }

        static void SwitchBranch(string projectPath, string gitFile, string branchName = "develop", bool isStash = false)
        {
            bool shouldSwitch = EditorUtility.DisplayDialog("切换分支", $"是否要切换到 {branchName} 分支？", "是", "否");
            if (shouldSwitch)
            {
                if (isStash)
                {
                    string stashCommand = "stash save -m Checkout autostash";
                    RunShellCommand(gitFile, projectPath, stashCommand);
                }
                else
                {
                    string resetCommand = "reset --hard";
                    string cleanCommand = "clean -df";
                    RunShellCommand(gitFile, projectPath, resetCommand);
                    RunShellCommand(gitFile, projectPath, cleanCommand);
                }

                string checkCommand = $"rev-parse --verify --quiet refs/heads/{branchName}";
                var count = RunShellCommand(gitFile, projectPath, checkCommand).Count;
                if (count <= 0)
                {
                    string createBranchCommand = $"branch -b {branchName} origin/{branchName}";
                    RunShellCommand(gitFile, projectPath, createBranchCommand);

                }
                string command = $"checkout {branchName}";
                RunShellCommand(gitFile, projectPath, command);

                string pullCommand = $"pull --rebase=false origin --prune --verbose";
                RunShellCommand(gitFile, projectPath, pullCommand);

                if (isStash)
                {
                    string stashPopCommand = "stash pop";
                    RunShellCommand(gitFile, projectPath, stashPopCommand);
                }
            }
            else
            {
                // 用户点击了“否”或关闭了对话框
                Debug.Log("取消切换分支");
            }
        }
//         static void DataTableInfo()
//         {
// #if UNITY_EDITOR_WIN
//              string gitFile = GetGitExecuteFile();
//              if (string.IsNullOrEmpty(gitFile))
//                  return;
//
//              // 获得git根目录 
//              string projectPath = Application.streamingAssetsPath.Substring(0,Application.streamingAssetsPath.IndexOf("project"));
//              string submoduleName = GetGitSubmoduleName(projectPath);
//              string buttonName = string.Format("配置：{0}", submoduleName);
//
//              GUILayout.Label(new GUIContent(buttonName), ToolbarStyles.HighlightStyle);
// #endif
//         }

        static string GetGitExecuteFile()
        {
#if UNITY_EDITOR_WIN
            string gitPath = GetGitInstallPath();
            if (string.IsNullOrEmpty(gitPath))
                return string.Empty;

            // GUILayout.FlexibleSpace();

            // 获得git安装路径
            string gitFile = System.IO.Path.Combine(gitPath, "git.exe");
            if (!File.Exists(gitFile))
            {
                EditorUtility.DisplayDialog("提示", "Git未安装或者是没有添加到环境变量,不会处理的话可以联系铁亮!", "OK");
                return string.Empty;
            }
            return gitFile;
#endif
            
#if UNITY_EDITOR_OSX

            // 通过which命令获取Git路径
            string whichGitResult = GetOSXGitInstallPath("which git");

            string gitFile = "";
            if (!string.IsNullOrEmpty(whichGitResult) && !whichGitResult.Contains("not found"))
            {
                return gitFile = whichGitResult.Trim();
            }

            return gitFile;
#endif
        }

        // 运行Shell命令的辅助方法
        static string GetOSXGitInstallPath(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = $"-c \"{command}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
        
        static List<string> RunShellCommand(string commandPath, string workingDirectory, string command)
        {
            List<string> outputLines = new List<string>();
            Process process = new Process();
            process.StartInfo.FileName = commandPath;
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += (sender, args) =>
            {
                if (args?.Data != null)
                {
                    outputLines.Add(args.Data);
                    Debug.Log(args.Data);
                }
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args?.Data != null)
                    Debug.Log(args.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
            return outputLines;
        }

        static string GetGitBranchName(string gitRootPath, string gitFile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = gitFile;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = gitRootPath;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "rev-parse --abbrev-ref HEAD";

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            return process.StandardOutput.ReadLine();
        }

        static string GetGitSubmodulePath(string gitRootPath)
        {
            return Path.Combine(gitRootPath, "UnityProject/Assets/GameAsset/Lua/DataTable/Tables/");
        }

        static string GetGitSubmoduleName(string gitRootPath)
        {
            string submoduleName = "游离状态";
 
            string key_work = "heads/"; 
            
            string headFile = Path.Combine(gitRootPath, ".git/modules/UnityProject/Assets/GameAsset/Lua/DataTable/Tables/HEAD");

            // SourceTree 3.4.7开始的版本子模块信息存储在下方的路径 
            if(!File.Exists(headFile))
                headFile = Path.Combine(gitRootPath, "UnityProject/Assets/GameAsset/Lua/DataTable/Tables/.git/HEAD");

            if (!File.Exists(headFile))
                return submoduleName;

            try
            {
                using (FileStream fileStream = new FileStream(headFile, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string content = reader.ReadToEnd(); 
                        int index = content.IndexOf(key_work);
                        if (index > 0)
                        {
                            submoduleName = content.Substring(index + key_work.Length);
                            submoduleName = submoduleName.Replace("\n", "");
                        }

                        reader.Close();
                        reader.Dispose();
                    }

                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"找圈圈看看吧\n {e.ToString()}");
            }
            finally
            {
                
            }

            return submoduleName;
        }


        static string GetGitRepositoryName(bool isShowAll = false)
        {
            int index = Application.streamingAssetsPath.IndexOf("UnityProject");
            string subPath = Application.streamingAssetsPath.Substring(0, index - 1);
            if (isShowAll) return subPath;
            int pos = subPath.LastIndexOf("\\");
            if (pos < 0)
                pos = subPath.LastIndexOf("/");

            return subPath.Substring(pos + 1);
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="folder"></param>
        static void InternalOpenFolder(string folder)
        {
            folder = string.Format("\"{0}\"", folder);
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;
                default:
                    UnityEngine.Debug.LogErrorFormat(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
                    break;
            }
        }
    }
}

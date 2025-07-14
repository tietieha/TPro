#if UNITY_STANDLONE || UNITY_STANDALONE_WIN
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Platform.desktop.common;

namespace Platform.Other
{
    internal static class UnityProcess
    {
        internal static Task<int> RunPlatformView(int port)
        {
            return Task.Run(() =>
            {
                var currentProcess = Process.GetCurrentProcess();
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                var versionName = null == version ? "unknown" : version.ToString();
                var paramObject = new JObject
                {
                    { "port", port },
                    { "pid", currentProcess.Id },
                    { "versionName", versionName }
                };
                var arguments = paramObject.ToString(Newtonsoft.Json.Formatting.None);
                var hex = Hex.Encode(arguments);
                Console.WriteLine(arguments + " - " + hex);
                Debug.WriteLine(arguments + " - " + hex);
                var exePath = FindFile(Environment.CurrentDirectory, "PlatformView.exe");
                var exeFileInfo = new FileInfo(exePath);
                var path = exePath + " " + hex;
                var environmentVariable = Environment.GetEnvironmentVariable("SDK_PLATFORM_ENV");
                var dir = environmentVariable == "UNITY_EDITOR"
                    ? Environment.CurrentDirectory
                    : exeFileInfo.DirectoryName;
                Console.WriteLine(dir);
                Debug.WriteLine(dir);
                var id = StartExternalProcess.Start(path, dir);
                //var id = 0;
                return (int)id;
            });
        }

        private static string FindFile(string folderPath, string fileName)
        {
            foreach (var file in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories))
            {
                if (Directory.Exists(file))
                    return FindFile(file, fileName);
                if (new FileInfo(file).Name.Equals(fileName))
                    return file;
            }

            return null;
        }
    }
}
#endif
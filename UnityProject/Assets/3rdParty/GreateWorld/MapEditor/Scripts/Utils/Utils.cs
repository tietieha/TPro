#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;

namespace GEngine.MapEditor
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class OpenFileName
	{
		public int structSize = 0;
		public IntPtr dlgOwner = IntPtr.Zero;
		public IntPtr instance = IntPtr.Zero;
		public String filter = null;
		public String customFilter = null;
		public int maxCustFilter = 0;
		public int filterIndex = 0;
		public String file = null;
		public int maxFile = 0;
		public String fileTitle = null;
		public int maxFileTitle = 0;
		public String initialDir = null;
		public String title = null;
		public int flags = 0;
		public short fileOffset = 0;
		public short fileExtension = 0;
		public String defExt = null;
		public IntPtr custData = IntPtr.Zero;
		public IntPtr hook = IntPtr.Zero;
		public String templateName = null;
		public IntPtr reservedPtr = IntPtr.Zero;
		public int reservedInt = 0;
		public int flagsEx = 0;
	}


	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class CHOOSECOLOR
	{
		public int lStructSize;
		public int hwndOwner;
		public int hInstance;
		public int rgbResult;
		public IntPtr lpCustColors;
		public int Flags;
		public int lCustData;
		public int lpfnHook;
		public int lpTemplateName;
	}

	public class DllTest
	{
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

		public static bool GetOpenFileName1([In, Out] OpenFileName ofn)

		{
			return GetOpenFileName(ofn);
		}

		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);

		public static bool GetSaveFileName1([In, Out] OpenFileName ofn)
		{
			return GetSaveFileName(ofn);
		}

		//[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		//public static extern bool ChooseColorA(CHOOSECOLOR pChoosecolor);//对应的win32API
		//public static bool ChooseColorA1(CHOOSECOLOR pChoosecolor)
		//{
		//    return ChooseColorA(pChoosecolor);
		//}
	}

	public static class Utils
	{
		private static string _defaultDir = "";

		public static void SaveDefaultDirectory(string fileName)
		{
			FileInfo fi = new FileInfo(fileName);
			_defaultDir = fi.Directory.FullName;
			PlayerPrefs.SetString("defaultDir", _defaultDir);
			PlayerPrefs.Save();
		}

		public static void GetOpenFileDialog(string title, System.Action<string> callback, string[] filters)
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

		public static void GetSaveFileDialog(string title, System.Action<string> callback, string defaultName = "",
			string extension = "")
		{
			if (string.IsNullOrEmpty(_defaultDir))
				_defaultDir = Application.dataPath;

			var fileName = EditorUtility.SaveFilePanel(title, _defaultDir, defaultName, extension);
			if (!string.IsNullOrEmpty(fileName))
			{
				SaveDefaultDirectory(fileName);
				callback?.Invoke(fileName);
			}
		}
	}
}
#endif
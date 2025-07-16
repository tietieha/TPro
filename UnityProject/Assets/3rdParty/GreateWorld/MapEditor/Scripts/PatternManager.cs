#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class PatternManager
	{
		public static PatternManager instance = null;

		public Dictionary<string, PatternZone> Patterns = new Dictionary<string, PatternZone>();

		public PatternManager()
		{
			instance = this;
		}

		public void AddPatternZone(Zone zone)
		{
			PatternZone p = new PatternZone();
			p.UpdateByZone(zone);
			Patterns[p.Guid] = p;
		}

		public void DelPatternZone(string guid)
		{
			Patterns.Remove(guid);
		}

		public void Save(string fileName)
		{
			var bw = new BinaryWriter(new FileStream(fileName, FileMode.Create));
			bw.Write(Patterns.Count);
			foreach (var pattern in Patterns.Values)
			{
				pattern.Save(bw);
			}

			bw.Close();
			EditorUtility.DisplayDialog("保存模具", $"保存模具 {Patterns.Count} 个\n{fileName}", "确定");
		}

		public void LoadOld(string fileName)
		{
			var br = new BinaryReader(new FileStream(fileName, FileMode.Open));
			var count = br.ReadInt32();
			Patterns.Clear();
			for (int i = 0; i < count; i++)
			{
				var pattern = new PatternZone();
				pattern.Load(br, true);
				Patterns[pattern.Guid] = pattern;
			}

			br.Close();
		}

		public void Load(string fileName)
		{
			var br = new BinaryReader(new FileStream(fileName, FileMode.Open));
			var count = br.ReadInt32();
			Patterns.Clear();
			for (int i = 0; i < count; i++)
			{
				var pattern = new PatternZone();
				pattern.Load(br);
				Patterns[pattern.Guid] = pattern;
			}

			br.Close();
		}
	}
}
#endif
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class PatternZone
	{
		public string Guid;
		public Texture2D Icon;
		public string Name;
		public int Level;
		public int Count => _save.Count;
		public int IsBorn;
		public int IsGuanqia;
		public int SubType;
		public int IsOddPos; //保存的位置奇偶行数

		private List<Vector2Int> _save = new List<Vector2Int>();

		public PatternZone()
		{
			Guid = GUID.Generate().ToString();
			Name = Guid;
		}

		public void UpdateByZone(Zone zone)
		{
			Icon = zone.GetImage(out _, out _, out _, out _);
			Level = zone.level;
			IsBorn = zone.isBorn;
			IsGuanqia = zone.isGuanqia;
			SubType = zone.subType;

			HexMap.ResetFind();
			SavePattern(zone.hexagon, zone.hexagon, ref _save);
		}

		public void Save(BinaryWriter bw)
		{
			int ver = 1;
			bw.Write(ver);
			bw.Write(Guid);
			bw.Write(Name);
			byte[] buf = Icon.EncodeToPNG();
			bw.Write(Icon.width);
			bw.Write(Icon.height);
			bw.Write(buf.Length);
			bw.Write(buf);
			bw.Write(Level);
			bw.Write(IsBorn);
			bw.Write(IsGuanqia);
			bw.Write(SubType);
			bw.Write(IsOddPos);
			bw.Write(_save.Count);
			for (int i = 0; i < _save.Count; i++)
			{
				bw.Write(_save[i].x);
				bw.Write(_save[i].y);
			}
		}

		public void Load(BinaryReader br, bool oldVer = false)
		{
			int ver = 0;
			if (!oldVer)
			{
				ver = br.ReadInt32();
			}

			Guid = br.ReadString();
			Name = br.ReadString();
			int w = br.ReadInt32();
			int h = br.ReadInt32();
			int len = br.ReadInt32();
			byte[] buf = br.ReadBytes(len);
			Icon = new Texture2D(w, h);
			Icon.LoadImage(buf);
			Level = br.ReadInt32();
			IsBorn = br.ReadInt32();
			IsGuanqia = br.ReadInt32();
			SubType = br.ReadInt32();

			if (!oldVer)
			{
				IsOddPos = br.ReadInt32();
			}

			int count = br.ReadInt32();
			_save.Clear();
			for (int i = 0; i < count; i++)
			{
				int x = br.ReadInt32();
				int y = br.ReadInt32();
				_save.Add(new Vector2Int(x, y));
			}
		}

		private void SavePattern(Hexagon start, Hexagon hex, ref List<Vector2Int> rets)
		{
			IsOddPos = start.y % 2;
			foreach (var t in hex.neigbours)
			{
				if (t == null || t.Dirty) continue;
				t.Dirty = true;
				if (t.zone == hex.zone)
				{
					rets.Add(new Vector2Int(t.x - start.x, t.y - start.y));
					SavePattern(start, t, ref rets);
				}
			}
		}

		public void ApplyPattern(Hexagon target)
		{
			var map = MapRender.instance.GetMap();
			if (map != null)
			{
				map.NewZone(this, target, 1);
			}
		}

		public void GetAllHexagon(Hexagon target, ref List<Hexagon> rets)
		{
			rets.Clear();

			var map = MapRender.instance.GetMap();
			if (map == null)
				return;

			int offx = 0;
			int todd = target.y % 2;
			rets.Add(target);
			foreach (var t in _save)
			{
				offx = 0;
				if (IsOddPos == 0)
				{
					if (todd == 1)
						offx = Mathf.Abs(t.y) % 2 == 1 ? 1 : 0;
				}
				else
				{
					if (todd == 0)
						offx = Mathf.Abs(t.y) % 2 == 1 ? -1 : 0;
				}

				var hex = map.GetHexagon(target.x + t.x + offx, target.y + t.y);
				if (hex != null)
				{
					rets.Add(hex);
				}
			}
		}
	}
}
#endif
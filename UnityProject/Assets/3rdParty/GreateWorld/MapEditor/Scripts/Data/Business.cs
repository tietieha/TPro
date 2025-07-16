// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-04-10       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class Business
	{
		// 商圈id
		public int ID;
		// 商圈包括的 zone
		public List<Zone> Zones = new List<Zone>();

		public string Name;
		public int    Rotation = 0;
		public bool   Vertical = false;
		
		public Vector2Int Pos  = Vector2Int.zero;
		public Vector2Int Rect = new Vector2Int(130, 60);

		public void Reset()
		{
			Name = $"商圈{ID}";
			Vertical = false;
			Rect = new Vector2Int(130, 60);
			Rotation = 0;
			Pos = CalcBoundCenter();
		}

		public override string ToString()
		{
			return $"{ID},{(Vertical ? 1 : 0)},{Pos.ToString()},{Rect.ToString()},{Rotation}";
		}

		public Vector2Int CalcBoundCenter()
		{
			Bounds bounds = new Bounds();
			bool inited = false;
			foreach (var zone in Zones)
			{
				var zoneBounds = zone.CalcBounds();
				if (!inited)
				{
					bounds = zoneBounds;
					inited = true;
				}
				else
				{
					bounds.Encapsulate(zoneBounds);
				}
			}
			var worldPos = Hex.OffsetToWorld((int) bounds.center.x, (int) bounds.center.y);
			return new Vector2Int((int) worldPos.x, (int) worldPos.z);
		}

		public void SaveData(BinaryWriter bw)
		{
			bw.Write(ID);
			bw.Write(Rotation);
			bw.Write(Vertical ? 1 : 0);
			bw.Write(Pos.x);
			bw.Write(Pos.y);
			bw.Write(Rect.x);
			bw.Write(Rect.y);
		}

		public void LoadData(BinaryReader br)
		{
			ID = br.ReadInt32();
			Rotation = br.ReadInt32();
			Vertical = br.ReadInt32() == 1;
			Pos.x = br.ReadInt32();
			Pos.y = br.ReadInt32();
			Rect.x = br.ReadInt32();
			Rect.y = br.ReadInt32();
			Name = $"商圈{ID}";
		}
	}
}
#endif
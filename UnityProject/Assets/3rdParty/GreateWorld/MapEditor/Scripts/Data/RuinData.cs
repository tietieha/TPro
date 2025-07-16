#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class RuinDepth
	{
		public int depth;
		public List<Hexagon> edges = new List<Hexagon>();

		public RuinDepth(int _depth)
		{
			depth = _depth;
		}
	}

	public class RuinData
	{
		public int RuinId;
		public List<Zone> Zones => _zones;

		private List<Hexagon> _hexs = new List<Hexagon>();
		private List<RuinDepth> _depths = new List<RuinDepth>();
		private List<Zone> _zones = new List<Zone>();
		private int _anchorX;
		private int _anchorY;
		private int _ruinW;
		private int _ruinH;
		private int _cellSideLen = 1;

		public RuinData(int ruinId, List<Zone> zones)
		{
			_zones.Clear();
			_zones.AddRange(zones);
			RuinId = ruinId;
			UpdateData(zones);
		}

		private void UpdateData(List<Zone> zones)
		{
			_zones.Clear();
			_zones.AddRange(zones);

			// 遗迹的所有地格列表
			_hexs.Clear();
			foreach (var zone in zones)
			{
				_hexs.AddRange(zone.hexagons);
			}

			List<Hexagon> temps = new List<Hexagon>(_hexs);

			int depth = 1;
			_depths.Clear();
			while (temps.Count > 0)
			{
				var depthInfo = new RuinDepth(depth);
				GetRuinEdgeList(ref temps, ref depthInfo.edges, depth);
				_depths.Add(depthInfo);
				Debug.Log($"Ruin[{RuinId}], depth[{depth}] = {depthInfo.edges.Count}");
				depth++;

				if (depth > 1000)
				{
					EditorUtility.DisplayDialog("遗迹", $"{RuinId}号遗迹边界遍历异常！！！", "确定");
					break;
				}
			}

			// 遗迹包围盒
			int minX = Int32.MaxValue, maxX = 0;
			int minY = Int32.MaxValue, maxY = 0;
			foreach (var hex in _hexs)
			{
				minX = Math.Min(hex.x, minX);
				minY = Math.Min(hex.y, minY);
				maxX = Math.Max(hex.x, maxX);
				maxY = Math.Max(hex.y, maxY);
			}

			_anchorX = minX;
			_anchorY = minY;
			_ruinW = maxX - minX + 1 + 1; // +1 变成length +1 多加1格用于偏移
			_ruinH = maxY - minY + 1 + 1;
		}

		// 获取遗迹区域地格集合的边界地格
		private void GetRuinEdgeList(ref List<Hexagon> hexs, ref List<Hexagon> edges, int depth)
		{
			edges.Clear();
			foreach (var hex in hexs)
			{
				hex.ruinDepth = depth;
			}

			foreach (var hex in hexs)
			{
				for (int i = 0; i < 6; i++)
				{
					if (hex.neigbours[i] == null || hex.neigbours[i].ruinDepth != depth ||
					    hex.zone.ruinId != hex.neigbours[i].zone.ruinId)
					{
						edges.Add(hex);
						break;
					}
				}
			}

			foreach (var eg in edges)
			{
				hexs.Remove(eg);
			}
		}

		public void ExportToBlock(BinaryWriter bw)
		{
			ExportBase(bw);
			// 遗迹包围盒
			ExportBounds(bw);
		}

		public void ExportToServer(BinaryWriter bw)
		{
			ExportBase(bw);
		}

		private void ExportBase(BinaryWriter bw)
		{
			int i, j;
			// 包含的所有地格
			bw.Write(RuinId);
			bw.Write(_hexs.Count);
			for (i = 0; i < _hexs.Count; i++)
			{
				bw.Write(_hexs[i].index);
			}

			// 每个深度的地格列表
			bw.Write(_depths.Count); // 深度层数
			for (i = 0; i < _depths.Count; i++)
			{
				bw.Write(_depths[i].depth); // 深度 depth
				bw.Write(_depths[i].edges.Count); // 包含的地格数量 
				for (j = 0; j < _depths[i].edges.Count; j++)
				{
					bw.Write(_depths[i].edges[j].index);
				}
			}
		}

		private void ExportBounds(BinaryWriter bw)
		{
			// 锚点
			bw.Write(_anchorX); // 锚点X
			bw.Write(_anchorY); // 锚点Y
			bw.Write(_ruinW); // 宽
			bw.Write(_ruinH); // 高

			// 导出贴图
			int texW = _ruinW * _cellSideLen;
			int texH = _ruinH * _cellSideLen;
			Texture2D ruin = new Texture2D(texW, texH, TextureFormat.RGB24, false);
			Color32 defaultColor = new Color32(0, 0, 0, 0);
			var ruinPixels = ruin.GetPixels32();
			for (int k = 0; k < ruinPixels.Length; k++)
			{
				ruinPixels[k] = defaultColor;
			}

			foreach (var hex in _hexs)
			{
				int x = (int) Math.Round((hex.x - _anchorX) * _cellSideLen + (hex.y & 1) * 1f);
				// int x = (hex.x - _anchorX) * _cellSideLen;
				int y = (hex.y - _anchorY) * _cellSideLen;

				for (int texRow = 0; texRow < _cellSideLen; texRow++)
				{
					for (int texCol = 0; texCol < _cellSideLen; texCol++)
					{
						// 边界处理
						int col = x + texCol;
						int row = y + texRow;
						if (col >= texW || row >= texH)
							continue;
						// 垂直翻转
						int index = col + row * texW;
						// int index = col + row * texW;
						ruinPixels[index].r = 255;
					}
				}
			}

			ruin.SetPixels32(ruinPixels);
			ruin.Apply();

			byte[] bytes = ruin.EncodeToPNG();
			bw.Write(bytes.Length);
			bw.Write(bytes);
			// FileStream s = bw.BaseStream as FileStream;
			// string dir = Path.GetDirectoryName(s.Name);
			// string name = Path.GetFileNameWithoutExtension(s.Name);
			// System.IO.File.WriteAllBytes(Path.Combine(dir, $"ruin_{RuinId}.png"), bytes);
			// AssetDatabase.CreateAsset(ruin, Path.Combine(Application.dataPath, $"fow_{RuinId}.png"));
		}
	}
}
#endif
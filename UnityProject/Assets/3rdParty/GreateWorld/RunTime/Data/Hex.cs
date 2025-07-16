// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-06-20 16:16 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using UnityEngine;

namespace GEngine.MapEditor
{
	public static class Hex
	{
		/*
		 * odd-q
		 * 单个六角形的height设为2个unit ==》边长为1.1547f;
		*/
		public static float HexRadius = 2.24f;

		public static float Sqrt3 = 1.7320508f;

		// public

		//中心点偏移
		public static Vector3 CenterUnit = new Vector3(HexRadius * Sqrt3 * 0.5f, 0, HexRadius);

		public static Vector3Int OffsetToCube(Vector2Int offset)
		{
			var x = offset.x - (offset.y - (offset.y & 1)) / 2;
			var z = offset.y;
			var y = -x - z;
			return new Vector3Int(x, y, z);
		}

		public static Vector2Int CubeToOffset(Vector3Int cube)
		{
			var col = cube.x + (cube.z - (cube.z & 1)) / 2;
			var row = cube.z;
			return new Vector2Int(col, row);
		}

		public static Vector3 OffsetToWorld(Vector2Int offset)
		{
			return OffsetToWorld(offset.x, offset.y);
		}

		public static Vector3 OffsetToWorld(int x, int y, float radius = -1)
		{
			var centerUnit = CenterUnit;
			if (radius < 0)
			{
				radius = HexRadius;
			}
			else
			{
				centerUnit = new Vector3(radius * Sqrt3 * 0.5f, 0, radius);
			}

			//先把负z转为正z 再加上中心点的偏移
			var posX = radius * Sqrt3 * (x + 0.5f * (y & 1) + 0.5f);
			var posY = radius * (3 / 2.0f * y + 1);
			return new Vector3(posX, 0, -posY);
		}

		public static Vector2Int WorldToOffset(Vector3 world)
		{
			//先把负z转为正z 再减去中心点的偏移
			world = new Vector3(world.x, world.y, -world.z) - CenterUnit;

			var r = (0 + 2 / 3f * world.z) / HexRadius;
			var q = (Sqrt3 / 3 * world.x - 1 / 3f * world.z) / HexRadius;

			return CubeToOffset(Cube_round(new Vector3(q, -q - r, r)));
		}

		private static Vector3Int Cube_round(Vector3 cube)
		{
			//三个轴先进行round 然后对diff最大的进行校正
			var rx = Mathf.RoundToInt(cube.x);
			var ry = Mathf.RoundToInt(cube.y);
			var rz = Mathf.RoundToInt(cube.z);

			var x_diff = Mathf.Abs(rx - cube.x);
			var y_diff = Mathf.Abs(ry - cube.y);
			var z_diff = Mathf.Abs(rz - cube.z);

			if (x_diff > y_diff && x_diff > z_diff)
				rx = -ry - rz;
			else if (y_diff > z_diff)
				ry = -rx - rz;
			else
				rz = -rx - ry;

			return new Vector3Int(rx, ry, rz);
		}

		private static int HexDistance(Vector3Int hexA, Vector3Int hexB)
		{
			return Mathf.Max(Mathf.Abs(hexB.x - hexA.x), Mathf.Abs(hexB.y - hexA.y), Mathf.Abs(hexB.z - hexA.z));
		}

		public static int TileDistance(Vector2Int a, Vector2Int b)
		{
			return HexDistance(OffsetToCube(a), OffsetToCube(b));
		}


		// 邻接点的偏移量
		// https://www.redblobgames.com/grids/hexagons/#neighbors-offset
		public static readonly Vector2Int[][] NeighborOffset =
		{
			// even cols 偶数列
			new[]
			{
				new Vector2Int(+1,  0),
				new Vector2Int( 0, -1),
				new Vector2Int(-1, -1),
				new Vector2Int(-1,  0),
				new Vector2Int(-1, +1),
				new Vector2Int( 0, +1),
			},
			// odd cols 奇数列
			new[]
			{
				new Vector2Int(+1,  0),
				new Vector2Int(+1, -1),
				new Vector2Int( 0, -1),
				new Vector2Int(-1,  0),
				new Vector2Int( 0, +1),
				new Vector2Int(+1, +1),
			}
		};

		public static Vector2Int[] OddRNeighborsOffset(Vector2Int pos)
		{
			var neighbors = new Vector2Int[6];
			for (int i = 0; i < 6; i++)
			{
				neighbors[i] = OddRNeighborOffset(pos, i);
			}

			return neighbors;
		}

		public static Vector2Int OddRNeighborOffset(Vector2Int pos, int dir)
		{
			var parity = pos.y & 1;
			var d = NeighborOffset[parity][dir];
			return new Vector2Int(pos.x + d.x, pos.y + d.y);
		}

		public static Vector2 GetBounds(int width, int height)
		{
			Vector2 bounds = Vector2.zero;
			bounds.y = width * HexRadius * 1.5f + HexRadius * 0.5f;
			bounds.x = 2 * height + HexRadius * 0.5f;
			return bounds;
		}

		public static Vector2 GetWH(int width, int height)
		{
			Vector2 bounds = Vector2.zero;
			bounds.x = width * HexRadius * Sqrt3 + HexRadius * Sqrt3 / 2;
			bounds.y = height * HexRadius * 1.5f + HexRadius * 0.5f;
			return bounds;
		}
	}
}

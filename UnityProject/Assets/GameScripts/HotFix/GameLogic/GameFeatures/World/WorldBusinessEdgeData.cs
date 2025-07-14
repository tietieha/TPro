// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-04-16       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System.IO;
using UnityEngine;
using XLua;

namespace World
{
	[LuaCallCSharp]
	public class WorldBusinessEdgeData
	{
		public int index = -1;
		public int b1 = -1;
		public int b2 = -1;
		public Mesh Mesh
		{
			get
			{
				if (mesh == null)
					CalcMesh();
				return mesh;
			}
		}

		private Mesh mesh;
		private Vector3[] vertexs;
		private Vector2[] uvs;
		private int[] triangles;
		
		public void Load(BinaryReader br)
		{
			b1 = br.ReadInt32();
			b2 = br.ReadInt32();
			int vertexCcount = br.ReadInt32();
			vertexs = new Vector3[vertexCcount];
			uvs = new Vector2[vertexCcount];
			for (int i = 0; i < vertexCcount; i++)
			{
				int posx = br.ReadInt32();
				int posz = br.ReadInt32();
				vertexs[i] = new Vector3(posx * 0.1f, 0f, posz * 0.1f);
				uvs[i] = new Vector2(0, i % 2);
			}

			int trianglesCount = br.ReadInt32();
			triangles = new int[trianglesCount];
			for (int i = 0; i < trianglesCount; i++)
			{
				triangles[i] = br.ReadUInt16();
			}
		}

		public Mesh CalcMesh()
		{
			mesh = new Mesh();
			mesh.name = $"{b1}-{b2}";
			mesh.vertices = vertexs;
			mesh.triangles = triangles;
			mesh.uv = uvs;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}
	}
}
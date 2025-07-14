using System.Collections.Generic;
using UnityEngine;

public class EdgeSubMeshInfo
{
    public int zoneId;
    public Vector3[] vertexs;
    public int[] triangles;
    public Vector2[] uvs;

    // 
    public List<int> preTriangles = new List<int>(512);

    public EdgeSubMeshInfo(int index)
    {
        zoneId = index;
        preTriangles.Clear();
    }

    // 计算子Mesh信息
    public void CalcMesh(ref Vector3[] allVextexs, ref Vector2[] allUvs)
    {
        HashSet<int> needVerts = new HashSet<int>(preTriangles.Count);
        for (int i = 0; i < preTriangles.Count; i++)
        {
            needVerts.Add(preTriangles[i]);
        }

        var idDict = new Dictionary<int, int>(needVerts.Count);
        vertexs = new Vector3[needVerts.Count];
        uvs = new Vector2[needVerts.Count];

        var index = 0;
        foreach (var idx in needVerts)
        {
            vertexs[index] = allVextexs[idx];
            uvs[index] = allUvs[idx];
            idDict[idx] = index;
            index++;
        }

        triangles = new int[preTriangles.Count];
        for (int i = 0; i < preTriangles.Count; i++)
        {
            triangles[i] = idDict[preTriangles[i]];
        }

        preTriangles.Clear();
    }
}
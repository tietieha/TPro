using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EdgeMeshBody
{
    public int zoneId;
    public MeshFilter mf;
    public MeshRenderer mr;
    public GameObject body;

    public EdgeMeshBody(EdgeSubMeshInfo info, GameObject body, Material mat)
    {
        zoneId = info.zoneId;
        this.body = body;
        mf = body.AddComponent<MeshFilter>();
        mr = body.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh
        {
            name = info.zoneId.ToString(),
            vertices = info.vertexs,
            uv = info.uvs,
            triangles = info.triangles
        };

        mf.mesh = mesh;
        mr.material = mat;
    }

    public void SetMaterial(Material mat)
    {
        if (mr != null)
            mr.material = mat;
    }

    //public void SetColor(Color color)
    //{
    //    if (mr != null && mr.material != null)
    //        mr.material.color = color;
    //}
}

public class WorldZoneEdge : MonoBehaviour
{
    public int zoneId;
    public Material _material1; // 未占领的材质
    public Material _material2; // 已占领的材质

    public Dictionary<int, EdgeMeshBody> subMeshes = new Dictionary<int, EdgeMeshBody>();
    private Material useMaterial;

    public static Dictionary<Color, Material> cacheMat1 = new Dictionary<Color, Material>();
    public static Dictionary<Color, Material> cacheMat2 = new Dictionary<Color, Material>();

    public static void ClearCacheMat()
    {
        foreach (var mat in cacheMat1.Values)
            Destroy(mat);
        cacheMat1.Clear();

        foreach (var mat in cacheMat2.Values)
            Destroy(mat);
        cacheMat2.Clear();
    }

    private void AddSubMesh(EdgeSubMeshInfo info)
    {
        if (subMeshes.ContainsKey(info.zoneId))
            return;

        var body = new GameObject(info.zoneId.ToString());
        body.transform.parent = transform;
        var meshBody = new EdgeMeshBody(info, body, useMaterial);
        subMeshes[info.zoneId] = meshBody;
    }

    public void SetRenderParam(int matIdx, Color color)
    {
        var cache = (matIdx == 0) ? cacheMat1 : cacheMat2;
        if (cache.TryGetValue(color, out var mat))
        {
            useMaterial = mat;
        }
        else
        {
            useMaterial = new Material(matIdx == 0 ? _material1 : _material2);
            useMaterial.name = Time.frameCount.ToString();
            useMaterial.color = color;
            cache.Add(color, useMaterial);
        }

        foreach (var subMesh in subMeshes.Values)
        {
            subMesh.SetMaterial(useMaterial);
        }
    }

    // matIdx  == 0  未占领的  == 1 已占领的
    public void SetData(WorldZoneData info, int matIdx, Color color)
    {
        if (info == null)
            return;

        zoneId = info.ZoneId;
        info.CalcMesh();

        SetRenderParam(matIdx, color);

        foreach (var subInfo in info.SubMeshes.Values)
        {
            AddSubMesh(subInfo);
        }

    }

    public void UpdateParts()
    {
  

        foreach (var it in subMeshes)
        {
            // var zone = GameEntry.World.Zone.GetWorldZoneInfoById(it.Key);
            // if (zone == null) continue; // || string.IsNullOrEmpty(zone.data.AllianceId)
            //
            // // 临时测试，这里应该判断是否是同一个联盟。
            // if (!string.IsNullOrEmpty(zone.data.AllianceId) && zoneInfo.data.AllianceId.Equals(zone.data.AllianceId))
            // {
            //     if (it.Value.body.activeSelf)
            //         it.Value.body.SetActiveEx(false);
            //     continue;
            // }

            // if (!zone.edge_visible)
            // {
            //     if (it.Value.body.activeSelf)
            //         it.Value.body.SetActiveEx(false);
            // }
            // else
            {
                if (!it.Value.body.activeSelf)
                    it.Value.body.SetActiveEx(true);
            }
        }
    }
}
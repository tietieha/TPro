using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EmbattleHexagonMesh : MonoBehaviour
{
    private Mesh m_HexagonMesh;
    private MeshRenderer m_HexagonRenderer;

    private List<Vector3> m_Vetices;
    private List<int> m_Triangles;
    private List<Color> m_Colors;
    private List<Vector2> m_UVs;

    private Dictionary<EmbattleCellColorType, Vector2[]> cellCenterUVMap =
        new Dictionary<EmbattleCellColorType, Vector2[]>();

    private Dictionary<EmbattleCellColorType, Vector2>
        cellColorUVMap = new Dictionary<EmbattleCellColorType, Vector2>();

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = m_HexagonMesh = new Mesh();
        m_HexagonRenderer = GetComponent<MeshRenderer>();

        m_HexagonMesh.name = "Hexagon Mesh";

        m_Vetices = new List<Vector3>();
        m_Triangles = new List<int>();
        m_Colors = new List<Color>();
        m_UVs = new List<Vector2>();
    }

    public void SetMaterial(Material material)
    {
        m_HexagonRenderer.sharedMaterial = material;
    }

    public void ShowRender(bool show)
    {
        m_HexagonRenderer.enabled = show;
    }

    public void Triangulate(EmbattleHexagonCell[] cells)
    {
        if (cells == null || cells.Length == 0)
        {
            return;
        }

        m_HexagonMesh.Clear();
        m_Vetices.Clear();
        m_Triangles.Clear();
        m_Colors.Clear();
        m_UVs.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }

        m_HexagonMesh.vertices = m_Vetices.ToArray();
        m_HexagonMesh.triangles = m_Triangles.ToArray();
        //m_HexagonMesh.colors = m_Colors.ToArray();
        m_HexagonMesh.uv = m_UVs.ToArray();
        m_HexagonMesh.RecalculateNormals();
    }

    public void InitUVMap(int hexagonTexGridNum)
    {
        for (int i = 1; i <= hexagonTexGridNum; i++)
        {
            if (cellCenterUVMap.ContainsKey((EmbattleCellColorType)i))
            {
                cellCenterUVMap[(EmbattleCellColorType)i] = EmbattleHexagonMetrics.GetUVByIndex(i);
            }
            else
            {
                cellCenterUVMap.Add((EmbattleCellColorType)i, EmbattleHexagonMetrics.GetUVByIndex(i));
            }

            if (cellColorUVMap.ContainsKey((EmbattleCellColorType)i))
            {
                cellColorUVMap[(EmbattleCellColorType)i] = EmbattleHexagonMetrics.GetCenterUVByIndex(i);
            }
            else
            {
                cellColorUVMap.Add((EmbattleCellColorType)i, EmbattleHexagonMetrics.GetCenterUVByIndex(i));
            }
        }
    }

    private void Triangulate(EmbattleHexagonCell cell)
    {
        if (cell == null) return;
        Vector3 center = cell.CenterPosition;
        //m_HexagonRenderer.material.mainTexture = cell.CellTexture;
        Vector2[] uvs = cellCenterUVMap.GetValueOrDefault(cell.ColorType);
        Vector2 uv_center = cellColorUVMap.GetValueOrDefault(cell.ColorType);

        for (int i = 0; i < 6; ++i)
        {
            AddTriangle(center, center + EmbattleHexagonMetrics.Corners_PointyTop[i],
                center + EmbattleHexagonMetrics.Corners_PointyTop[i + 1]);
            // AddTriangleColor(cell.Color);

            if (uvs != null)
            {
                AddUV(uv_center, uvs[i], uvs[i + 1]);
            }
        }
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = m_Vetices.Count;
        m_Vetices.Add(v1);
        m_Vetices.Add(v2);
        m_Vetices.Add(v3);
        m_Triangles.Add(vertexIndex);
        m_Triangles.Add(vertexIndex + 1);
        m_Triangles.Add(vertexIndex + 2);
    }

    private void AddTriangleColor(Color color)
    {
        m_Colors.Add(color);
        m_Colors.Add(color);
        m_Colors.Add(color);
    }

    private void AddUV(Vector2 a, Vector2 b, Vector2 c)
    {
        m_UVs.Add(a);
        m_UVs.Add(b);
        m_UVs.Add(c);
    }

    private void OnDestroy()
    {
        m_HexagonMesh.Clear();
        Destroy(m_HexagonMesh);
        m_HexagonMesh = null;
        m_Vetices.Clear();
        m_Triangles.Clear();
        m_Colors.Clear();
        m_UVs.Clear();
        cellCenterUVMap.Clear();
        cellColorUVMap.Clear();
        m_HexagonMesh = null;
        m_Vetices = null;
        m_Triangles = null;
        m_Colors = null;
        m_UVs = null;
        cellCenterUVMap = null;
        cellColorUVMap = null;
    }
}
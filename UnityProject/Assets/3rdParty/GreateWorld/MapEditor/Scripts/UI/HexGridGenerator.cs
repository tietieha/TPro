#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexPrefab;     // 六边形预制体
    public int ringCount = 1;        // 圈数
    public float hexRadius = 1.1547f;     // 六边形外接圆半径

    public void Generate()
    {
        if (hexPrefab == null)
        {
            Debug.LogError("请指定 hexPrefab！");
            return;
        }

        ClearChildren();

        float width = Mathf.Sqrt(3f) * hexRadius;
        float height = 2f * hexRadius;
        float horizSpacing = width;
        float vertSpacing = height * 0.75f;

        for (int q = -ringCount + 1; q < ringCount; q++)
        {
            for (int r = Mathf.Max(-ringCount + 1, -q - ringCount + 1); r < Mathf.Min(ringCount, -q + ringCount); r++)
            {
                Vector2 pos = HexToWorld(q, r, horizSpacing, vertSpacing);
                GameObject hex = Instantiate(hexPrefab, transform);
                hex.transform.localPosition = new Vector3(pos.x, 0, pos.y);
                hex.name = $"Hex_{q}_{r}";
            }
        }
    }

    Vector2 HexToWorld(int q, int r, float horiz, float vert)
    {
        float x = horiz * (q + r / 2f);
        float z = vert * r;
        return new Vector2(x, z);
    }

    public void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (Application.isEditor)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }
    }
}

[CustomEditor(typeof(HexGridGenerator))]
public class HexGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexGridGenerator generator = (HexGridGenerator)target;

        GUILayout.Space(10);
        if (GUILayout.Button("生成六边形格子"))
        {
            generator.Generate();
        }

        if (GUILayout.Button("清空格子"))
        {
            generator.ClearChildren();
        }
    }
}
#endif
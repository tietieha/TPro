using UnityEngine;

namespace GameLogic
{
    // [ExecuteInEditMode]
    public class LineRendererVertical : MonoBehaviour
    {
        public enum SourceType
        {
            LineRenderer,
            CustomWaypoints
        }

        [Header("Source Settings")] public SourceType sourceType = SourceType.CustomWaypoints;
        public Vector3[] customWaypoints;

        [Header("Mesh Settings")] public float height = 1.0f;
        public Vector3 rotation = Vector3.zero; // 新增旋转功能
        public bool autoUpdate = false;
        public Material material;
        public bool generateCollider = false;
        public bool doubleSided = false;

        [Header("UV Settings")] public bool useLineRendererUV = true;
        public Vector2 uvTiling = Vector2.one;
        public Vector2 uvOffset = Vector2.zero;

        private LineRenderer lineRenderer;
        private Transform meshChild;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh mesh;
        private bool dirty = false;

        void OnEnable()
        {
            InitializeComponents();
            GenerateVerticalMesh();
        }

        // private void OnDisable()
        // {
        //     Cleanup();
        // }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            // 销毁生成的Mesh
            if (mesh != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(mesh);
                }
                else
                {
                    DestroyImmediate(mesh);
                }
            }

            // 销毁子物体
            if (meshChild != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(meshChild.gameObject);
                }
                else
                {
                    DestroyImmediate(meshChild.gameObject);
                }
            }
        }



        void OnValidate()
        {
            if (autoUpdate && Application.isEditor)
            {
                InitializeComponents();
                GenerateVerticalMesh();
            }
        }

        void Update()
        {
            if (autoUpdate || dirty)
            {
                GenerateVerticalMesh();
            }
        }

        void InitializeComponents()
        {
            lineRenderer = GetComponent<LineRenderer>();

            // 创建或获取子节点
            if (transform.Find("GeneratedMesh") == null)
            {
                meshChild = new GameObject("GeneratedMesh").transform;
                meshChild.SetParent(transform);
                meshChild.localPosition = Vector3.zero;
                meshChild.localRotation = Quaternion.identity;
            }
            else
            {
                meshChild = transform.Find("GeneratedMesh");
            }

            // 应用旋转设置
            meshChild.localEulerAngles = rotation;

            meshFilter = meshChild.GetComponent<MeshFilter>();
            if (meshFilter == null) meshFilter = meshChild.gameObject.AddComponent<MeshFilter>();

            meshRenderer = meshChild.GetComponent<MeshRenderer>();
            if (meshRenderer == null) meshRenderer = meshChild.gameObject.AddComponent<MeshRenderer>();

            if (material != null)
            {
                meshRenderer.sharedMaterial = material;
            }
        }

        [ContextMenu("Generate Vertical Mesh")]
        public void GenerateVerticalMesh()
        {
            if (mesh == null) mesh = new Mesh();
            mesh.Clear();

            Vector3[] points = GetSourcePoints();
            if (points == null || points.Length < 2) return;

            GenerateMeshData(points);
            meshFilter.mesh = mesh;

            if (generateCollider)
            {
                MeshCollider collider = meshChild.GetComponent<MeshCollider>();
                if (collider == null) collider = meshChild.gameObject.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
            }
            else
            {
                MeshCollider collider = meshChild.GetComponent<MeshCollider>();
                if (collider != null) DestroyImmediate(collider);
            }

            // 应用旋转设置
            meshChild.localEulerAngles = rotation;
        }

        Vector3[] GetSourcePoints()
        {
            switch (sourceType)
            {
                case SourceType.LineRenderer:
                    Vector3[] linePoints = new Vector3[lineRenderer.positionCount];
                    lineRenderer.GetPositions(linePoints);
                    return linePoints;

                case SourceType.CustomWaypoints:
                    return customWaypoints;

                default:
                    return null;
            }
        }

        void GenerateMeshData(Vector3[] points)
        {
            // Vertices
            Vector3[] vertices = new Vector3[points.Length * 2];
            for (int i = 0; i < points.Length; i++)
            {
                vertices[i * 2] = points[i]; // Bottom vertex
                vertices[i * 2 + 1] = points[i] + Vector3.up * height; // Top vertex
            }

            // Triangles
            int triangleCount = (points.Length - 1) * 6;
            if (doubleSided) triangleCount *= 2;

            int[] triangles = new int[triangleCount];
            for (int i = 0; i < points.Length - 1; i++)
            {
                // Front side
                triangles[i * 6] = i * 2;
                triangles[i * 6 + 1] = i * 2 + 1;
                triangles[i * 6 + 2] = i * 2 + 2;

                triangles[i * 6 + 3] = i * 2 + 1;
                triangles[i * 6 + 4] = i * 2 + 3;
                triangles[i * 6 + 5] = i * 2 + 2;

                // Back side (if doubleSided)
                if (doubleSided)
                {
                    int offset = (points.Length - 1) * 6;
                    triangles[offset + i * 6] = i * 2 + 2;
                    triangles[offset + i * 6 + 1] = i * 2 + 1;
                    triangles[offset + i * 6 + 2] = i * 2;

                    triangles[offset + i * 6 + 3] = i * 2 + 2;
                    triangles[offset + i * 6 + 4] = i * 2 + 3;
                    triangles[offset + i * 6 + 5] = i * 2 + 1;
                }
            }

            // UVs
            Vector2[] uv = GenerateUVs(points);

            // Apply to mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        Vector2[] GenerateUVs(Vector3[] points)
        {
            Vector2[] uv = new Vector2[points.Length * 2];

            // 计算路径总长度和累积长度
            float totalLength = 0;
            float[] cumulativeLengths = new float[points.Length];
            for (int i = 1; i < points.Length; i++)
            {
                totalLength += Vector3.Distance(points[i - 1], points[i]);
                cumulativeLengths[i] = totalLength;
            }

            // 根据数据源类型生成UV
            if (sourceType == SourceType.LineRenderer && useLineRendererUV)
            {
                // 尝试获取LineRenderer的实际UV数据
                Vector2[] lineUV = GetLineRendererUV(points, totalLength, cumulativeLengths);

                // 将LineRenderer的UV扩展到垂直面
                for (int i = 0; i < points.Length; i++)
                {
                    float uvX = lineUV != null && lineUV.Length > i
                        ? lineUV[i].x
                        : (totalLength > 0 ? cumulativeLengths[i] / totalLength : 0);

                    uv[i * 2] = new Vector2(uvX * uvTiling.x + uvOffset.x, 0 * uvTiling.y + uvOffset.y);
                    uv[i * 2 + 1] = new Vector2(uvX * uvTiling.x + uvOffset.x, 1 * uvTiling.y + uvOffset.y);
                }
            }
            else
            {
                // 使用自定义UV生成方式
                for (int i = 0; i < points.Length; i++)
                {
                    float uvX = (totalLength > 0) ? cumulativeLengths[i] / totalLength : 0;

                    uv[i * 2] = new Vector2(uvX * uvTiling.x + uvOffset.x, 0 * uvTiling.y + uvOffset.y);
                    uv[i * 2 + 1] = new Vector2(uvX * uvTiling.x + uvOffset.x, 1 * uvTiling.y + uvOffset.y);
                }
            }

            return uv;
        }

        Vector2[] GetLineRendererUV(Vector3[] points, float totalLength, float[] cumulativeLengths)
        {
            // LineRenderer没有直接公开UV数组，所以我们需要根据其设置模拟UV

            // 1. 检查LineRenderer的textureMode
            switch (lineRenderer.textureMode)
            {
                case LineTextureMode.Tile:
                    // 平铺模式 - 根据材质平铺设置和线长度计算UV
                    if (lineRenderer.sharedMaterial != null && lineRenderer.sharedMaterial.mainTexture != null)
                    {
                        float tileLength = lineRenderer.textureScale.x;
                        if (tileLength <= 0) tileLength = 1.0f;

                        Vector2[] uv = new Vector2[points.Length];
                        for (int i = 0; i < points.Length; i++)
                        {
                            uv[i] = new Vector2(cumulativeLengths[i] / tileLength, 0);
                        }

                        return uv;
                    }

                    break;

                case LineTextureMode.Stretch:
                    // 拉伸模式 - 整个纹理沿整条线拉伸
                    Vector2[] stretchUV = new Vector2[points.Length];
                    for (int i = 0; i < points.Length; i++)
                    {
                        stretchUV[i] = new Vector2((float)i / (points.Length - 1), 0);
                    }

                    return stretchUV;

                case LineTextureMode.DistributePerSegment:
                    // 每段分布 - 每个线段使用完整的纹理
                    Vector2[] segmentUV = new Vector2[points.Length];
                    for (int i = 0; i < points.Length; i++)
                    {
                        segmentUV[i] = new Vector2(i % 2 == 0 ? 0 : 1, 0);
                    }

                    return segmentUV;

                case LineTextureMode.RepeatPerSegment:
                    // 每段重复 - 根据线段长度重复纹理
                    Vector2[] repeatUV = new Vector2[points.Length];
                    float segmentLength = 0;
                    for (int i = 1; i < points.Length; i++)
                    {
                        segmentLength += Vector3.Distance(points[i - 1], points[i]);
                    }

                    float repeatRate = lineRenderer.textureScale.x;
                    if (repeatRate <= 0) repeatRate = 1.0f;

                    float currentLength = 0;
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (i > 0) currentLength += Vector3.Distance(points[i - 1], points[i]);
                        repeatUV[i] = new Vector2(currentLength * repeatRate / segmentLength, 0);
                    }

                    return repeatUV;
            }

            // 默认情况 - 基于长度比例
            Vector2[] defaultUV = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                defaultUV[i] = new Vector2(totalLength > 0 ? cumulativeLengths[i] / totalLength : 0, 0);
            }

            return defaultUV;
        }

        public void SetCustomWaypoints(Vector3[] newWaypoints)
        {
            customWaypoints = newWaypoints;
            if (sourceType == SourceType.CustomWaypoints)
            {
                GenerateVerticalMesh();
            }
        }

        public void SetHeight(float newHeight)
        {
            height = newHeight;
            GenerateVerticalMesh();
        }

        public void SetRotation(Vector3 newRotation)
        {
            rotation = newRotation;
            if (meshChild != null)
            {
                meshChild.localEulerAngles = rotation;
            }
        }

        [ContextMenu("Cleanup Generated Objects")]
        public void ManualCleanup()
        {
            Cleanup();
        }

        public void SetDirty()
        {
            dirty = true;
        }
    }
}
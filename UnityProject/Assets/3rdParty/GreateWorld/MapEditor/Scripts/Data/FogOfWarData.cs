// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-05-27       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************
#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
    public class FogOfWarDataMan
    {
        public static int DefaultFogOfWarId = 0;

        private const int GridSize = 5; // 5x5顶点
        private static Mesh _defaultFogOfWarMesh;
        public static Mesh DefaultFogOfWarMesh
        {
            get
            {
                return MapRender.instance.PveFogOfWarMesh;
            }
        }

        private static Material _defaultFogOfWarMaterial;
        public static Material DefaultFogOfWarMaterial
        {
            get
            {
                if (_defaultFogOfWarMaterial == null)
                {
                    _defaultFogOfWarMaterial = new Material(Shader.Find("HP/FX/Features/FogOfWarRT"));
                    _defaultFogOfWarMaterial.name = "DefaultFogOfWarMaterial";
                    _defaultFogOfWarMaterial.SetColor("_Color", Color.black);
                }
                return _defaultFogOfWarMaterial;
            }
        }

        #region GridMesh
        static void CreateDynamicMesh()
        {
            _defaultFogOfWarMesh = new Mesh();
            Vector3[] originalVertices = new Vector3[GridSize * GridSize];

            // 创建初始网格（中心在原点）
            for (int z = 0; z < GridSize; z++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    originalVertices[z * GridSize + x] = new Vector3(x - 2, 0, z - 2) * 10;
                }
            }

            GenerateMeshTriangles(_defaultFogOfWarMesh, originalVertices);
            AssetDatabase.CreateAsset(_defaultFogOfWarMesh, "Assets/DefaultFogOfWarMesh.mesh");
        }

        static void GenerateMeshTriangles(Mesh targetMesh, Vector3[] vertices)
        {
            List<int> triangles = new List<int>();
            for (int z = 0; z < GridSize - 1; z++)
            {
                for (int x = 0; x < GridSize - 1; x++)
                {
                    int tl = z * GridSize + x;
                    int tr = tl + 1;
                    int bl = (z + 1) * GridSize + x;
                    int br = bl + 1;

                    triangles.Add(tl); triangles.Add(bl); triangles.Add(tr);
                    triangles.Add(tr); triangles.Add(bl); triangles.Add(br);
                }
            }

            targetMesh.vertices = vertices;
            targetMesh.triangles = triangles.ToArray();
            targetMesh.RecalculateNormals();
        }
        #endregion

        public FogOfWarData defaultFogOfWarData = new FogOfWarData(DefaultFogOfWarId, Color.black);

        public Dictionary<int, FogOfWarData> FogOfWarDatas => fogOfWarDatas;
        private Dictionary<int, FogOfWarData> fogOfWarDatas = new Dictionary<int, FogOfWarData>();

        public List<FogOfWarData> GetAllFog()
        {
            List<FogOfWarData> list = new List<FogOfWarData>(fogOfWarDatas.Count);
            // list.Add(defaultFogOfWarData);
            list.AddRange(fogOfWarDatas.Values);
            return list;
        }

        public FogOfWarData GetFogOfWarData(int fogOfWarId)
        {
            if (fogOfWarDatas.TryGetValue(fogOfWarId, out var fogData))
            {
                return fogData;
            }
            else
            {
                Debug.LogWarning($"Fog of War ID {fogOfWarId} not found.");
                return null;
            }
        }

        public void RemoveFogOfWarData(int fogOfWarId)
        {
            var fogOfWarData = GetFogOfWarData(fogOfWarId);
            fogOfWarData.Release();
            fogOfWarDatas.Remove(fogOfWarId);
        }

        public int AddFogOfWarData()
        {
            int newFogID = fogOfWarDatas.Count > 0 ? fogOfWarDatas.Max(f => f.Value.FogOfWarId) + 1 : 1;
            var newFogOfWar = new FogOfWarData(newFogID);
            fogOfWarDatas.Add(newFogID, newFogOfWar);
            return newFogID;
        }

        public void SetFogOfWarDataVisible(bool visible)
        {
            foreach (var fogData in fogOfWarDatas.Values)
            {
                fogData.SetVisible(visible);
            }
        }

        public void Reset()
        {
            foreach (var fogData in fogOfWarDatas.Values)
            {
                fogData.Release();
            }
            fogOfWarDatas.Clear();
        }

        public void Save(BinaryWriter bw, List<CampaignFogInfo> fogInfos = null)
        {
            defaultFogOfWarData.Save(bw);

            bw.Write(fogOfWarDatas.Count);
            foreach (var fogData in fogOfWarDatas.Values)
            {
                CampaignFogInfo fogInfo = null;
                if (fogInfos != null)
                {
                    fogInfo = fogInfos.FirstOrDefault(f => f.FogId == fogData.FogOfWarId);
                }
                fogData.Save(bw, fogInfo);
            }

        }

        public void Load(BinaryReader br, int fileVer, List<CampaignFogInfo> fogInfos = null)
        {
            defaultFogOfWarData.Load(br, fileVer);
            fogOfWarDatas.Clear();
            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                FogOfWarData fogData = new FogOfWarData(-1);
                fogData.Load(br, fileVer, fogInfos);
                if (!fogOfWarDatas.ContainsKey(fogData.FogOfWarId))
                {
                    fogOfWarDatas.Add(fogData.FogOfWarId, fogData);
                }
                else
                {
                    Debug.LogWarning($"Fog of War ID {fogData.FogOfWarId} already exists. Skipping duplicate.");
                }
            }

            if (count <= 0 && fogInfos != null && fogInfos.Count > 0)
            {
                // 如果没有FogOfWar数据，但有预设的FogInfos，创建默认的FogOfWar数据
                foreach (var fogInfo in fogInfos)
                {
                    if (!fogOfWarDatas.ContainsKey(fogInfo.FogId))
                    {
                        var newFogData = new FogOfWarData(fogInfo.FogId);
                        newFogData.FogOfWarPosition = fogInfo.FogPos;
                        newFogData.UpdateMeshFromIntVertices(fogInfo.FogGrid.ToArray(), false);
                        newFogData.FogOfWarGameObject.transform.localPosition = fogInfo.FogPos;
                        newFogData.FogOfWarGameObject.SetActive(false);
                        fogOfWarDatas.Add(newFogData.FogOfWarId, newFogData);
                    }
                }
            }
            if (fogInfos != null && count < fogInfos.Count)
            {
                // 如果FogOfWar数据数量少于预设的FogInfos数量，创建缺失的FogOfWar数据
                foreach (var fogInfo in fogInfos)
                {
                    if (!fogOfWarDatas.ContainsKey(fogInfo.FogId))
                    {
                        var newFogData = new FogOfWarData(fogInfo.FogId);
                        newFogData.FogOfWarPosition = fogInfo.FogPos;
                        newFogData.UpdateMeshFromIntVertices(fogInfo.FogGrid.ToArray(), false);
                        newFogData.FogOfWarGameObject.transform.localPosition = fogInfo.FogPos;
                        newFogData.FogOfWarGameObject.SetActive(false);
                        fogOfWarDatas.Add(newFogData.FogOfWarId, newFogData);
                    }
                }
            }
        }
    }

    public class FogOfWarData
    {
        public int FogOfWarId;

        private Color _fogColor = Color.black;

        public Color FogColor
        {
            get { return _fogColor; }
            set
            {
                _fogColor = value;
                FogOfWarMaterial?.SetColor("_Color", FogColor);
            }
        }
        public Vector3 FogOfWarPosition = Vector3.zero;

        public Mesh FogOfWarMesh;
        public Material FogOfWarMaterial;
        public GameObject FogOfWarGameObject;

        public FogOfWarData(int newFogID)
        {
            this.FogOfWarId = newFogID;
            this.FogColor = RenderUtil.GetRandomColor();
            createGO();
        }

        public FogOfWarData(int newFogID, Color fogColor)
        {
            this.FogOfWarId = newFogID;
            this.FogColor = fogColor;
            createGO();
        }

        private void createGO()
        {
            this.FogOfWarMesh = Object.Instantiate(FogOfWarDataMan.DefaultFogOfWarMesh);
            this.FogOfWarMaterial = Object.Instantiate(FogOfWarDataMan.DefaultFogOfWarMaterial);
            this.FogOfWarGameObject = new GameObject($"FogOfWar_{FogOfWarId}");
            FogOfWarMaterial.SetColor("_Color", FogColor);
            FogOfWarGameObject.AddComponent<MeshFilter>().sharedMesh = FogOfWarMesh;
            FogOfWarGameObject.AddComponent<MeshRenderer>().sharedMaterial = FogOfWarMaterial;
        }

        public void UpdateMeshFromIntVertices(Vector3[] verticesInt, bool autoCenterPivot = true)
        {
            // 直接使用原始顶点
            FogOfWarMesh.vertices = verticesInt;

            // 重新计算网格
            FogOfWarMesh.RecalculateNormals();
            FogOfWarMesh.RecalculateBounds();
        }

        public void Release()
        {
            if (FogOfWarGameObject != null)
            {
                Object.DestroyImmediate(FogOfWarGameObject);
                FogOfWarGameObject = null;
            }
            if (FogOfWarMesh != null)
            {
                Object.DestroyImmediate(FogOfWarMesh);
                FogOfWarMesh = null;
            }
            if (FogOfWarMaterial != null)
            {
                Object.DestroyImmediate(FogOfWarMaterial);
                FogOfWarMaterial = null;
            }
        }

        // 重新计算网格锚点的方法
        private void RecalculateMeshPivot()
        {
            if (FogOfWarMesh == null || FogOfWarMesh.vertices == null || FogOfWarMesh.vertices.Length == 0)
                return;

            // 1. 获取当前网格顶点数据
            Vector3[] vertices = FogOfWarMesh.vertices;

            // 2. 计算当前本地空间包围盒
            Bounds localBounds = new Bounds(vertices[0], Vector3.zero);
            foreach (Vector3 vertex in vertices)
            {
                localBounds.Encapsulate(vertex);
            }

            // 3. 计算中心点偏移量（当前锚点到几何中心的向量）
            Vector3 centerOffset = localBounds.center;

            // 4. 调整顶点位置（使几何中心与锚点重合）
            Vector3[] adjustedVertices = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                adjustedVertices[i] = vertices[i] - centerOffset;
            }

            // 5. 更新网格数据
            FogOfWarMesh.vertices = adjustedVertices;
            FogOfWarMesh.RecalculateBounds();
            FogOfWarMesh.RecalculateNormals();

            // 6. 调整对象位置以补偿顶点偏移（保持世界位置不变）
            FogOfWarGameObject.transform.localPosition += centerOffset;
        }

        public void Save(BinaryWriter bw, CampaignFogInfo fogInfo = null)
        {
            // 1. 保存前先重新计算锚点
            RecalculateMeshPivot();

            // 2. 保存基础数据
            bw.Write(FogOfWarId);

            // 3. 保存颜色
            bw.Write(FogColor.r);
            bw.Write(FogColor.g);
            bw.Write(FogColor.b);
            bw.Write(FogColor.a);

            // 4. 保存变换位置
            var localPosition = FogOfWarGameObject.transform.localPosition;
            bw.Write(localPosition.x);
            bw.Write(0f);
            bw.Write(localPosition.z);
            if (fogInfo != null)
            {
                fogInfo.FogPos = localPosition;
            }
            // 5. 保存顶点数据
            var vertices = FogOfWarMesh.vertices;
            var count = vertices?.Length ?? 0;
            bw.Write(count);
            if (vertices != null)
            {
                if (fogInfo != null)
                {
                    fogInfo.FogGrid.Clear();
                    foreach (var vertex in vertices)
                    {
                        fogInfo.FogGrid.Add(new Vector3(vertex.x, 0f, vertex.z));
                    }
                }
                foreach (var vertex in vertices)
                {
                    bw.Write(vertex.x);
                    bw.Write(0f);
                    bw.Write(vertex.z);
                }
            }
        }

        public void Load(BinaryReader br, int fileVer, List<CampaignFogInfo> fogInfos = null)
        {
            FogOfWarId = br.ReadInt32();
            CampaignFogInfo campaignFogInfo = null;
            if (fogInfos != null)
            {
                campaignFogInfo = fogInfos.FirstOrDefault(f => f.FogId == FogOfWarId);
            }
            FogColor = new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            if (fileVer >= 11)
            {
                FogOfWarPosition = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                if (campaignFogInfo != null)
                {
                    FogOfWarPosition = campaignFogInfo.FogPos;
                }
                var count = br.ReadInt32();
                Vector3[] vertices = new Vector3[count];
                for (int i = 0; i < count; i++)
                {
                    vertices[i] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
                if (campaignFogInfo != null)
                {
                    // 如果有预设的顶点数据，使用它
                    vertices = campaignFogInfo.FogGrid.ToArray();
                }
                FogOfWarMesh.vertices = vertices;
            }

            FogOfWarGameObject.name = $"FogOfWar_{FogOfWarId}";
            FogOfWarGameObject.transform.localPosition = FogOfWarPosition;
            FogOfWarGameObject.SetActive(false);
        }


        public void SetVisible(bool visible)
        {
            FogOfWarGameObject?.SetActive(visible);
        }
    }
}
#endif
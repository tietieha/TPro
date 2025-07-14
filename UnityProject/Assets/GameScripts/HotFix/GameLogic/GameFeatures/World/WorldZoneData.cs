using System.Collections.Generic;
using System.IO;
using UnityEngine;

 public class WorldZoneData
    { 
        // 城点信息
        public short ZoneId { get; private set; } //cityId
        public short Level { get; private set; }
        
        public int OriLevel { get; private set; }
        public int PortIndex { get; private set; }
        // public int PortSailIndex { get; private set; }

        // public HashSet<int> VipStation;
        // public HashSet<int> NormalStation;

        // 区域描边图信息
        public short X { get; private set; }
        public short Y { get; private set; }
        public short W { get; private set; }
        public short H { get; private set; }
        public short Cx { get; private set; }
        public short Cy { get; private set; }

        // 联盟信息
        public int color = -1;
        public int outlineColor = -1;
        public string AllianceId = string.Empty;

        private Vector3[] vertexs;
        private int[] triangles;
        private int vertCount1;
        private int vertCount2;
        private List<int> vertexSorts;
        private List<int> edgeNeigbours;
        
        public Dictionary<int, EdgeSubMeshInfo> SubMeshes { get; private set; } = new Dictionary<int, EdgeSubMeshInfo>(256);

        public void Load(BinaryReader br)
        {
            // 城点数据
            ZoneId = br.ReadInt16();
            Level = br.ReadInt16();
            PortIndex = br.ReadInt32();
            // PortSailIndex = br.ReadInt32();
            
            // int stopCount = br.ReadInt32();
            // VipStation = new HashSet<int>(stopCount);
            // for (int j = 0; j < stopCount; j++)
            // {
            //     VipStation.Add(br.ReadInt32());
            // }
            //
            // stopCount = br.ReadInt32();
            // NormalStation = new HashSet<int>(stopCount);
            // for (int k = 0; k < stopCount; k++)
            // {
            //     NormalStation.Add(br.ReadInt32());
            // }

            int i, tx, tz;
            // 区域描边图信息
            X = br.ReadInt16();
            Y = br.ReadInt16();
            W = br.ReadInt16();
            H = br.ReadInt16();
            Cx = br.ReadInt16();
            Cy = br.ReadInt16();

            // 边界的 Mesh 数据，为减小文件，uv 和 分段信息 需要加载后计算。
            int count = br.ReadInt32();
            vertexs = new Vector3[count];
            for (i = 0; i < count; i++)
            {
                // x,z 的取值范围比较小，为保证精度，放大10倍后转为short存储。
                tx = br.ReadInt32();
                tz = br.ReadInt32();
                vertexs[i] = new Vector3(tx * 0.1f, 0f, tz * 0.1f);
            }

            count = br.ReadInt32();
            triangles = new int[count];
            for (i = 0; i < count; i++)
            {
                triangles[i] = br.ReadUInt16();
            }

            // EdgeNeigbours
            count = br.ReadInt32();
            edgeNeigbours = new List<int>(count);
            for (i = 0; i < count; i++)
            {
                edgeNeigbours.Add(br.ReadUInt16());
            }

            // verSorts
            int tdx = 0;
            vertCount1 = 0;
            vertCount2 = 0;

            count = br.ReadInt32();
            var bytes = br.ReadBytes(count);
            vertexSorts = new List<int>(count * 8);
            for (i = 0; i < count; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tdx = i * 8 + j;
                    if (tdx >= vertexs.Length) break;
                    byte a = (byte) (1 << j);
                    if ((bytes[i] & a) > 0)
                    {
                        vertexSorts.Add(2);
                        vertCount2++;
                    }
                    else
                    {
                        vertexSorts.Add(1);
                        vertCount1++;
                    }
                }

                if (tdx >= vertexs.Length) break;
            }

            SubMeshes.Clear();
        }

        public void Reset()
        {
            color = -1;
            outlineColor = -1;
            AllianceId = string.Empty;
        }

        // 计算uv  计算边界SubMesh
        public void CalcMesh()
        {
            if (SubMeshes.Count > 0)
                return;

            // 计算uv 
            int i;
            float step1 = 1f;
            float step2 = 1f;
            if (vertCount1 > vertCount2)
            {
                step1 = (float) vertCount2 / vertCount1;
            }
            else
            {
                step2 = (float) vertCount1 / vertCount2;
            }

            int num1 = 0;
            int num2 = 0;
            var uvs = new Vector2[vertexs.Length];
            for (i = 0; i < vertexSorts.Count; i++)
            {
                var type = vertexSorts[i];
                if (type == 1)
                {
                    uvs[i] = new Vector2(num1 * step1, 0f);
                    num1++;
                }
                else
                {
                    uvs[i] = new Vector2(num2 * step2, 1f);
                    num2++;
                }
            }

            EdgeSubMeshInfo curMesh = null;
            SubMeshes.Clear();
            for (i = 0; i < edgeNeigbours.Count; i++)
            {
                var zoneId = edgeNeigbours[i];
                if (curMesh == null || curMesh.zoneId != zoneId)
                {
                    //if (curMesh != null)
                    //{
                    //    curMesh.preTriangles.Add(triangles[i * 3]);
                    //    curMesh.preTriangles.Add(triangles[i * 3 + 1]);
                    //    curMesh.preTriangles.Add(triangles[i * 3 + 2]);
                    //}

                    if (!SubMeshes.ContainsKey(zoneId))
                    {
                        curMesh = new EdgeSubMeshInfo(zoneId);
                        SubMeshes[zoneId] = curMesh;
                    }
                    else
                    {
                        curMesh = SubMeshes[zoneId];
                    }

                    //if (i > 0)
                    //{
                    //    curMesh.preTriangles.Add(triangles[(i - 1) * 3]);
                    //    curMesh.preTriangles.Add(triangles[(i - 1) * 3 + 1]);
                    //    curMesh.preTriangles.Add(triangles[(i - 1) * 3 + 2]);
                    //}
                }

                curMesh.preTriangles.Add(triangles[i * 3]);
                curMesh.preTriangles.Add(triangles[i * 3 + 1]);
                curMesh.preTriangles.Add(triangles[i * 3 + 2]);
            }


            //List<int> tmps = new List<int>();
            foreach (var subMesh in SubMeshes.Values)
            {
                subMesh.CalcMesh(ref vertexs, ref uvs);
            }

            vertexs = null;
            uvs = null;
            triangles = null;
            edgeNeigbours.Clear();
            vertexSorts.Clear();
        }
    }

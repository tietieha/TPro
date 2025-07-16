#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GEngine.MapEditor
{
    public class Cube
    {
        public int x, y, z;

        public Cube(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public Axial Cube2Axial()
        {
            return new Axial(x, z + (x - (x & 1)) / 2);
        }

        public int Distance2Origin()
        {
            int max = math.max(math.abs(x), math.abs(y));
            return math.max(math.abs(z), max);
        }

        public static Cube Subtract(Cube cube1, Cube cube2)
        {
            return new Cube(cube1.x - cube2.x, cube1.y - cube2.y, cube1.z - cube2.z);
        }
    }
    public class Axial
    {
        public int q;
        public int r;
        
        public Axial(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public static Cube Axial2Cube(Axial hex)
        {
            var x = hex.q;
            var z = hex.r - (hex.q - (hex.q & 1)) / 2;
            var y = -x - z;
            return new Cube(x, y, z);
        }

        //CWCLOCK旋转60°倍数
        public Axial Rotate(int sextants) {
            if (q == 0 && r == 0) return this;
            sextants = sextants % 6;
            if (sextants == 0) return this;
            var cube = Axial2Cube(this);
            if (sextants == 1) cube = new Cube(-cube.z, -cube.x, -cube.y);
            if (sextants == 2) cube = new Cube(cube.y, cube.z, cube.x);
            if (sextants == 3) cube = new Cube(-cube.x, -cube.y, -cube.z);
            if (sextants == 4) cube = new Cube(cube.z, cube.x, cube.y);
            if (sextants == 5) cube = new Cube(-cube.y, -cube.z, -cube.x);

            return cube.Cube2Axial();
        }
        
        //各方向朝向 奇偶数列不相同
        public static Vector2Int[][] direction_vec = new[]
        {
            // 偶数列
            new[]
            {
                new Vector2Int(+1,  -1),
                new Vector2Int(0,   -1),
                new Vector2Int(-1,  -1),
                new Vector2Int(-1,  0),
                new Vector2Int(0,   +1),
                new Vector2Int(+1,  0),
            },
            // 奇数列
            new[]
            {
                new Vector2Int(+1, 0),
                new Vector2Int(0,  -1),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, +1),
                new Vector2Int(0,  +1),
                new Vector2Int(+1, +1),
            },
        };
        
        public static Axial Direction(int even, int direction)
        {
            var vector = direction_vec[even][direction];
            return new Axial(vector.x, vector.y);
        }

        public static Axial Add(Axial hex1, Axial hex2)
        {
            return new Axial(hex1.q + hex2.q, hex1.r + hex2.r);
        }

        public static Axial Sub(Axial hex1, Axial hex2)
        {
            return new Axial(hex1.q - hex2.q, hex1.r - hex2.r);
        }
        
        private static Axial Neighbor(Axial hex, int even, int direction)
        {
            return Add(hex, Direction(even, direction));
        }

        private static Axial Scale(Axial hex, int factor)
        {
            return new Axial(hex.q * factor, hex.r * factor);
        }

        public static List<Axial> Ring(Axial center, int radius)
        {
            var results = new List<Axial>();
            int even = center.q & 1;
            var hex = Add(center, Scale(new Axial(0, 1), radius));

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    results.Add(hex);
                    even = hex.q & 1;
                    hex = Neighbor(hex, even, i);
                }
            }

            return results;
        }

        public static List<Axial> Spiral(Axial center, int radius)
        {
            var results = new List<Axial>();
            results.Add(center);
            for (int i = 1; i < radius; i++)
            {
                results.AddRange(Ring(center, i));
            }

            return results;
        }

        public static List<Axial> Ring4Port(HexMap map, Axial center, int radius)
        {
            var origin = map.GetHexagonPos(center);

            var result = new List<Axial>();
            var ring = Ring(new Axial(0, 0), radius);
            foreach (var ax in ring)
            {
                var offset = map.GetHexagonPos(ax) - map.GetHexagonPos(0, 0);;
                var newAx = Hex.WorldToOffset(origin + offset);
                result.Add(new Axial(newAx.x, newAx.y));
            }

            return result;
        }
        
        public static List<Axial> Spiral4Port(HexMap map, Axial center, int radius)
        {
            var origin = map.GetHexagonPos(center);

            var result = new List<Axial>();
            var ring = Spiral(new Axial(0, 0), radius);
            foreach (var ax in ring)
            {
                var offset = map.GetHexagonPos(ax) - map.GetHexagonPos(0, 0);
                var newAx = Hex.WorldToOffset(origin + offset);
                result.Add(new Axial(newAx.x, newAx.y));
            }

            return result;
        }
    }
    
    public class IslandTemplate
    {
        //岛屿ID(可能是策划配置ID)
        public int _Id = 0;
        public int _Rotate = 0;
        //占地坐标
        public List<Axial> _OwnHexCoords;
        //可放置港口坐标
        public List<Axial> _PlaceHexCoords;
        //旋转之后的坐标
        public List<Axial> _RotateOwnHexCoords;
        public List<Axial> _RotatePlaceHexCoords;

        //暂定8圈模板 不够用再增大
        public static readonly int Radius = 15;

        public IslandTemplate()
        {
            _OwnHexCoords = new List<Axial>();
            _PlaceHexCoords = new List<Axial>();
            _RotateOwnHexCoords = new List<Axial>();
            _RotatePlaceHexCoords = new List<Axial>();
            _OwnHexCoords.Add(new Axial(0, 0));
        }

        public void AddOwnHex(int q, int r)
        {
            if (_Rotate > 0)
            {
                return;
            }

            var axial = new Axial(q, r);
            var cube = Axial.Axial2Cube(axial);

            if (cube.Distance2Origin() >= Radius)
            {
                return;
            }
            
            if (q == 0 && r == 0)
            {
                return;
            }
            
            if (ExistOwnHex(q, r) == false)
                _OwnHexCoords.Add(axial);
        }

        public void AddPlaceHex(int q, int r)
        {
            if (_Rotate > 0)
            {
                return;
            }
            
            var axial = new Axial(q, r);
            var cube = Axial.Axial2Cube(axial);

            if (cube.Distance2Origin() >= Radius)
            {
                return;
            }
            
            if(ExistPlaceHex(q, r) == false)
                _PlaceHexCoords.Add(axial);
        }

        public bool ExistOwnHex(int q, int r, bool notRm = false)
        {
            var list = _Rotate > 0 ? _RotateOwnHexCoords : _OwnHexCoords;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var hex = list[i];
                if (hex.q == q && hex.r == r)
                {
                    if (notRm == false)
                    {
                        list.Remove(hex);
                    }
                        
                    return true;
                }
            }

            return false;
        }

        public bool ExistPlaceHex(int q, int r, bool notRm = false)
        {
            var list = _Rotate > 0 ? _RotatePlaceHexCoords : _PlaceHexCoords;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var hex = list[i];
                if (hex.q == q && hex.r == r)
                {
                    if (notRm == false)
                    {
                        list.Remove(hex);
                    }
                        
                    return true;
                }
            }

            return false;
        }
        
        public void Rotate(int sextants)
        {
            _Rotate = sextants;
            var temp = new List<Axial>();
            foreach (var hexCoord in _OwnHexCoords)
            {
                temp.Add(hexCoord.Rotate(sextants));
            }
            _RotateOwnHexCoords.Clear();
            _RotateOwnHexCoords.AddRange(temp);

            temp.Clear();
            foreach (var hexCoord in _PlaceHexCoords)
            {
                temp.Add(hexCoord.Rotate(sextants));
            }
            _RotatePlaceHexCoords.Clear();
            _RotatePlaceHexCoords.AddRange(temp);
        }

        //随机获取海港Index
        public Axial GetPortIndex(HexMap map, Axial island)
        {
            int count = _RotatePlaceHexCoords.Count;
            var rdIdx = Random.Range(0, count - 1);
            var hex = _RotatePlaceHexCoords[rdIdx];
            
            var originOffset = map.GetHexagonPos(island);
            var offset = map.GetHexagonPos(hex) - map.GetHexagonPos(0, 0);
            var coord = Hex.WorldToOffset(originOffset + offset);
            
            return new Axial(coord.x, coord.y);
        }

        public void AssignMapIsland(HexMap map, Axial island)
        {
            var list = _Rotate > 0 ? _RotateOwnHexCoords : _OwnHexCoords;
            var originOffset = map.GetHexagonPos(island);
            foreach (var axial in list)
            {
                var offset = map.GetHexagonPos(axial) - map.GetHexagonPos(0, 0);;
                var coord = Hex.WorldToOffset(originOffset + offset);
                var hex = map.GetHexagon(coord);
                if (hex != null)
                {
                    hex.attribute = HexAttribute.Island;
                }
            }
        }

        //修复海港位置接口 一次性使用 寻找模板中距离最近的落港点
        public int FixPortIndex(HexMap map, Axial island, Axial port)
        {
            int count = _RotatePlaceHexCoords.Count;
            int index = map.GetIndex(port.q, port.r);
            float min = 1000;
            foreach (var axial in _RotatePlaceHexCoords)
            {
                var originOffset = map.GetHexagonPos(island);
                var offset = map.GetHexagonPos(axial) - map.GetHexagonPos(0, 0);
                var hexWp = originOffset + offset;
                var portWp = map.GetHexagonPos(port);
                var distance = Vector3.Distance(hexWp, portWp);
                if (distance < min)
                {
                    min = distance;

                    var coord = Hex.WorldToOffset(hexWp);
                    index = map.GetIndex(coord.x, coord.y);
                }
            }
            
            
            return index;
        }
        
        public void Reset()
        {
            _Id = 0;
            _Rotate = 0;
            _OwnHexCoords.Clear();
            _PlaceHexCoords.Clear();
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(_Id);
            // bw.Write(_Rotate); //模板不存旋转 只是为了测试加的
            int count = _OwnHexCoords.Count;
            bw.Write(count);
            foreach (var qr in _OwnHexCoords)
            {
                bw.Write(qr.q);
                bw.Write(qr.r);
            }

            count = _PlaceHexCoords.Count;
            bw.Write(count);
            foreach (var qr in _PlaceHexCoords)
            {
                bw.Write(qr.q);
                bw.Write(qr.r);
            }
        }

        public void Load(BinaryReader br)
        {
            Reset();
            _Id = br.ReadInt32();
            // _Rotate = br.ReadInt32();
            int count = br.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var q = br.ReadInt32();
                var r = br.ReadInt32();
                _OwnHexCoords.Add(new Axial(q, r));
            }

            count = br.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var q = br.ReadInt32();
                var r = br.ReadInt32();
                _PlaceHexCoords.Add(new Axial(q, r));
            }
        }
    }
}
#endif
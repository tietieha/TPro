using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GEgineRunTime
{
    [Serializable]
    public class WorldRenderVariable
    {
        public int resId;
        public List<float> boundRadiusList;
        public List<Vector3> posList;
        public List<Vector3> eulerList;
        public List<Vector3> scaleList;

        public WorldRenderVariable()
        {
            posList = new List<Vector3>();
            eulerList = new List<Vector3>();
            scaleList = new List<Vector3>();
            boundRadiusList = new List<float>();
        }
    }
    /// <summary>
    /// 每个区域的数据
    /// </summary>
    public class WorldRegionDetailScriptableObject : ScriptableObject
    {
        public int regionRow;
        public int regionCol;
        public Bounds regionBounds;
        public List<WorldRenderVariable> renderResList = new List<WorldRenderVariable>();
    }
}

namespace XLua
{
    using System;
    using UnityEngine;

    [Serializable]
    public class LuaObjectReference
    {
        public string Name;
        public GameObject Object;
        public UnityEngine.Object Component;
        public int TypeIndex;
        public string Params;
    }
}


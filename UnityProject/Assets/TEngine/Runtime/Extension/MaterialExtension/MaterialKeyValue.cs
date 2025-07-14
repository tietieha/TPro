using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine.MaterialExtension
{
    [Serializable]
    public class MaterialKeyValue
    {
        public string key;
        public List<Material> mats;
    }
}
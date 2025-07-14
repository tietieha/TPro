using UWParticleSystemProfiler;
using UnityEngine;
using XLua;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

namespace M.Battle.Render
{
    [LuaCallCSharp]
    public class ParticleTime : MonoBehaviour
    {
        [SerializeField] public float time = -1;
#if UNITY_EDITOR
        [Button]
        void GetTime()
        {
            time = this.gameObject.GetParticleDuration(true, false, false);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

#endif
    }
}
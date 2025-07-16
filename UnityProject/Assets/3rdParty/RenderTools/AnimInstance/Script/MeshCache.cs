using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RenderTools.AnimInstance
{
    internal class MeshEntry
    {
        public Mesh mesh;
        public int referenceCount;

        public void Release()
        {
            if (mesh)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    UnityEngine.Object.DestroyImmediate(mesh, false);
                else
                    UnityEngine.Object.Destroy(mesh);
#endif
                    // UnityEngine.Object.Destroy(mesh);
            }

            mesh = null;
        }
    }

    internal static class MeshCache
    {
        static readonly Dictionary<Hash128, MeshEntry> s_MeshMap = new Dictionary<Hash128, MeshEntry>();

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void ClearCache()
        {
            foreach (var entry in s_MeshMap.Values)
            {
                entry.Release();
            }

            s_MeshMap.Clear();
        }
#endif

        public static Mesh Get(Hash128 hash, Mesh orimesh, Bounds bounds)
        {
            if (!hash.isValid) return null;
            
            MeshEntry entry;
            if (!s_MeshMap.TryGetValue(hash, out entry))
            {
#if UNITY_EDITOR
                var mesh = Object.Instantiate(orimesh);
#else
                var mesh = orimesh;
#endif
                mesh.bounds = bounds;
                entry = new MeshEntry()
                {
                    mesh = mesh,
                };

                // onModify(entry.mesh);
                s_MeshMap.Add(hash, entry);
            }

            entry.referenceCount++;
            //Debug.LogFormat("Register: {0}, {1} (Total: {2})", hash, entry.referenceCount, materialMap.Count);
            return entry.mesh;
        }

        public static void Release(Hash128 hash)
        {
            MeshEntry entry;
            if (!hash.isValid ||!s_MeshMap.TryGetValue(hash, out entry)) return;
            //Debug.LogFormat("Unregister: {0}, {1}", hash, entry.referenceCount -1);

            if (--entry.referenceCount > 0) return;

            entry.Release();
            s_MeshMap.Remove(hash);
            //Debug.LogFormat("Unregister: Release Emtry: {0}, {1} (Total: {2})", hash, entry.referenceCount, materialMap.Count);
        }
    }
}

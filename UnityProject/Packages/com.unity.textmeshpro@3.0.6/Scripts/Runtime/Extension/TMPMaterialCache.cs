using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal class MaterialEntry
{
    public Material material;
    public int referenceCount;

    public void Release()
    {
        if (material)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(material, false);
            else
                Object.Destroy(material);
#else
                Object.Destroy(material);
#endif
        }

        material = null;
    }
}

internal static class TMPMaterialCache
{
    static readonly Dictionary<Hash128, MaterialEntry> s_MaterialMap =
        new Dictionary<Hash128, MaterialEntry>();

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void ClearCache()
    {
        foreach (var entry in s_MaterialMap.Values)
        {
            entry.Release();
        }

        s_MaterialMap.Clear();
    }
#endif

    public static Material Get(Hash128 hash, Material originalMaterial)
    {
        if (!hash.isValid || originalMaterial == null) return null;

        bool isNewEntry = false;
        if (!s_MaterialMap.TryGetValue(hash, out var entry))
        {
            var material = Object.Instantiate(originalMaterial);
            entry = new MaterialEntry()
            {
                material = material,
            };
            s_MaterialMap.Add(hash, entry);
            isNewEntry = true;
        }
        else if (entry.material == null)
        {
            // 材质已被销毁，重新创建
            entry.material = Object.Instantiate(originalMaterial);
            entry.referenceCount = 0;
            isNewEntry = true;
        }

        entry.referenceCount++;
#if UNITY_EDITOR
        // Debug.Log($"TMPRenderParameter_Log MaterialCache - Hash: {hash}, IsNew: {isNewEntry}, RefCount: {entry.referenceCount}, " +
        //           $"Material InstanceID: {entry.material.GetInstanceID()}, Total Cached: {s_MaterialMap.Count}");
#endif
        return entry.material;
    }

    // 添加获取缓存统计信息的方法
    public static void LogCacheStats()
    {
#if UNITY_EDITOR
        Debug.Log($"TMPMaterialCache Stats - Total Entries: {s_MaterialMap.Count}");
        foreach (var kvp in s_MaterialMap)
        {
            Debug.Log($"Hash: {kvp.Key}, RefCount: {kvp.Value.referenceCount}, MaterialID: {kvp.Value.material.GetInstanceID()}");
        }
#endif
    }

    public static void Release(Hash128 hash)
    {
        if (!hash.isValid || !s_MaterialMap.TryGetValue(hash, out var entry)) return;

        if (--entry.referenceCount > 0) return;

        entry.Release();
        s_MaterialMap.Remove(hash);
    }
}
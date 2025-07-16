#if UNITY_EDITOR
using System;
using System.IO;
using TEngine;
using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
    public class SpriteMesh : MonoBehaviour
    {
        [ContextMenu("Save SpriteRenderer Mesh")]
        public void SaveMesh()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null || spriteRenderer.sprite == null)
            {
                Debug.LogError("No SpriteRenderer or Sprite found!");
                return;
            }

            // 获取 Sprite 的网格数据
            Sprite sprite = spriteRenderer.sprite;
            //
            // // 设置顶点
            // mesh.vertices = Array.ConvertAll(sprite.vertices, v => (Vector3)v);
            //
            // // 设置三角形索引
            // mesh.triangles = Array.ConvertAll(sprite.triangles, t => (int)t);
            //
            // // 设置 UV 坐标
            // mesh.uv = sprite.uv;
            //
            // // 保存 Mesh 到文件
            // string path =
            //     EditorUtility.SaveFilePanel("Save Mesh", "Assets", sprite.name + ".mesh", "asset");
            // if (string.IsNullOrEmpty(path))
            // {
            //     Debug.LogWarning("Mesh saving canceled.");
            //     return;
            // }
            //
            // path = FileUtil.GetProjectRelativePath(path);
            // if (string.IsNullOrEmpty(path))
            // {
            //     Debug.LogError(
            //         "Failed to get relative path. Make sure the file is saved within the Assets folder.");
            //     return;
            // }
            //
            // AssetDatabase.CreateAsset(mesh, path);
            // AssetDatabase.SaveAssets();
            //
            // Debug.Log($"Mesh saved to: {path}");

            string path =
                EditorUtility.SaveFilePanel("Save Mesh", "Assets", sprite.name + ".mesh", "asset");
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("Mesh saving canceled.");
                return;
            }
            SaveToFile(spriteRenderer, path);
        }

        public static void SaveToFile(SpriteRenderer spriteRenderer, string path)
        {
            // 获取 Sprite 的网格数据
            Sprite sprite = spriteRenderer.sprite;
            Mesh mesh = new Mesh
            {
                name = sprite.name
            };

            // 设置顶点
            mesh.vertices = Array.ConvertAll(sprite.vertices, v => (Vector3)v);

            // 设置三角形索引
            mesh.triangles = Array.ConvertAll(sprite.triangles, t => (int)t);

            // 设置 UV 坐标
            mesh.uv = sprite.uv;



            path = FileUtil.GetProjectRelativePath(path);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError(
                    "Failed to get relative path. Make sure the file is saved within the Assets folder.");
                return;
            }

            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"Mesh saved to: {path}");
        }

    }

    public class ExportSpriteMesh
    {
        [MenuItem("GameObject/Export Sprite Mesh")]
        public static void ExportMesh()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogError("No GameObject selected!");
                return;
            }
            SpriteRenderer[] renderers = selectedObject.GetComponentsInChildren<SpriteRenderer>(true);
            if (renderers.Length == 0)
            {
                Debug.LogError("No SpriteRenderer found in the selected GameObject!");
                return;
            }


            string folderPath = EditorUtility.OpenFolderPanel("Select Mesh Directory", "Assets/GameAssets/Scenes", "");

            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogWarning("Mesh saving canceled.");
                return;
            }

            SaveMeshMaterial(folderPath, renderers);

        }

        public static void SaveMeshMaterial(string folderPath, SpriteRenderer[] renderers)
        {
            var defaultMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Test/Scenes/Campaign/Common/mat/common_fog.mat");

            if (defaultMaterial == null)
            {
                Debug.LogError("Default material not found!");
                return;
            }

            foreach (var sr in renderers)
            {
                // Debug.Log(sr.name, sr.gameObject);
                var path = Path.Combine(folderPath + "/fbx", sr.name + "_mesh.asset");
                SpriteMesh.SaveToFile(sr, path);

                path = Path.Combine(folderPath + "/mat", sr.name + ".mat");
                path = FileUtil.GetProjectRelativePath(path);
                Material material = new Material(defaultMaterial);
                material.name = sr.name;
                material.SetTexture("_ControlTex", sr.sprite.texture);
                AssetDatabase.CreateAsset(material, path);
                AssetDatabase.SaveAssets();

            }
        }

        [MenuItem("GameObject/Create Fog Collider")]
        public static void CreateFogCollider()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogError("No GameObject selected!");
                return;
            }

            SpriteRenderer[] renderers = selectedObject.GetComponentsInChildren<SpriteRenderer>(true);
            if (renderers.Length == 0)
            {
                Debug.LogError("No SpriteRenderer found in the selected GameObject!");
                return;
            }

            string folderPath = EditorUtility.OpenFolderPanel("Select Mesh Directory", "Assets/GameAssets/Scenes", "");

            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogWarning("Mesh saving canceled.");
                return;
            }
            SaveFogCollider(folderPath, selectedObject, renderers);
        }

        public static void SaveFogCollider(string folderPath, GameObject selectedObject, SpriteRenderer[] renderers)
        {
            var defaultCollider = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Test/Scenes/Campaign/Common/prefabs/common_fog_collider.prefab");
            if (defaultCollider == null)
            {
                Debug.LogError("Default collider not found!");
                return;
            }
            var parent = selectedObject.transform.parent;
            if (parent == null)
            {
                Debug.LogError("Parent not found!");
                return;
            }


            var collider = parent.Find("collider");
            if (collider == null)
            {
                GameObject colliderObj = new GameObject("collider", typeof(RectTransform));
                collider = colliderObj.transform;
                collider.transform.SetParent(parent);
                collider.localPosition = Vector3.zero;
                collider.localRotation = Quaternion.identity;
                collider.localScale = Vector3.one;
            }


            foreach (var sr in renderers)
            {
                var name = sr.name;
                string pattern = @"_(\d+)$";
                var match = System.Text.RegularExpressions.Regex.Match(sr.name, pattern);

                if (match.Success)
                {
                    name = match.Groups[1].Value; // 只取数字部分
                }

                Transform existing = collider.Find(name);
                GameObject newCollider;

                if (existing != null)
                {
                    newCollider = existing.gameObject;
                }
                else
                {
                    newCollider = (GameObject)PrefabUtility.InstantiatePrefab(defaultCollider);
                    if (newCollider == null)
                    {
                        Debug.LogError("Failed to instantiate defaultCollider prefab.");
                        continue;
                    }
                    PrefabUtility.UnpackPrefabInstance(newCollider, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                }



                newCollider.name = name;
                newCollider.transform.SetParent(collider);
                newCollider.transform.localPosition = sr.transform.localPosition;
                newCollider.transform.localRotation = sr.transform.localRotation;
                newCollider.transform.localScale = sr.transform.localScale;

                // 2. 加载 mesh
                string meshPath = folderPath + "/fbx/" + sr.name + "_mesh.asset";
                meshPath = FileUtil.GetProjectRelativePath(meshPath);
                Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
                if (mesh == null)
                {
                    Debug.LogWarning($"Mesh not found at path: {meshPath}");
                }
                else
                {

                    MeshFilter meshFilter = newCollider.GetComponent<MeshFilter>();
                    if (meshFilter == null)
                    {
                        meshFilter = newCollider.AddComponent<MeshFilter>();
                    }
                    meshFilter.sharedMesh = mesh;

                    MeshCollider meshCollider = newCollider.GetComponent<MeshCollider>();
                    if (meshCollider != null)
                    {
                        meshCollider.sharedMesh = mesh;
                    }
                    else
                    {
                        Debug.LogWarning("MeshCollider component not found on newCollider.");
                    }
                }

                // 3. 加载材质
                string matPath = folderPath + "/mat/" + sr.name + ".mat";
                matPath = FileUtil.GetProjectRelativePath(matPath);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
                if (material == null)
                {
                    Debug.LogWarning($"Material not found at path: {matPath}");
                }
                else
                {
                    MeshRenderer meshRenderer = newCollider.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.sharedMaterial = material;
                    }
                    else
                    {
                        Debug.LogWarning("MeshRenderer component not found on newCollider.");
                    }
                }
            }
        }

        [MenuItem("GameObject/Create Fog")]
        public static void CreateFog()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogError("No GameObject selected!");
                return;
            }
            SpriteRenderer[] renderers = selectedObject.GetComponentsInChildren<SpriteRenderer>(true);
            if (renderers.Length == 0)
            {
                Debug.LogError("No SpriteRenderer found in the selected GameObject!");
                return;
            }

            string folderPath = EditorUtility.OpenFolderPanel("Select Mesh Directory", "Assets/GameAssets/Scenes", "");

            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogWarning("Mesh saving canceled.");
                return;
            }

            SaveMeshMaterial(folderPath, renderers);
            SaveFogCollider(folderPath, selectedObject, renderers);
        }
    }
}
#endif
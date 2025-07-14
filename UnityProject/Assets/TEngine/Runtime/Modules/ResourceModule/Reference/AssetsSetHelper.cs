using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
namespace TEngine
{
    public static class AssetsSetHelper
    {
        private static IResourceManager _resourceManager;

        private static void CheckResourceManager()
        {
            if (_resourceManager == null)
            {
                _resourceManager = ModuleImpSystem.GetModule<IResourceManager>();
            }
        }

        #region SetMaterial

        public static void SetMaterial(this Image image, string location, bool isAsync = false, string packageName = "")
        {
            if (image == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                image.material = material;
                AssetsReference.Ref(material, image.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (image == null || image.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    image.material = material;
                    AssetsReference.Ref(material, image.gameObject);
                }, packageName);
            }
        }

        public static void SetMaterial(this SpriteRenderer spriteRenderer, string location, bool isAsync = false, string packageName = "")
        {
            if (spriteRenderer == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                spriteRenderer.material = material;
                AssetsReference.Ref(material, spriteRenderer.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (spriteRenderer == null || spriteRenderer.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    spriteRenderer.material = material;
                    AssetsReference.Ref(material, spriteRenderer.gameObject);
                }, packageName);
            }
        }

        public static void SetMaterial(this MeshRenderer meshRenderer, string location, bool needInstance = true, bool isAsync = false, string packageName = "")
        {
            if (meshRenderer == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                meshRenderer.material = needInstance ? UnityEngine.Object.Instantiate(material) : material;
                AssetsReference.Ref(material, meshRenderer.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (meshRenderer == null || meshRenderer.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    meshRenderer.material = needInstance ? UnityEngine.Object.Instantiate(material) : material;
                    AssetsReference.Ref(material, meshRenderer.gameObject);
                }, packageName);
            }
        }

        public static void SetSharedMaterial(this MeshRenderer meshRenderer, string location, bool isAsync = false, string packageName = "")
        {
            if (meshRenderer == null)
            {
                throw new GameFrameworkException($"SetSprite failed. Because image is null.");
            }

            CheckResourceManager();

            if (!isAsync)
            {
                Material material = _resourceManager.LoadAsset<Material>(location, packageName);
                meshRenderer.sharedMaterial = material;
                AssetsReference.Ref(material, meshRenderer.gameObject);
            }
            else
            {
                _resourceManager.LoadAsset<Material>(location, material =>
                {
                    if (meshRenderer == null || meshRenderer.gameObject == null)
                    {
                        _resourceManager.UnloadAsset(material);
                        material = null;
                        return;
                    }

                    meshRenderer.sharedMaterial = material;
                    AssetsReference.Ref(material, meshRenderer.gameObject);
                }, packageName);
            }
        }

        #endregion

        public static Type GetType(string typeName)
        {
            // 先在当前Assembly找,再在UnityEngine里找.
            // 如果都找不到,再遍历所有Assembly
            var type = Assembly.GetExecutingAssembly().GetType(typeName) ?? typeof(ParticleSystem).Assembly.GetType(typeName);
            if (type != null) return type;
            type = GetUnityType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            Debug.LogError("未找到类型：" + typeName);
            return null;
        }
        private static Type GetUnityType(string typeName)
        {
            string namespaceStr = "UnityEngine";
            if (!typeName.Contains(namespaceStr))
                typeName = namespaceStr + "." + typeName;
            var assembly = Assembly.Load(namespaceStr);
            if (assembly == null)
                return null;
            return assembly.GetType(typeName);

        }
    }
}
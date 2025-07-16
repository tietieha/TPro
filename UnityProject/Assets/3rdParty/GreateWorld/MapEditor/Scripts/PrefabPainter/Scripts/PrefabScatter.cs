#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GEngine.MapEditor
{
    public partial class PrefabPainter : EditorWindow
    {
        public enum ScatterType
        {
            DensityBased,
            QuantityBased
        }

        public class ScatterParameters
        {
            public List<PrefabWithWeight> prefabs = new List<PrefabWithWeight>();
            public GameObject parent;
            public Texture2D maskTexture;
            public Rect scatterRect;
            public int numberOfInstances = 100;
            public float minHeight = 0f;
            public float maxHeight = 0f;
            public float spacing = 1f;
            
            public Vector3 rotationRange = new Vector3(0f, 360f, 0f);

            public float minScale = 1;
            public float maxScale = 1;
        }

        [System.Serializable]
        public class PrefabWithWeight
        {
            public GameObject prefab;
            public float weight = 1f;
            public float radius = 1f; // Radius now needs to consider scaling
        }

        private Dictionary<Vector2Int, List<(Transform, float)>> grid =
            new Dictionary<Vector2Int, List<(Transform, float)>>(); // Store radius with position
        
        public float gridCellSize = 100f;
        private float totalWeight;
        private int instancesCreated = 0;

        private ScatterType scatterType;
        private ScatterParameters scatterParameters = new ScatterParameters();

        // [MenuItem("Tools/Scatter Utils")]
        // public static void ShowWindow()
        // {
        //     GetWindow<ScatterUtils>("Scatter Utils");
        // }
        
        private void InitializeScatter(bool clearForce = false)
        {
            if (clearForce)
            {
                grid.Clear();
                instancesCreated = 0;
            }
        }

        // New static method for external calls
        public void ScatterPrefabs(ScatterParameters parameters, Brush brush,
            ScatterType scatterType = ScatterType.QuantityBased)
        {
            InitializeScatter();
            CalculateTotalWeight(parameters);

            if (scatterType == ScatterType.DensityBased)
            {
                ScatterPrefabsBasedOnDensity(parameters, brush);
            }
            else if (scatterType == ScatterType.QuantityBased)
            {
                ScatterPrefabsBasedOnQuantity(parameters, brush);
            }
        }

        private void ScatterPrefabsBasedOnDensity(ScatterParameters parameters, Brush brush)
        {
            int maxAttempts = 10000;
            int attempts = 0;
            int instanceCount = 0;

            while (instanceCount < parameters.numberOfInstances && attempts < maxAttempts)
            {
                if (EditorUtility.DisplayCancelableProgressBar("Scattering Prefabs",
                        $"{instanceCount}/{parameters.numberOfInstances}",
                        1f * instanceCount / parameters.numberOfInstances))
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }

                Vector3 randomPosition = GetRandomPosition(parameters);
                float density = 1.0f;

                if (parameters.maskTexture != null)
                {
                    density = GetDensityFromMask(randomPosition, parameters);
                }

                if (Random.value <= density)
                {
                    GameObject selectedPrefab = GetRandomPrefabBasedOnWeight(parameters,
                        out float radius, out float randomScale);

                    if (!IsPositionTooClose(randomPosition, radius, randomScale, parameters))
                    {
                        if (selectedPrefab != null)
                        {
                            Vector3 randomRotation = GetRandomRotation(parameters);
                            
                            GameObject instance =
                                PrefabUtility.InstantiatePrefab(selectedPrefab,
                                    parameters.parent.transform) as GameObject;
                            instance.transform.position = randomPosition;
                            instance.transform.rotation = Quaternion.Euler(randomRotation);
                            instance.transform.localScale *= randomScale;
                            
                            if (brush.settings.brushOverlapCheckMode != OverlapCheckMode.None)
                            {
                                Bounds bounds;

                                if (GetObjectWorldBounds(instance, out bounds) &&
                                    CheckOverlap(brush, brush.settings, bounds))
                                {
                                    GameObject.DestroyImmediate(instance);
                                    continue;
                                }
                            }
                            AddGoToGrid(instance.transform, radius * randomScale);
                            instanceCount++;
                        }
                    }
                }

                attempts++;
            }
            
            EditorUtility.ClearProgressBar();
        }

        private void ScatterPrefabsBasedOnQuantity(ScatterParameters parameters, Brush brush)
        {
            int instanceCount = 0;
            while (instanceCount < parameters.numberOfInstances)
            {
                if (EditorUtility.DisplayCancelableProgressBar("Scattering Prefabs",
                        $"{instanceCount}/{parameters.numberOfInstances}",
                        1f * instanceCount / parameters.numberOfInstances))
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
                
                Vector3 randomPosition = GetRandomPosition(parameters);
                float density = 1.0f;

                if (parameters.maskTexture != null)
                {
                    density = GetDensityFromMask(randomPosition, parameters);
                }

                if (Random.value <= density)
                {
                    GameObject selectedPrefab = GetRandomPrefabBasedOnWeight(parameters,
                        out float radius, out float randomScale);

                    if (!IsPositionTooClose(randomPosition, radius, randomScale, parameters))
                    {
                        if (selectedPrefab != null)
                        {
                            Vector3 randomRotation = GetRandomRotation(parameters);
                            GameObject instance =
                                PrefabUtility.InstantiatePrefab(selectedPrefab,
                                    parameters.parent.transform) as GameObject;
                            instance.transform.position = randomPosition;
                            instance.transform.rotation = Quaternion.Euler(randomRotation);
                            instance.transform.localScale *= randomScale;
                            
                            if (brush.settings.brushOverlapCheckMode != OverlapCheckMode.None)
                            {
                                Bounds bounds;

                                if (GetObjectWorldBounds(instance, out bounds) &&
                                    CheckOverlap(brush, brush.settings, bounds))
                                {
                                    GameObject.DestroyImmediate(instance);
                                    continue;
                                }
                            }
                            
                            AddGoToGrid(instance.transform, radius * randomScale);
                            instanceCount++;
                        }
                    }
                }
            }
            
            EditorUtility.ClearProgressBar();
        }

        private static Vector3 GetRandomPosition(ScatterParameters parameters)
        {
            float x = Random.Range(parameters.scatterRect.xMin, parameters.scatterRect.xMax);
            float z = Random.Range(parameters.scatterRect.yMin, parameters.scatterRect.yMax);
            float y = Random.Range(parameters.minHeight, parameters.maxHeight);

            return new Vector3(x, y, z);
        }

        private static float GetDensityFromMask(Vector3 worldPosition, ScatterParameters parameters)
        {
            float u = (worldPosition.x - parameters.scatterRect.xMin) /
                      parameters.scatterRect.width;
            float v = (worldPosition.z - parameters.scatterRect.yMin) /
                      parameters.scatterRect.height;

            u = Mathf.Clamp01(u);
            v = Mathf.Clamp01(v);

            int x = Mathf.Clamp((int)(u * parameters.maskTexture.width), 0,
                parameters.maskTexture.width - 1);
            int y = Mathf.Clamp((int)(v * parameters.maskTexture.height), 0,
                parameters.maskTexture.height - 1);

            Color pixelColor = parameters.maskTexture.GetPixel(x, y);
            return pixelColor.grayscale;
        }

        private bool IsPositionTooClose(Vector3 position, float radius, float scale,
            ScatterParameters parameters)
        {
            Vector2Int gridCoord = WorldToGridCoord(position);
            float scaledRadius = radius * scale; // Consider scale

            if (grid.TryGetValue(gridCoord, out List<(Transform, float)> positions))
            {
                foreach ((Transform trans, float occupiedRadius) in positions)
                {
                    if (trans == null || 
                        Vector3.Distance(position, trans.position) <
                        (scaledRadius + occupiedRadius + parameters.spacing))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void AddGoToGrid(Transform trans, float scaledRadius = 0f)
        {
            Vector2Int gridCoord = WorldToGridCoord(trans);

            if (!grid.ContainsKey(gridCoord))
            {
                grid[gridCoord] = new List<(Transform, float)>();
            }

            grid[gridCoord].Add((trans, scaledRadius));
            instancesCreated++;
        }

        private void RemoveGoFromGrid(Transform trans)
        {
            Vector2Int gridCoord = WorldToGridCoord(trans);
            if (!grid.ContainsKey(gridCoord))
            {
                return;
            }   
            // 根据trans删除
            grid[gridCoord].RemoveAll(x => x.Item1 == trans);
        }

        private Vector2Int WorldToGridCoord(Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x / gridCellSize);
            int z = Mathf.FloorToInt(position.z / gridCellSize);
            return new Vector2Int(x, z);
        }
        private Vector2Int WorldToGridCoord(Transform trans)
        {
            return WorldToGridCoord(trans.position);
        }

        private void CalculateTotalWeight(ScatterParameters parameters)
        {
            totalWeight = 0f;
            foreach (var prefabWithWeight in parameters.prefabs)
            {
                totalWeight += prefabWithWeight.weight;
            }
        }

        private GameObject GetRandomPrefabBasedOnWeight(ScatterParameters parameters,
            out float radius, out float randomScale)
        {
            float randomValue = Random.Range(0, totalWeight);
            float cumulativeWeight = 0f;

            foreach (var prefabWithWeight in parameters.prefabs)
            {
                cumulativeWeight += prefabWithWeight.weight;
                if (randomValue < cumulativeWeight)
                {
                    radius = prefabWithWeight.radius;
                    randomScale = GetRandomScale(parameters);
                    return prefabWithWeight.prefab;
                }
            }

            radius = 1f;
            randomScale = 1f;
            return null;
        }

        private static float GetRandomScale(ScatterParameters parameters)
        {
            return Random.Range(parameters.minScale, parameters.maxScale);
        }

        private static Vector3 GetRandomRotation(ScatterParameters parameters)
        {
            float xRotation = Random.Range(0, parameters.rotationRange.x);
            float yRotation = Random.Range(0, parameters.rotationRange.y);
            float zRotation = Random.Range(0, parameters.rotationRange.z);
            return new Vector3(xRotation, yRotation, zRotation);
        }

        public void ResetScatter()
        {
            InitializeScatter(true);
        }
    }
}
#endif
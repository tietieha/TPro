using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RenderTools.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using M.Battle.Render;

namespace RenderTools.AnimInstance
{
    public class AnimatorInstanceBatchBake : AnimatorInstanceCreator
    {
        //
        private static string[] inputFolder =
            { "GameAssets/Actor/Creature/", "GameAssets/Actor/Creature_old/" };

        private static string replacePath = "Assets/GameAssets/Actor/";
        private static string targetPath = "Assets/GameAssets/ActorInstance/";

        private static string[] bakeAnimations =
        {
            "idle", "battle_behit", "battle_revive", "battle_move", "battle_atk", "battle_skill", "battle_dead",
            "battle_victory", "fall"
        };

        private GameObject _singleBakePrefab;

        private static AnimatorInstanceBatchBake window;

        [MenuItem("Tools/Animator Instance Generator Batch", false, 999)]
        static void OpenWindow()
        {
            // EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            // EditorApplication.isPlaying = true;
            window = GetWindow<AnimatorInstanceBatchBake>();
        }

        protected void OnEnable()
        {
            titleContent = new GUIContent("Animator Instance Generator Batch");
        }

        protected void OnDisable()
        {
        }

        private void OnGUI()
        {
            GUI.skin.label.wordWrap = true;

            DrawFoldOut("Asset to Bake", () =>
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("批量Bake", new GUILayoutOption[] { GUILayout.Width(100) }))
                    {
                        BakeAnimInstancingBatch();
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    _singleBakePrefab =
                        EditorGUILayout.ObjectField("Asset to Bake", _singleBakePrefab, typeof(GameObject), true) as
                            GameObject;

                    if (GUILayout.Button("bake 一个", new GUILayoutOption[] { GUILayout.Width(100) }))
                    {
                        if (_singleBakePrefab == null)
                        {
                            return;
                        }

                        var path = AssetDatabase.GetAssetPath(_singleBakePrefab);
                        var realPath = Path.Combine(Application.dataPath, path);
                        realPath = realPath.Replace("\\", "/");
                        realPath = realPath.Replace("/Assets/Assets/", "/Assets/");
                        var parentFolder = Path.GetDirectoryName(realPath);
                        BakeSingleBoneAnimInstance(realPath, parentFolder);
                    }

                    GUILayout.EndHorizontal();
                }
            });
        }

        public void BakeAnimInstancingBatch()
        {
            for (int i = 0; i < inputFolder.Length; i++)
            {
                string inputPath = inputFolder[i];
                string fullPath = Path.Combine(Application.dataPath, inputPath);
                fullPath = fullPath.Replace("\\", "/");
                if (!Directory.Exists(fullPath)) continue;
                string[] directories = Directory.GetDirectories(fullPath);
                for (int j = 0; j < directories.Length; j++)
                {
                    var prefabPath = directories[j] + "/Prefab";
                    if (Directory.Exists(prefabPath))
                    {
                        string[] allFiles = Directory.GetFiles(prefabPath);
                        for (int k = 0; k < allFiles.Length; k++)
                        {
                            if (allFiles[k].EndsWith(".prefab") && ! allFiles[k].EndsWith("_pve.prefab"))
                            {
                                BakeSingleBoneAnimInstance(allFiles[k], prefabPath);
                            }
                        }
                    }
                }
            }
        }

        private void BakeSingleBoneAnimInstance(string prefabPath, string parentFloderPath)
        {
            try
            {
                var relativePath = parentFloderPath.Replace('\\', '/');
                relativePath = relativePath.Replace(Application.dataPath, "Assets");

                var outPutPath = relativePath.Replace(replacePath, targetPath);
                _outputDir = outPutPath;
                CreateDirectoryByPath(outPutPath);
                var relativePrefabPath = prefabPath.Replace('\\', '/');
                relativePrefabPath = relativePrefabPath.Replace(Application.dataPath, "Assets");
                Debug.Log($"动画烘焙 开始 {relativePrefabPath}");
                _prefab = AssetDatabase.LoadAssetAtPath<GameObject>(relativePrefabPath);

                OnPrefabChanged();
                BakeAllAnimations();
                _outputDir = outPutPath;
                GetNeedBakeBones();
                BakeBoneAnimInstance();

                Debug.Log($"动画烘焙 结束 {relativePrefabPath}");
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
        }

        private static void CreateDirectoryByPath(string path)
        {
            string[] folderNames = path.Split('/');
            string currentPath = Application.dataPath;
            currentPath = currentPath.Replace("\\", "/");
            for (int i = 1; i < folderNames.Length; i++)
            {
                currentPath = currentPath + "/" + folderNames[i];
                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                }
            }
        }

        // protected void AddModelSocket(GameObject asset)
        // {
        //     ModelSocket comp = asset.AddComponent<ModelSocket>();
        //     comp.CreateAndGetSockets();
        // }

        protected void BakeAllAnimations()
        {
            for (int i = 0; i < clipsCache.Count; i++)
            {
                string clipName = clipsCache[i].name;
                if (bakeAnimations.Contains(clipName) && _preferences.bakeAnimStates.dictionary.ContainsKey(clipName))
                    _preferences.bakeAnimStates[clipName].isBake = true;
            }
        }

        protected void GetNeedBakeBones()
        {
            boneTransList.Clear();
            ModelSocket comp = _spawnedPrefab.GetComponent<ModelSocket>();
            if (comp == null) return;
            var allSockets = comp.GetAllSockets();
            foreach (var socket in allSockets)
            {
                boneTransList.Add(socket);
            }
        }
    }
}
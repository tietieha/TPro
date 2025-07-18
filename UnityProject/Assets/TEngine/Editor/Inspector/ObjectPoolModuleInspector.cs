﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace TEngine.Editor.Inspector
{
    [CustomEditor(typeof(ObjectPoolModule))]
    internal sealed class ObjectPoolModuleInspector : GameFrameworkInspector
    {
        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();

        private string m_SearchText = string.Empty;
        private SearchField _searchField;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            ObjectPoolModule t = (ObjectPoolModule)target;

            if (IsPrefabInHierarchy(t.gameObject))
            {
                EditorGUILayout.LabelField("Object Pool Count", t.Count.ToString());
                m_SearchText = _searchField.OnGUI(m_SearchText);
                // unity editor 自带的搜索框
                ObjectPoolBase[] objectPools = t.GetAllObjectPools(true);
                foreach (ObjectPoolBase objectPool in objectPools)
                {
                    DrawObjectPool(objectPool);
                }
            }

            Repaint();
        }

        private void OnEnable()
        {
            _searchField = new SearchField();
            if (!string.IsNullOrEmpty(EditorGUIUtility.systemCopyBuffer))
                m_SearchText = EditorGUIUtility.systemCopyBuffer;
        }

        private void DrawObjectPool(ObjectPoolBase objectPool)
        {
            bool lastState = m_OpenedItems.Contains(objectPool.FullName);
            bool currentState = EditorGUILayout.Foldout(lastState, objectPool.FullName);
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(objectPool.FullName);
                }
                else
                {
                    m_OpenedItems.Remove(objectPool.FullName);
                }
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Name", objectPool.Name);
                    EditorGUILayout.LabelField("Type", objectPool.ObjectType.FullName);
                    EditorGUILayout.LabelField("Auto Release Interval", objectPool.AutoReleaseInterval.ToString());
                    EditorGUILayout.LabelField("Capacity", objectPool.Capacity.ToString());
                    EditorGUILayout.LabelField("Used Count", objectPool.Count.ToString());
                    EditorGUILayout.LabelField("Can Release Count", objectPool.CanReleaseCount.ToString());
                    EditorGUILayout.LabelField("Expire Time", objectPool.ExpireTime.ToString());
                    EditorGUILayout.LabelField("Priority", objectPool.Priority.ToString());
                    ObjectInfo[] objectInfos = objectPool.GetAllObjectInfos();

                    if (!string.IsNullOrEmpty(m_SearchText))
                        objectInfos = objectInfos.Where(info => info.Name.Contains(m_SearchText, StringComparison.OrdinalIgnoreCase)).ToArray();
                    if (objectInfos.Length > 0)
                    {
                        EditorGUILayout.LabelField("Name",
                            objectPool.AllowMultiSpawn ? "Locked\tCount\tFlag\tPriority\tLast Use Time" : "Locked\tIn Use\tFlag\tPriority\tLast Use Time", GUILayout.ExpandWidth(true));
                        foreach (ObjectInfo objectInfo in objectInfos)
                        {
                            EditorGUILayout.LabelField(string.IsNullOrEmpty(objectInfo.Name) ? "<None>" : objectInfo.Name,
                                objectPool.AllowMultiSpawn
                                    ? Utility.Text.Format("{0}\t{1}\t{2}\t{3}\t{4:yyyy-MM-dd HH:mm:ss}", objectInfo.Locked, objectInfo.SpawnCount,
                                        objectInfo.CustomCanReleaseFlag,
                                        objectInfo.Priority, objectInfo.LastUseTime.ToLocalTime())
                                    : Utility.Text.Format("{0}\t{1}\t{2}\t{3}\t{4:yyyy-MM-dd HH:mm:ss}", objectInfo.Locked, objectInfo.IsInUse,
                                        objectInfo.CustomCanReleaseFlag,
                                        objectInfo.Priority, objectInfo.LastUseTime.ToLocalTime()),
                                GUILayout.ExpandWidth(true));
                        }

                        if (GUILayout.Button("Release"))
                        {
                            objectPool.Release();
                        }

                        if (GUILayout.Button("Release All Unused"))
                        {
                            objectPool.ReleaseAllUnused();
                        }

                        if (GUILayout.Button("Export CSV Data"))
                        {
                            string exportFileName = EditorUtility.SaveFilePanel("Export CSV Data", string.Empty,
                                Utility.Text.Format("Object Pool Data - {0}.csv", objectPool.Name),
                                string.Empty);
                            if (!string.IsNullOrEmpty(exportFileName))
                            {
                                try
                                {
                                    int index = 0;
                                    string[] data = new string[objectInfos.Length + 1];
                                    data[index++] = Utility.Text.Format("Name,Locked,{0},Custom Can Release Flag,Priority,Last Use Time",
                                        objectPool.AllowMultiSpawn ? "Count" : "In Use");
                                    foreach (ObjectInfo objectInfo in objectInfos)
                                    {
                                        data[index++] = objectPool.AllowMultiSpawn
                                            ? Utility.Text.Format("{0},{1},{2},{3},{4},{5:yyyy-MM-dd HH:mm:ss}", objectInfo.Name, objectInfo.Locked,
                                                objectInfo.SpawnCount,
                                                objectInfo.CustomCanReleaseFlag, objectInfo.Priority, objectInfo.LastUseTime.ToLocalTime())
                                            : Utility.Text.Format("{0},{1},{2},{3},{4},{5:yyyy-MM-dd HH:mm:ss}", objectInfo.Name, objectInfo.Locked,
                                                objectInfo.IsInUse,
                                                objectInfo.CustomCanReleaseFlag, objectInfo.Priority, objectInfo.LastUseTime.ToLocalTime());
                                    }

                                    File.WriteAllLines(exportFileName, data, Encoding.UTF8);
                                    Debug.Log(Utility.Text.Format("Export object pool CSV data to '{0}' success.", exportFileName));
                                }
                                catch (Exception exception)
                                {
                                    Debug.LogError(Utility.Text.Format("Export object pool CSV data to '{0}' failure, exception is '{1}'.", exportFileName,
                                        exception));
                                }
                            }
                        }
                    }
                    else
                    {
                        GUILayout.Label("Object Pool is Empty ...");
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
            }
        }
    }
}
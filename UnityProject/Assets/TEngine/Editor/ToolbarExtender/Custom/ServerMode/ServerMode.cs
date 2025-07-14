using System;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace TEngine.Editor
{
    [InitializeOnLoad]
    public class ServerMode
    {
        static class ToolbarStyles
        {
            public static readonly GUIStyle ToolBarButtonGuiStyle;

            static ToolbarStyles()
            {
                ToolBarButtonGuiStyle = new GUIStyle(ButtonStyleName)
                {
                    padding = new RectOffset(2, 8, 2, 2),
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };
            }
        }

        static ServerMode()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
            _gameNetType = (GameNetType)EditorPrefs.GetInt("EditorGameNetType", 1);
        }

        private const string ButtonStyleName = "Tab middle";
        static GUIStyle _buttonGuiStyle;

        private static GameNetType _gameNetType;
        private static readonly string[] _gameNetTypeNames = EnumHelper.GetAllHeaders<GameNetType>();
        
        static void OnToolbarGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                var color = GUI.color;
                GUI.color = _gameNetType == 0 ? Color.red : Color.green;
                GameNetType selected = (GameNetType)EditorGUILayout.Popup("", (int)_gameNetType, _gameNetTypeNames,
                    ToolbarStyles.ToolBarButtonGuiStyle, GUILayout.Width(150));

                if (selected != _gameNetType)
                {
                    Debug.Log($"编辑器 : {selected}");
                    _gameNetType = selected;
                    EditorPrefs.SetInt("EditorGameNetType", (int)_gameNetType);

                    Utility.File.DeleteFolder(SettingsUtils.ConfigSandBoxFolder);
                }
                GUI.color = color;
                GUILayout.Space(10);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
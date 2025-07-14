using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace TEngine.Editor
{
    [InitializeOnLoad]
    public class ConfigMode
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

        static ConfigMode()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
            _configModeIndex = EditorPrefs.GetInt("EnableUpdateConfig", 1);
            EditorPrefs.SetInt("EnableUpdateConfig", _configModeIndex);
        }

        private const string ButtonStyleName = "Tab middle";
        static GUIStyle _buttonGuiStyle;

        private static readonly string[] _configeModeNames =
        {
            "配置热更关",
            "配置热更开",
        };

        private static int _configModeIndex = 1;
        public static int ConfigModeIndex => _configModeIndex;

        static void OnToolbarGUI()
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                var color = GUI.color;
                GUI.color = _configModeIndex == 1 ? Color.green : Color.red;
                int selectedIndex = EditorGUILayout.Popup("", _configModeIndex, _configeModeNames,
                    ToolbarStyles.ToolBarButtonGuiStyle, GUILayout.Width(150));
                if (selectedIndex != _configModeIndex)
                {
                    Debug.Log($"编辑器 : {_configeModeNames[selectedIndex]}");
                    _configModeIndex = selectedIndex;
                    EditorPrefs.SetInt("EnableUpdateConfig", selectedIndex);
                }
                GUI.color = color;
                GUILayout.Space(10);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
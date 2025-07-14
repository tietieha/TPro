using UnityEditor;
using UnityEngine;

namespace TEngine.Editor.Inspector
{
    [CustomEditor(typeof(UIModule))]
    internal sealed class UIModuleInspector : GameFrameworkInspector
    {
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_DefaultUI = null;
        private SerializedProperty m_dontDestroyUIRoot = null;
        private SerializedProperty m_UICamera = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            UIModule t = (UIModule)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);
                EditorGUILayout.PropertyField(m_DefaultUI);
                EditorGUILayout.PropertyField(m_dontDestroyUIRoot);
                EditorGUILayout.PropertyField(m_UICamera);
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("AdapterScaler"))
            {
                t.AdapterScaler();
            }

            
            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_DefaultUI = serializedObject.FindProperty("m_DefaultUI");
            m_dontDestroyUIRoot = serializedObject.FindProperty("m_dontDestroyUIRoot");
            m_UICamera = serializedObject.FindProperty("m_UICamera");

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}

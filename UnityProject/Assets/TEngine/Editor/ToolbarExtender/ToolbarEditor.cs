using UnityEditor;
using UnityEngine.UIElements;

namespace UnityToolbarExtender
{
    public class ToolbarEditor : EditorWindow
    {
        [MenuItem("Tools/ToolbarExtender/EnableGit")]
        public static void EnableGit()
        {
            EditorPrefs.SetBool("EnableGitToolBar", !EditorPrefs.GetBool("EnableGitToolBar"));
        }
    }
}
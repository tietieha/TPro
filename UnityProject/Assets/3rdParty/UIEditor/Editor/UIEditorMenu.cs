// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-02-13       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using UnityEditor;
using UnityEngine;

namespace U3DExtends
{
    public class UIEditorMenu
    {
        [MenuItem("UI编辑器/配置/SceneView菜单", priority = 1000)]
        private static void SceneMenu()
        {
            UIEditorConfigure.IsShowSceneMenu = !UIEditorConfigure.IsShowSceneMenu;
        }

        [MenuItem("UI编辑器/配置/SceneView菜单", true)]
        public static bool CheckPlatform()
        {
            Menu.SetChecked("UI编辑器/配置/SceneView菜单", UIEditorConfigure.IsShowSceneMenu);
            return true;
        }

        [MenuItem("GameObject/UI/UIFullScreenPanel")]
        public static void UIFullScreenPanel()
        {
            var selectGo = Selection.activeGameObject;
            if (selectGo ==null || selectGo.gameObject.name != "UIContainer")
            {
                EditorUtility.DisplayDialog("警告", "请选中 UIContainer 节点！", "ok");
                return;
            }
            
            var path = "Assets/_Test/UITemp/UIFullScreenPanel.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            
            go.transform.SetParent(selectGo.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<RectTransform>().anchorMax = Vector2.one;
            go.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            go.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            go.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            Selection.activeGameObject = go;
        }
    }
}
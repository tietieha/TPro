// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-08-21       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using UnityEditor;
using UnityEngine;

namespace GEngine.MapEditor
{
	public class MapEditorMenu
	{
		[MenuItem("GameObject/MapEditor/CustomLOD", false, 1)]
		static void CustomLOD()
		{
			// 获取选中的游戏对象
			GameObject selectedGameObject = Selection.activeGameObject;
			if (selectedGameObject == null)
			{
				Debug.LogWarning("No GameObject selected!");
				return;
			}

			var trans = selectedGameObject.transform;
			foreach (var lodname in GlobalDef.S_LODS.Values)
			{
				var lodtrans = trans.Find(lodname);
				if (lodtrans == null)
				{
					new GameObject(lodname).transform.SetParent(trans);
				}
			}
		}
	}
}

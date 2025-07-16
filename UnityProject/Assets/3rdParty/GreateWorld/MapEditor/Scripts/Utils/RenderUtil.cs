#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace GEngine
{
	public class RenderUtil
	{
		public static void SwitchSelectedRenderState(GameObject obj, bool showWireFrame)
		{
			
			if (null != obj)
			{
				EditorSelectedRenderState state =
					showWireFrame ? EditorSelectedRenderState.Wireframe : EditorSelectedRenderState.Highlight;
				Renderer r = obj.GetComponent<Renderer>();
				if (null != r)
				{
					EditorUtility.SetSelectedRenderState(r, state);
				}

				int count = obj.transform.childCount;
				for (int i = 0; i < count; ++i)
				{
					Transform tr = obj.transform.GetChild(i);
					r = tr.gameObject.GetComponent<Renderer>();
					if (r != null)
					{
						EditorUtility.SetSelectedRenderState(r, state);
					}
				}
			}
		}
		
		public static Bounds GetBounds(GameObject target, bool include_children = true)
		{
			Vector3 center = target.transform.position;
			Bounds bounds = new Bounds();
			if (include_children)
			{
				Renderer[] mrs = target.gameObject.GetComponentsInChildren<Renderer>();
				if (mrs.Length != 0)
				{
					bounds = mrs[0].bounds;
					foreach (Renderer mr in mrs)
					{
						bounds.Encapsulate(mr.bounds);
					}
				}
			}
			else
			{
				Renderer rend = target.GetComponentInChildren<Renderer>();
				if (rend)
				{
					bounds = rend.bounds;
				}
			}
 
			return bounds;
		}
		
		public static float GetBoundsRadius(GameObject go)
		{
			Bounds bounds = RenderUtil.GetBounds(go);
			return Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z)) / 2;
		}

		public static float GetBoundsRadius(Bounds bounds)
		{
			return Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z)) / 2;
		}

		public static Color GetRandomColor()
		{
			float r = Random.Range(0f, 1f);
			float g = Random.Range(0f, 1f);
			float b = Random.Range(0f, 1f);
			Color color = new Color(r, g, b);
			return color;
		}
		
		public static Vector2 GetMainGameViewSize()
		{
			System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
			System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
			return (Vector2)Res;
		}
	}
}
#endif
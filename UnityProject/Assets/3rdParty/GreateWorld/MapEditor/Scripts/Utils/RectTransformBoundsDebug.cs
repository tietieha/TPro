// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-04-15       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using UnityEngine;

#if UNITY_EDITOR
namespace GreateWorld.MapEditor
{
	[ExecuteAlways]
	public class RectTransformBoundsDebug : MonoBehaviour
	{
		private RectTransform rectTransform;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		private void OnRenderObject()
		{
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.identity);
			// 获取RectTransform的四个顶点
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);

			GL.Begin(GL.LINE_STRIP);
			GL.Color(Color.yellow);
			GL.Vertex3(corners[0].x, corners[0].y, corners[0].z);
			GL.Vertex3(corners[1].x, corners[1].y, corners[1].z);
			GL.Vertex3(corners[2].x, corners[2].y, corners[2].z);
			GL.Vertex3(corners[3].x, corners[3].y, corners[3].z);
			GL.Vertex3(corners[0].x, corners[0].y, corners[0].z);
			GL.End();
			
			GL.PopMatrix();
		}
	}
}
#endif
//#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GEngine
{
	public enum LOD_DEF
	{
		[LabelText("默认,按照默认的LOD设置")]
		LOD_NONE = 0,
		
		[LabelText("LOD_1 -- 20m")]
		LOD_1,
		
		[LabelText("LOD_2 -- 36m")]
		LOD_2,
		
		[LabelText("LOD_3 -- 54m")]
		LOD_3,
		
		[LabelText("LOD_4 -- 60m")]
		LOD_4,
		
		[LabelText("LOD_5 -- 120m")]
		LOD_5,
		
		[LabelText("LOD_6 -- 360m")]
		LOD_6,
	}

	[ExecuteAlways]
	public class HDAToolScatterPrefabMark : MonoBehaviour
	{
		[Header("LOD设置")]
		[InfoBox("1.羊皮卷		= LOD_6 \n" +
		         "2.地形,山,路	= LOD_1 ~ LOD_5 \n" +
		         "3.石头,树,草	= LOD_1 ~ LOD_4")]
		public LOD_DEF renderlod = LOD_DEF.LOD_NONE;
		// private void OnDrawGizmosSelected()
		// {
		// 	if (WorldEditor.Instance.HDAToolResData.WorldEditorConf != null && WorldEditor.Instance.HDAToolResData.WorldEditorConf.IsDrawGrid)
		// 	{
		// 		Bounds bound = RenderUtil.GetBounds(gameObject);
		// 		float boundsRadius = RenderUtil.GetBoundsRadius(bound);
		// 		Gizmos.DrawWireSphere(bound.center, boundsRadius);
		//
		// 		Gizmos.color = Color.red;
		// 		Vector3 offset = bound.center - transform.position;
		// 		offset.y = 0;
		// 		Transform[] allChild = gameObject.GetComponentsInChildren<Transform>();
		// 		foreach (Transform child in allChild)
		// 		{
		// 			MeshFilter meshFilter = child.GetComponent<MeshFilter>();
		// 			MeshRenderer renderer = child.GetComponent<MeshRenderer>();
		// 			if (meshFilter != null && renderer != null)
		// 			{
		// 				Gizmos.DrawWireSphere(child.position, boundsRadius);
		// 			}
		// 		}
		// 	}
		// }
	}
}
//#endif
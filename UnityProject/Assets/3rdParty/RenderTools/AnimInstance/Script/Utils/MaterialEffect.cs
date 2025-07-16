// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2022-08-31 16:51 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEditor;
using UnityEngine;

namespace RenderTools.Utils
{
	[Serializable]
	public class MaterailPropertyInfo
	{
#if ODIN_INSPECTOR
		[HorizontalGroup, HideLabel]
#endif
		public string propertyName;
#if ODIN_INSPECTOR
		[HorizontalGroup, HideLabel]
#endif
		public Texture texture;
	}

	[Serializable]
	public class MaterailInfo
	{
#if ODIN_INSPECTOR
		[TableColumnWidth(100, Resizable = false)]
#endif
		public string name;
#if ODIN_INSPECTOR
		[ListDrawerSettings(DraggableItems = false)]
#endif
		public List<MaterailPropertyInfo> propertyInfos;
	}

	[Serializable]
	public class RendererMaterialInfo
	{
		public Renderer renderer;
#if ODIN_INSPECTOR
		[TableList]
#endif
		public List<MaterailInfo> MaterailInfos;
	}

	public class MaterialEffect : MonoBehaviour
	{
#if UNITY_EDITOR
#if ODIN_INSPECTOR
		[ListDrawerSettings(DraggableItems = false, ShowIndexLabels = true,
			OnBeginListElementGUI = "BeginDrawInfoElement",
			OnEndListElementGUI = "EndDrawInfoElement")]
#endif
		public string[] matKey = new[] {"copper", "plaster"};
		private void BeginDrawInfoElement(int index)
		{
			EditorGUILayout.BeginHorizontal();
		}

		private void EndDrawInfoElement(int index)
		{
			if (GUILayout.Button("Change"))
			{
				ChangeMat(matKey[index]);
			}
			
			EditorGUILayout.EndHorizontal();
		}
#endif
		
		[SerializeField]
#if ODIN_INSPECTOR
		[ListDrawerSettings(DraggableItems = false, ShowIndexLabels = true)]
#endif
		List<RendererMaterialInfo> _rendererMaterialInfos = new List<RendererMaterialInfo>();

		private bool _inited;
		private Dictionary<Renderer, MaterialPropertyBlock> _renderMatBlockDic = new Dictionary<Renderer, MaterialPropertyBlock>();
		
		public void Init()
		{
			if (_inited)
				return;
			_inited = true;
		}

		// copper plaster
		public void ChangeMat(string matkey)
		{
			for (int i = 0; i < _rendererMaterialInfos.Count; i++)
			{
				RendererMaterialInfo rm = _rendererMaterialInfos[i];
				Renderer r = rm.renderer;
				MaterailInfo info = rm.MaterailInfos.First(item => item.name.Equals(matkey));
				if (r != null && info != null && info.propertyInfos != null)
				{
					MaterailPropertyInfo pInfo;
					if (!_renderMatBlockDic.TryGetValue(r, out MaterialPropertyBlock block))
					{
						block = new MaterialPropertyBlock();
						_renderMatBlockDic.Add(r, block);
					}
					r.GetPropertyBlock(block);
						
					for (int j = 0; j < r.sharedMaterials.Length; j++)
					{
						for (int infoIdx = 0; infoIdx < info.propertyInfos.Count; infoIdx++)
						{
							pInfo = info.propertyInfos[infoIdx];
							block.SetTexture(pInfo.propertyName, pInfo.texture);
						}
					}
						
					r.SetPropertyBlock(block);
				}
			}
		}
	}
}
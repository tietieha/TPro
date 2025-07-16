// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-16 14:54 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using System.Collections.Generic;
using UnityEngine;

namespace BigWorldRender
{
	public class RenderProcessInstance : RenderProcessBase
	{
		public const int RENDERMAXNUM = 120;

		private List<Matrix4x4> _matrix4X4List;
		private RegionAssetData _renderAsset;
		private int _currentRenderIndex = 0;
		private int _layer = 0;

		public RenderProcessInstance()
		{
			_matrix4X4List = new List<Matrix4x4>(RENDERMAXNUM);
		}

		/// <summary>
		/// 设置渲染资源
		/// </summary>
		/// <param name="renderAsset"></param>
		public override void SetUpAsset(RegionAssetData renderAsset)
		{
			_renderAsset = renderAsset;
			Mesh mesh = renderAsset.mesh;
			Material mat = renderAsset.material;

			if (!mat.enableInstancing)
			{
				Debug.LogError("材质错误 unInstance mesh " + mesh.name + " mat name " + mat.name);
				return;
			}

			
		}

		public override bool Draw(Matrix4x4 renderMatrix, int layer = 0)
		{
			_layer = layer;
			_matrix4X4List.Add(renderMatrix);
			_currentRenderIndex++;
			if (_currentRenderIndex == RENDERMAXNUM)
			{
				Draw();
				return true;
			}

			_dirty = true;
			return false;
		}

		public override void DrawRemain()
		{
			Draw();
		}

		private void Draw()
		{
			Graphics.DrawMeshInstanced(_renderAsset.mesh, _renderAsset.subMeshIndex, _renderAsset.material, _matrix4X4List);
			_matrix4X4List.Clear();
			_currentRenderIndex = 0;
			_dirty = false;
		}
	}
}
// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-16 14:53 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using UnityEngine;

namespace BigWorldRender
{
	public class RenderProcessSingle : RenderProcessBase
	{
		private RegionAssetData _renderAsset;
		public override void SetUpAsset(RegionAssetData renderAsset)
		{
			_renderAsset = renderAsset;
		}

		public override bool Draw(Matrix4x4 renderMatrix, int layer = 0)
		{
			Graphics.DrawMesh(_renderAsset.mesh, renderMatrix, _renderAsset.material, layer, null, _renderAsset.subMeshIndex);
			return true;
		}

	}
}
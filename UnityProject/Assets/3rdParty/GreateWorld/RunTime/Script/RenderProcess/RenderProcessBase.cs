// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-13 18:38 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using UnityEngine;

namespace BigWorldRender
{
	public static class RenderProcessType
	{
		public static readonly int SINGLE = 1;		// DrawMesh
		public static readonly int INSTANCE = 2;	// DrawMeshInstance
		
		public static readonly int COUNT = 2;		// count
	}
	/// <summary>
	/// 渲染流程
	/// 1.DrawMesh
	/// 2.DrawMeshInstance
	/// </summary>
	public abstract class RenderProcessBase
	{
		public bool Dirty => _dirty;
		protected bool _dirty = false;
		// Leon-TODO: RegionAssetData => RenderAsset
		public abstract void SetUpAsset(RegionAssetData renderAsset);
		public abstract bool Draw(Matrix4x4 renderMatrix, int layer = 0);

		public virtual void DrawRemain()
		{
		}
	}
}
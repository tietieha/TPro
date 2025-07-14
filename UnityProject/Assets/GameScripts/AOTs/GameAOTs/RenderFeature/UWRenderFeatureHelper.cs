// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-09-12 12:21 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

namespace RenderFeature
{
	[System.Serializable]
	public enum ResolutionDownSample
	{
		None,
		_2x,
		_4x,
		_8x,
	}
	
	public class UWRenderFeatureHelper
	{
		public static float GetResolutionScaleValue(ResolutionDownSample resolutionDownSample)
		{
			switch (resolutionDownSample)
			{
				case ResolutionDownSample.None:
					return 1f;
				case ResolutionDownSample._2x:
					return 0.5f;
				case ResolutionDownSample._4x:
					return 0.25f;
				case ResolutionDownSample._8x:
					return 0.125f;
			}

			return 0.5f; // default to half res
		}
	}
}
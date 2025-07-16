// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon			 /
//   / @Modified   2023-10-16 15:27 /
//  /_____________________ooo______/
//  			  |_ | _|
//  			  /-'Y'-\
//  			 (__/ \__)
// ******************************************************************

using UnityEngine;

namespace BigWorldRender
{
	public class RenderProcessHelper
	{
		public static RenderProcessBase GetRenderProcess(int renderProcessType)
		{
			if (SystemInfo.supportsInstancing && renderProcessType == RenderProcessType.INSTANCE)
			{
				return new RenderProcessInstance();
			}
			else
			{
				return new RenderProcessSingle();
			}
		}
	}
}
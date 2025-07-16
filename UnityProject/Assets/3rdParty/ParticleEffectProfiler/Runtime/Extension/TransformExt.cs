// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-01-30       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    |_ | _|			                *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using UnityEngine;

namespace UWParticleSystemProfiler
{
	public static class TransformExt
	{
		public static string GetFullHierarchyPath(this Transform transform)
		{
			if (transform.parent != null)
			{
				return transform.parent.GetFullHierarchyPath() + "/" + transform.name;
			}
			else
			{
				return transform.name;
			}
		}
	}
}
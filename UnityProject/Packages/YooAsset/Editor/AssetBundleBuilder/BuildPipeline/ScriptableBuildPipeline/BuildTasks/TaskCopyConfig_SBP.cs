// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-01-20       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

namespace YooAsset.Editor
{
    public class TaskCopyConfig_SBP : TaskCopyFolder, IBuildTask
    {
        public void Run(BuildContext context)
        {
            CopyFolder("Assets/GameAssets/Config", "Assets/StreamingAssets/Config");
        }
    }
}
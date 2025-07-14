// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-01-21       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

namespace TEngine
{
    public class ConfigVersion
    {
        public bool IsValid = false;
        public string Version = "1.0.0.0";
        public string GMVersion = "0";
        public string VersionStr;
        private string[] SubVersion = new[] { "1", "0", "0", "0" };

        public ConfigVersion()
        {
        }

        public ConfigVersion(string versionStr)
        {
            VersionStr = versionStr;
            if (string.IsNullOrEmpty(versionStr))
            {
                return;
            }

            var Ver = versionStr.Split(";");
            if (Ver.Length != 2)
            {
                Log.Error("ConfigVersion Parse Error: " + versionStr);
                return;
            }

            var subVer = Ver[0].Split(".");
            if (subVer.Length != 4)
            {
                Log.Error("ConfigVersion SubVersion Parse Error: " + versionStr);
                return;
            }

            IsValid = true;
            Version = Ver[0];
            GMVersion = Ver[1];
            SubVersion = subVer;
        }

        public static bool Compare(ConfigVersion v1, ConfigVersion v2)
        {
            for (int i = 0; i < v1.SubVersion.Length; i++)
            {
                if (int.Parse(v1.SubVersion[i]) > int.Parse(v2.SubVersion[i]))
                {
                    return true;
                }
            }

            if (int.Parse(v1.GMVersion) > int.Parse(v2.GMVersion))
            {
                return true;
            }

            return false;
        }
    }
}
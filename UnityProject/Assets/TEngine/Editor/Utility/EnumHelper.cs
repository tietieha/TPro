// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2025-05-24       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TEngine.Editor
{
    public class EnumHelper
    {
        public static string[] GetAllHeaders<TEnum>() where TEnum : Enum
        {
            return typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(field => field.GetCustomAttribute<HeaderAttribute>())
                .Where(attr => attr != null)
                .Select(attr => attr.header)
                .ToArray();
        }
    }
}
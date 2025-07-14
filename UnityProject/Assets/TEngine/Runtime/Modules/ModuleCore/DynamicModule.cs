// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-12-13       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;

namespace TEngine
{
    /// <summary>
    /// 因为xlua的原因，需要有动态的module
    /// </summary>
    public class DynamicModule : Module
    {
        protected override void Awake()
        {
        }

        public virtual void Start()
        {

        }
    }
}
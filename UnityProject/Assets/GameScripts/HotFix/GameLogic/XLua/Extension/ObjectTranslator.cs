// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-12-11       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;

namespace XLua
{
    public partial class ObjectTranslator
    {
        internal object PopValues2(RealStatePtr L, int oldTop)
        {
            int newTop = LuaAPI.lua_gettop(L);
            if (oldTop == newTop)
                return null;

            // 我们只取第一个返回值，多个返回值请使用LuaTabel自行处理
            object ret = null;
            for (int i = oldTop + 1; i <= newTop; i++)
            {
                ret = GetObject(L, i);
                break;
            }
            LuaAPI.lua_settop(L, oldTop);

            return ret;
        }
    }
}
// **********************************************************
// *		                .-"""-.							*
// *		               / .===. \			            *
// *		               \/ 6 6 \/			            *
// *		     ______ooo__\__=__/_____________			*
// *		    / @author     Leon			   /			*
// *		   / @Modified   2024-11-25       /			    *
// *		  /_____________________ooo______/			    *
// *		  			    /-'Y'-\			                *
// *		  			   (__/ \__)			            *
// **********************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace TEngine
{
    public static class SystemExtension
    {
        public static List<string> ToStrList(this string str, char splitChar)
        {
            List<string> strList = new List<string>();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strs = str.Split(new char[] { splitChar },
                    StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strs.Length; i++)
                {
                    strList.Add(strs[i]);
                }
            }

            return strList;
        }

        public static List<int> ToIntList(this string str, char splitChar)
        {
            List<int> iList = new List<int>();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strs = str.Split(new char[] { splitChar },
                    StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strs.Length; i++)
                {
                    iList.Add(strs[i].ToInt());
                }
            }

            return iList;
        }

        public static int ToInt(this string str)
        {
            int i = 0;
            int.TryParse(str, out i);
            return i;
        }

        public static int ToInt(this object str)
        {
            int i = 0;
            int.TryParse(str.ToString(), out i);
            return i;
        }

        public static long ToLong(this object str)
        {
            long.TryParse(str.ToString(), out var i);
            return i;
        }

        // 这个函数用来实现ReadOnlySpan.ToInt，因为int.Parse在后面版本才支持ReadOnlySpan，而我们用的是.Net 2.0
        // 但是为了这个函数去升级CLR，又显得臃肿，所以这里自己特殊处理一下；这个转化只支持10进制。
        // 目前这个代码支持前端有空格，但不支持数字中间有空格的情况。
        public static int ToInt(this ReadOnlySpan<char> str)
        {
            int sign = 1, Base = 0, i = 0;

            // if whitespaces then ignore.
            while (str[i] == ' ')
            {
                i++;
            }

            // sign of number
            if (str[i] == '-' || str[i] == '+')
            {
                sign = 1 - 2 * (str[i++] == '-' ? 1 : 0);
            }

            // checking for valid input
            while (
                i < str.Length
                && str[i] >= '0'
                && str[i] <= '9')
            {
                // handling overflow test case
                if (Base > int.MaxValue / 10 || (Base == int.MaxValue / 10 && str[i] - '0' > 7))
                {
                    if (sign == 1)
                        return int.MaxValue;
                    else
                        return int.MinValue;
                }

                Base = 10 * Base + (str[i++] - '0');
            }

            return Base * sign;
        }

        public static float ToFloat(this string value)
        {
            return ToSingle(value);
        }

        public static float ToSingle(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            float i = 0f;
            try
            {
                i = Convert.ToSingle(value, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Log.Error($"string convert to single error : {e}");
            }

            return i;
        }

        // 为了直接从ReadOnlySpan -> float!
        public static float ToFloat(this ReadOnlySpan<char> str)
        {
            float f = (float)Strtod_CSharp.strtod(str);

#if UNITY_EDITOR && !FINAL_RELEASE
            float ttt = str.ToString().ToFloat();
            if (!Mathf.Approximately(f, ttt))
            {
                Log.Error("BUGBUGBUG! ToFloat() not same!");
            }
#endif

            return f;
        }

        public static long ToLong(this string str)
        {
            long i = 0;
            if (str.Contains("."))
            {
                List<string> strVec = new List<string>();
                StringUtils.SplitString(str, '.', ref strVec);
                long.TryParse(strVec[0], out i);
            }
            else
            {
                long.TryParse(str, out i);
            }

            return i;
        }
    }
}
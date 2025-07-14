using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace TEngine
{
    public static class StringUtils
    {
        static private string[] s_roman_level = new string[]
        {
            "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X",
            "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX",
            "XXI", "XXII", "XXIII", "XXIV", "XXV", "XXVI", "XXVII", "XXVIII", "XXIX", "XXX"
        };

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsInt(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        public static string FixNewLine(this string str)
        {
            return str.Replace("\\n", "\n");
        }

        public static string GetMD5(string msg)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }

            destString = destString.PadLeft(32, '0');
            return destString;
        }

        public static string GetFileNameNoExtension(this string path, char separator = '.')
        {
            if (path.IsNullOrEmpty())
            {
                return "";
            }

            return path.Substring(0, path.LastIndexOf(separator));
        }

        public static string GetFileName(this string path, char separator = '/')
        {
            if (path.IsNullOrEmpty())
            {
                return "";
            }

            return path.Substring(path.LastIndexOf(separator) + 1);
        }

        public static string GetExtensionName(this string path)
        {
            return System.IO.Path.GetExtension(path);
        }

        public static string GetUrlName(this string url)
        {
            // string str = StringUtls.GetFileName("https://gslls.im30app.com/gameservice/getserverlist.php");
            // str = StringUtls.GetFileNameNoExtension(str); str = getserverlist
            // https://gslls.im30app.com/gameservice/getserverlist.php
            string str = StringUtils.GetFileName(url);
            str = StringUtils.GetFileNameNoExtension(str);
            return str;
        }

        public static string GetDirectoryName(this string fileName)
        {
            if (fileName.IsNullOrEmpty() || !fileName.Contains("/"))
            {
                return "";
            }

            return fileName.Substring(0, fileName.LastIndexOf("/"));
        }

        //千位分隔符
        public static string GetFormattedSeperatorNum(int value)
        {
            return value.ToString("N0");
        }

        public static string GetFormattedSeperatorNum(long value)
        {
            return value.ToString("N0");
        }

        public static string GetFormattedSeperatorNum(string value)
        {
            //小数点和百分值不做分隔
            if (value.Contains(".") || value.Contains("%"))
            {
                return value;
            }

            return value.ToLong().ToString("N0");
        }

        public static string GetFormattedInt(int value)
        {
            //string unit = "";
            var kVal = (float)value / 1000f;
            var mVal = (float)value / 1000000f;
            if (mVal >= 1f)
            {
                return mVal.ToString("F1") + "M";
            }

            if (kVal >= 1f)
            {
                return kVal.ToString("F1") + "K";
            }

            return value.ToString("");
        }

        public static string GetFormattedLong(long value)
        {
            //string unit = "";
            var kVal = (float)value / 1000f;
            var mVal = (float)value / 1000000f;
            if (mVal >= 1f)
            {
                return mVal.ToString("F1") + "M";
            }

            if (kVal >= 1f)
            {
                return kVal.ToString("F1") + "K";
            }

            return value.ToString("");
        }

        public static string GetFormattedStr(double value)
        {
            var kVal = value / 1000f;
            var mVal = value / 1000000f;
            var gVal = value / 1000000000f;
            if (gVal >= 1f)
            {
                return gVal.ToString("F1") + "G";
            }

            if (mVal >= 1f)
            {
                return mVal.ToString("F1") + "M";
            }

            if (kVal >= 1f)
            {
                return kVal.ToString("F1") + "K";
            }

            return value.ToString("");
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="_charCount">生成的字符数</param>
        /// <returns></returns>
        private static int rep = 0;

        public static string GenerateRandomStr(int _codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < _codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }

            return str;
        }

        public static string FormatDBString(string str)
        {
            return "'" + str + "'";
        }

        public static string ConvertToRomanFromInt(int lv)
        {
            if (lv < 1 || lv > 30)
                return "";
            else
                return s_roman_level[lv - 1];
        }

        public static void SplitString(string str, char key, ref List<string> list)
        {
            list = str.Split(key).ToList();
        }

        public static string[] SplitString(string str, char key)
        {
            return str.Split(key);
        }


        public static string FormatStringMaxLength(string str, int maxLen = 18)
        {
            int strLen = str.Length;
            if (strLen > maxLen)
            {
                return str.Substring(0, maxLen);
            }

            return str;
        }

        /// <summary>
        /// string to section 逗号分段
        /// </summary>
        /// <returns>The s.</returns>
        /// <param name="rawStr">Raw string.</param>
        public static string S2Sec(string rawStr)
        {
            if (rawStr == null || rawStr == "")
                return "";

            if (rawStr.Length <= 3)
                return rawStr;

            string retStr = rawStr;
            int curPos = rawStr.Length;
            while (curPos > 3)
            {
                curPos -= 3;
                retStr = retStr.Insert(curPos, ",");
            }

            return retStr;
        }

        /// <summary>
        /// string to section 逗号分段
        /// </summary>
        /// <returns>The s.</returns>
        /// <param name="rawNum">Raw int.</param>
        public static string S2Sec(int rawNum)
        {
            return S2Sec(rawNum.ToString());
        }

        public static int TryParseInt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            else
            {
                return int.Parse(str);
            }
        }

        static private string[] s_num_string = new string[]
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30",
        };

        static private Dictionary<int, string> intValueToString = new Dictionary<int, string>(1000);

        public static string IntToString(int variable)
        {
            if (variable >= 0 && variable < s_num_string.Length)
            {
                return s_num_string[variable];
            }

            if (variable == -1)
            {
                return "-1";
            }

            if (!intValueToString.TryGetValue(variable, out var result))
            {
                intValueToString.Add(variable, result = variable.ToString());
            }

            return result;
        }
    }
}
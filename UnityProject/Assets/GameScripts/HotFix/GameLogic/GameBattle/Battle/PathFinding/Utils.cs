#if UNITY_2018_2_OR_NEWER
using UnityEngine.Assertions;
using UnityEngine;
#else
using System.Diagnostics;
#endif

using System;
using System.Collections.Generic;

namespace M.PathFinding
{
    static class Utils
    {

        static public long MAX_DISTANCE = long.MaxValue;
        static public double TWO_PI = Math.PI * 2.0;
        //断言
        static public void Assert(bool condition, string message = "")
        {
            if(!condition)
            {


#if UNITY_2018_2_OR_NEWER
                Debug.Log("M.PathFinding Asset Error");
#else
                Debug.Print("M.PathFinding Asset Error");
#endif
            }
#if UNITY_2018_2_OR_NEWER
            UnityEngine.Assertions.Assert.IsTrue(condition, message);
#else
            Debug.Assert(condition, message);
#endif

        }

#if UNITY_EDITOR && DEBUG
        static private bool s_isDebug = true;
#else
        static private bool s_isDebug = false;
#endif
        static public bool IsDebug() { return s_isDebug; }
        static public void SetDebug(bool isDebug) { s_isDebug = isDebug; }

        static private HashSet<int> s_LogUnitSet = new HashSet<int>
        {
            //在这里填写想要打印Log的Unit的ID
            // 11,
        };

        //设置当前需要打印Log的单位
        static public void SetLogUnitList(List<int> unitIdList)
        {
            s_LogUnitSet.Clear();
            foreach (var u in unitIdList)
            {
                s_LogUnitSet.Add(u);
            }
        }

        static public bool IsLogUnit(int unitId) 
        { 
            return s_LogUnitSet.Contains(unitId);
        }
        //static public void SetLogUnit(bool logUnit) { s_isLogUnit = logUnit; }

        static public void Log(string content, params object[] args)
        {
            if (IsDebug())
            {
#if UNITY_2018_2_OR_NEWER
                Debug.LogFormat(content, args);
#else
            Console.WriteLine(content, args);
#endif
            }
        }

        static public void LogWarning(string content, params object[] args)
        {
            if (IsDebug())
            {
#if UNITY_2018_2_OR_NEWER
                Debug.LogWarningFormat(content, args);
#else
            Console.WriteLine(content, args);
#endif
            }
        }


    }





}
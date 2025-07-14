using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

// 使用前请先打开ENABLE_CRASHSIGHT_STACKTRACE宏
public class CrashSightStackTrace
{
    public static bool enable = false;

    //CrashSightStackTrace开关，仅在il2cpp编译时允许打开
    public static void setEnable(bool enable)
    {
#if ENABLE_IL2CPP && ENABLE_CRASHSIGHT_STACKTRACE
        CrashSightStackTrace.enable = enable;
#endif
    }

#if UNITY_ANDROID
    public const string lib = "il2cpp";
#elif UNITY_IOS
    public const string lib = "__Internal";
#endif

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR && ENABLE_IL2CPP && ENABLE_CRASHSIGHT_STACKTRACE
    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern void il2cpp_current_thread_walk_frame_stack(Il2CppFrameWalkFunc func, IntPtr data);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_method_get_name(IntPtr method);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int il2cpp_method_get_param_count(IntPtr method);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_method_get_class(IntPtr method);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_method_get_param(IntPtr method, int index);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_class_get_name(IntPtr klass);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_class_get_namespace(IntPtr klass);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_type_get_name(IntPtr type);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_class_get_declaring_type(IntPtr klass);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr il2cpp_class_from_il2cpp_type(IntPtr type);

#if UNITY_2018_1_OR_NEWER
    [DllImport(lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool il2cpp_type_is_byref(IntPtr type);
#endif

    //type结构体，用于直接读取指针指向的内容，能不用则不用，尽量使用il2cpp的API
    public struct Il2CppType
    {
        public IntPtr unionData;
        public Int16 attrs;
        public Byte type;
        public Byte num_mods; 
        public bool byref()
        {
            return (num_mods & 64) != 0;//0b01000000
        }
    };

    public delegate void Il2CppFrameWalkFunc(IntPtr info, IntPtr user_data);

    //FrameWalk回调，每次处理一帧的信息
    [MonoPInvokeCallback(typeof(Il2CppFrameWalkFunc))]
    public static void Il2CppFrameWalkFuncImp(IntPtr info, IntPtr data)
    {
        string stackFrame = "";
        IntPtr method = (IntPtr)Marshal.PtrToStructure(info, typeof(IntPtr));
        string name = Marshal.PtrToStringAnsi(il2cpp_method_get_name(method));
        IntPtr klass = il2cpp_method_get_class(method);
        string klassName = Marshal.PtrToStringAnsi(il2cpp_class_get_name(klass));
        string klassNamespaze = Marshal.PtrToStringAnsi(il2cpp_class_get_namespace(klass));

        //模板类等，额外执行命名空间查找
        if(string.IsNullOrEmpty(klassNamespaze))
        {
            IntPtr declaring_type = il2cpp_class_get_declaring_type(klass);
            if (declaring_type != IntPtr.Zero)
            {
                klassNamespaze = Marshal.PtrToStringAnsi(il2cpp_class_get_namespace(declaring_type));
            }
        }

        if (!string.IsNullOrEmpty(klassNamespaze))
            stackFrame += klassNamespaze + ".";
        stackFrame += klassName + ":" + name + "(";

        //遍历参数
        int paraCount = il2cpp_method_get_param_count(method);
        for (int i = 0; i < paraCount; i++)
        {
            if (i != 0)
            {
                stackFrame += ", ";
            }
            IntPtr type = il2cpp_method_get_param(method, i);
            IntPtr typeClass = il2cpp_class_from_il2cpp_type(type);
            string typeName = Marshal.PtrToStringAnsi(il2cpp_class_get_name(typeClass));
            stackFrame += typeName;
#if UNITY_2018_1_OR_NEWER
            //il2cpp_type_is_byref可用时，直接使用它判断参数是否为引用
            bool byref = il2cpp_type_is_byref(type);
            if (byref)
            {
                stackFrame += "&";
            }
#else
            //il2cpp_type_is_byref不可用时，只能通过指针访问结构（可能不安全）
            Il2CppType typeimpl = (Il2CppType)Marshal.PtrToStructure(type, typeof(Il2CppType));
            if (typeimpl.byref())
            {
                stackFrame += "&";
            }
#endif
        }
        stackFrame += ")\n";
        stackTrace = stackFrame + stackTrace;
    }
#endif

    static string stackTrace;
    
    //stacktrace入口，默认使用StackTraceUtility.ExtractStackTrace()，需要setEnable来切换到CrashSightStackTrace
    public static string ExtractStackTrace()
    {
        if (!enable)
        {
            return StackTraceUtility.ExtractStackTrace();
        }
        stackTrace = string.Empty;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR && ENABLE_IL2CPP && ENABLE_CRASHSIGHT_STACKTRACE
        il2cpp_current_thread_walk_frame_stack(Il2CppFrameWalkFuncImp, new IntPtr());

        //有些il2cpp版本会把il2cpp_current_thread_walk_frame_stack打出来，有些则不会，判断并丢弃
        int line1 = stackTrace.IndexOf('\n');
        int line2 = stackTrace.IndexOf('\n', line1 + 1);
        if (stackTrace.Substring(0, line1) == "CrashSightStackTrace:ExtractStackTrace()")
        {
            stackTrace = stackTrace.Substring(line1 + 1);
        }
        else
        {
            stackTrace = stackTrace.Substring(line2 + 1);
        }
#endif
        return stackTrace;
    }
}
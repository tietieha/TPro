using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

// ʹ��ǰ���ȴ�ENABLE_CRASHSIGHT_STACKTRACE��
public class CrashSightStackTrace
{
    public static bool enable = false;

    //CrashSightStackTrace���أ�����il2cpp����ʱ�����
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

    //type�ṹ�壬����ֱ�Ӷ�ȡָ��ָ������ݣ��ܲ������ã�����ʹ��il2cpp��API
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

    //FrameWalk�ص���ÿ�δ���һ֡����Ϣ
    [MonoPInvokeCallback(typeof(Il2CppFrameWalkFunc))]
    public static void Il2CppFrameWalkFuncImp(IntPtr info, IntPtr data)
    {
        string stackFrame = "";
        IntPtr method = (IntPtr)Marshal.PtrToStructure(info, typeof(IntPtr));
        string name = Marshal.PtrToStringAnsi(il2cpp_method_get_name(method));
        IntPtr klass = il2cpp_method_get_class(method);
        string klassName = Marshal.PtrToStringAnsi(il2cpp_class_get_name(klass));
        string klassNamespaze = Marshal.PtrToStringAnsi(il2cpp_class_get_namespace(klass));

        //ģ����ȣ�����ִ�������ռ����
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

        //��������
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
            //il2cpp_type_is_byref����ʱ��ֱ��ʹ�����жϲ����Ƿ�Ϊ����
            bool byref = il2cpp_type_is_byref(type);
            if (byref)
            {
                stackFrame += "&";
            }
#else
            //il2cpp_type_is_byref������ʱ��ֻ��ͨ��ָ����ʽṹ�����ܲ���ȫ��
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
    
    //stacktrace��ڣ�Ĭ��ʹ��StackTraceUtility.ExtractStackTrace()����ҪsetEnable���л���CrashSightStackTrace
    public static string ExtractStackTrace()
    {
        if (!enable)
        {
            return StackTraceUtility.ExtractStackTrace();
        }
        stackTrace = string.Empty;
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR && ENABLE_IL2CPP && ENABLE_CRASHSIGHT_STACKTRACE
        il2cpp_current_thread_walk_frame_stack(Il2CppFrameWalkFuncImp, new IntPtr());

        //��Щil2cpp�汾���il2cpp_current_thread_walk_frame_stack���������Щ�򲻻ᣬ�жϲ�����
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
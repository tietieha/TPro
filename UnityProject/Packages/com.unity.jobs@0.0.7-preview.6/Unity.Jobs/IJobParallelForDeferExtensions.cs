﻿using System;
using System.Reflection;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
#if !UNITY_ZEROPLAYER
    public static class IJobParallelForDeferExtensions
    {
        internal struct ParallelForJobStruct<T> where T : struct, IJobParallelFor
        {
            public static IntPtr                            jobReflectionData;

            public static IntPtr Initialize()
            {
                if (jobReflectionData == IntPtr.Zero)
                {
                    var attribute = (JobProducerTypeAttribute)typeof(IJobParallelFor).GetCustomAttribute(typeof(JobProducerTypeAttribute));
                    var jobStruct = attribute.ProducerType.MakeGenericType(typeof(T));
                    var method = jobStruct.GetMethod("Initialize", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    var res = method.Invoke(null, new object[0]);
                    jobReflectionData = (IntPtr) res;
                }
                    
                return jobReflectionData;
            }
        }

        unsafe public static JobHandle Schedule<T, U>(this T jobData, NativeList<U> list, int innerloopBatchCount, JobHandle dependsOn = new JobHandle()) 
            where T : struct, IJobParallelFor 
            where U : struct
        {
            var scheduleParams = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), ParallelForJobStruct<T>.Initialize(), dependsOn, ScheduleMode.Parallel);
            void* atomicSafetyHandlePtr = null;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var safety = NativeListUnsafeUtility.GetAtomicSafetyHandle(ref list);
            atomicSafetyHandlePtr = UnsafeUtility.AddressOf(ref safety);
#endif
            return JobsUtility.ScheduleParallelForDeferArraySize(ref scheduleParams, innerloopBatchCount, NativeListUnsafeUtility.GetInternalListDataPtrUnchecked(ref list), atomicSafetyHandlePtr);
        }
/*
        unsafe public static void Run<T, U>(this T jobData, NativeList<U> list, int innerloopBatchCount) where T : struct, IJobParallelFor
        {
            var scheduleParams = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), ParallelForJobStruct<T>.Initialize(), new JobHandle(), ScheduleMode.Run);
            return JobsUtility.ScheduleParallelFor(ref scheduleParams, innerloopBatchCount, list.m_Buffer);
        }
*/
    }
#endif
}

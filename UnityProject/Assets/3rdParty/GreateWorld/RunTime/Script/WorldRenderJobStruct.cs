using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace BigWorldRender
{
    //用于裁剪静态数据  是否在相机范围内
    [BurstCompile]
    public struct FilterViewFrustumCullingRenderDataJob : IJobParallelForFilter
    {
        [ReadOnly] public NativeArray<int> _cullingDataNativeList;
        public NativeList<WorldStaticRenderStructData> _worldStaticRenderStructDatas;
        public NativeList<float4> _frustumPlanes;
        public int currentRenderLod;
        public float3 _regionW;
        public NativeList<int> _regionOffsetNativeArray;
        public bool Execute(int i)
        {
            unsafe
            {
                var renderDataPtr = (WorldStaticRenderStructData*)_worldStaticRenderStructDatas.GetUnsafePtr();
                var planeNormalPtr = (float4*)_frustumPlanes.GetUnsafePtr();
                var cullingDataNativeListPtr = (int*)_cullingDataNativeList.GetUnsafeReadOnlyPtr();

                int index = cullingDataNativeListPtr[i];
                float3 planeNormal = planeNormalPtr[0].xyz;

                float planeConstant = planeNormalPtr[0].w;

                WorldStaticRenderStructData renderStaticData = renderDataPtr[index];
                if (renderStaticData.renderLod < currentRenderLod)
                    return false;
                renderStaticData.offsetPos = _regionOffsetNativeArray[renderStaticData.regionRow] * _regionW;
                renderDataPtr[index] = renderStaticData;
                float3 dataPos = renderDataPtr[index].GetRenderPos();
                //int regionIndex = (int)dataPos.x/
                
                float3 pos = dataPos;
                float3 extent = new float3(renderDataPtr[index].boundRadius, renderDataPtr[index].boundRadius, renderDataPtr[index].boundRadius);
                if (math.dot(extent, math.abs(planeNormal)) +
                    math.dot(planeNormal, pos) +
                    planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[1].xyz;
                planeConstant = planeNormalPtr[1].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[2].xyz;
                planeConstant = planeNormalPtr[2].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[3].xyz;
                planeConstant = planeNormalPtr[3].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[4].xyz;
                planeConstant = planeNormalPtr[4].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[5].xyz;
                planeConstant = planeNormalPtr[5].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;
                
                

                return true;
            }

        }
    }
    //用于裁剪动态数据
    [BurstCompile]
    public struct FilterViewCullingDynamicRenderDataJob : IJobParallelForFilter
    {
        public NativeList<int> _cullingDataNativeList;
        public NativeList<WorldDynamicRenderStructData> _worldCullingColliderStructDatas;
        public NativeList<float4> _frustumPlanes;
        public bool Execute(int i)
        {
            unsafe
            {
                var renderDataPtr = (WorldDynamicRenderStructData*)_worldCullingColliderStructDatas.GetUnsafePtr();
                var planeNormalPtr = (float4*)_frustumPlanes.GetUnsafePtr();
                var cullingDataNativeListPtr = (int*)_cullingDataNativeList.GetUnsafePtr();

                int index = cullingDataNativeListPtr[i];
                float3 planeNormal = planeNormalPtr[0].xyz;

                float planeConstant = planeNormalPtr[0].w;

                float3 pos = renderDataPtr[index].GetRenderPos();
                float3 extent = new float3(renderDataPtr[index].boundRadius, renderDataPtr[index].boundRadius, renderDataPtr[index].boundRadius);
                if (math.dot(extent, math.abs(planeNormal)) +
                    math.dot(planeNormal, pos) +
                    planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[1].xyz;
                planeConstant = planeNormalPtr[1].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[2].xyz;
                planeConstant = planeNormalPtr[2].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[3].xyz;
                planeConstant = planeNormalPtr[3].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[4].xyz;
                planeConstant = planeNormalPtr[4].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[5].xyz;
                planeConstant = planeNormalPtr[5].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                return true;
            }

        }
    }


    //用于裁剪动态数据
    [BurstCompile]
    public struct FilterViewCullingColliderRenderDataJob : IJobParallelForFilter
    {
        public NativeList<int> _cullingDataNativeList;
        public NativeList<WorldColliderStructData> _worldCullingColliderStructDatas;
        public NativeList<float4> _frustumPlanes;
        public bool Execute(int i)
        {
            unsafe
            {
                var renderDataPtr = (WorldColliderStructData*)_worldCullingColliderStructDatas.GetUnsafePtr();
                var planeNormalPtr = (float4*)_frustumPlanes.GetUnsafePtr();
                var cullingDataNativeListPtr = (int*)_cullingDataNativeList.GetUnsafePtr();

                int index = cullingDataNativeListPtr[i];
                float3 planeNormal = planeNormalPtr[0].xyz;

                float planeConstant = planeNormalPtr[0].w;

                float3 pos = renderDataPtr[index].GetColliderPos();
                float boundRadius = renderDataPtr[index].GetColliderBound();
                float3 extent = new float3(boundRadius, boundRadius, boundRadius);
                if (math.dot(extent, math.abs(planeNormal)) +
                    math.dot(planeNormal, pos) +
                    planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[1].xyz;
                planeConstant = planeNormalPtr[1].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[2].xyz;
                planeConstant = planeNormalPtr[2].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[3].xyz;
                planeConstant = planeNormalPtr[3].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[4].xyz;
                planeConstant = planeNormalPtr[4].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[5].xyz;
                planeConstant = planeNormalPtr[5].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                return true;
            }

        }
    }



    [BurstCompile]
    public struct FilterViewFrustumGlobalCullingRenderDataJob : IJobParallelForFilter
    {
        public NativeList<WorldStaticRenderStructData> _worldStaticRenderStructDatas;
        public NativeList<float4> _frustumPlanes;
        public int currentRenderLod;
        public int toprenderlod;
        public bool Execute(int index)
        {
            unsafe
            {
                var renderDataPtr = (WorldStaticRenderStructData*)_worldStaticRenderStructDatas.GetUnsafePtr();
                var planeNormalPtr = (float4*)_frustumPlanes.GetUnsafePtr();

                float3 planeNormal = planeNormalPtr[0].xyz;

                float planeConstant = planeNormalPtr[0].w;

                WorldStaticRenderStructData renderStaticData = renderDataPtr[index];

                if (renderStaticData.renderLod == toprenderlod)
                {
                    if (currentRenderLod == toprenderlod)
                        return true;
                    else
                        return false;
                }
                
                if (renderStaticData.renderLod < currentRenderLod)
                    return false;
                float3 pos = renderDataPtr[index].GetRenderPos();
                float3 extent = new float3(renderDataPtr[index].boundRadius, renderDataPtr[index].boundRadius, renderDataPtr[index].boundRadius);
                if (math.dot(extent, math.abs(planeNormal)) +
                    math.dot(planeNormal, pos) +
                    planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[1].xyz;
                planeConstant = planeNormalPtr[1].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[2].xyz;
                planeConstant = planeNormalPtr[2].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[3].xyz;
                planeConstant = planeNormalPtr[3].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[4].xyz;
                planeConstant = planeNormalPtr[4].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[5].xyz;
                planeConstant = planeNormalPtr[5].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                return true;
            }

        }
    }


    [BurstCompile]
    public struct FilterViewFrustumEffectCullingRenderDataJob : IJobParallelForFilter
    {
        public NativeList<WorldParticleEffectRenderStruct> worldEffectRenderStructDataNativeList;
        public NativeList<float4> _frustumPlanes;
        public int currentRenderLod;
        public bool Execute(int index)
        {
            unsafe
            {
                var renderDataPtr = (WorldParticleEffectRenderStruct*)worldEffectRenderStructDataNativeList.GetUnsafePtr();
                var planeNormalPtr = (float4*)_frustumPlanes.GetUnsafePtr();

                float3 planeNormal = planeNormalPtr[0].xyz;

                float planeConstant = planeNormalPtr[0].w;

                WorldParticleEffectRenderStruct effectRenderData = renderDataPtr[index];


                if (effectRenderData.lod > currentRenderLod)
                    return false;
                float3 pos = renderDataPtr[index].GetPos();
                float rafius = renderDataPtr[index].GetRadius();
                float3 extent = new float3(rafius, rafius, rafius);
                if (math.dot(extent, math.abs(planeNormal)) +
                    math.dot(planeNormal, pos) +
                    planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[1].xyz;
                planeConstant = planeNormalPtr[1].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[2].xyz;
                planeConstant = planeNormalPtr[2].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[3].xyz;
                planeConstant = planeNormalPtr[3].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[4].xyz;
                planeConstant = planeNormalPtr[4].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                planeNormal = planeNormalPtr[5].xyz;
                planeConstant = planeNormalPtr[5].w;

                if (math.dot(extent, math.abs(planeNormal)) +
                        math.dot(planeNormal, pos) +
                            planeConstant <= 0f)
                    return false;

                return true;
            }

        }
    }

    [BurstCompile]
    public struct CompareNowEffectDataJob : IJobParallelFor
    {
        
        public NativeArray<WorldParticleEffectRenderStruct> lastWorldParticleEffectRenderDataNativeList;

        public NativeArray<WorldParticleEffectRenderStruct> nowWorldParticleEffectRenderDataNativeList;

        public void Execute(int index)
        {
            unsafe
            {

                var nowWorldPaticleEffectRenderDataNativeListPtr = (WorldParticleEffectRenderStruct*)nowWorldParticleEffectRenderDataNativeList.GetUnsafePtr();
                var lastWorldParticleEffectRenderDataNativeListPtr = (WorldParticleEffectRenderStruct*)lastWorldParticleEffectRenderDataNativeList.GetUnsafePtr();

                Vector3 nowPos, lastPos;
                WorldParticleEffectRenderStruct nowWorldParticleEffectRenderStruct = nowWorldPaticleEffectRenderDataNativeListPtr[index];
                nowPos = nowWorldParticleEffectRenderStruct.GetPos();
                bool isEquals = false;
                for (int lastWorldPaticleIndex = 0; lastWorldPaticleIndex < lastWorldParticleEffectRenderDataNativeList.Length; lastWorldPaticleIndex++)
                {
                    WorldParticleEffectRenderStruct lastWorldParticleEffectRenderStruct = lastWorldParticleEffectRenderDataNativeListPtr[lastWorldPaticleIndex];
                    lastPos = lastWorldParticleEffectRenderStruct.GetPos();
                    if (nowWorldParticleEffectRenderStruct.effectTypeId == lastWorldParticleEffectRenderStruct.effectTypeId
                        && nowPos == lastPos)
                    {
                        isEquals = true;
                        break;
                    }

                }
                if (!isEquals)
                {
                    nowWorldPaticleEffectRenderDataNativeListPtr[index].showState = 2;//设置为添加状态
                }
                else
                {
                    nowWorldPaticleEffectRenderDataNativeListPtr[index].showState = 3;//设置为不变状态
                }
            }
           
        }
    }
    public struct CompareLastEffectDataJob : IJobParallelFor
    {

        public NativeArray<WorldParticleEffectRenderStruct> nowWorldParticleEffectRenderDataNativeList;
        public NativeArray<WorldParticleEffectRenderStruct> lastWorldParticleEffectRenderDataNativeList;
        public void Execute(int index)
        {
            unsafe
            {

                var nowWorldPaticleEffectRenderDataNativeListPtr = (WorldParticleEffectRenderStruct*)nowWorldParticleEffectRenderDataNativeList.GetUnsafePtr();
                var lastWorldParticleEffectRenderDataNativeListPtr = (WorldParticleEffectRenderStruct*)lastWorldParticleEffectRenderDataNativeList.GetUnsafePtr();

                Vector3 nowPos, lastPos;
                WorldParticleEffectRenderStruct lastWorldParticleEffectRenderStruct = lastWorldParticleEffectRenderDataNativeListPtr[index];
                lastPos = lastWorldParticleEffectRenderStruct.GetPos();
                bool isEquals = false;
                for (int nowWorldPaticleIndex = 0; nowWorldPaticleIndex < nowWorldParticleEffectRenderDataNativeList.Length; nowWorldPaticleIndex++)
                {
                    WorldParticleEffectRenderStruct nowWorldParticleEffectRenderStruct = nowWorldPaticleEffectRenderDataNativeListPtr[nowWorldPaticleIndex];
                    nowPos = nowWorldParticleEffectRenderStruct.GetPos();
                    if (nowWorldParticleEffectRenderStruct.effectTypeId == lastWorldParticleEffectRenderStruct.effectTypeId
                        && nowPos == lastPos)
                    {
                        isEquals = true;
                        break;
                    }

                }
                if (!isEquals)
                {
                    lastWorldParticleEffectRenderDataNativeListPtr[index].showState = 1;//设置为隐藏状态
                }
                else
                {
                    lastWorldParticleEffectRenderDataNativeListPtr[index].showState = 3;//设置为不变状态
                }
            }

        }
    }

}
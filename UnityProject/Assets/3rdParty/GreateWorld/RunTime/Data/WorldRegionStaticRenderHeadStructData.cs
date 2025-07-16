//Data
//begin new data design
#region static data variable
using Unity.Mathematics;
using UnityEngine;

public struct WorldRegionStaticRenderHeadStructData
{
    //分块开始下标`
    public int beginIndex;
    //分块结束下标
    public int endIndex;
    //分块的位置
    public float3 regionPos;
    public Bounds regionBounds;
    //分块列
    public int regionX;
    //分块偏移值
    public float3 offsetPos;

}
public struct WorldStaticRenderStructData
{
    public int assetId;
    public int regionRow;
    public float3 offsetPos;
    public float4x4 renderMatrix;
    public float boundRadius;
    public int renderLod;
    public int cullingGrade;

    public float3 GetRenderPos()//(0,3)(1,3)(2,3)表示的是Position
    {
        float3 tempPos = new float3();
        Matrix4x4 tt = renderMatrix;
        tempPos.x = tt.m03;
        tempPos.y = tt.m13;
        tempPos.z = tt.m23;
        return tempPos + offsetPos;

    }
    public float4x4 GetMatrix()
    {
        Matrix4x4 tt = renderMatrix;
        tt.m03 += offsetPos.x;
        return tt;
    }
}


#endregion

#region mobile data variable

public struct WorldDynamicRenderStructData
{
    //public float3 logicPos;
    public float boundRadius;
    public float4x4 renderMatrix;
    public int assetId;
    public float3 GetRenderPos()//(0,3)(1,3)(2,3)表示的是Position
    {
        float3 tempPos = float3.zero;
        Matrix4x4 tt = renderMatrix;
        tempPos.x = tt.m03;
        tempPos.y = tt.m13;
        tempPos.z = tt.m23;
        return tempPos;

    }
}

public struct WorldColliderStructData
{
    //前三位表示位置 后一位代表半径
    public float4 colliderData;

    public float3 GetColliderPos()
    {

        return colliderData.xyz;
    }

    public float GetColliderBound()
    {
        return colliderData.w;
    }
}

#endregion

#region Particle 

public struct WorldParticleEffectRenderStruct
{
    public int id;//特效id
    public int effectTypeId;//特效类型id
    public float4 posAndRadius;//特效位置和半径
    public int lod;
    public int showState;  //特效显示状态 1 不显示状态 2添加状态 3不变状态

    public float3 GetPos()
    {
        return posAndRadius.xyz;
    }
    public float GetRadius()
    {
        return posAndRadius.w;
    }

}

#endregion
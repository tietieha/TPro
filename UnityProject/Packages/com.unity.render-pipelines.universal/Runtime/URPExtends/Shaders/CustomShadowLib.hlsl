#ifndef CUSTOM_SHADOW_LIB_INCLUDED
#define CUSTOM_SHADOW_LIB_INCLUDED


#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Shadow/ShadowSamplingTent.hlsl"


// #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Core.hlsl"


#define MAX_SHADOW_SLICE_COUNT 2
#if defined (_CUSTOM_LIGHT_SHADOWS)
    #define CUSTOM_LIGHT_CALCULATE_SHADOWS
#endif
TEXTURE2D_SHADOW(_CustomShadowMap);
SAMPLER_CMP(sampler_CustomShadowMap);

// float4x4    _CustomMainLightWorldToShadow[MAX_SHADOW_SLICE_COUNT];
// half4  _SliceDataUVOffset[MAX_SHADOW_SLICE_COUNT];

float4x4    _CustomMainLightWorldToShadow_0;
float4x4    _CustomMainLightWorldToShadow_1;
float4x4    _CustomMainLightWorldToShadow_2;
half4  _SliceDataUVOffset_0;
half4  _SliceDataUVOffset_1;
half4  _SliceDataUVOffset_2;


half4       _CustomMainLightShadowOffset0;
half4       _CustomMainLightShadowOffset1;
half4       _CustomMainLightShadowOffset2;
half4       _CustomMainLightShadowOffset3;
half4       _CustomMainLightShadowParams;  // (x: shadowStrength, y: 1.0 if soft shadows, 0.0 otherwise, z: oneOverFadeDist, w: minusStartFade)
float4      _CustomMainLightShadowmapSize; // (xy: 1/width and 1/height, zw: width and height)

// // GLES3 causes a performance regression in some devices when using CBUFFER.
// #ifndef SHADER_API_GLES3
// CBUFFER_START(CustomMainLightShadow)
// #endif
// // Last cascade is initialized with a no-op matrix. It always transforms
// // shadow coord to half3(0, 0, NEAR_PLANE). We use this trick to avoid
// // branching since ComputeCascadeIndex can return cascade index = MAX_SHADOW_CASCADES
// float4x4    _CustomMainLightWorldToShadow;

// half4       _CustomMainLightShadowOffset0;
// half4       _CustomMainLightShadowOffset1;
// half4       _CustomMainLightShadowOffset2;
// half4       _CustomMainLightShadowOffset3;
// half4       _CustomMainLightShadowParams;  // (x: shadowStrength, y: 1.0 if soft shadows, 0.0 otherwise, z: oneOverFadeDist, w: minusStartFade)
// float4      _CustomMainLightShadowmapSize; // (xy: 1/width and 1/height, zw: width and height)
// #ifndef SHADER_API_GLES3
// CBUFFER_END
// #endif



struct CustomShadowSamplingData
{
    half4 shadowOffset0;
    half4 shadowOffset1;
    half4 shadowOffset2;
    half4 shadowOffset3;
    float4 shadowmapSize;
};


float4 _CustomShadowBias;// x: depth bias, y: normal bias


#define BEYOND_SHADOW_FAR(shadowCoord) shadowCoord.z <= 0.0 || shadowCoord.z >= 1.0

CustomShadowSamplingData CustomGetMainLightShadowSamplingData()
{
    CustomShadowSamplingData shadowSamplingData;
    shadowSamplingData.shadowOffset0 = _CustomMainLightShadowOffset0;
    shadowSamplingData.shadowOffset1 = _CustomMainLightShadowOffset1;
    shadowSamplingData.shadowOffset2 = _CustomMainLightShadowOffset2;
    shadowSamplingData.shadowOffset3 = _CustomMainLightShadowOffset3;
    shadowSamplingData.shadowmapSize = _CustomMainLightShadowmapSize;
    return shadowSamplingData;
}

half4 CustomGetMainLightShadowParams()
{
    return _CustomMainLightShadowParams;
}

float4 CustomTransformWorldToShadowCoord(int index,float3 positionWS)
{
    // float4 shadowCoord = mul(_CustomMainLightWorldToShadow[index], float4(positionWS, 1.0));
    // float4 wrap = _SliceDataUVOffset[index];
    float4 shadowCoord = (float4)0;
    [branch] if(index == 0)
    {
        shadowCoord = mul(_CustomMainLightWorldToShadow_0, float4(positionWS, 1.0));
        shadowCoord.x = shadowCoord.x * _SliceDataUVOffset_0.x + _SliceDataUVOffset_0.y;
        if (shadowCoord.x < _SliceDataUVOffset_0.z || shadowCoord.x > _SliceDataUVOffset_0.w)
        {
            return half4(1, 1, 1, 0);
        }

    }
    else if(index == 1)
    {
        shadowCoord = mul(_CustomMainLightWorldToShadow_1, float4(positionWS, 1.0));
        shadowCoord.x = shadowCoord.x * _SliceDataUVOffset_1.x + _SliceDataUVOffset_1.y;
        if (shadowCoord.x < _SliceDataUVOffset_1.z || shadowCoord.x > _SliceDataUVOffset_1.w)
        {
            return half4(1, 1, 1, 0);
        }
    }
    else if(index == 2)
    {
        shadowCoord = mul(_CustomMainLightWorldToShadow_2, float4(positionWS, 1.0));
        shadowCoord.x = shadowCoord.x * _SliceDataUVOffset_2.x + _SliceDataUVOffset_2.y;
        if (shadowCoord.x < _SliceDataUVOffset_2.z || shadowCoord.x > _SliceDataUVOffset_2.w)
        {
            return half4(1, 1, 1, 0);
        }
    }

    // shadowCoord = shadowCoord + wrap;
    float cascadeIndex = 0;

    return float4(shadowCoord.xyz, cascadeIndex);
}


real CustomSampleShadowmapFiltered(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord, CustomShadowSamplingData samplingData)
{
    real attenuation;

    real4 attenuation4;
    attenuation4.x = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset0.xyz);
    attenuation4.y = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset1.xyz);
    attenuation4.z = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset2.xyz);
    attenuation4.w = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset3.xyz);
    attenuation = dot(attenuation4, 0.25);

// #if defined(SHADER_API_MOBILE) || defined(SHADER_API_SWITCH)
//     // 4-tap hardware comparison
//     real4 attenuation4;
//     attenuation4.x = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset0.xyz);
//     attenuation4.y = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset1.xyz);
//     attenuation4.z = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset2.xyz);
//     attenuation4.w = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz + samplingData.shadowOffset3.xyz);
//     attenuation = dot(attenuation4, 0.25);
// #else
    // float fetchesWeights[9];
    // float2 fetchesUV[9];
    // SampleShadow_ComputeSamples_Tent_5x5(samplingData.shadowmapSize, shadowCoord.xy, fetchesWeights, fetchesUV);

    // attenuation = fetchesWeights[0] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[0].xy, shadowCoord.z));
    // attenuation += fetchesWeights[1] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[1].xy, shadowCoord.z));
    // attenuation += fetchesWeights[2] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[2].xy, shadowCoord.z));
    // attenuation += fetchesWeights[3] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[3].xy, shadowCoord.z));
    // attenuation += fetchesWeights[4] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[4].xy, shadowCoord.z));
    // attenuation += fetchesWeights[5] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[5].xy, shadowCoord.z));
    // attenuation += fetchesWeights[6] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[6].xy, shadowCoord.z));
    // attenuation += fetchesWeights[7] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[7].xy, shadowCoord.z));
    // attenuation += fetchesWeights[8] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[8].xy, shadowCoord.z));
// #endif

    return attenuation;
}

real CustomSampleShadowmap(TEXTURE2D_SHADOW_PARAM(ShadowMap, sampler_ShadowMap), float4 shadowCoord, CustomShadowSamplingData samplingData, half4 shadowParams, bool isPerspectiveProjection = true)
{
    // Compiler will optimize this branch away as long as isPerspectiveProjection is known at compile time
    // if (isPerspectiveProjection)
    //     shadowCoord.xyz /= shadowCoord.w;

    real attenuation;
    real shadowStrength = shadowParams.x;

    // attenuation = CustomSampleShadowmapFiltered(TEXTURE2D_SHADOW_ARGS(ShadowMap, sampler_ShadowMap), shadowCoord, samplingData);


    #if defined (SHADER_API_D3D11)
    
    real fetchesWeights[9];
    real2 fetchesUV[9];
    SampleShadow_ComputeSamples_Tent_5x5(samplingData.shadowmapSize, shadowCoord.xy, fetchesWeights, fetchesUV);
    
    attenuation = fetchesWeights[0] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[0].xy, shadowCoord.z));
    attenuation += fetchesWeights[1] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[1].xy, shadowCoord.z));
    attenuation += fetchesWeights[2] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[2].xy, shadowCoord.z));
    attenuation += fetchesWeights[3] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[3].xy, shadowCoord.z));
    attenuation += fetchesWeights[4] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[4].xy, shadowCoord.z));
    attenuation += fetchesWeights[5] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[5].xy, shadowCoord.z));
    attenuation += fetchesWeights[6] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[6].xy, shadowCoord.z));
    attenuation += fetchesWeights[7] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[7].xy, shadowCoord.z));
    attenuation += fetchesWeights[8] * SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, float3(fetchesUV[8].xy, shadowCoord.z));
    
    #else
    attenuation = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz);
    
    #endif
   
    
    // TODO: We could branch on if this light has soft shadows (shadowParams.y) to save perf on some platforms.
// #ifdef _SHADOWS_SOFT
//     attenuation = CustomSampleShadowmapFiltered(TEXTURE2D_SHADOW_ARGS(ShadowMap, sampler_ShadowMap), shadowCoord, samplingData);
// #else
//     // 1-tap hardware comparison
//     attenuation = SAMPLE_TEXTURE2D_SHADOW(ShadowMap, sampler_ShadowMap, shadowCoord.xyz);
// #endif

    attenuation = LerpWhiteTo(attenuation, shadowStrength);

    // Shadow coords that fall out of the light frustum volume must always return attenuation 1.0
    // TODO: We could use branch here to save some perf on some platforms.

    // return attenuation;
    return BEYOND_SHADOW_FAR(shadowCoord) ? 1.0 : attenuation;
}
float3 CustomApplyShadowBias(float3 positionWS, float3 normalWS, float3 lightDirection)
{
    float invNdotL = 1.0 - saturate(dot(lightDirection, normalWS));
    float scale = invNdotL * _CustomShadowBias.y;

    // normal bias is negative since we want to apply an inset normal offset
    positionWS = lightDirection * _CustomShadowBias.xxx + positionWS;
    positionWS = normalWS * scale.xxx + positionWS;
    return positionWS;
}


half CustomMainLightRealtimeShadow(float4 shadowCoord)
{
    #if !defined(CUSTOM_LIGHT_CALCULATE_SHADOWS)
        return 1.0h;
    #endif
    CustomShadowSamplingData shadowSamplingData = CustomGetMainLightShadowSamplingData();
    half4 shadowParams = CustomGetMainLightShadowParams();
    return CustomSampleShadowmap(TEXTURE2D_ARGS(_CustomShadowMap, sampler_CustomShadowMap), shadowCoord, shadowSamplingData, shadowParams, false);
}

float4 CustomGetShadowCoords(VertexPositionInputs vertexInput, half index, float3 worldPos)
{
    float4 ShadowCoords;
    #if defined(ENABLE_CUSTOM_SHADOW)
        ShadowCoords = CustomTransformWorldToShadowCoord(index, worldPos);
    #else
        ShadowCoords = GetShadowCoord(vertexInput);
    #endif

    return ShadowCoords;
}

float4 CustomGetShadowCoords(half index, float3 worldPos)
{
    float4 ShadowCoords;
    #if defined(ENABLE_CUSTOM_SHADOW)
        ShadowCoords = CustomTransformWorldToShadowCoord(index, worldPos);
    #else
        ShadowCoords = TransformWorldToShadowCoord(worldPos);
    #endif

    return ShadowCoords;
}

void GetMainLightColorDirAndLightAtten(in float4 ShadowCoords,out half3 MainLightColor, out float lightAtten, out float3 lightDir)
{
    
    lightAtten = 1;

    #if defined(ENABLE_CUSTOM_SHADOW)
        Light mainLight = GetMainLight();
        lightDir = normalize(mainLight.direction);
        lightAtten = CustomMainLightRealtimeShadow(ShadowCoords);
    #else
        Light mainLight = GetMainLight(ShadowCoords);
        lightDir = normalize(mainLight.direction);
        lightAtten = mainLight.shadowAttenuation;
        // half3 attenColor = lightAtten * mainLight.color;
    #endif

    MainLightColor = mainLight.color.rgb;
}


half CustomGetAllShadow(float3 WorldPos)
{
    float4 ShadowCoords;
    half shadowAtten = 1;
    #if defined(_CUSTOMSHADOWMAPCOUNT_1)
    
        ShadowCoords = CustomTransformWorldToShadowCoord(0, WorldPos);
        shadowAtten = CustomMainLightRealtimeShadow(ShadowCoords);

    #elif (_CUSTOMSHADOWMAPCOUNT_2)
        ShadowCoords = CustomTransformWorldToShadowCoord(0, WorldPos);
        shadowAtten = CustomMainLightRealtimeShadow(ShadowCoords);
        
        ShadowCoords = CustomTransformWorldToShadowCoord(1, WorldPos);
        shadowAtten = min(CustomMainLightRealtimeShadow(ShadowCoords), shadowAtten);
    #elif (_CUSTOMSHADOWMAPCOUNT_3)
        
        ShadowCoords = CustomTransformWorldToShadowCoord(0, WorldPos);
        shadowAtten = CustomMainLightRealtimeShadow(ShadowCoords);
        
        ShadowCoords = CustomTransformWorldToShadowCoord(1, WorldPos);
        shadowAtten = min(CustomMainLightRealtimeShadow(ShadowCoords), shadowAtten);
        
        ShadowCoords = CustomTransformWorldToShadowCoord(2, WorldPos);
        shadowAtten = min(CustomMainLightRealtimeShadow(ShadowCoords), shadowAtten);
    #endif

    return shadowAtten;
}

#endif
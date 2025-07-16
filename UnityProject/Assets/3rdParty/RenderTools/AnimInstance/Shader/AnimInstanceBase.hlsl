#ifndef ANIM_INSTANCING_BASE
    #define ANIM_INSTANCING_BASE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    //#pragma multi_compile_instancing


    sampler2D _AnimMap;
    float4 _AnimMap_TexelSize;
    int _PixelCountPerFrame;

    struct a2vAnim
    {
        float4 vertex       : POSITION;
        float3 normal       : NORMAL;
        float4 texcoord     : TEXCOORD0;
        float4 texcoord1    : TEXCOORD1;
        half4 boneIndex    : TEXCOORD2;
        half4 boneWeight   : TEXCOORD3;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    #if (SHADER_TARGET < 30 || SHADER_API_GLES)
        float4 _AnimTimeInfo;
        float4 _AnimInfo;
        half4 _InnerGlowColor;
        half _InnerGlowIntensity;
    #else
        UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _AnimTimeInfo)
            #define _AnimTimeInfo_arr Props
            
            UNITY_DEFINE_INSTANCED_PROP(float4, _AnimInfo)
            #define _AnimInfo_arr Props

            UNITY_DEFINE_INSTANCED_PROP(half4, _InnerGlowColor)
            #define _InnerGlowColor_arr Props
            UNITY_DEFINE_INSTANCED_PROP(half, _InnerGlowIntensity)
            #define _InnerGlowIntensity_arr Props
        UNITY_INSTANCING_BUFFER_END(Props)
    #endif

    float4 GetUV(int index)
    {
        int row = (int)(index / _AnimMap_TexelSize.z);
        int col = (int)(index % _AnimMap_TexelSize.z);
        return float4(col / _AnimMap_TexelSize.z, row / _AnimMap_TexelSize.w, 0, 0);
    }

    float4x4 GetMatrix(int startIndex, float boneIndex)
    {
        int matrixIndex = startIndex + boneIndex * 3;

        float4 row0 = tex2Dlod(_AnimMap, GetUV(matrixIndex));
        float4 row1 = tex2Dlod(_AnimMap, GetUV(matrixIndex + 1));
        float4 row2 = tex2Dlod(_AnimMap, GetUV(matrixIndex + 2));
        float4 row3 = float4(0, 0, 0, 1);

        return float4x4(row0, row1, row2, row3);
    }
    
    void skinning(half4 vertex, half4 boneIndex, half4 boneWeight, out half4 positionOS)
    {
        #if defined(_ANIMINSTANCE_ON)
            #if (SHADER_TARGET < 30 || SHADER_API_GLES)
                int loop = _AnimInfo.x;
                float speed = _AnimInfo.y;
                int startFrame = _AnimInfo.z;
                int frameCount = _AnimInfo.w;
                float startTime = _AnimTimeInfo.x;
                float offsetSeconds = _AnimTimeInfo.y;
            #else
                float4 animInfo = UNITY_ACCESS_INSTANCED_PROP(_AnimInfo_arr, _AnimInfo);
                float4 animTimeInfo = UNITY_ACCESS_INSTANCED_PROP(_AnimTimeInfo_arr, _AnimTimeInfo);
                    
                int loop = animInfo.x;
                float speed = animInfo.y;
                int startFrame = animInfo.z;
                int frameCount = animInfo.w;
                float startTime = animTimeInfo.x;
                float offsetSeconds = animTimeInfo.y;
            #endif
            
            int offsetFrame = (int)((_Time.y - startTime + offsetSeconds) * 30 * speed);
            int ret = step(frameCount, offsetFrame);
            offsetFrame = lerp(offsetFrame, 
                                lerp(frameCount - 1, offsetFrame, loop), 
                                ret);

            int currentFrame = startFrame + offsetFrame % frameCount;
            int clampedIndex = currentFrame * _PixelCountPerFrame;

            float4x4 bone1Matrix = GetMatrix(clampedIndex, boneIndex.x);
            float4x4 bone2Matrix = GetMatrix(clampedIndex, boneIndex.y);
            float4x4 bone3Matrix = GetMatrix(clampedIndex, boneIndex.z);
            float4x4 bone4Matrix = GetMatrix(clampedIndex, boneIndex.w);

            positionOS =
                mul(bone1Matrix, vertex) * boneWeight.x +
                mul(bone2Matrix, vertex) * boneWeight.y +
                mul(bone3Matrix, vertex) * boneWeight.z +
                mul(bone4Matrix, vertex) * boneWeight.w;
        #else
            positionOS = vertex;
        #endif
    }

    void skinning(half4 vertex, half3 normal3, half4 boneIndex, half4 boneWeight, out half4 positionOS, out half3 normalOS)
    {

        #if defined(_ANIMINSTANCE_ON)
            half4 normal = half4(normal3, 0);
            #if (SHADER_TARGET < 30 || SHADER_API_GLES)
                int loop = _AnimInfo.x;
                float speed = _AnimInfo.y;
                int startFrame = _AnimInfo.z;
                int frameCount = _AnimInfo.w;
                float startTime = _AnimTimeInfo.x;
                float offsetSeconds = _AnimTimeInfo.y;
            #else
                float4 animInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _AnimInfo);
                float4 animTimeInfo = UNITY_ACCESS_INSTANCED_PROP(Props, _AnimTimeInfo);
                
                int loop = animInfo.x;
                float speed = animInfo.y;
                int startFrame = animInfo.z;
                int frameCount = animInfo.w;
                float startTime = animTimeInfo.x;
                float offsetSeconds = animTimeInfo.y;
            #endif

            int offsetFrame = (int)((_Time.y - startTime + offsetSeconds) * 30 * speed);
            int ret = step(frameCount, offsetFrame);
            offsetFrame = lerp(offsetFrame, lerp(frameCount - 1, offsetFrame, loop), ret);
    
            int currentFrame = startFrame + offsetFrame % frameCount;
            int clampedIndex = currentFrame * _PixelCountPerFrame;

            float4x4 bone1Matrix = GetMatrix(clampedIndex, boneIndex.x);
            float4x4 bone2Matrix = GetMatrix(clampedIndex, boneIndex.y);
            float4x4 bone3Matrix = GetMatrix(clampedIndex, boneIndex.z);
            float4x4 bone4Matrix = GetMatrix(clampedIndex, boneIndex.w);

            positionOS =
                mul(bone1Matrix, vertex) * boneWeight.x +
                mul(bone2Matrix, vertex) * boneWeight.y +
                mul(bone3Matrix, vertex) * boneWeight.z +
                mul(bone4Matrix, vertex) * boneWeight.w;

            half4 normalOS1 =
                mul(bone1Matrix, normal) * boneWeight.x +
                mul(bone2Matrix, normal) * boneWeight.y +
                mul(bone3Matrix, normal) * boneWeight.z +
                mul(bone4Matrix, normal) * boneWeight.w;
            normalOS = normalOS1.xyz;
        #else
            positionOS = vertex;
            normalOS = normal3;
        #endif
    }

#endif

Shader "Terrain/IndexBlend16atlas"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]DiffuseAtlas ("Diffuse Atlas", 2D) = "white" {}
        [NoScaleOffset]NormalAtlas ("Normal Atlas", 2D) = "normal" {}
        [NoScaleOffset]IndexTex ("IndexTex Map (RGBA)", 2D) = "white" {}
        [NoScaleOffset]BlendTex ("BlendTex (RGBA)", 2D) = "white" {}
        
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "Queue"="Geometry-10" "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "ForwardLit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _NORMALMAP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"


            TEXTURE2D(DiffuseAtlas);
            SAMPLER(sampler_DiffuseAtlas);
            TEXTURE2D(NormalAtlas);
            SAMPLER(sampler_NormalAtlas);

            TEXTURE2D(IndexTex);
            SAMPLER(sampler_IndexTex);

            TEXTURE2D(BlendTex);
            SAMPLER(sampler_BlendTex);
            
            float4 _UVScales[16];
            float4 _LowTerrainTileSize;

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 positionWS : TEXCOORD1;

                float3 normalWS : TEXCOORD2;
                half4 tangentWS : TEXCOORD3;
                float4 shadowCoord: TEXCOORD4;
                float3 vertexSH   : TEXCOORD5;
            };

            void SplatmapMix(v2f i, out half4 mixedDiffuse, out half3 mixedNormal)
            {                
                float4 splat_control = SAMPLE_TEXTURE2D(IndexTex, sampler_IndexTex, i.uv.xy);

                float clipSize = 512;
                int clipCount = 4; //4x4 16张的图集

                float2 initScale = (i.uv.xy * 100); //terrain Size/ tile scale


                float space = 1.0 / clipSize;
                float clipRepeatWid = (1.0 / clipCount - 2.0 * space);

                float2 dx = clamp(clipRepeatWid * ddx(initScale), -1.0 / clipCount / 2, 1.0 / clipCount / 2);
                float2 dy = clamp(clipRepeatWid * ddy(initScale), -1.0 / clipCount / 2, 1.0 / clipCount / 2);
                // int mipmap = (int)(0.5 + log2(max(sqrt(dot(dx, dx)), sqrt(dot(dy, dy))) * clipSize));
                int mipmap = 0;
                space = (pow(2.0, mipmap) - 0.5) / clipSize;
                clipRepeatWid = (1.0 / clipCount - 2.0 * space);
                float2 initUVAlbedo = clipRepeatWid * frac(initScale) + space;

                float2 dxSplat = clamp(0.5 * ddx(i.uv.xy), -1.0 / clipSize / 2, 1.0 / clipSize / 2);
                float2 dySplat = clamp(0.5 * ddy(i.uv.xy), -1.0 / clipSize / 2, 1.0 / clipSize / 2);
                
                float2 dxBound = 1.0 / clipCount / 2;

                int idr = (int)(splat_control.r * 16 + 0.5);
                float2 uvR = clipRepeatWid * frac(i.uv.zw * _UVScales[idr].xy) + space + float2(idr % clipCount, idr / clipCount) / clipCount;
                half3 colorR = SAMPLE_TEXTURE2D_GRAD(DiffuseAtlas, sampler_DiffuseAtlas, uvR, 
                    clamp(clipRepeatWid * ddx(i.uv.zw * _UVScales[idr].xy), -dxBound, dxBound), clamp(clipRepeatWid * ddy(i.uv.zw * _UVScales[idr].xy), -dxBound, dxBound));
                // float weightR = SAMPLE_TEXTURE2D_GRAD(BlendTex, sampler_BlendTex,
                //                   i.uv.xy * 0.5 + float2((idr / 4) % 2, idr / 8) * 0.5, dxSplat,
                //                   dySplat)[idr % 4];

                int idg = (int)(splat_control.g * 16 + 0.5);
                float2 uvG = clipRepeatWid * frac(i.uv.zw * _UVScales[idg].xy) + space + float2(idg % clipCount, idg / clipCount) / clipCount;
                half3 colorG = SAMPLE_TEXTURE2D_GRAD(DiffuseAtlas, sampler_DiffuseAtlas, uvG, 
                    clamp(clipRepeatWid * ddx(i.uv.zw * _UVScales[idg].xy), -dxBound, dxBound), clamp(clipRepeatWid * ddy(i.uv.zw * _UVScales[idg].xy), -dxBound, dxBound));
                float weightG = SAMPLE_TEXTURE2D_GRAD(BlendTex, sampler_BlendTex,
                                                      i.uv.xy * 0.5 + float2((idg / 4) % 2, idg / 8) * 0.5, dxSplat,
                                                      dySplat)[idg % 4];

                int idb = (int)(splat_control.b * 16 + 0.5);
                float2 uvB = clipRepeatWid * frac(i.uv.zw * _UVScales[idb].xy) + space + float2(idb % clipCount, idb / clipCount) / clipCount;
                half3 colorB = SAMPLE_TEXTURE2D_GRAD(DiffuseAtlas, sampler_DiffuseAtlas, uvB, 
                    clamp(clipRepeatWid * ddx(i.uv.zw * _UVScales[idb].xy), -dxBound, dxBound), clamp(clipRepeatWid * ddy(i.uv.zw * _UVScales[idb].xy), -dxBound, dxBound));
                float weightB = SAMPLE_TEXTURE2D_GRAD(BlendTex, sampler_BlendTex,
                                                      i.uv.xy * 0.5 + float2((idb / 4) % 2, idb / 8) * 0.5, dxSplat,
                                                      dySplat)[idb % 4];

                mixedDiffuse.rgb = colorR * (1 - weightG - weightB) + colorG * weightG + colorB * weightB;
                // float totalWeight = weightR + weightG + weightB;
                // mixedDiffuse.rgb = colorR * (weightR / totalWeight) + colorG * (weightG / totalWeight) + colorB * (weightB / totalWeight);
                mixedDiffuse.a = 1;

                half4 nrm = lerp(SAMPLE_TEXTURE2D_GRAD(NormalAtlas, sampler_NormalAtlas, uvG, dx, dy),
                                 SAMPLE_TEXTURE2D_GRAD(NormalAtlas, sampler_NormalAtlas, uvR, dx, dy),
                                 1 - weightG - weightB);

                mixedNormal = UnpackNormal(nrm);
            }

            v2f vert(a2v v)
            {
                v2f o;
                float3 worldPos = TransformObjectToWorld(v.vertex);
                o.vertex = TransformWorldToHClip(worldPos);
                o.uv.xy = v.uv;
                
                o.uv.zw = frac((worldPos.xz - _LowTerrainTileSize.zw) / (_LowTerrainTileSize.xy) * float2(1, -1));

                o.positionWS.xyz = worldPos;
                o.shadowCoord = TransformWorldToShadowCoord(worldPos);
                
                real sign = v.tangentOS.w * GetOddNegativeScale();
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.tangentWS = half4(real3(TransformObjectToWorldDir(v.tangentOS.xyz)), sign);

                o.vertexSH = SampleSHVertex(o.normalWS);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Light MainLight = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                // Light MainLight = GetMainLight(i.shadowCoord);
                // return MainLight.shadowAttenuation;
                
                float4 mixedDiffuse;
                float3 normalTS;
                SplatmapMix(i, mixedDiffuse, normalTS);
                
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.alpha = 1;
                surfaceData.albedo = mixedDiffuse;                
                surfaceData.normalTS = normalTS;
                surfaceData.occlusion = 1;

                InputData inputData = (InputData)0;
                // #if defined(_NORMALMAP)
                    float sgn = i.tangentWS.w;      // should be either +1 or -1
                    float3 bitangent = sgn * cross(i.normalWS.xyz, i.tangentWS.xyz);
                    half3x3 tangentToWorld = half3x3(i.tangentWS.xyz, bitangent.xyz, i.normalWS.xyz);
                    inputData.tangentToWorld = tangentToWorld;

                    inputData.normalWS = TransformTangentToWorld(normalTS, tangentToWorld);
                // #else
                //     inputData.normalWS = i.normalWS;
                // #endif
                inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(i.positionWS);
                inputData.bakedGI = SampleSHPixel(i.vertexSH, inputData.normalWS);
                inputData.positionWS = i.positionWS;
                
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    inputData.shadowCoord = i.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    inputData.shadowCoord = TransformWorldToShadowCoord(i.positionWS);
                #else
                    inputData.shadowCoord = float4(0, 0, 0, 0);
                #endif

                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                // color.rgb = inputData.normalWS;
                // color.rgb = mixedDiffuse;
                return color;
            }
            ENDHLSL
        }

        //        Pass
        //        {
        //            Name "ShadowCaster"
        //            Tags{"LightMode" = "ShadowCaster"}
        //
        //            ZWrite On
        //            ZTest LEqual
        //            ColorMask 0
        //            Cull[_Cull]
        //
        //            HLSLPROGRAM
        //            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
        //            // #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        //
        //            #pragma vertex ShadowPassVertex
        //            #pragma fragment ShadowPassFragment
        //
        //            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
        //            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
        //            ENDHLSL
        //        }
        //        
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            // #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap

            #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Terrain/TerrainLitPasses.hlsl"
            ENDHLSL
        }
    }
}
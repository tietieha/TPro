Shader "URP/Tools/ShadowCaster"
{
    Properties
	{
        _MainTex ("Main Tex", 2D) = "white" {}
        _Basemap ("Base map", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
			Name "ShadowCaster"

			ZWrite On
            ZTest LEqual
            ColorMask 0

			HLSLPROGRAM
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
			
			#pragma vertex vert_shadow
			#pragma fragment frag_shadow

			// #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Assets/GameAssets/Shaders/URP/CommonLib/ShadowLib.hlsl"
			struct a2v_shadow
			{
				float4 vertex		: POSITION;
				float3 normal		: NORMAL;

			    UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f_shadow
			{
				float4 positionCS	: SV_POSITION;

			    UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			float3 _LightDirection;

			v2f_shadow vert_shadow ( a2v_shadow v )
			{
				v2f_shadow o = (v2f_shadow)0;

			    UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldDir(v.normal);

				float4 shadowData = GetShadowData();
				float4 positionCS = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection,shadowData.x,shadowData.y ) );

				#if UNITY_REVERSED_Z
					positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
				#else
					positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
				#endif

				o.positionCS = positionCS;

				return o;
			}

			half4 frag_shadow ( v2f_shadow i ) : SV_TARGET
			{
			    UNITY_SETUP_INSTANCE_ID(i);
				return 0;
			}

			ENDHLSL
		}
    }
}

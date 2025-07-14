Shader "URP/SD_CustomShadowCaster"
{
    Properties { }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM

            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            // #pragma vertex CustomShadowPassVertex
            // #pragma fragment CustomShadowPassFragment
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
			// #include "Assets/HeroRes/ArtContent/TAZone/TAWork/CustomShadow/CustomShadowLib.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"


            ENDHLSL

        }
    }
  
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}

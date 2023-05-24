Shader "Custom/Simple"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend("Src Blend", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend("Dst Blend", Int) = 0
        [Enum(UnityEngine.Rendering.CullMode)]_Cull("Cull", Int) = 2
        [Toggle]_ZWrite("ZWrite", Int) = 1

        [MainColor]_Color("Color", Color) = (1, 1, 1, 1)
    }
    
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "Forward" }
            
            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]
            Cull [_Cull]
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "./ForwardPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "ShadowCaster" }
            
            ColorMask 0
            Cull [_Cull]
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "./ShadowCasterPass.hlsl"

            ENDHLSL
        }
    }
}
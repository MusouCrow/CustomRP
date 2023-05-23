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

            cbuffer UnityPerDraw {
                float4x4 unity_ObjectToWorld;
                float4x4 unity_WorldToObject;
                float4 unity_LODFade;
                half4 unity_WorldTransformParams;
            };

            float4x4 unity_MatrixVP;

            cbuffer UnityPerMaterial {
                float4 _Color;
            };
            
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                float4 positionWS = mul(unity_ObjectToWorld, input.positionOS);
                output.positionCS = mul(unity_MatrixVP, positionWS);

                return output;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                return _Color;
            }

            ENDHLSL
        }
    }
}
Shader "Custom/Simple"
{
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "Forward" }

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
                return 1;
            }

            ENDHLSL
        }
    }
}
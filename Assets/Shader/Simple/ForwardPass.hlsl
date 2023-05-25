#pragma once

#include "./Include.hlsl"
#include "Assets/Source/Render/ShaderLibrary/Shadow.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float3 positionWS : TEXCOORD0;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings Vert(Attributes input)
{
    Varyings output;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    float3 positionWS = TransformObjectToWorld(input.positionOS);
    output.positionCS = TransformWorldToHClip(positionWS);
    output.positionWS = positionWS;

    return output;
}

float4 Frag(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);

    float4 color = _Color;
    float shadowAttenuation = ShadowAttenuation(input.positionWS);
    color.rgb *= shadowAttenuation;

    return color;
}

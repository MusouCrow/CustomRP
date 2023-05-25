#pragma once

#include "./Include.hlsl"
#include "Assets/Source/Render/ShaderLibrary/Shadow.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float4 positionWS : TEXCOORD0;
};

Varyings Vert(Attributes input)
{
    Varyings output;
    float4 positionWS = TransformObjectToWorld(input.positionOS);
    output.positionCS = TransformWorldToHClip(positionWS);
    output.positionWS = positionWS;

    return output;
}

float4 Frag(Varyings input) : SV_Target
{
    float shadowAttenuation = ShadowAttenuation(input.positionWS);

    return _Color;
}

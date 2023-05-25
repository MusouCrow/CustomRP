#pragma once

#include "./Include.hlsl"
#include "Assets/Source/Render/ShaderLibrary/Shadow.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;

    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
};

Varyings Vert(Attributes input)
{
    Varyings output;

    UNITY_SETUP_INSTANCE_ID(input);
    
    float3 normalWS = TransformObjectToWorldNormal(input.positionOS);
    float3 positionWS = TransformObjectToWorld(input.positionOS);
    positionWS = ApplyShadowBias(positionWS, normalWS);
    
    float4 positionCS = TransformWorldToHClip(positionWS);

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif

    output.positionCS = positionCS;

    return output;
}

float4 Frag(Varyings input) : SV_Target
{
    return 0;
}

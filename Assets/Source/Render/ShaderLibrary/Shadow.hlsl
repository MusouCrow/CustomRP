#pragma once

#include "./Input.hlsl"

TEXTURE2D_SHADOW(_ShadowTexture);
SAMPLER_CMP(sampler_ShadowTexture);

CBUFFER_START(MainLightShadows)
float4x4 _WorldToShadowMatrix;
CBUFFER_END

float SampleShadowMap(float4 shadowCoord)
{
    return SAMPLE_TEXTURE2D_SHADOW(_ShadowTexture, sampler_ShadowTexture, shadowCoord.xyz);
}

float4 TransformWorldToShadowCoord(float4 positionWS)
{
    return mul(_WorldToShadowMatrix, positionWS);
}

float ShadowAttenuation(float4 positionWS)
{
    float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
    
    return SampleShadowMap(shadowCoord);
}

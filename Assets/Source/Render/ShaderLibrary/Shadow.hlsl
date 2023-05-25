#pragma once

#include "./Input.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"

TEXTURE2D_SHADOW(_ShadowTexture);
SAMPLER_CMP(sampler_ShadowTexture);

CBUFFER_START(MainLightShadows)
float4x4 _WorldToShadowMatrix;
float3 _LightDirection;
float4 _ShadowParam; // x: Depth Bias, y: Normal Bias, z: Shadow Strength, w: Is Soft Shadow
CBUFFER_END

float SampleShadowMap(float4 shadowCoord)
{
    return SAMPLE_TEXTURE2D_SHADOW(_ShadowTexture, sampler_ShadowTexture, shadowCoord.xyz);
}

float4 TransformWorldToShadowCoord(float3 positionWS)
{
    return mul(_WorldToShadowMatrix, float4(positionWS, 1.0));
}

float ShadowAttenuation(float3 positionWS)
{
    float4 shadowCoord = TransformWorldToShadowCoord(positionWS);
    float attenuation = SampleShadowMap(shadowCoord);
    attenuation = LerpWhiteTo(attenuation, _ShadowParam.z);
    
    return attenuation;
}

float3 ApplyShadowBias(float3 positionWS, float3 normalWS)
{
    float invNdotL = 1.0 - saturate(dot(_LightDirection, normalWS));
    float scale = invNdotL * _ShadowParam.y;
    
    positionWS = _LightDirection * _ShadowParam.xxx + positionWS;
    positionWS = normalWS * scale.xxx + positionWS;
    
    return positionWS;
}


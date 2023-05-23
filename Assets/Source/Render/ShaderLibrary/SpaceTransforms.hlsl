#pragma once

#include "./Input.hlsl"

float4 TransformObjectToWorld(float4 positionOS)
{
    return mul(unity_ObjectToWorld, positionOS);
}

float4 TransformWorldToHClip(float4 positionWS)
{
    return mul(unity_MatrixVP, positionWS);
}

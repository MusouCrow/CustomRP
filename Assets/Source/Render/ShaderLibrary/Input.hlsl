#pragma once

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

CBUFFER_START(UnityPerDraw)
float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObject;
float4 unity_LODFade;
half4 unity_WorldTransformParams;
CBUFFER_END

float4x4 unity_MatrixVP;

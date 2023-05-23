#pragma once

cbuffer UnityPerDraw {
    float4x4 unity_ObjectToWorld;
    float4x4 unity_WorldToObject;
    float4 unity_LODFade;
    half4 unity_WorldTransformParams;
};

float4x4 unity_MatrixVP;

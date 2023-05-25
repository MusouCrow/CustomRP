using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public class MainLightShadowPass : IRenderPass {
        private LaviRenderPipelineAsset asset;

        public MainLightShadowPass(LaviRenderPipelineAsset asset) {
            this.asset = asset;
        }

        public void Render(ref ScriptableRenderContext context, ref RenderData data) {
            if (!this.IsActive(ref data)) {
                return;
            }

            var cmd = CommandBufferPool.Get("MainLightShadowPass");

            int shadowResolution = (int)this.asset.ShadowResolution;
            var rti = this.ReadyTexture(cmd, ref context, ref data, shadowResolution);

            var light = data.cullingResults.visibleLights[0];
            data.cullingResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(0, 0, 1, new Vector3(1, 0, 0), shadowResolution, light.light.shadowNearPlane,
            out var viewMatrix, out var projMatrix, out var shadowSplitData);
            
            var viewport = new Rect(0, 0, shadowResolution, shadowResolution);
            cmd.SetViewport(viewport);

            var scissor = new Rect(viewport.x + 4, viewport.y + 4, viewport.width - 8, viewport.height - 8);
            cmd.EnableScissorRect(scissor);

            cmd.SetViewProjectionMatrices(viewMatrix, projMatrix);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            var shadowSettings = new ShadowDrawingSettings(data.cullingResults, 0);
            shadowSettings.splitData = shadowSplitData;

            var tileMatrix = Matrix4x4.identity;
            tileMatrix.m00 = tileMatrix.m11 = 0.5f;
            tileMatrix.m03 = tileMatrix.m13 = 0;
            var worldToShadowMatrix = this.CalculateWorldToShadowMatrix(ref viewMatrix, ref projMatrix);
            // worldToShadowMatrix = tileMatrix * worldToShadowMatrix;

            context.DrawShadows(ref shadowSettings);

            cmd.DisableScissorRect();
            cmd.SetGlobalTexture(RenderConst.SHADOW_TEXTURE_ID, rti);
            cmd.SetGlobalMatrix(RenderConst.WORLD_TO_SHADOW_MTX_ID, worldToShadowMatrix);
            context.ExecuteCommandBuffer(cmd);
            context.SetupCameraProperties(data.camera);
            cmd.Clear();
            
            CommandBufferPool.Release(cmd);
        }

        public void Clean(ref ScriptableRenderContext context, ref RenderData data) {
            if (!this.IsActive(ref data)) {
                return;
            }

            var cmd = CommandBufferPool.Get("MainLightShadowPass");
            var tid = RenderConst.SHADOW_TEXTURE_ID;
            cmd.ReleaseTemporaryRT(tid);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        private bool IsActive(ref RenderData data) {
            if (data.cullingResults.visibleLights.Length == 0) {
                return false;
            }

            var first = data.cullingResults.visibleLights[0];

            if (first.lightType != LightType.Directional) {
                return false;
            }
            else if (first.light.shadows == LightShadows.None) {
                return false;
            }

            return true;
        }

        private RenderTargetIdentifier ReadyTexture(CommandBuffer cmd, ref ScriptableRenderContext context, ref RenderData data, int size) {
            var tid = RenderConst.SHADOW_TEXTURE_ID;
            var rtd = this.CreateRenderTextureDescriptor(size);
            var rti = new RenderTargetIdentifier(tid);

            cmd.GetTemporaryRT(tid, rtd, FilterMode.Bilinear);
            cmd.SetRenderTarget(rti, 
                RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                RenderBufferLoadAction.Load, RenderBufferStoreAction.Store
            );
            cmd.ClearRenderTarget(true, true, data.camera.backgroundColor.linear);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            return rti;
        }

        private RenderTextureDescriptor CreateRenderTextureDescriptor(int size) {
            var rtd = new RenderTextureDescriptor(size, size, RenderTextureFormat.Shadowmap, 16);

            return rtd;
        }

        private Matrix4x4 CalculateWorldToShadowMatrix(ref Matrix4x4 viewMatrix, ref Matrix4x4 projMatrix) {
            if (SystemInfo.usesReversedZBuffer) {
                projMatrix.m20 -= projMatrix.m20;
                projMatrix.m21 -= projMatrix.m21;
                projMatrix.m22 -= projMatrix.m22;
                projMatrix.m23 -= projMatrix.m23;
            }

            // [-1, 1] -> [0, 1]
            var scaleOffset = Matrix4x4.identity;
            scaleOffset.m00 = scaleOffset.m11 = scaleOffset.m22 = 0.5f;
		    scaleOffset.m03 = scaleOffset.m13 = scaleOffset.m23 = 0.5f;

            return scaleOffset * (projMatrix * viewMatrix);
        }
    }
}
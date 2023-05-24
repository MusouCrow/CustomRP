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
            this.ReadyTexture(cmd, ref context, ref data, shadowResolution);

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

            context.DrawShadows(ref shadowSettings);

            cmd.DisableScissorRect();
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

        private void ReadyTexture(CommandBuffer cmd, ref ScriptableRenderContext context, ref RenderData data, int size) {
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
        }

        private RenderTextureDescriptor CreateRenderTextureDescriptor(int size) {
            var rtd = new RenderTextureDescriptor(size, size, RenderTextureFormat.Shadowmap, 16);

            return rtd;
        }
    }
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public class MainLightShadowPass : IRenderPass {
        public void Render(ref ScriptableRenderContext context, ref RenderData data) {
            var cmd = CommandBufferPool.Get("MainLightShadowPass");
            this.ReadyTexture(cmd, ref context, ref data);

            var light = data.cullingResults.visibleLights[0];

            data.cullingResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(0, 0, 1, new Vector3(1, 0, 0), 1024, light.light.shadowNearPlane,
            out var viewMatrix, out var projMatrix, out var shadowSplitData);
            
            /*
            cmd.SetViewProjectionMatrices(viewMatrix, projMatrix);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            */
            
            /*
            var drawingSettings = RenderUtil.CreateDrawingSettings(ref data, "Forward", true);
            var filteringSettings = FilteringSettings.defaultValue;
            context.DrawRenderers(data.cullingResults, ref drawingSettings, ref filteringSettings);
            */

            CommandBufferPool.Release(cmd);
        }

        public void Clean(ref ScriptableRenderContext context, ref RenderData data) {
            var cmd = CommandBufferPool.Get("MainLightShadowPass");
            var tid = RenderConst.SHADOW_TEXTURE_ID;
            cmd.ReleaseTemporaryRT(tid);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        private void ReadyTexture(CommandBuffer cmd, ref ScriptableRenderContext context, ref RenderData data) {
            var tid = RenderConst.SHADOW_TEXTURE_ID;
            var rtd = this.CreateRenderTextureDescriptor(1024);
            var rti = new RenderTargetIdentifier(tid);

            cmd.GetTemporaryRT(tid, rtd, FilterMode.Bilinear);
            cmd.SetRenderTarget(rti, 
                RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                RenderBufferLoadAction.Load, RenderBufferStoreAction.DontCare
            );
            cmd.ClearRenderTarget(true, true, data.camera.backgroundColor.linear);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }

        private RenderTextureDescriptor CreateRenderTextureDescriptor(int size) {
            // var rtd = new RenderTextureDescriptor(size, size, RenderTextureFormat.Shadowmap, 16);
            var rtd = new RenderTextureDescriptor(size, size, RenderTextureFormat.Default, 32);

            return rtd;
        }
    }
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public class TestPass : IRenderPass {
        public void Render(ref ScriptableRenderContext context, ref RenderData data) {
            var rtd = new RenderTextureDescriptor(1280, 720, GraphicsFormat.R8G8B8A8_UNorm, 32);
            var id = Shader.PropertyToID("_TestTexture");
            var rti = new RenderTargetIdentifier(id);

            var cmd = CommandBufferPool.Get();
            cmd.GetTemporaryRT(id, rtd, FilterMode.Bilinear);
            cmd.SetRenderTarget(rti, 
                RenderBufferLoadAction.Load, RenderBufferStoreAction.Store,
                RenderBufferLoadAction.Load, RenderBufferStoreAction.DontCare
            );
            cmd.ClearRenderTarget(true, true, Color.black);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            var drawingSettings = RenderUtil.CreateDrawingSettings(ref data, "Forward");
            var filteringSettings = FilteringSettings.defaultValue;
            context.DrawRenderers(data.cullingResults, ref drawingSettings, ref filteringSettings);
        }

        public void Clean(ref ScriptableRenderContext context, ref RenderData data) {
            var id = Shader.PropertyToID("_TestTexture");
            var cmd = CommandBufferPool.Get();
            cmd.ReleaseTemporaryRT(id);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
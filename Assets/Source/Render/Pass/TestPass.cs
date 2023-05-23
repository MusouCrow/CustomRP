using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public class TestPass : IRenderPass {
        private ProfilingSampler profilingSampler;

        public TestPass() {
            this.profilingSampler = new ProfilingSampler("TestPass");
        }

        public void Render(ref ScriptableRenderContext context, ref RenderData data) {
            var cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, this.profilingSampler)) {
                var rtd = data.cameraRTD;
                var id = Shader.PropertyToID("_TestTexture");
                var rti = new RenderTargetIdentifier(id);

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
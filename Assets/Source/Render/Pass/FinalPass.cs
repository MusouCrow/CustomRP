using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public class FinalPass : IRenderPass {
        private ProfilingSampler profilingSampler;

        public FinalPass() {
            this.profilingSampler = new ProfilingSampler("FinalPass");
        }

        public void Render(ref ScriptableRenderContext context, ref RenderData data) {
            var cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, this.profilingSampler)) {
                var id = Shader.PropertyToID("_TestTexture");
                var srcRTI = new RenderTargetIdentifier(id);
                var dstRTI = new RenderTargetIdentifier(data.camera.targetTexture);
                cmd.Blit(srcRTI, dstRTI);
            }
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Clean(ref ScriptableRenderContext context, ref RenderData data) {
            
        }
    }
}
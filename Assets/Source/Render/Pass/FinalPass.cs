using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public class FinalPass : IRenderPass {
        public void Render(ref ScriptableRenderContext context, ref RenderData data) {
            var id = Shader.PropertyToID("_TestTexture");
            var srcRTI = new RenderTargetIdentifier(id);
            var dstRTI = new RenderTargetIdentifier(data.camera.targetTexture);
            
            var cmd = CommandBufferPool.Get();
            cmd.Blit(srcRTI, dstRTI);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Clean(ref ScriptableRenderContext context, ref RenderData data) {
            
        }
    }
}
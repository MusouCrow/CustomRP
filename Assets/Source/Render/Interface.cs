using UnityEngine.Rendering;

namespace Game.Render {
    public interface IRenderPass {
        public bool Setup(ref ScriptableRenderContext context, ref RenderData data);
        public void Render(ref ScriptableRenderContext context, ref RenderData data);
        public void Clean(ref ScriptableRenderContext context, ref RenderData data);
    }
}
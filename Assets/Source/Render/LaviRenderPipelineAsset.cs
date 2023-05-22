using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    [CreateAssetMenu(menuName = "Rendering/LaviRenderPipelineAsset")]
    public class LaviRenderPipelineAsset : RenderPipelineAsset {
        protected override RenderPipeline CreatePipeline() {
            return new LaviRenderPipeline();
        }
    }
}
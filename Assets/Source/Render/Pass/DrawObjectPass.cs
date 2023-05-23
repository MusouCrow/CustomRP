using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    public class DrawObjectPass : IRenderPass {
        public void Render(ref ScriptableRenderContext context, ref RenderData data) {
            var drawingSettings = RenderUtil.CreateDrawingSettings(ref data, "Forward");
            var filteringSettings = FilteringSettings.defaultValue;
            context.DrawRenderers(data.cullingResults, ref drawingSettings, ref filteringSettings);
        }

        public void Clean(ref ScriptableRenderContext context, ref RenderData data) {

        }
    }
}
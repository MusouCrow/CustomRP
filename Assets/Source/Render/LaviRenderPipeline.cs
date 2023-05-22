using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    public class LaviRenderPipeline : RenderPipeline {
        private Renderer renderer;

        public LaviRenderPipeline() {
            this.renderer = new Renderer();
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
            foreach (var camera in cameras) {
                this.renderer?.Render(ref context, camera);
            }
        }
    }
}
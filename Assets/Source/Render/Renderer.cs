using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    public class Renderer {
        private List<IRenderPass> passes;
        private LaviRenderPipelineAsset asset;

        public Renderer(LaviRenderPipelineAsset asset) {
            this.passes = new List<IRenderPass>() {
                new MainLightShadowPass(),
                new SetupPass(),
                new DrawObjectPass(true),
                new DrawObjectPass(false),
                new FinalPass()
            };
            
            this.asset = asset;
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
        }

        public void Render(ref ScriptableRenderContext context, Camera camera) {
            camera.TryGetCullingParameters(out var cullingParameters);
            var cullingResults = context.Cull(ref cullingParameters);
            context.SetupCameraProperties(camera);
            
            var data = new RenderData() {
                camera = camera,
                cullingResults = cullingResults,
                cameraRTD = RenderUtil.CreateCameraRenderTextureDescriptor(camera, this.asset.MSAA, this.asset.RenderScale)
            };

            for (int i = 0; i < this.passes.Count; i++) {
                this.passes[i].Render(ref context, ref data);
            }

            for (int i = this.passes.Count - 1; i >= 0; i--) {
                this.passes[i].Clean(ref context, ref data);
            }

            context.Submit();
        }
    }
}
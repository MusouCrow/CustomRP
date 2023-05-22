using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    public class LaviRenderPipeline : RenderPipeline {
        protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
            foreach (var camera in cameras) {
                var cmd = CommandBufferPool.Get();
                cmd.ClearRenderTarget(true, true, Color.black);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
                
                camera.TryGetCullingParameters(out var cullingParameters);
                var cullingResults = context.Cull(ref cullingParameters);
                context.SetupCameraProperties(camera);

                var shaderTagId = new ShaderTagId("Forward");
                var sortingSettings = new SortingSettings(camera);
                var drawingSettings = new DrawingSettings(shaderTagId, sortingSettings);
                var filteringSettings = FilteringSettings.defaultValue;
                context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

                context.Submit();
            }
        }
    }
}
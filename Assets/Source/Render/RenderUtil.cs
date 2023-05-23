using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public static class RenderUtil {
        public static DrawingSettings CreateDrawingSettings(ref RenderData data, string lightMode) {
            var sortingSettings = new SortingSettings(data.camera);
            var tagId = new ShaderTagId(lightMode);
            var drawingSettings = new DrawingSettings(tagId, sortingSettings);

            return drawingSettings;
        }

        public static RenderTextureDescriptor CreateRenderTextureDescriptor(Camera camera) {
            var rtd = new RenderTextureDescriptor(camera.pixelWidth, camera.pixelHeight);
            rtd.graphicsFormat = GraphicsFormat.B10G11R11_UFloatPack32;
            rtd.depthBufferBits = 32;
            rtd.msaaSamples = 1;
            rtd.sRGB = true;
            rtd.bindMS = false;
            rtd.enableRandomWrite = false;

            return rtd;
        }
    }
}
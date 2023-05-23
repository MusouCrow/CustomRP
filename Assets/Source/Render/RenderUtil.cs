using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Game.Render {
    public static class RenderUtil {
        public static DrawingSettings CreateDrawingSettings(ref RenderData data, string lightMode, bool isOpaque) {
            var criteria = isOpaque ? SortingCriteria.CommonOpaque : SortingCriteria.CommonTransparent;
            var sortingSettings = new SortingSettings(data.camera) {criteria = criteria};
            var tagId = new ShaderTagId(lightMode);
            var drawingSettings = new DrawingSettings(tagId, sortingSettings);
            
            return drawingSettings;
        }

        public static DrawingSettings CreateDrawingSettings(ref RenderData data, string[] lightModes, bool isOpaque) {
            var settings = CreateDrawingSettings(ref data, lightModes[0], isOpaque);
            
            for (int i = 1; i < lightModes.Length; i++) {
                var tagId = new ShaderTagId(lightModes[i]);
                settings.SetShaderPassName(i, tagId);
            }

            return settings;
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
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    public static class RenderUtil {
        public static DrawingSettings CreateDrawingSettings(ref RenderData data, string lightMode) {
            var sortingSettings = new SortingSettings(data.camera);
            var tagId = new ShaderTagId(lightMode);
            var drawingSettings = new DrawingSettings(tagId, sortingSettings);

            return drawingSettings;
        }
    }
}
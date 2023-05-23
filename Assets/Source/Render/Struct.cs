using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    public struct RenderData {
        public Camera camera;
        public CullingResults cullingResults;
        public RenderTextureDescriptor cameraRTD;
    }

    public enum MSAASamples {
        None = 1,
        MSAA2x = 2,
        MSAA4x = 4,
        MSAA8x = 8
    }
}
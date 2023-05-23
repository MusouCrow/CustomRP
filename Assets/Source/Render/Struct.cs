using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Render {
    public struct RenderData {
        public Camera camera;
        public CullingResults cullingResults;
        public RenderTextureDescriptor cameraRTD;
    }
}
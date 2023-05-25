using UnityEngine;

namespace Game.Render {
    public static class RenderConst {
        public static int CAMERA_TEXTURE_ID = Shader.PropertyToID("_CameraTexture");
        public static int SHADOW_TEXTURE_ID = Shader.PropertyToID("_ShadowTexture");
        public static int WORLD_TO_SHADOW_MTX_ID = Shader.PropertyToID("_WorldToShadowMatrix");
    }
}
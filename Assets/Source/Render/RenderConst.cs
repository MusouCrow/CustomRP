using UnityEngine;

namespace Game.Render {
    public static class RenderConst {
        public static int CAMERA_TEXTURE_ID = Shader.PropertyToID("_CameraTexture");
        public static int SHADOW_TEXTURE_ID = Shader.PropertyToID("_ShadowTexture");
        public static int SHADOW_TEXUTRE_SIZE_ID = Shader.PropertyToID("_ShadowTexture_TexelSize");
        public static int WORLD_TO_SHADOW_MTX_ID = Shader.PropertyToID("_WorldToShadowMatrix");
        public static int MAIN_LIGHT_DIRECTION_ID = Shader.PropertyToID("_LightDirection");
        public static int SHADOW_PARAM_ID = Shader.PropertyToID("_ShadowParam");
        
        public static float SHADOW_BIAS_RADIUS = 2.5f;

        public static string MAIN_LIGHT_SHADOW_KEYWORD = "_MAIN_LIGHT_SHADOWS";
        public static string SOFT_SHADOW_KEYWORD = "_SHADOWS_SOFT";
    }
}
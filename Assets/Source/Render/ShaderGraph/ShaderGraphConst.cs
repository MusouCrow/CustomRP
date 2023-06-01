namespace Game.Render.ShaderGraph {
    public static class ShaderGraphConst {
        public static string TEMPLATE_PATH = "Assets/Source/Render/ShaderGraph/Template";
        public static string INNER_TEMPLATE_PATH = "Packages/com.unity.shadergraph/Editor/Generation/Templates";
        public static string SHADER_PASS_PATH = TEMPLATE_PATH + "/ShaderPass.template";
        public static string SHADERLIB_CORE = "Assets/Source/Render/ShaderLibrary/Core.hlsl";
        public static string SHADERLIB_FUNCTIONS = "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl";
        public static string SHADERLIB_FORWARD_PASS = "Assets/Source/Render/ShaderGraph/ShaderLibrary/ForwardPass.hlsl";
    }
}
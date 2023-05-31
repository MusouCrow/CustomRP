using System;
using UnityEditor;
using UnityEditor.ShaderGraph;

namespace Game.Render.ShaderGraph.Editor {
    public static class CreateShaderGraph {
        [MenuItem("Assets/Create/Shader Graph/Lavivagnar/Test Shader Graph")]
        public static void CreateTestGraph() {
            var target = (LaviTarget)Activator.CreateInstance(typeof(LaviTarget));
            
            
            GraphUtil.CreateNewGraphWithOutputs(new [] {target}, null);
        }
    }
}
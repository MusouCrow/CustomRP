using UnityEditor;
using UnityEditor.ShaderGraph;

namespace Game.Render.ShaderGraph.Editor {
    static class TestPasses {
        public static PassDescriptor Forward() {
            var result = new PassDescriptor() {
                // Definition
                displayName = "Forward",
                referenceName = "SHADERPASS_FORWARD",
                lightMode = "Forward",
                useInPreview = true,

                // Template
                passTemplatePath = ShaderGraphConst.SHADER_PASS_PATH,
                sharedTemplateDirectories = new string[] {ShaderGraphConst.TEMPLATE_PATH},

                // Port Mask
                validVertexBlocks = new BlockFieldDescriptor[] {},
                validPixelBlocks = new BlockFieldDescriptor[] {},

                // Fields
                structs = new StructCollection(),
                requiredFields = new FieldCollection(),
                fieldDependencies = new DependencyCollection(),

                // Conditional State
                renderStates = new RenderStateCollection(),
                pragmas = new PragmaCollection(),
                defines = new DefineCollection(),
                keywords = new KeywordCollection(),
                includes = new IncludeCollection(),
            };

            return result;
        }

        public static SubShaderDescriptor SubShader() {
            var result = new SubShaderDescriptor() {
                generatesPreview = true,
                passes = new PassCollection()
            };

            result.passes.Add(Forward());

            return result;
        }
    }
}
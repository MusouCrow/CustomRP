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
                sharedTemplateDirectories = new string[] {
                    ShaderGraphConst.INNER_TEMPLATE_PATH,
                    ShaderGraphConst.TEMPLATE_PATH
                },

                // Port Mask
                validVertexBlocks = new BlockFieldDescriptor[] {BlockFields.VertexDescription.Position},
                validPixelBlocks = new BlockFieldDescriptor[] {BlockFields.SurfaceDescription.BaseColor},

                // Fields
                structs = new StructCollection() {
                    Structs.Attributes,
                    LaviStructs.Varyings,
                    Structs.SurfaceDescriptionInputs,
                    Structs.VertexDescriptionInputs
                },
                requiredFields = new FieldCollection(),
                fieldDependencies = new DependencyCollection(),

                // Conditional State
                renderStates = new RenderStateCollection(),
                pragmas = new PragmaCollection() {
                    Pragma.Vertex("Vert"), 
                    Pragma.Fragment("Frag"), 
                    Pragma.MultiCompileInstancing
                },
                defines = new DefineCollection(),
                keywords = new KeywordCollection(),
                includes = new IncludeCollection() {
                    {ShaderGraphConst.SHADERLIB_CORE, IncludeLocation.Pregraph},
                    {ShaderGraphConst.SHADERLIB_FUNCTIONS, IncludeLocation.Pregraph},
                    {ShaderGraphConst.SHADERLIB_FORWARD_PASS, IncludeLocation.Postgraph},
                },
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
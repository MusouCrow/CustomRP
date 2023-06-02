using UnityEditor;
using UnityEditor.ShaderGraph;

namespace Game.Render.ShaderGraph.Editor {
    static class TestPasses {
        public static PassDescriptor Forward(LaviTarget target) {
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
                renderStates = target.overrideMaterial ? GetOverrideRenderState() : GetRenderState(target),
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

        public static SubShaderDescriptor SubShader(LaviTarget target, string renderType, string renderQueue) {
            var result = new SubShaderDescriptor() {
                // pipelineTag = typeof(LaviRenderPipeline).Name,
                renderType = renderType,
                renderQueue = renderQueue,
                generatesPreview = true,
                passes = new PassCollection()
            };

            result.passes.Add(Forward(target));

            return result;
        }

        private static RenderStateCollection GetRenderState(LaviTarget target) {
            var result = new RenderStateCollection();
            target.GetBlend(out var srcBlend, out var dstBlend);

            var zTest = (float)target.zTest;

            result.Add(RenderState.Blend(srcBlend, dstBlend));
            result.Add(RenderState.ZWrite(target.zWrite ? ZWrite.On : ZWrite.Off));
            result.Add(RenderState.ZTest(target.zTest));
            result.Add(RenderState.Cull(target.cullMode.ToString()));

            return result;
        }

        private static RenderStateCollection GetOverrideRenderState() {
            var result = new RenderStateCollection();

            var srcBlend = "[" + ShaderGraphConst.SRC_BLEND_PROPERTY + "]";
            var dstBlend = "[" + ShaderGraphConst.DST_BLEND_PROPERTY + "]";
            var zWrite = "[" + ShaderGraphConst.ZWRITE_PROPERTY + "]";
            var zTest = "[" + ShaderGraphConst.ZTEST_PROPERTY + "]";
            var cull = "[" + ShaderGraphConst.CULL_PROPERTY + "]";

            result.Add(RenderState.Blend(srcBlend, dstBlend));
            result.Add(RenderState.ZWrite(zWrite));
            result.Add(RenderState.ZTest(zTest));
            result.Add(RenderState.Cull(cull));

            return result;
        }
    }
}
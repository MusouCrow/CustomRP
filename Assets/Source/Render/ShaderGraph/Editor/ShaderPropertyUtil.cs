using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;

namespace Game.Render.ShaderGraph.Editor {
    static class ShaderPropertyUtil {
        public static void AddFloatProperty(PropertyCollector collector, string referenceName, float defaultValue) {
            var property = new Vector1ShaderProperty() {
                floatType = FloatType.Default,
                hidden = true,
                overrideHLSLDeclaration = true,
                hlslDeclarationOverride = HLSLDeclaration.DoNotDeclare,
                value = defaultValue,
                displayName = referenceName,
                overrideReferenceName = referenceName,
            };
            
            collector.AddShaderProperty(property);
        }
    }
}
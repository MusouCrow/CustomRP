using System;
using UnityEditor;
using UnityEditor.ShaderGraph;

namespace Game.Render.ShaderGraph.Editor {
    class LaviSubTarget : SubTarget<LaviTarget> {
        private static readonly GUID SOURCE_GUID = new GUID("14197d0eaac064a4abd20400ac59b2c2"); // LaviSubTarget.cs

        public LaviSubTarget() {
            this.displayName = "Test";
        }

        public override bool IsActive() {
            return true;
        }

        public override void Setup(ref TargetSetupContext context) {
            context.AddAssetDependency(SOURCE_GUID, AssetCollection.Flags.SourceDependency);
            context.AddSubShader(TestPasses.SubShader());
        }

        public override void GetFields(ref TargetFieldContext context) {

        }

        public override void GetActiveBlocks(ref TargetActiveBlockContext context) {

        }

        public override void GetPropertiesGUI(ref TargetPropertyGUIContext context, Action onChange, Action<String> registerUndo) {
            var target = this.target as LaviTarget;
            
            // universalTarget.AddDefaultMaterialOverrideGUI(ref context, onChange, registerUndo);
            // universalTarget.AddDefaultSurfacePropertiesGUI(ref context, onChange, registerUndo, showReceiveShadows: false);
        }
    }
}
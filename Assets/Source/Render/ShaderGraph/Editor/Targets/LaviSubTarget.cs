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
            context.AddSubShader(TestPasses.SubShader(this.target, this.target.RenderType, this.target.RenderQueue));
        }

        public override void GetFields(ref TargetFieldContext context) {

        }

        public override void GetActiveBlocks(ref TargetActiveBlockContext context) {
            context.AddBlock(BlockFields.VertexDescription.Position);
            context.AddBlock(BlockFields.SurfaceDescription.BaseColor);
        }

        public override void GetPropertiesGUI(ref TargetPropertyGUIContext context, Action onChange, Action<String> registerUndo) {
            
        }
    }
}
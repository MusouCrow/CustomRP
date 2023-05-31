using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Serialization;

namespace Game.Render.ShaderGraph.Editor {
    class LaviTarget : Target {
        private static readonly GUID SOURCE_GUID = new GUID("f2e395f06ff9343bb93b405895558824"); // LaviTarget.cs

        private List<SubTarget> subTargets;
        private List<string> subTargetsNames;

        [SerializeField]
        private JsonData<SubTarget> activeSubTarget;

        public LaviTarget() {
            this.displayName = "Lavivagnar";
            this.subTargets = TargetUtils.GetSubTargets(this);
            this.subTargetsNames = new List<string>();

            foreach (var t in this.subTargets) {
                this.subTargetsNames.Add(t.displayName);
            }

            TargetUtils.ProcessSubTargetList(ref this.activeSubTarget, ref this.subTargets);
        }

        public override bool IsActive() {
            bool ok = GraphicsSettings.currentRenderPipeline is LaviRenderPipelineAsset;

            return ok && this.activeSubTarget.value.IsActive();
        }
        
        public override void Setup(ref TargetSetupContext context) {
            context.AddAssetDependency(SOURCE_GUID, AssetCollection.Flags.SourceDependency);

            TargetUtils.ProcessSubTargetList(ref this.activeSubTarget, ref this.subTargets);

            this.activeSubTarget.value.target = this;
            this.activeSubTarget.value.Setup(ref context);
        }

        public override void GetFields(ref TargetFieldContext context) {
            context.AddField(Fields.GraphVertex);
            context.AddField(Fields.GraphPixel);
            
            this.activeSubTarget.value.GetFields(ref context);
        }

        public override void GetActiveBlocks(ref TargetActiveBlockContext context) {
            context.AddBlock(BlockFields.VertexDescription.Position);
            context.AddBlock(BlockFields.SurfaceDescription.BaseColor);

            this.activeSubTarget.value.GetActiveBlocks(ref context);
        }

        public override void GetPropertiesGUI(ref TargetPropertyGUIContext context, Action onChange, Action<String> registerUndo) {
            int activeSubTargetIndex = this.subTargets.IndexOf(this.activeSubTarget);
            var subTargetField = new PopupField<string>(this.subTargetsNames, activeSubTargetIndex);

            context.AddProperty("Material", subTargetField, (evt) =>
            {
                if (activeSubTargetIndex == subTargetField.index) {
                    return;
                }

                registerUndo("Change Material");
                this.activeSubTarget = this.subTargets[subTargetField.index];
                onChange();
            });

            this.activeSubTarget.value.GetPropertiesGUI(ref context, onChange, registerUndo);
        }

        public override bool WorksWithSRP(RenderPipelineAsset scriptableRenderPipeline) {
            return scriptableRenderPipeline is LaviRenderPipelineAsset;
        }

        public bool TrySetActiveSubTarget(Type subTargetType) {
            if (!subTargetType.IsSubclassOf(typeof(SubTarget))) {
                return false;
            }
            
            foreach (var subTarget in this.subTargets) {
                if (subTarget.GetType().Equals(subTargetType)) {
                    this.activeSubTarget = subTarget;
                    return true;
                }
            }
            
            return false;
        }
    }
}
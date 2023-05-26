using System;
using UnityEditor.VFX;

namespace Game.Render.VFX.Editor {
    class VFXBinder : VFXSRPBinder {
        public override string templatePath {
            get {
                return VFXConst.TEMPLATE_PATH;
            }
        }

        public override string runtimePath {
            get {
                return VFXConst.SHADER_PATH;
            }
        }

        public override string SRPAssetTypeStr {
            get {
                return typeof(LaviRenderPipelineAsset).Name;
            }
        }

        public override Type SRPOutputDataType {
            get {
                return null;
            }
        }
    }
}
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace LynxCity.EditorTools
{
    public static class LynxHeroAssetPipeline
    {
        const string HeroFbx = "Assets/LynxCity/Art/Characters/Lynx/Lynx_Hero.fbx";

        [MenuItem("Lynx City/Character/Prepare Production Lynx FBX")]
        public static void PrepareHero()
        {
            var importer = AssetImporter.GetAtPath(HeroFbx) as ModelImporter;
            if (importer == null)
            {
                Debug.LogWarning($"No production model found at {HeroFbx}. V2.1 will continue using its articulated authoring fallback until the hero mesh is ready.");
                return;
            }

            importer.animationType = ModelImporterAnimationType.Human;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importer.importAnimation = true;
            importer.importBlendShapes = true;
            importer.importCameras = false;
            importer.importLights = false;
            importer.isReadable = false;
            importer.SaveAndReimport();
            Debug.Log("Production Lynx FBX prepared as a Unity Humanoid. Inspect Avatar Configuration before using it in-game.");
        }
    }
}
#endif

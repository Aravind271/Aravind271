#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace LynxCity.EditorTools
{
    public static class LynxCityMacBuild
    {
        const string ScenePath = "Assets/LynxCity/Scenes/LynxCity_V2_1_CharacterLab.unity";
        const string ProductName = "Lynx City";
        const string BuildPath = "Builds/macOS-AppleSilicon/Lynx City.app";

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            EditorApplication.delayCall += () =>
            {
                EnsureScene();
#if UNITY_EDITOR_OSX
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX)
                    ApplyMacSettings(false);
#endif
            };
        }

        [MenuItem("Lynx City/V2.1/Open Character Lab")]
        public static void OpenCharacterLab()
        {
            EnsureScene();
            EditorSceneManager.OpenScene(ScenePath);
        }

        [MenuItem("Lynx City/macOS/Configure Native Apple Silicon")]
        public static void ConfigureNativeMacOS()
        {
#if UNITY_EDITOR_OSX
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneOSX)
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
            ApplyMacSettings(true);
#else
            Debug.LogError("Native Lynx City builds are intentionally produced from the macOS Unity Editor.");
#endif
        }

        [MenuItem("Lynx City/macOS/Build Native Apple Silicon")]
        public static void BuildNativeMacOS()
        {
#if UNITY_EDITOR_OSX
            ConfigureNativeMacOS();
            EnsureScene();
            Directory.CreateDirectory(Path.GetDirectoryName(BuildPath));
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = BuildPath,
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.StrictMode
            });
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                Debug.Log($"LYNX CITY V2.1 native Apple Silicon build complete: {Path.GetFullPath(BuildPath)}");
            else
                Debug.LogError($"LYNX CITY build failed: {report.summary.result}. Check the Console.");
#else
            Debug.LogError("This branch is macOS-only. Build it in Unity on macOS.");
#endif
        }

        [MenuItem("Lynx City/macOS/Validate Native Settings")]
        public static void ValidateNativeSettings()
        {
            var apis = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneOSX);
            bool metalOnly = apis.Length == 1 && apis[0] == GraphicsDeviceType.Metal;
            int architecture = PlayerSettings.GetArchitecture(NamedBuildTarget.Standalone);
            var backend = PlayerSettings.GetScriptingBackend(NamedBuildTarget.Standalone);
            Debug.Log($"LYNX CITY macOS validation\nArchitecture: {(architecture == 1 ? "ARM64 / Apple Silicon" : architecture.ToString())}\nGraphics API: {(metalOnly ? "Metal only" : string.Join(", ", apis))}\nScripting: {backend}\nColor space: {PlayerSettings.colorSpace}");
        }

        static void ApplyMacSettings(bool log)
        {
            PlayerSettings.companyName = "Lynx City Project";
            PlayerSettings.productName = ProductName;
            PlayerSettings.bundleVersion = "0.2.1";
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.defaultScreenWidth = 1920;
            PlayerSettings.defaultScreenHeight = 1080;
            PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.allowFullscreenSwitch = true;
            PlayerSettings.runInBackground = false;

            PlayerSettings.SetArchitecture(NamedBuildTarget.Standalone, 1);
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.Standalone, Il2CppCompilerConfiguration.Release);
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneOSX, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneOSX, new[] { GraphicsDeviceType.Metal });

            AssetDatabase.SaveAssets();
            if (log) ValidateNativeSettings();
        }

        static void EnsureScene()
        {
            if (File.Exists(ScenePath))
            {
                EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
                return;
            }

            Directory.CreateDirectory("Assets/LynxCity/Scenes");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Debug.Log("Lynx City V2.1: Character Lab scene created. Open it and press Play.");
        }
    }
}
#endif

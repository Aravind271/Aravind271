#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LynxCity.EditorTools
{
    public static class LynxCityProjectSetup
    {
        const string ScenePath = "Assets/LynxCity/Scenes/LynxCity_Main.unity";

        [InitializeOnLoadMethod]
        static void EnsureSceneExists()
        {
            EditorApplication.delayCall += () =>
            {
                if (File.Exists(ScenePath)) return;
                Directory.CreateDirectory("Assets/LynxCity/Scenes");
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                EditorSceneManager.SaveScene(scene, ScenePath);
                EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
                AssetDatabase.SaveAssets();
                Debug.Log("Lynx City: created the playable main scene. Press Play.");
            };
        }

        [MenuItem("Lynx City/Open Main Scene")]
        public static void OpenMainScene()
        {
            EnsureSceneExistsNow();
            EditorSceneManager.OpenScene(ScenePath);
        }

        [MenuItem("Lynx City/Build macOS")]
        public static void BuildMacOS()
        {
            EnsureSceneExistsNow();
            Directory.CreateDirectory("Builds");
            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = "Builds/LynxCity.app",
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.None
            };
            var report = BuildPipeline.BuildPlayer(options);
            Debug.Log($"Lynx City macOS build result: {report.summary.result}");
        }

        static void EnsureSceneExistsNow()
        {
            if (!File.Exists(ScenePath))
            {
                Directory.CreateDirectory("Assets/LynxCity/Scenes");
                var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                EditorSceneManager.SaveScene(scene, ScenePath);
            }
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        }
    }
}
#endif

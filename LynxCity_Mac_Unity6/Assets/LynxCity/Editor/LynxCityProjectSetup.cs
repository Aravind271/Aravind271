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
        const string ScenePath="Assets/LynxCity/Scenes/LynxCity_Main.unity";
        [InitializeOnLoadMethod]static void EnsureSceneExists(){EditorApplication.delayCall+=()=>{if(File.Exists(ScenePath))return;CreateScene();Debug.Log("Lynx City V2: created the playable main scene. Press Play.");};}
        [MenuItem("Lynx City/Open Main Scene")]public static void OpenMainScene(){EnsureSceneExistsNow();EditorSceneManager.OpenScene(ScenePath);}
        [MenuItem("Lynx City/Build/Desktop/macOS")]public static void BuildMacOS()=>Build(BuildTarget.StandaloneOSX,"Builds/macOS/LynxCity.app");
        [MenuItem("Lynx City/Build/Desktop/Windows x64")]public static void BuildWindows()=>Build(BuildTarget.StandaloneWindows64,"Builds/Windows/LynxCity.exe");
        [MenuItem("Lynx City/Build/Desktop/Linux x64")]public static void BuildLinux()=>Build(BuildTarget.StandaloneLinux64,"Builds/Linux/LynxCity.x86_64");
        static void Build(BuildTarget target,string path){EnsureSceneExistsNow();Directory.CreateDirectory(Path.GetDirectoryName(path));var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{ScenePath},locationPathName=path,target=target,options=BuildOptions.None});Debug.Log($"Lynx City {target} build result: {report.summary.result}");}
        static void CreateScene(){Directory.CreateDirectory("Assets/LynxCity/Scenes");var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);EditorSceneManager.SaveScene(scene,ScenePath);EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(ScenePath,true)};AssetDatabase.SaveAssets();}
        static void EnsureSceneExistsNow(){if(!File.Exists(ScenePath))CreateScene();EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(ScenePath,true)};}
    }
}
#endif

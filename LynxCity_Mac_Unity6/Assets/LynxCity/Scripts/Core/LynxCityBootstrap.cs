using UnityEngine;
using LynxCity.Player;
using LynxCity.Characters;
using LynxCity.UI;
using LynxCity.World;
using LynxCity.Platform;

namespace LynxCity.Core
{
    /// <summary>V2.1 is intentionally a character vertical slice. City systems stay in source but are not spawned.</summary>
    public static class LynxCityBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void BuildCharacterLab()
        {
            if (Object.FindFirstObjectByType<GameState>() != null) return;

            var state = new GameObject("GameState").AddComponent<GameState>();
            state.CurrentDistrict = "Kosei University Character Lab";
            state.Year = 1991;
            new GameObject("MacNativeRuntime").AddComponent<MacNativeRuntime>();

            CharacterLabBuilder.BuildLighting();
            CharacterLabBuilder.Build();
            var player = BuildPlayer();
            BuildCamera(player.transform);
            BuildHUD(player);
        }

        static GameObject BuildPlayer()
        {
            var player = new GameObject("Lynx_Player");
            player.transform.position = new Vector3(0f,.05f,-1.0f);
            player.transform.rotation = Quaternion.identity;

            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.77f;
            controller.radius = .285f;
            controller.center = new Vector3(0f,.885f,0f);
            controller.stepOffset = .30f;
            controller.slopeLimit = 48f;

            player.AddComponent<LynxMotor>();
            LynxHeroFactory.Build(player.transform);
            player.AddComponent<LynxAppearance>();
            player.AddComponent<CharacterCustomizer>();
            return player;
        }

        static void BuildCamera(Transform player)
        {
            var cameraObject = new GameObject("Main Camera");
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
            camera.fieldOfView = 52f;
            camera.nearClipPlane = .06f;
            camera.farClipPlane = 180f;
            camera.allowHDR = true;
            camera.allowMSAA = true;

            var rig = cameraObject.AddComponent<ThirdPersonCamera>();
            rig.target = player;
            cameraObject.transform.position = player.position + new Vector3(0f,1.4f,-4.2f);
            player.GetComponent<LynxMotor>().cameraTransform = cameraObject.transform;
        }

        static void BuildHUD(GameObject player)
        {
            var hud = new GameObject("V2.1 Character HUD").AddComponent<PrototypeHUD>();
            hud.player = player;
        }
    }
}

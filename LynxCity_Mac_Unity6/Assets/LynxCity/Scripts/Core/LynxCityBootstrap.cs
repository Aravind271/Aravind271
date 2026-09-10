using UnityEngine;
using LynxCity.Player;
using LynxCity.Combat;
using LynxCity.Interaction;
using LynxCity.World;
using LynxCity.Characters;
using LynxCity.UI;

namespace LynxCity.Core
{
    public static class LynxCityBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void BuildPrototype(){if(Object.FindFirstObjectByType<GameState>()!=null)return;new GameObject("GameState").AddComponent<GameState>();var performance=new GameObject("PerformanceBudget").AddComponent<PerformanceBudget>();BuildLighting();var player=BuildPlayer();BuildCamera(player.transform);LateShowaStreetBuilder.BuildWorld(player.transform,performance.UrbanCrowdPerDistrict);BuildEnemyEncounter(player.transform,Vector3.zero,5);BuildEnemyEncounter(player.transform,new Vector3(150,0,0),4);BuildHUD(player.GetComponent<Health>());}
        static void BuildLighting(){RenderSettings.fog=true;RenderSettings.fogColor=new Color(.10f,.105f,.13f);RenderSettings.fogDensity=.0035f;RenderSettings.ambientLight=new Color(.26f,.28f,.34f);RenderSettings.ambientIntensity=.72f;var sun=new GameObject("Late Afternoon Sun").AddComponent<Light>();sun.type=LightType.Directional;sun.intensity=.85f;sun.color=new Color(1f,.78f,.62f);sun.shadows=LightShadows.Soft;sun.transform.rotation=Quaternion.Euler(22,-38,0);}
        static GameObject BuildPlayer(){var p=new GameObject("Lynx_Player");p.transform.position=new Vector3(0,.06f,-5);var cc=p.AddComponent<CharacterController>();cc.height=1.82f;cc.radius=.34f;cc.center=new Vector3(0,.91f,0);p.AddComponent<Health>().maxHealth=120;p.AddComponent<LynxMotor>();p.AddComponent<BrawlerCombat>();p.AddComponent<GameInteractor>();HumanFigureFactory.BuildJapaneseAdult(p.transform,"Visual",1f,HumanFigureFactory.LynxOutfits[0],true);p.AddComponent<LynxAppearance>();p.AddComponent<CharacterCustomizer>();return p;}
        static void BuildCamera(Transform player){var old=Camera.main;var go=old?old.gameObject:new GameObject("Main Camera");if(!old)go.AddComponent<Camera>().tag="MainCamera";var cam=go.GetComponent<Camera>();cam.fieldOfView=58f;cam.nearClipPlane=.08f;cam.farClipPlane=550f;var rig=go.GetComponent<ThirdPersonCamera>()??go.AddComponent<ThirdPersonCamera>();rig.target=player;player.GetComponent<LynxMotor>().cameraTransform=go.transform;go.transform.position=new Vector3(-4,3,-6);}
        static void BuildEnemyEncounter(Transform player,Vector3 center,int count){for(int i=0;i<count;i++){var e=new GameObject($"StreetTroublemaker_{i+1}");e.transform.position=center+new Vector3(7+i*1.3f,.05f,8+(i%2)*1.4f);var col=e.AddComponent<CapsuleCollider>();col.height=1.82f;col.radius=.34f;col.center=new Vector3(0,.91f,0);var rb=e.AddComponent<Rigidbody>();rb.constraints=RigidbodyConstraints.FreezeRotation;rb.mass=72f;e.AddComponent<Health>().maxHealth=58;HumanFigureFactory.BuildJapaneseAdult(e.transform,"Visual",Random.Range(.96f,1.06f),HumanFigureFactory.LynxOutfits[(i+1)%HumanFigureFactory.LynxOutfits.Length]);e.AddComponent<StreetEnemy>().target=player;}}
        static void BuildHUD(Health hp){var h=new GameObject("LynxCityHUD").AddComponent<PrototypeHUD>();h.playerHealth=hp;}
    }
}

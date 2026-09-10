using UnityEngine;
using LynxCity.Player;
using LynxCity.Combat;
using LynxCity.Interaction;
using LynxCity.World;
using LynxCity.Social;
using LynxCity.Karaoke;
using LynxCity.University;
using LynxCity.UI;

namespace LynxCity.Core
{
    public static class LynxCityBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void BuildPrototype()
        {
            if (Object.FindFirstObjectByType<GameState>() != null) return;
            new GameObject("GameState").AddComponent<GameState>();
            BuildLighting();
            BuildGround();
            var player = BuildPlayer();
            BuildCamera(player.transform);
            BuildDistrict(player.transform);
            BuildHUD(player.GetComponent<Health>());
        }

        static void BuildLighting()
        {
            if (!Object.FindFirstObjectByType<Light>())
            {
                var sun = new GameObject("Sun").AddComponent<Light>();
                sun.type = LightType.Directional; sun.intensity = 1.25f;
                sun.transform.rotation = Quaternion.Euler(45, -30, 0);
            }
            RenderSettings.ambientIntensity = 1.1f;
        }

        static void BuildGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Tokyo Prototype Ground";
            ground.transform.localScale = new Vector3(14, 1, 14);
            ground.GetComponent<Renderer>().material.color = new Color(.18f,.19f,.21f);
        }

        static GameObject BuildPlayer()
        {
            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            p.name = "Lynx_Player"; p.transform.position = new Vector3(0,1,0);
            Object.Destroy(p.GetComponent<CapsuleCollider>());
            var cc = p.AddComponent<CharacterController>(); cc.height = 2; cc.radius = .38f; cc.center = new Vector3(0,0,0);
            p.AddComponent<Health>().maxHealth = 120;
            p.AddComponent<LynxMotor>(); p.AddComponent<BrawlerCombat>(); p.AddComponent<GameInteractor>();
            p.GetComponent<Renderer>().material.color = new Color(.12f,.18f,.28f);
            return p;
        }

        static void BuildCamera(Transform player)
        {
            var old = Camera.main;
            var go = old ? old.gameObject : new GameObject("Main Camera");
            if (!old) go.AddComponent<Camera>().tag = "MainCamera";
            var rig = go.GetComponent<ThirdPersonCamera>() ?? go.AddComponent<ThirdPersonCamera>();
            rig.target = player;
            player.GetComponent<LynxMotor>().cameraTransform = go.transform;
            go.transform.position = new Vector3(-4,4,-5);
        }

        static void BuildDistrict(Transform player)
        {
            for (int i=0;i<55;i++)
            {
                Vector2 r = Random.insideUnitCircle * 26;
                var npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                npc.name = "Pedestrian"; npc.transform.position = new Vector3(r.x,1,r.y);
                npc.transform.localScale = new Vector3(.78f, .92f, .78f);
                npc.GetComponent<Renderer>().material.color = Random.ColorHSV(.0f,1f,.25f,.75f,.35f,.85f);
                npc.AddComponent<CrowdWalker>().radius = Random.Range(8f,25f);
                npc.AddComponent<NPCConversation>().line = i%2==0 ? "こんにちは。今日は人が多いですね。" : "Excuse me, the station is straight ahead.";
            }

            for (int i=0;i<4;i++)
            {
                var e = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                e.name = "StreetTroublemaker"; e.transform.position = new Vector3(8+i*1.7f,1,7);
                e.GetComponent<Renderer>().material.color = new Color(.45f,.12f,.12f);
                var h=e.AddComponent<Health>(); h.maxHealth=55;
                e.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                e.AddComponent<StreetEnemy>().target = player;
            }

            MakeShop("Konbini", new Vector3(4,.75f,-7), ShopTerminal.ShopKind.ConvenienceStore);
            MakeShop("Ramen Restaurant", new Vector3(9,.75f,-7), ShopTerminal.ShopKind.Restaurant);
            MakeShop("Bicycle Store", new Vector3(14,.75f,-7), ShopTerminal.ShopKind.BicycleShop);
            MakeShop("Mall Entrance", new Vector3(19,.75f,-7), ShopTerminal.ShopKind.Mall);

            var school = MakeBlock("University Lecture Hall", new Vector3(-8,1,-8), new Vector3(5,2,4), new Color(.28f,.34f,.40f));
            school.AddComponent<UniversityRoutine>();
            var date = MakeBlock("Aoi", new Vector3(-5,1,6), Vector3.one, new Color(.75f,.35f,.48f)); date.AddComponent<DateNPC>();
            var karaoke = MakeBlock("Karaoke", new Vector3(2,1,10), new Vector3(3,2,2), new Color(.42f,.18f,.55f)); karaoke.AddComponent<KaraokeBooth>();

            MakeTaxi("Taxi — Shibuya", new Vector3(-13,.6f,5), player, "Shibuya", new Vector3(35,1,30), 1800);
            MakeTaxi("Taxi — Shinjuku", new Vector3(-13,.6f,10), player, "Shinjuku", new Vector3(-35,1,30), 2100);
            MakeTaxi("Taxi — Countryside", new Vector3(-13,.6f,15), player, "Aizu Countryside", new Vector3(0,1,65), 8500);

            for(int i=0;i<26;i++)
            {
                Vector2 r=Random.insideUnitCircle.normalized*Random.Range(34,55);
                float h=Random.Range(5,20);
                MakeBlock("City Building", new Vector3(r.x,h/2,r.y), new Vector3(Random.Range(3,8),h,Random.Range(3,8)), new Color(.22f,.24f,.28f));
            }
        }

        static GameObject MakeBlock(string name, Vector3 pos, Vector3 scale, Color color)
        {
            var g=GameObject.CreatePrimitive(PrimitiveType.Cube); g.name=name; g.transform.position=pos; g.transform.localScale=scale;
            g.GetComponent<Renderer>().material.color=color; return g;
        }
        static void MakeShop(string name, Vector3 pos, ShopTerminal.ShopKind kind)
        {
            var g=MakeBlock(name,pos,new Vector3(3,1.5f,2),new Color(.15f,.38f,.48f)); var s=g.AddComponent<ShopTerminal>(); s.kind=kind;
        }
        static void MakeTaxi(string name, Vector3 pos, Transform player, string destination, Vector3 target, int fare)
        {
            var g=MakeBlock(name,pos,new Vector3(2.4f,1.1f,1.3f),new Color(.1f,.1f,.1f));
            var t=g.AddComponent<DistrictTravel>(); t.player=player; t.destinationName=destination; t.destinationPosition=target; t.fare=fare;
        }
        static void BuildHUD(Health hp)
        {
            var h=new GameObject("PrototypeHUD").AddComponent<PrototypeHUD>(); h.playerHealth=hp;
        }
    }
}

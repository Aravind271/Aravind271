using UnityEngine;
using LynxCity.Characters;
using LynxCity.Interaction;

namespace LynxCity.World
{
    public static class CrowdSpawnerV2
    {
        static readonly string[] archetypes={"Student","Salaryman","OfficeWorker","ShopStaff","Elder","Parent","Teen","NightWorker"};
        static readonly string[] jpLines={"こんばんは。駅はあちらですよ。","今日は人が多いですね。","この辺は夜になると賑やかですよ。","大学生ですか？ 頑張ってください。","すみません、ちょっと急いでいます。","あの喫茶店、昔から人気ですよ。"};
        public static void SpawnJapaneseCrowd(Vector3 center,int count,float radius)
        {
            for(int i=0;i<count;i++)
            {
                Vector2 r=Random.insideUnitCircle*radius; bool visitor=Random.value<.08f; string archetype=archetypes[Random.Range(0,archetypes.Length)];
                var root=new GameObject(visitor?$"Visitor_{archetype}_{i:00}":$"Japanese_{archetype}_{i:00}"); root.transform.position=center+new Vector3(r.x,0,r.y);
                float scale=Random.Range(.90f,1.07f); var outfit=RandomOutfit(); HumanFigureFactory.BuildJapaneseAdult(root.transform,"Visual",scale,outfit);
                var walker=root.AddComponent<CrowdWalker>();walker.radius=Random.Range(8f,20f);walker.speed=Random.Range(.75f,1.35f);
                var talk=root.AddComponent<NPCConversation>();talk.line=visitor?"Excuse me, do you know where the station is?":jpLines[Random.Range(0,jpLines.Length)];
                var col=root.AddComponent<CapsuleCollider>();col.height=1.75f*scale;col.radius=.28f*scale;col.center=new Vector3(0,.9f*scale,0);
            }
        }
        static HumanFigureFactory.OutfitPalette RandomOutfit(){Color[] coats={new(.14f,.15f,.16f),new(.27f,.22f,.17f),new(.14f,.22f,.30f),new(.38f,.32f,.24f),new(.25f,.10f,.09f)};Color[] shirts={new(.78f,.78f,.72f),new(.55f,.57f,.55f),new(.76f,.68f,.53f),new(.34f,.36f,.34f)};Color[] pants={new(.09f,.10f,.12f),new(.16f,.18f,.20f),new(.23f,.20f,.17f),new(.12f,.17f,.23f)};return new HumanFigureFactory.OutfitPalette("Citizen",coats[Random.Range(0,coats.Length)],shirts[Random.Range(0,shirts.Length)],pants[Random.Range(0,pants.Length)],new Color(.08f,.07f,.06f),new Color(.3f,.2f,.1f));}
    }
}

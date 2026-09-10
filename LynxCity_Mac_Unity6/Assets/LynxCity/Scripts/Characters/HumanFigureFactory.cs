using System.Collections.Generic;
using UnityEngine;

namespace LynxCity.Characters
{
    public static class HumanFigureFactory
    {
        static readonly Dictionary<string, Material> materials = new();

        public struct OutfitPalette
        {
            public string name;
            public Color jacket;
            public Color shirt;
            public Color pants;
            public Color shoes;
            public Color accessory;

            public OutfitPalette(string name, Color jacket, Color shirt, Color pants, Color shoes, Color accessory)
            {
                this.name = name; this.jacket = jacket; this.shirt = shirt; this.pants = pants; this.shoes = shoes; this.accessory = accessory;
            }
        }

        public static readonly OutfitPalette[] LynxOutfits =
        {
            new("Campus Casual '91", new Color(.54f,.47f,.36f), new Color(.91f,.89f,.82f), new Color(.16f,.17f,.18f), new Color(.82f,.80f,.72f), new Color(.24f,.20f,.16f)),
            new("Denim Street '88", new Color(.16f,.28f,.42f), new Color(.70f,.69f,.62f), new Color(.09f,.10f,.12f), new Color(.15f,.15f,.16f), new Color(.35f,.25f,.16f)),
            new("Varsity Night '90", new Color(.38f,.08f,.10f), new Color(.48f,.49f,.48f), new Color(.10f,.14f,.22f), new Color(.84f,.82f,.74f), new Color(.76f,.67f,.44f)),
            new("Smart Date '93", new Color(.11f,.13f,.16f), new Color(.82f,.84f,.79f), new Color(.22f,.20f,.18f), new Color(.10f,.08f,.07f), new Color(.42f,.12f,.10f)),
            new("Leather Weekend '89", new Color(.06f,.06f,.065f), new Color(.42f,.10f,.09f), new Color(.20f,.24f,.27f), new Color(.07f,.07f,.07f), new Color(.28f,.20f,.10f)),
        };

        public static GameObject BuildJapaneseAdult(Transform parent, string name, float heightScale, OutfitPalette outfit, bool lynx = false)
        {
            var visual = new GameObject(name); visual.transform.SetParent(parent, false);
            visual.transform.localScale = lynx ? Vector3.one * heightScale : new Vector3(heightScale * Random.Range(.94f,1.08f), heightScale, heightScale * Random.Range(.94f,1.06f));
            Color skin = lynx ? new Color(.74f,.56f,.43f) : RandomSkin();
            Color hair = Random.value < .82f ? new Color(.035f,.028f,.024f) : new Color(.11f,.075f,.045f);
            Part(visual.transform,"Torso",PrimitiveType.Cube,new Vector3(0,1.12f,0),new Vector3(.50f,.62f,.27f),outfit.jacket);
            Part(visual.transform,"Shirt",PrimitiveType.Cube,new Vector3(0,1.24f,-.145f),new Vector3(.25f,.28f,.035f),outfit.shirt);
            Part(visual.transform,"Hips",PrimitiveType.Cube,new Vector3(0,.77f,0),new Vector3(.43f,.22f,.25f),outfit.pants);
            Part(visual.transform,"Neck",PrimitiveType.Cylinder,new Vector3(0,1.49f,0),new Vector3(.13f,.08f,.13f),skin);
            Part(visual.transform,"Head",PrimitiveType.Sphere,new Vector3(0,1.72f,0),new Vector3(.34f,.42f,.34f),skin);
            Part(visual.transform,"Hair",PrimitiveType.Sphere,new Vector3(0,1.88f,.015f),new Vector3(.355f,.24f,.35f),hair);
            Part(visual.transform,"Fringe",PrimitiveType.Cube,new Vector3(0,1.82f,-.17f),new Vector3(.29f,.10f,.035f),hair);
            Part(visual.transform,"Nose",PrimitiveType.Sphere,new Vector3(0,1.70f,-.183f),new Vector3(.065f,.075f,.055f),skin);
            Part(visual.transform,"LeftEar",PrimitiveType.Sphere,new Vector3(-.178f,1.72f,0),new Vector3(.045f,.075f,.045f),skin);
            Part(visual.transform,"RightEar",PrimitiveType.Sphere,new Vector3(.178f,1.72f,0),new Vector3(.045f,.075f,.045f),skin);
            var leftArm=Part(visual.transform,"LeftArm",PrimitiveType.Capsule,new Vector3(-.36f,1.10f,0),new Vector3(.16f,.47f,.16f),outfit.jacket);
            var rightArm=Part(visual.transform,"RightArm",PrimitiveType.Capsule,new Vector3(.36f,1.10f,0),new Vector3(.16f,.47f,.16f),outfit.jacket);
            leftArm.transform.rotation=Quaternion.Euler(0,0,-5); rightArm.transform.rotation=Quaternion.Euler(0,0,5);
            Part(visual.transform,"LeftHand",PrimitiveType.Sphere,new Vector3(-.38f,.73f,0),new Vector3(.12f,.15f,.12f),skin);
            Part(visual.transform,"RightHand",PrimitiveType.Sphere,new Vector3(.38f,.73f,0),new Vector3(.12f,.15f,.12f),skin);
            Part(visual.transform,"LeftLeg",PrimitiveType.Capsule,new Vector3(-.13f,.42f,0),new Vector3(.19f,.49f,.20f),outfit.pants);
            Part(visual.transform,"RightLeg",PrimitiveType.Capsule,new Vector3(.13f,.42f,0),new Vector3(.19f,.49f,.20f),outfit.pants);
            Part(visual.transform,"LeftShoe",PrimitiveType.Cube,new Vector3(-.13f,.075f,-.055f),new Vector3(.21f,.13f,.36f),outfit.shoes);
            Part(visual.transform,"RightShoe",PrimitiveType.Cube,new Vector3(.13f,.075f,-.055f),new Vector3(.21f,.13f,.36f),outfit.shoes);
            if(lynx) Part(visual.transform,"Watch",PrimitiveType.Cube,new Vector3(-.405f,.79f,-.01f),new Vector3(.05f,.07f,.07f),outfit.accessory);
            parent.gameObject.AddComponent<ProceduralHumanoidMotion>(); return visual;
        }

        public static void ApplyOutfit(Transform visual, OutfitPalette outfit)
        {
            SetPart(visual,"Torso",outfit.jacket); SetPart(visual,"LeftArm",outfit.jacket); SetPart(visual,"RightArm",outfit.jacket);
            SetPart(visual,"Shirt",outfit.shirt); SetPart(visual,"Hips",outfit.pants); SetPart(visual,"LeftLeg",outfit.pants); SetPart(visual,"RightLeg",outfit.pants);
            SetPart(visual,"LeftShoe",outfit.shoes); SetPart(visual,"RightShoe",outfit.shoes); SetPart(visual,"Watch",outfit.accessory);
        }

        static GameObject Part(Transform parent,string name,PrimitiveType type,Vector3 localPos,Vector3 localScale,Color color)
        {
            var g=GameObject.CreatePrimitive(type); g.name=name; g.transform.SetParent(parent,false); g.transform.localPosition=localPos; g.transform.localScale=localScale;
            var col=g.GetComponent<Collider>(); if(col) col.enabled=false; g.GetComponent<Renderer>().sharedMaterial=Mat(color); return g;
        }
        static void SetPart(Transform visual,string part,Color color){ var t=visual.Find(part); if(t&&t.TryGetComponent<Renderer>(out var r)) r.sharedMaterial=Mat(color); }
        static Material Mat(Color c){ Color32 c32=c; string key=$"{c32.r}-{c32.g}-{c32.b}"; if(materials.TryGetValue(key,out var m)&&m)return m; var shader=Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard")??Shader.Find("Diffuse"); m=new Material(shader){color=c}; materials[key]=m; return m; }
        static Color RandomSkin(){ float t=Random.Range(-.045f,.045f); return new Color(.72f+t,.54f+t,.41f+t); }
    }

    public class ProceduralHumanoidMotion : MonoBehaviour
    {
        Vector3 lastPosition; Transform leftArm,rightArm,leftLeg,rightLeg; Quaternion la0,ra0,ll0,rl0;
        void Start(){ lastPosition=transform.position; var v=transform.Find("Visual")??transform.GetChild(0); leftArm=v.Find("LeftArm"); rightArm=v.Find("RightArm"); leftLeg=v.Find("LeftLeg"); rightLeg=v.Find("RightLeg"); if(leftArm)la0=leftArm.localRotation; if(rightArm)ra0=rightArm.localRotation; if(leftLeg)ll0=leftLeg.localRotation; if(rightLeg)rl0=rightLeg.localRotation; }
        void LateUpdate(){ float speed=(transform.position-lastPosition).magnitude/Mathf.Max(.001f,Time.deltaTime); lastPosition=transform.position; float amount=Mathf.Clamp01(speed/3.5f); float swing=Mathf.Sin(Time.time*Mathf.Lerp(4f,9f,amount))*28f*amount; if(leftArm)leftArm.localRotation=la0*Quaternion.Euler(swing,0,0); if(rightArm)rightArm.localRotation=ra0*Quaternion.Euler(-swing,0,0); if(leftLeg)leftLeg.localRotation=ll0*Quaternion.Euler(-swing*.75f,0,0); if(rightLeg)rightLeg.localRotation=rl0*Quaternion.Euler(swing*.75f,0,0); }
    }
}

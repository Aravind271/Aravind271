using System.Collections.Generic;
using UnityEngine;

namespace LynxCity.World
{
    /// <summary>A deliberately small, well-lit space used to judge Lynx before city production resumes.</summary>
    public static class CharacterLabBuilder
    {
        static readonly Dictionary<string, Material> Materials = new();

        public static void Build()
        {
            Block("Concrete Floor", new Vector3(0f,-.09f,1f), new Vector3(18f,.18f,16f), Mat("floor", new Color(.22f,.225f,.23f), .32f));
            Block("Rear Wall", new Vector3(0f,2.25f,8f), new Vector3(18f,4.5f,.20f), Mat("wall", new Color(.36f,.35f,.32f), .24f));
            Block("Left Wall", new Vector3(-9f,1.6f,1f), new Vector3(.18f,3.2f,16f), Mat("wall2", new Color(.29f,.30f,.29f), .22f));

            // Warm wood trim gives the test room a believable early-Heisei university feel.
            var wood = Mat("wood", new Color(.25f,.15f,.085f), .38f);
            Block("Wood Trim Rear", new Vector3(0f,.55f,7.86f), new Vector3(17.7f,.12f,.10f), wood);
            Block("Wood Trim Left", new Vector3(-8.86f,.55f,1f), new Vector3(.10f,.12f,15.7f), wood);

            Bench(new Vector3(4.5f,.42f,4.8f), wood);
            Vending(new Vector3(6.8f,1.05f,6.75f));
            Payphone(new Vector3(-6.7f,1.05f,6.8f));
            Mirror(new Vector3(-5.1f,1.55f,7.82f));
            OutfitRail(new Vector3(1.2f,0f,6.4f));
            Poster(new Vector3(4.4f,2.3f,7.84f), new Color(.52f,.18f,.12f));
            Poster(new Vector3(6.0f,2.3f,7.84f), new Color(.12f,.29f,.42f));
        }

        public static void BuildLighting()
        {
            RenderSettings.fog = false;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.40f,.43f,.48f);
            RenderSettings.ambientEquatorColor = new Color(.23f,.24f,.25f);
            RenderSettings.ambientGroundColor = new Color(.12f,.11f,.10f);
            RenderSettings.ambientIntensity = .82f;

            var sun = new GameObject("Window Key Light").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.intensity = 1.05f;
            sun.color = new Color(1f,.83f,.68f);
            sun.shadows = LightShadows.Soft;
            sun.shadowStrength = .82f;
            sun.transform.rotation = Quaternion.Euler(38f,-32f,0f);

            var fill = new GameObject("Soft Fill").AddComponent<Light>();
            fill.type = LightType.Point;
            fill.range = 10f;
            fill.intensity = 1.5f;
            fill.color = new Color(.63f,.73f,1f);
            fill.shadows = LightShadows.None;
            fill.transform.position = new Vector3(-3.4f,3.0f,-1.2f);

            var rim = new GameObject("Warm Rim").AddComponent<Light>();
            rim.type = LightType.Point;
            rim.range = 8f;
            rim.intensity = 1.25f;
            rim.color = new Color(1f,.52f,.30f);
            rim.shadows = LightShadows.None;
            rim.transform.position = new Vector3(3.5f,2.7f,4.8f);
        }

        static void Bench(Vector3 p, Material wood)
        {
            Block("Bench Seat", p, new Vector3(3.2f,.14f,.58f), wood);
            Block("Bench Back", p + new Vector3(0f,.62f,.27f), new Vector3(3.2f,.85f,.12f), wood);
            var metal = Mat("metal", new Color(.11f,.115f,.12f), .64f, .35f);
            Block("Bench Leg L", p + new Vector3(-1.25f,-.32f,0f), new Vector3(.12f,.65f,.12f), metal);
            Block("Bench Leg R", p + new Vector3(1.25f,-.32f,0f), new Vector3(.12f,.65f,.12f), metal);
        }

        static void Vending(Vector3 p)
        {
            var cream = Mat("vending", new Color(.73f,.69f,.58f), .33f);
            Block("1991 Vending Machine", p, new Vector3(1.15f,2.1f,.82f), cream);
            Block("Vending Product Window", p + new Vector3(0f,.34f,-.42f), new Vector3(.82f,.72f,.025f), Mat("vendingWindow", new Color(.28f,.45f,.54f), .70f));
            for (int x=-1;x<=1;x++)
                for (int y=0;y<2;y++)
                    Block("Drink Can", p + new Vector3(x*.22f,.50f-y*.25f,-.445f), new Vector3(.12f,.17f,.025f), Mat($"can{x}{y}", new Color(.52f+.08f*y,.18f+.11f*x,.15f+.14f*y), .35f));
            Block("Coin Panel", p + new Vector3(.32f,-.42f,-.43f), new Vector3(.22f,.36f,.035f), Mat("coin", new Color(.13f,.14f,.14f), .42f));
        }

        static void Payphone(Vector3 p)
        {
            var green = Mat("phoneGreen", new Color(.075f,.28f,.19f), .45f);
            Block("Public Phone Stand", p + new Vector3(0f,-.55f,0f), new Vector3(.78f,1.0f,.54f), Mat("phoneStand", new Color(.17f,.17f,.16f), .25f));
            Block("Green Payphone", p, new Vector3(.72f,.58f,.42f), green);
            Block("Receiver", p + new Vector3(-.30f,.06f,-.23f), new Vector3(.12f,.40f,.10f), Mat("receiver", new Color(.035f,.055f,.045f), .35f));
        }

        static void Mirror(Vector3 p)
        {
            Block("Mirror Frame", p, new Vector3(2.25f,3.15f,.10f), Mat("mirrorFrame", new Color(.12f,.09f,.06f), .42f));
            Block("Mirror Surface", p + new Vector3(0f,0f,-.061f), new Vector3(2.02f,2.90f,.018f), Mat("mirror", new Color(.23f,.29f,.32f), .92f, .72f));
        }

        static void OutfitRail(Vector3 p)
        {
            var metal = Mat("rail", new Color(.16f,.17f,.18f), .72f, .45f);
            Block("Wardrobe Rail Top", p + new Vector3(0f,2.15f,0f), new Vector3(2.8f,.07f,.07f), metal);
            Block("Wardrobe Rail L", p + new Vector3(-1.30f,1.05f,0f), new Vector3(.08f,2.2f,.08f), metal);
            Block("Wardrobe Rail R", p + new Vector3(1.30f,1.05f,0f), new Vector3(.08f,2.2f,.08f), metal);
            var colors = new[]{new Color(.18f,.27f,.39f),new Color(.30f,.06f,.07f),new Color(.09f,.10f,.12f),new Color(.66f,.62f,.52f)};
            for(int i=0;i<4;i++) Block("Hanging Jacket", p + new Vector3(-.75f+i*.50f,1.35f,0f), new Vector3(.36f,.95f,.16f), Mat($"hanger{i}",colors[i],.28f));
        }

        static void Poster(Vector3 p, Color c) => Block("Period Poster", p, new Vector3(1.1f,1.55f,.025f), Mat($"poster{c.r:F2}{c.g:F2}",c,.22f));

        static GameObject Block(string name, Vector3 position, Vector3 scale, Material material)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = position;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = material;
            return go;
        }

        static Material Mat(string key, Color color, float smoothness, float metallic = 0f)
        {
            if (Materials.TryGetValue(key, out var m) && m) return m;
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Diffuse");
            m = new Material(shader) { name=$"Lab_{key}_Runtime" };
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
            if (m.HasProperty("_Color")) m.SetColor("_Color", color);
            if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smoothness);
            if (m.HasProperty("_Glossiness")) m.SetFloat("_Glossiness", smoothness);
            if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);
            Materials[key] = m;
            return m;
        }
    }
}

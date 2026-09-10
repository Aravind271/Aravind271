using System.Collections.Generic;
using UnityEngine;

namespace LynxCity.Characters
{
    /// <summary>
    /// V2.1 runtime hero authoring fallback. It is intentionally much more structured than the
    /// old mannequin: a real transform hierarchy, articulated limbs, a readable face and five
    /// genuinely different clothing silhouettes. The production FBX will replace only the visual
    /// layer later; gameplay code can keep using the same Lynx_Player root.
    /// </summary>
    public static class LynxHeroFactory
    {
        public readonly struct OutfitDefinition
        {
            public readonly string Name;
            public readonly string Year;
            public readonly string Description;
            public readonly Color Primary;
            public readonly Color Secondary;
            public readonly Color Trousers;
            public readonly Color Shoes;
            public readonly Color Accent;

            public OutfitDefinition(string name, string year, string description, Color primary, Color secondary, Color trousers, Color shoes, Color accent)
            {
                Name = name;
                Year = year;
                Description = description;
                Primary = primary;
                Secondary = secondary;
                Trousers = trousers;
                Shoes = shoes;
                Accent = accent;
            }
        }

        public static readonly OutfitDefinition[] Outfits =
        {
            new("University Casual", "1991", "Tucked ivory shirt, charcoal slacks, white court shoes and a brown shoulder bag.",
                C(0.88f,0.85f,0.76f), C(0.73f,0.67f,0.55f), C(0.13f,0.14f,0.15f), C(0.76f,0.74f,0.66f), C(0.25f,0.17f,0.10f)),
            new("Denim Street", "1989", "Washed indigo denim jacket, pale tee, straight dark jeans and black trainers.",
                C(0.12f,0.24f,0.39f), C(0.70f,0.69f,0.63f), C(0.08f,0.10f,0.13f), C(0.08f,0.08f,0.09f), C(0.52f,0.42f,0.29f)),
            new("Varsity", "1990", "Burgundy varsity jacket with cream sleeves, grey tee and relaxed navy denim.",
                C(0.31f,0.055f,0.07f), C(0.76f,0.71f,0.59f), C(0.08f,0.12f,0.20f), C(0.78f,0.76f,0.69f), C(0.77f,0.65f,0.35f)),
            new("Smart Date", "1992", "Dark tailored blazer, light shirt, pleated brown trousers and polished shoes.",
                C(0.075f,0.09f,0.12f), C(0.81f,0.82f,0.76f), C(0.19f,0.17f,0.15f), C(0.07f,0.055f,0.045f), C(0.35f,0.08f,0.07f)),
            new("Night Leather", "1989", "Short black leather jacket, wine tee, faded jeans and dark boots.",
                C(0.035f,0.038f,0.043f), C(0.30f,0.055f,0.055f), C(0.17f,0.20f,0.23f), C(0.045f,0.045f,0.045f), C(0.20f,0.13f,0.08f)),
        };

        static readonly Dictionary<string, Material> Materials = new();
        static Color C(float r, float g, float b) => new(r,g,b,1f);

        public static GameObject Build(Transform parent)
        {
            var old = parent.Find("Visual");
            if (old) Object.Destroy(old.gameObject);

            var visual = new GameObject("Visual");
            visual.transform.SetParent(parent, false);

            var rig = Bone(visual.transform, "Rig", Vector3.zero);
            var root = Bone(rig, "Root", Vector3.zero);
            var pelvis = Bone(root, "Pelvis", new Vector3(0f, 0.90f, 0f));
            var spine = Bone(pelvis, "Spine", new Vector3(0f, 0.18f, 0f));
            var chest = Bone(spine, "Chest", new Vector3(0f, 0.24f, 0f));
            var neck = Bone(chest, "Neck", new Vector3(0f, 0.20f, 0f));
            var head = Bone(neck, "Head", new Vector3(0f, 0.10f, 0f));

            var lShoulder = Bone(chest, "LeftShoulder", new Vector3(-0.225f, 0.16f, 0f));
            var rShoulder = Bone(chest, "RightShoulder", new Vector3(0.225f, 0.16f, 0f));
            var lElbow = Bone(lShoulder, "LeftElbow", new Vector3(0f, -0.30f, 0f));
            var rElbow = Bone(rShoulder, "RightElbow", new Vector3(0f, -0.30f, 0f));
            var lWrist = Bone(lElbow, "LeftWrist", new Vector3(0f, -0.27f, 0f));
            var rWrist = Bone(rElbow, "RightWrist", new Vector3(0f, -0.27f, 0f));

            var lThigh = Bone(pelvis, "LeftThigh", new Vector3(-0.105f, -0.06f, 0f));
            var rThigh = Bone(pelvis, "RightThigh", new Vector3(0.105f, -0.06f, 0f));
            var lKnee = Bone(lThigh, "LeftKnee", new Vector3(0f, -0.38f, 0f));
            var rKnee = Bone(rThigh, "RightKnee", new Vector3(0f, -0.38f, 0f));
            var lAnkle = Bone(lKnee, "LeftAnkle", new Vector3(0f, -0.37f, 0f));
            var rAnkle = Bone(rKnee, "RightAnkle", new Vector3(0f, -0.37f, 0f));

            BuildFace(head);
            BuildHands(lWrist, rWrist);

            for (int i = 0; i < Outfits.Length; i++)
                BuildOutfit(i, Outfits[i], pelvis, spine, chest, lShoulder, rShoulder, lElbow, rElbow, lWrist, rWrist, lThigh, rThigh, lKnee, rKnee, lAnkle, rAnkle);

            ApplyOutfit(visual.transform, 0);

            var animator = parent.GetComponent<LynxHeroAnimator>() ?? parent.gameObject.AddComponent<LynxHeroAnimator>();
            animator.Bind(visual.transform);
            return visual;
        }

        public static void ApplyOutfit(Transform visual, int index)
        {
            index = Mathf.Clamp(index, 0, Outfits.Length - 1);
            foreach (var renderer in visual.GetComponentsInChildren<Renderer>(true))
            {
                if (!renderer.name.StartsWith("O")) continue;
                int separator = renderer.name.IndexOf('_');
                if (separator <= 1) continue;
                if (int.TryParse(renderer.name.Substring(1, separator - 1), out int outfit))
                    renderer.enabled = outfit == index;
            }
        }

        static void BuildFace(Transform head)
        {
            var skin = Mat("skin", C(0.74f,0.56f,0.43f), 0.34f);
            var skinWarm = Mat("skin_warm", C(0.68f,0.48f,0.37f), 0.30f);
            var hair = Mat("hair", C(0.018f,0.016f,0.015f), 0.22f);
            var eyeWhite = Mat("eye_white", C(0.90f,0.89f,0.84f), 0.48f);
            var iris = Mat("iris", C(0.055f,0.038f,0.026f), 0.56f);
            var pupil = Mat("pupil", C(0.008f,0.008f,0.008f), 0.32f);
            var lip = Mat("lip", C(0.43f,0.22f,0.19f), 0.32f);

            Part(head, "Skin_Head", PrimitiveType.Sphere, new Vector3(0f,0.015f,0f), new Vector3(0.28f,0.30f,0.26f), skin);
            Part(head, "Skin_Jaw", PrimitiveType.Sphere, new Vector3(0f,-0.082f,0.010f), new Vector3(0.22f,0.16f,0.21f), skin);
            Part(head, "Skin_LeftEar", PrimitiveType.Sphere, new Vector3(-0.145f,0.010f,0f), new Vector3(0.035f,0.065f,0.025f), skinWarm);
            Part(head, "Skin_RightEar", PrimitiveType.Sphere, new Vector3(0.145f,0.010f,0f), new Vector3(0.035f,0.065f,0.025f), skinWarm);
            Part(head, "Skin_Nose", PrimitiveType.Sphere, new Vector3(0f,0.005f,0.143f), new Vector3(0.045f,0.065f,0.055f), skinWarm);

            Eye(head, -0.072f, eyeWhite, iris, pupil);
            Eye(head,  0.072f, eyeWhite, iris, pupil);
            Part(head, "Face_LeftBrow", PrimitiveType.Cube, new Vector3(-0.073f,0.090f,0.128f), new Vector3(0.085f,0.014f,0.012f), hair, new Vector3(0f,0f,-5f));
            Part(head, "Face_RightBrow", PrimitiveType.Cube, new Vector3(0.073f,0.090f,0.128f), new Vector3(0.085f,0.014f,0.012f), hair, new Vector3(0f,0f,5f));
            Part(head, "Face_UpperLip", PrimitiveType.Cube, new Vector3(0f,-0.070f,0.133f), new Vector3(0.085f,0.012f,0.010f), lip);
            Part(head, "Face_LowerLip", PrimitiveType.Cube, new Vector3(0f,-0.087f,0.131f), new Vector3(0.073f,0.011f,0.010f), lip);

            // Layered 1990-era natural black hair: a crown plus side/back masses and a broken fringe.
            Part(head, "Hair_Crown", PrimitiveType.Sphere, new Vector3(0f,0.125f,-0.005f), new Vector3(0.29f,0.14f,0.27f), hair);
            Part(head, "Hair_Back", PrimitiveType.Sphere, new Vector3(0f,0.065f,-0.115f), new Vector3(0.25f,0.16f,0.075f), hair);
            Part(head, "Hair_LeftSide", PrimitiveType.Capsule, new Vector3(-0.125f,0.065f,-0.025f), new Vector3(0.070f,0.14f,0.055f), hair, new Vector3(0f,0f,-7f));
            Part(head, "Hair_RightSide", PrimitiveType.Capsule, new Vector3(0.125f,0.065f,-0.025f), new Vector3(0.070f,0.14f,0.055f), hair, new Vector3(0f,0f,7f));
            Part(head, "Hair_FringeL", PrimitiveType.Capsule, new Vector3(-0.050f,0.105f,0.112f), new Vector3(0.046f,0.095f,0.035f), hair, new Vector3(62f,0f,-18f));
            Part(head, "Hair_FringeR", PrimitiveType.Capsule, new Vector3(0.050f,0.108f,0.110f), new Vector3(0.046f,0.090f,0.035f), hair, new Vector3(62f,0f,18f));
        }

        static void Eye(Transform head, float x, Material white, Material iris, Material pupil)
        {
            Part(head, x < 0 ? "Face_LeftEye" : "Face_RightEye", PrimitiveType.Sphere, new Vector3(x,0.035f,0.125f), new Vector3(0.058f,0.032f,0.018f), white);
            Part(head, x < 0 ? "Face_LeftIris" : "Face_RightIris", PrimitiveType.Sphere, new Vector3(x,0.035f,0.136f), new Vector3(0.021f,0.021f,0.009f), iris);
            Part(head, x < 0 ? "Face_LeftPupil" : "Face_RightPupil", PrimitiveType.Sphere, new Vector3(x,0.035f,0.142f), new Vector3(0.009f,0.009f,0.006f), pupil);
        }

        static void BuildHands(Transform leftWrist, Transform rightWrist)
        {
            var skin = Mat("skin", C(0.74f,0.56f,0.43f), 0.34f);
            Part(leftWrist, "Skin_LeftHand", PrimitiveType.Sphere, new Vector3(0f,-0.045f,0.015f), new Vector3(0.105f,0.14f,0.075f), skin);
            Part(rightWrist, "Skin_RightHand", PrimitiveType.Sphere, new Vector3(0f,-0.045f,0.015f), new Vector3(0.105f,0.14f,0.075f), skin);
        }

        static void BuildOutfit(int i, OutfitDefinition o, Transform pelvis, Transform spine, Transform chest,
            Transform lShoulder, Transform rShoulder, Transform lElbow, Transform rElbow, Transform lWrist, Transform rWrist,
            Transform lThigh, Transform rThigh, Transform lKnee, Transform rKnee, Transform lAnkle, Transform rAnkle)
        {
            var primary = Mat($"o{i}_primary", o.Primary, i == 4 ? 0.62f : 0.30f, i == 4 ? 0.08f : 0f);
            var secondary = Mat($"o{i}_secondary", o.Secondary, 0.26f);
            var trousers = Mat($"o{i}_trousers", o.Trousers, 0.24f);
            var shoes = Mat($"o{i}_shoes", o.Shoes, i == 3 ? 0.58f : 0.32f);
            var accent = Mat($"o{i}_accent", o.Accent, 0.38f);

            // Trousers use articulated upper/lower segments so knees can bend convincingly.
            Clothing(lThigh, $"O{i}_LeftThigh", PrimitiveType.Capsule, new Vector3(0f,-0.19f,0f), new Vector3(0.205f,0.22f,0.205f), trousers);
            Clothing(rThigh, $"O{i}_RightThigh", PrimitiveType.Capsule, new Vector3(0f,-0.19f,0f), new Vector3(0.205f,0.22f,0.205f), trousers);
            Clothing(lKnee, $"O{i}_LeftCalf", PrimitiveType.Capsule, new Vector3(0f,-0.185f,0f), new Vector3(0.185f,0.215f,0.185f), trousers);
            Clothing(rKnee, $"O{i}_RightCalf", PrimitiveType.Capsule, new Vector3(0f,-0.185f,0f), new Vector3(0.185f,0.215f,0.185f), trousers);
            Clothing(lAnkle, $"O{i}_LeftShoe", PrimitiveType.Cube, new Vector3(0f,-0.05f,0.085f), new Vector3(0.22f,0.12f,0.36f), shoes);
            Clothing(rAnkle, $"O{i}_RightShoe", PrimitiveType.Cube, new Vector3(0f,-0.05f,0.085f), new Vector3(0.22f,0.12f,0.36f), shoes);

            float torsoWidth = i == 2 ? 0.43f : (i == 3 ? 0.395f : 0.41f);
            float torsoDepth = i == 4 ? 0.235f : 0.22f;
            Clothing(chest, $"O{i}_Torso", PrimitiveType.Capsule, new Vector3(0f,-0.11f,0f), new Vector3(torsoWidth,0.33f,torsoDepth), primary);
            Clothing(pelvis, $"O{i}_Waist", PrimitiveType.Capsule, new Vector3(0f,0.08f,0f), new Vector3(0.35f,0.19f,0.205f), trousers);

            // Shirt/tee visible at the chest opening.
            Clothing(chest, $"O{i}_ChestInsert", PrimitiveType.Cube, new Vector3(0f,0.035f,0.216f), new Vector3(0.22f,0.22f,0.024f), secondary);

            Material upperSleeve = i == 2 ? secondary : primary;
            Material lowerSleeve = i == 0 ? secondary : upperSleeve;
            Clothing(lShoulder, $"O{i}_LUpperSleeve", PrimitiveType.Capsule, new Vector3(0f,-0.16f,0f), new Vector3(0.165f,0.20f,0.165f), upperSleeve);
            Clothing(rShoulder, $"O{i}_RUpperSleeve", PrimitiveType.Capsule, new Vector3(0f,-0.16f,0f), new Vector3(0.165f,0.20f,0.165f), upperSleeve);
            Clothing(lElbow, $"O{i}_LForearmSleeve", PrimitiveType.Capsule, new Vector3(0f,-0.15f,0f), new Vector3(0.145f,0.185f,0.145f), lowerSleeve);
            Clothing(rElbow, $"O{i}_RForearmSleeve", PrimitiveType.Capsule, new Vector3(0f,-0.15f,0f), new Vector3(0.145f,0.185f,0.145f), lowerSleeve);

            if (i == 0)
            {
                // Open collar, belt and shoulder bag make the default silhouette distinctly student-like.
                Clothing(chest, $"O{i}_CollarL", PrimitiveType.Cube, new Vector3(-0.07f,0.095f,0.235f), new Vector3(0.095f,0.055f,0.018f), secondary, new Vector3(0f,0f,-25f));
                Clothing(chest, $"O{i}_CollarR", PrimitiveType.Cube, new Vector3(0.07f,0.095f,0.235f), new Vector3(0.095f,0.055f,0.018f), secondary, new Vector3(0f,0f,25f));
                Clothing(pelvis, $"O{i}_Belt", PrimitiveType.Cube, new Vector3(0f,0.13f,0.205f), new Vector3(0.33f,0.035f,0.024f), accent);
                Clothing(chest, $"O{i}_BagStrap", PrimitiveType.Cube, new Vector3(-0.07f,-0.02f,-0.24f), new Vector3(0.045f,0.54f,0.025f), accent, new Vector3(0f,0f,-24f));
                Clothing(spine, $"O{i}_ShoulderBag", PrimitiveType.Cube, new Vector3(-0.36f,-0.18f,-0.18f), new Vector3(0.30f,0.34f,0.16f), accent);
            }
            else if (i == 1)
            {
                Clothing(chest, $"O{i}_DenimPlacket", PrimitiveType.Cube, new Vector3(0f,-0.08f,0.236f), new Vector3(0.032f,0.34f,0.022f), secondary);
                Clothing(chest, $"O{i}_PocketL", PrimitiveType.Cube, new Vector3(-0.15f,-0.03f,0.238f), new Vector3(0.13f,0.11f,0.018f), primary);
                Clothing(chest, $"O{i}_PocketR", PrimitiveType.Cube, new Vector3(0.15f,-0.03f,0.238f), new Vector3(0.13f,0.11f,0.018f), primary);
            }
            else if (i == 2)
            {
                Clothing(chest, $"O{i}_VarsityRib", PrimitiveType.Cube, new Vector3(0f,-0.36f,0.01f), new Vector3(0.40f,0.055f,0.225f), secondary);
                Clothing(chest, $"O{i}_LetterPatch", PrimitiveType.Cube, new Vector3(-0.14f,0.02f,0.238f), new Vector3(0.10f,0.12f,0.018f), accent);
            }
            else if (i == 3)
            {
                Clothing(chest, $"O{i}_LapelL", PrimitiveType.Cube, new Vector3(-0.085f,0.04f,0.24f), new Vector3(0.105f,0.24f,0.022f), primary, new Vector3(0f,0f,-18f));
                Clothing(chest, $"O{i}_LapelR", PrimitiveType.Cube, new Vector3(0.085f,0.04f,0.24f), new Vector3(0.105f,0.24f,0.022f), primary, new Vector3(0f,0f,18f));
                Clothing(chest, $"O{i}_PocketSquare", PrimitiveType.Cube, new Vector3(0.145f,0.03f,0.248f), new Vector3(0.075f,0.04f,0.016f), accent);
            }
            else
            {
                Clothing(chest, $"O{i}_LeatherZip", PrimitiveType.Cube, new Vector3(0.035f,-0.09f,0.245f), new Vector3(0.018f,0.34f,0.014f), accent, new Vector3(0f,0f,-5f));
                Clothing(chest, $"O{i}_LeatherCollarL", PrimitiveType.Cube, new Vector3(-0.09f,0.08f,0.245f), new Vector3(0.12f,0.08f,0.022f), primary, new Vector3(0f,0f,-28f));
                Clothing(chest, $"O{i}_LeatherCollarR", PrimitiveType.Cube, new Vector3(0.09f,0.08f,0.245f), new Vector3(0.12f,0.08f,0.022f), primary, new Vector3(0f,0f,28f));
            }

            // Analog watch remains part of every outfit, with a more formal band on Smart Date.
            Clothing(lWrist, $"O{i}_WatchBand", PrimitiveType.Cube, new Vector3(0f,0.025f,0f), new Vector3(0.12f,0.045f,0.10f), accent);
            Clothing(lWrist, $"O{i}_WatchFace", PrimitiveType.Cube, new Vector3(0f,0.025f,0.060f), new Vector3(0.070f,0.050f,0.025f), Mat($"o{i}_watchface", C(0.68f,0.66f,0.57f), 0.65f, 0.15f));
        }

        static Transform Bone(Transform parent, string name, Vector3 localPosition)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            return go.transform;
        }

        static GameObject Clothing(Transform parent, string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material, Vector3 euler = default)
            => Part(parent, name, type, position, scale, material, euler);

        static GameObject Part(Transform parent, string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material, Vector3 euler = default)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            go.transform.localScale = scale;
            go.transform.localRotation = Quaternion.Euler(euler);
            if (go.TryGetComponent<Collider>(out var collider)) collider.enabled = false;
            if (go.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                renderer.receiveShadows = true;
            }
            return go;
        }

        static Material Mat(string key, Color color, float smoothness, float metallic = 0f)
        {
            if (Materials.TryGetValue(key, out var existing) && existing) return existing;
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Diffuse");
            var material = new Material(shader) { name = $"Lynx_{key}_Runtime" };
            if (material.HasProperty("_BaseColor")) material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Color")) material.SetColor("_Color", color);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
            if (material.HasProperty("_Glossiness")) material.SetFloat("_Glossiness", smoothness);
            if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
            Materials[key] = material;
            return material;
        }
    }
}

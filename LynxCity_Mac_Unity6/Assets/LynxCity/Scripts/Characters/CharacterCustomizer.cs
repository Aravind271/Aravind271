using UnityEngine;
using UnityEngine.InputSystem;
using LynxCity.Player;

namespace LynxCity.Characters
{
    public sealed class CharacterCustomizer : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        LynxAppearance appearance;
        LynxMotor motor;
        ThirdPersonCamera cameraRig;
        GUIStyle title, body, selected, hint, small;

        void Awake()
        {
            appearance = GetComponent<LynxAppearance>();
            motor = GetComponent<LynxMotor>();
            title = Style(27, FontStyle.Bold, Color.white);
            body = Style(17, FontStyle.Normal, new Color(.92f,.92f,.92f));
            selected = Style(18, FontStyle.Bold, Color.white);
            hint = Style(14, FontStyle.Normal, new Color(.82f,.82f,.82f));
            small = Style(13, FontStyle.Normal, new Color(.72f,.72f,.72f));
        }

        void Start() => cameraRig = Camera.main ? Camera.main.GetComponent<ThirdPersonCamera>() : null;

        void Update()
        {
            var kb = Keyboard.current;
            if (kb == null || appearance == null) return;
            if (kb.cKey.wasPressedThisFrame) SetOpen(!IsOpen);
            if (!IsOpen) return;

            if (kb.leftArrowKey.wasPressedThisFrame) appearance.Cycle(-1);
            if (kb.rightArrowKey.wasPressedThisFrame) appearance.Cycle(1);
            if (kb.digit1Key.wasPressedThisFrame) appearance.Apply(0);
            if (kb.digit2Key.wasPressedThisFrame) appearance.Apply(1);
            if (kb.digit3Key.wasPressedThisFrame) appearance.Apply(2);
            if (kb.digit4Key.wasPressedThisFrame) appearance.Apply(3);
            if (kb.digit5Key.wasPressedThisFrame) appearance.Apply(4);

            float rotate = 0f;
            if (kb.qKey.isPressed) rotate += 1f;
            if (kb.eKey.isPressed) rotate -= 1f;
            if (Mathf.Abs(rotate) > .01f) transform.Rotate(Vector3.up, rotate * 58f * Time.deltaTime, Space.World);
        }

        void SetOpen(bool open)
        {
            IsOpen = open;
            if (motor) motor.ControlsLocked = open;
            if (!cameraRig && Camera.main) cameraRig = Camera.main.GetComponent<ThirdPersonCamera>();
            if (cameraRig) cameraRig.SetWardrobeMode(open);
        }

        void OnGUI()
        {
            if (!IsOpen || appearance == null) return;
            float w = Mathf.Min(520f, Screen.width - 40f);
            float h = 405f;
            var rect = new Rect(25f, Screen.height * .5f - h * .5f, w, h);
            var old = GUI.color;
            GUI.color = new Color(0.035f,0.038f,0.045f,.94f);
            GUI.Box(rect, GUIContent.none);
            GUI.color = old;

            GUI.Label(new Rect(rect.x+26, rect.y+22, w-52, 36), "LYNX — WARDROBE", title);
            GUI.Label(new Rect(rect.x+26, rect.y+58, w-52, 26), "Tokyo student wardrobe • late 1980s / early 1990s", small);

            for (int i=0; i<LynxHeroFactory.Outfits.Length; i++)
            {
                var o = LynxHeroFactory.Outfits[i];
                bool active = i == appearance.OutfitIndex;
                if (active)
                {
                    GUI.color = new Color(.24f,.28f,.34f,.95f);
                    GUI.Box(new Rect(rect.x+20, rect.y+96+i*43, w-40, 38), GUIContent.none);
                    GUI.color = old;
                }
                GUI.Label(new Rect(rect.x+34, rect.y+103+i*43, w-68, 30), $"{i+1}. {o.Name}  ·  {o.Year}", active ? selected : body);
            }

            var current = appearance.Current;
            GUI.Label(new Rect(rect.x+26, rect.y+323, w-52, 42), current.Description, hint);
            GUI.Label(new Rect(rect.x+26, rect.y+372, w-52, 25), "← → / 1–5 change • Q/E rotate • C finish", small);
        }

        static GUIStyle Style(int size, FontStyle style, Color color)
        {
            var s = new GUIStyle { fontSize=size, fontStyle=style, wordWrap=true };
            s.normal.textColor = color;
            return s;
        }
    }
}

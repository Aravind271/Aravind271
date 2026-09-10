using UnityEngine;
using LynxCity.Core;
using LynxCity.Characters;
using LynxCity.Player;

namespace LynxCity.UI
{
    public sealed class PrototypeHUD : MonoBehaviour
    {
        public GameObject player;
        GUIStyle title, body, small, strong;
        float fps;

        void Awake()
        {
            title = Style(21, FontStyle.Bold, Color.white);
            strong = Style(15, FontStyle.Bold, new Color(.96f,.96f,.96f));
            body = Style(14, FontStyle.Normal, new Color(.90f,.90f,.90f));
            small = Style(12, FontStyle.Normal, new Color(.74f,.76f,.78f));
        }

        void Update()
        {
            float instant = 1f / Mathf.Max(.001f, Time.unscaledDeltaTime);
            fps = Mathf.Lerp(fps <= 0f ? instant : fps, instant, .08f);
        }

        void OnGUI()
        {
            var state = GameState.Instance;
            if (state == null) return;
            var appearance = player ? player.GetComponent<LynxAppearance>() : null;
            var motor = player ? player.GetComponent<LynxMotor>() : null;
            var customizer = player ? player.GetComponent<CharacterCustomizer>() : null;

            var old = GUI.color;
            GUI.color = new Color(.025f,.028f,.034f,.90f);
            GUI.Box(new Rect(18,18,430,144), GUIContent.none);
            GUI.color = old;
            GUI.Label(new Rect(32,29,390,26), "LYNX CITY — V2.1 CHARACTER LAB", title);
            GUI.Label(new Rect(32,59,390,22), "Native macOS • Apple Silicon • 1991", strong);
            GUI.Label(new Rect(32,83,390,22), $"Attire: {(appearance ? appearance.OutfitName : state.CurrentOutfit)}", body);
            GUI.Label(new Rect(32,105,390,22), $"Locomotion: {(motor != null && motor.IsSprinting ? "RUN" : motor != null && motor.CurrentSpeed > .08f ? "WALK" : "IDLE")}   •   {fps:0} FPS", body);
            GUI.Label(new Rect(32,131,390,22), "WASD move • Shift run • mouse camera • wheel zoom • C wardrobe", small);

            if (customizer == null || !customizer.IsOpen)
            {
                GUI.color = new Color(.025f,.028f,.034f,.80f);
                GUI.Box(new Rect(Screen.width/2f-205, Screen.height-68, 410, 40), GUIContent.none);
                GUI.color = old;
                GUI.Label(new Rect(Screen.width/2f-188, Screen.height-57, 380, 24), "V2.1 goal: judge Lynx first — city development is paused.", small);
            }
        }

        static GUIStyle Style(int size, FontStyle style, Color color)
        {
            var s = new GUIStyle { fontSize=size, fontStyle=style, wordWrap=true };
            s.normal.textColor = color;
            return s;
        }
    }
}

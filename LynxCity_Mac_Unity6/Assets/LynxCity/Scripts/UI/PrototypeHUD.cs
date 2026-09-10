using UnityEngine;
using LynxCity.Core;
using LynxCity.Combat;

namespace LynxCity.UI
{
    public class PrototypeHUD : MonoBehaviour
    {
        public Health playerHealth;
        GUIStyle title, body;
        void Awake()
        {
            title = new GUIStyle { fontSize = 22, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
            body = new GUIStyle { fontSize = 16, normal = { textColor = Color.white } };
        }
        void OnGUI()
        {
            var s = GameState.Instance;
            if (s == null) return;
            GUI.Box(new Rect(18, 18, 390, 150), "");
            GUI.Label(new Rect(32, 28, 350, 28), "LYNX CITY — prototype", title);
            GUI.Label(new Rect(32, 62, 350, 24), $"{s.CurrentDistrict}    ¥{s.Yen:N0}", body);
            GUI.Label(new Rect(32, 86, 350, 24), $"HP {(playerHealth ? playerHealth.Current : 0):0}    HEAT {s.Heat:0}/100", body);
            GUI.Label(new Rect(32, 112, 360, 44), "WASD move | Shift sprint | LMB combo | RMB heavy | Space dodge | Q Heat", body);
            if (!string.IsNullOrWhiteSpace(s.InteractionPrompt))
            {
                GUI.Box(new Rect(Screen.width/2f-260, Screen.height-90, 520, 55), "");
                GUI.Label(new Rect(Screen.width/2f-245, Screen.height-75, 490, 35), s.InteractionPrompt, body);
            }
        }
    }
}

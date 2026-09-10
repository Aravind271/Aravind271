using UnityEngine;
using LynxCity.Core;
using LynxCity.Combat;

namespace LynxCity.UI
{
    public class PrototypeHUD : MonoBehaviour
    {
        public Health playerHealth;GUIStyle title,body,small;
        void Awake(){title=new GUIStyle{fontSize=22,fontStyle=FontStyle.Bold,normal={textColor=Color.white}};body=new GUIStyle{fontSize=16,normal={textColor=Color.white}};small=new GUIStyle{fontSize=13,normal={textColor=new Color(.88f,.88f,.88f)}};}
        void OnGUI(){var s=GameState.Instance;if(s==null)return;GUI.Box(new Rect(18,18,455,177),"");GUI.Label(new Rect(32,28,410,28),"LYNX CITY — V2 / 1990s Japan",title);GUI.Label(new Rect(32,60,410,24),$"{s.CurrentDistrict}   •   ¥{s.Yen:N0}   •   {s.Year}",body);GUI.Label(new Rect(32,84,410,24),$"HP {(playerHealth?playerHealth.Current:0):0}   HEAT {s.Heat:0}/100",body);GUI.Label(new Rect(32,108,415,24),$"Attire: {s.CurrentOutfit}",body);GUI.Label(new Rect(32,134,420,42),"LMB light • RMB heavy • F guard • R grab • Space dodge • Tab lock • Q Heat",small);GUI.Label(new Rect(32,165,420,22),"WASD move • Shift sprint • C wardrobe • Esc cursor",small);if(!string.IsNullOrWhiteSpace(s.InteractionPrompt)){GUI.Box(new Rect(Screen.width/2f-280,Screen.height-92,560,56),"");GUI.Label(new Rect(Screen.width/2f-260,Screen.height-75,520,34),s.InteractionPrompt,body);}var combat=Object.FindFirstObjectByType<BrawlerCombat>();if(combat&&combat.LockedTarget)GUI.Label(new Rect(Screen.width/2f-55,Screen.height/2f-60,110,30),"◉ LOCK",body);}
    }
}

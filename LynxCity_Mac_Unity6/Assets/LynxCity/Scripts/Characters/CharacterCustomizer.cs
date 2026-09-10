using UnityEngine;
using UnityEngine.InputSystem;
using LynxCity.Player;

namespace LynxCity.Characters
{
    public class CharacterCustomizer : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        LynxAppearance appearance; LynxMotor motor; GUIStyle title,body;
        void Awake(){ appearance=GetComponent<LynxAppearance>(); motor=GetComponent<LynxMotor>(); title=new GUIStyle{fontSize=26,fontStyle=FontStyle.Bold,normal={textColor=Color.white}}; body=new GUIStyle{fontSize=18,normal={textColor=Color.white}}; }
        void Update(){ var kb=Keyboard.current;if(kb==null)return; if(kb.cKey.wasPressedThisFrame){IsOpen=!IsOpen;if(motor)motor.ControlsLocked=IsOpen;} if(!IsOpen||appearance==null)return; if(kb.digit1Key.wasPressedThisFrame)appearance.Apply(0); if(kb.digit2Key.wasPressedThisFrame)appearance.Apply(1); if(kb.digit3Key.wasPressedThisFrame)appearance.Apply(2); if(kb.digit4Key.wasPressedThisFrame)appearance.Apply(3); if(kb.digit5Key.wasPressedThisFrame)appearance.Apply(4); }
        void OnGUI(){ if(!IsOpen||appearance==null)return; float w=470f,h=290f;var rect=new Rect(Screen.width/2f-w/2f,Screen.height/2f-h/2f,w,h);GUI.Box(rect,"");GUI.Label(new Rect(rect.x+25,rect.y+20,w-50,36),"LYNX — WARDROBE",title);GUI.Label(new Rect(rect.x+25,rect.y+62,w-50,28),"Late Showa / early Heisei attire",body);for(int i=0;i<HumanFigureFactory.LynxOutfits.Length;i++){string mark=i==appearance.OutfitIndex?"  ◀":"";GUI.Label(new Rect(rect.x+35,rect.y+102+i*30,w-70,27),$"{i+1}. {HumanFigureFactory.LynxOutfits[i].name}{mark}",body);}GUI.Label(new Rect(rect.x+25,rect.y+h-42,w-50,28),"1–5 choose attire   •   C close",body); }
    }
}

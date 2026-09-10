using UnityEngine;
using UnityEngine.InputSystem;
using LynxCity.Core;

namespace LynxCity.Interaction
{
    public class GameInteractor : MonoBehaviour
    {
        public float range=2.4f;public LayerMask mask=~0;IInteractable current;
        void Update(){current=null;float best=range;foreach(var col in Physics.OverlapSphere(transform.position,range,mask,QueryTriggerInteraction.Collide)){foreach(var b in col.GetComponents<MonoBehaviour>()){if(b is IInteractable i){float d=Vector3.Distance(transform.position,col.ClosestPoint(transform.position));if(d<best){best=d;current=i;}}}}if(GameState.Instance)GameState.Instance.InteractionPrompt=current!=null?current.Prompt:"";bool pressed=(Keyboard.current!=null&&Keyboard.current.eKey.wasPressedThisFrame)||(Gamepad.current!=null&&Gamepad.current.buttonSouth.wasPressedThisFrame);if(pressed&&current!=null)current.Interact(this);}
    }
}

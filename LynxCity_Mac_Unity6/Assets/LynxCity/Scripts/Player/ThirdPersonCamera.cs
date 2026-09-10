using UnityEngine;
using UnityEngine.InputSystem;

namespace LynxCity.Player
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;public Vector3 targetOffset=new(0,1.42f,0);public float distance=4.6f,sensitivity=115f,smooth=18f;float yaw=25f,pitch=17f;
        void Start()=>SetCursor(true);
        void Update(){if(Keyboard.current!=null&&Keyboard.current.escapeKey.wasPressedThisFrame)SetCursor(Cursor.lockState!=CursorLockMode.Locked);if(Mouse.current!=null&&Mouse.current.leftButton.wasPressedThisFrame&&Cursor.lockState!=CursorLockMode.Locked)SetCursor(true);}
        void LateUpdate(){if(!target)return;Vector2 look=Vector2.zero;if(Mouse.current!=null&&Cursor.lockState==CursorLockMode.Locked)look=Mouse.current.delta.ReadValue()*.075f;if(Gamepad.current!=null)look+=Gamepad.current.rightStick.ReadValue()*8f;yaw+=look.x*sensitivity*Time.deltaTime;pitch=Mathf.Clamp(pitch-look.y*sensitivity*Time.deltaTime,-8f,58f);Quaternion rot=Quaternion.Euler(pitch,yaw,0);Vector3 focus=target.position+targetOffset;Vector3 desired=focus-rot*Vector3.forward*distance;if(Physics.Linecast(focus,desired,out var hit,~0,QueryTriggerInteraction.Ignore)&&hit.transform.root!=target)desired=hit.point+hit.normal*.18f;transform.position=Vector3.Lerp(transform.position,desired,1f-Mathf.Exp(-smooth*Time.deltaTime));transform.rotation=Quaternion.Slerp(transform.rotation,rot,1f-Mathf.Exp(-smooth*Time.deltaTime));}
        static void SetCursor(bool locked){Cursor.lockState=locked?CursorLockMode.Locked:CursorLockMode.None;Cursor.visible=!locked;}
    }
}

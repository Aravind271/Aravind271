using UnityEngine;
using UnityEngine.InputSystem;

namespace LynxCity.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class LynxMotor : MonoBehaviour
    {
        public float walkSpeed=3.8f,sprintSpeed=6.7f,rotationSharpness=14f,gravity=-22f;public Transform cameraTransform;public Vector3 DesiredMove{get;private set;}public float CurrentSpeed01{get;private set;}public bool ControlsLocked{get;set;}CharacterController controller;float yVelocity;
        void Awake()=>controller=GetComponent<CharacterController>();
        void Update(){if(ControlsLocked){DesiredMove=Vector3.zero;CurrentSpeed01=0f;ApplyGravity();return;}var kb=Keyboard.current;var pad=Gamepad.current;Vector2 input=Vector2.zero;if(kb!=null){input.x=(kb.dKey.isPressed?1:0)-(kb.aKey.isPressed?1:0);input.y=(kb.wKey.isPressed?1:0)-(kb.sKey.isPressed?1:0);}if(pad!=null&&pad.leftStick.ReadValue().sqrMagnitude>input.sqrMagnitude)input=pad.leftStick.ReadValue();bool sprint=(kb!=null&&kb.leftShiftKey.isPressed)||(pad!=null&&pad.leftStickButton.isPressed);float speed=sprint?sprintSpeed:walkSpeed;Vector3 forward=cameraTransform?cameraTransform.forward:Vector3.forward;Vector3 right=cameraTransform?cameraTransform.right:Vector3.right;forward.y=0;right.y=0;forward.Normalize();right.Normalize();DesiredMove=forward*input.y+right*input.x;if(DesiredMove.sqrMagnitude>1f)DesiredMove.Normalize();CurrentSpeed01=Mathf.Clamp01(DesiredMove.magnitude*(sprint?1f:.62f));if(DesiredMove.sqrMagnitude>.01f){var target=Quaternion.LookRotation(DesiredMove);transform.rotation=Quaternion.Slerp(transform.rotation,target,1f-Mathf.Exp(-rotationSharpness*Time.deltaTime));}if(controller.isGrounded&&yVelocity<0)yVelocity=-2f;yVelocity+=gravity*Time.deltaTime;controller.Move((DesiredMove*speed+Vector3.up*yVelocity)*Time.deltaTime);}
        void ApplyGravity(){if(controller.isGrounded&&yVelocity<0)yVelocity=-2f;yVelocity+=gravity*Time.deltaTime;controller.Move(Vector3.up*yVelocity*Time.deltaTime);}
    }
}

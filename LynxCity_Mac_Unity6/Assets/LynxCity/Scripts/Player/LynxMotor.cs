using UnityEngine;
using UnityEngine.InputSystem;

namespace LynxCity.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class LynxMotor : MonoBehaviour
    {
        public float walkSpeed = 4.2f;
        public float sprintSpeed = 7.2f;
        public float rotationSharpness = 14f;
        public float gravity = -22f;
        public Transform cameraTransform;
        CharacterController controller;
        float yVelocity;
        public bool ControlsLocked { get; set; }

        void Awake() => controller = GetComponent<CharacterController>();

        void Update()
        {
            if (ControlsLocked) return;
            var kb = Keyboard.current;
            var pad = Gamepad.current;
            Vector2 input = Vector2.zero;
            if (kb != null)
            {
                input.x = (kb.dKey.isPressed ? 1 : 0) - (kb.aKey.isPressed ? 1 : 0);
                input.y = (kb.wKey.isPressed ? 1 : 0) - (kb.sKey.isPressed ? 1 : 0);
            }
            if (pad != null && pad.leftStick.ReadValue().sqrMagnitude > input.sqrMagnitude)
                input = pad.leftStick.ReadValue();

            bool sprint = (kb != null && kb.leftShiftKey.isPressed) || (pad != null && pad.leftStickButton.isPressed);
            float speed = sprint ? sprintSpeed : walkSpeed;

            Vector3 forward = cameraTransform ? cameraTransform.forward : Vector3.forward;
            Vector3 right = cameraTransform ? cameraTransform.right : Vector3.right;
            forward.y = 0; right.y = 0; forward.Normalize(); right.Normalize();
            Vector3 move = (forward * input.y + right * input.x);
            if (move.sqrMagnitude > 1f) move.Normalize();

            if (move.sqrMagnitude > 0.01f)
            {
                var target = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, 1f - Mathf.Exp(-rotationSharpness * Time.deltaTime));
            }

            if (controller.isGrounded && yVelocity < 0) yVelocity = -2f;
            yVelocity += gravity * Time.deltaTime;
            controller.Move((move * speed + Vector3.up * yVelocity) * Time.deltaTime);
        }
    }
}

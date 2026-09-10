using UnityEngine;
using UnityEngine.InputSystem;

namespace LynxCity.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class LynxMotor : MonoBehaviour
    {
        [Header("1990s student locomotion")]
        public float walkSpeed = 2.35f;
        public float sprintSpeed = 5.25f;
        public float acceleration = 11f;
        public float deceleration = 15f;
        public float rotationSharpness = 11f;
        public float gravity = -23f;
        public Transform cameraTransform;

        public Vector3 DesiredMove { get; private set; }
        public Vector3 Velocity => horizontalVelocity + Vector3.up * yVelocity;
        public float CurrentSpeed => horizontalVelocity.magnitude;
        public float CurrentSpeed01 => Mathf.Clamp01(CurrentSpeed / sprintSpeed);
        public bool IsSprinting { get; private set; }
        public bool ControlsLocked { get; set; }

        CharacterController controller;
        Vector3 horizontalVelocity;
        float yVelocity;

        void Awake() => controller = GetComponent<CharacterController>();

        void Update()
        {
            Vector2 input = ControlsLocked ? Vector2.zero : ReadMove();
            float magnitude = Mathf.Clamp01(input.magnitude);
            IsSprinting = !ControlsLocked && ReadSprint();

            Vector3 forward = cameraTransform ? cameraTransform.forward : Vector3.forward;
            Vector3 right = cameraTransform ? cameraTransform.right : Vector3.right;
            forward.y = 0f; right.y = 0f;
            if (forward.sqrMagnitude > .001f) forward.Normalize();
            if (right.sqrMagnitude > .001f) right.Normalize();

            DesiredMove = (forward * input.y + right * input.x);
            if (DesiredMove.sqrMagnitude > 1f) DesiredMove.Normalize();
            float targetSpeed = (IsSprinting ? sprintSpeed : walkSpeed) * magnitude;
            Vector3 targetVelocity = DesiredMove.sqrMagnitude > .001f ? DesiredMove.normalized * targetSpeed : Vector3.zero;
            float rate = targetVelocity.sqrMagnitude > horizontalVelocity.sqrMagnitude ? acceleration : deceleration;
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, rate * Time.deltaTime);

            if (!ControlsLocked && DesiredMove.sqrMagnitude > .015f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(DesiredMove.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f - Mathf.Exp(-rotationSharpness * Time.deltaTime));
            }

            if (controller.isGrounded && yVelocity < 0f) yVelocity = -1.8f;
            yVelocity += gravity * Time.deltaTime;
            controller.Move((horizontalVelocity + Vector3.up * yVelocity) * Time.deltaTime);
        }

        static Vector2 ReadMove()
        {
            Vector2 value = Vector2.zero;
            var kb = Keyboard.current;
            if (kb != null)
            {
                value.x = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);
                value.y = (kb.wKey.isPressed ? 1f : 0f) - (kb.sKey.isPressed ? 1f : 0f);
            }
            var pad = Gamepad.current;
            if (pad != null)
            {
                Vector2 stick = pad.leftStick.ReadValue();
                if (stick.sqrMagnitude > value.sqrMagnitude) value = stick;
            }
            return Vector2.ClampMagnitude(value, 1f);
        }

        static bool ReadSprint()
        {
            var kb = Keyboard.current;
            var pad = Gamepad.current;
            return (kb != null && kb.leftShiftKey.isPressed) || (pad != null && pad.leftStickButton.isPressed);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace LynxCity.Player
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 targetOffset = new(0, 1.5f, 0);
        public float distance = 5f;
        public float sensitivity = 120f;
        public float smooth = 18f;
        float yaw = 25f, pitch = 18f;

        void LateUpdate()
        {
            if (!target) return;
            Vector2 look = Vector2.zero;
            if (Mouse.current != null && Mouse.current.rightButton.isPressed)
                look = Mouse.current.delta.ReadValue() * 0.08f;
            if (Gamepad.current != null)
                look += Gamepad.current.rightStick.ReadValue() * Time.deltaTime * 12f;

            yaw += look.x * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch - look.y * sensitivity * Time.deltaTime, -10f, 65f);
            Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
            Vector3 desired = target.position + targetOffset - rot * Vector3.forward * distance;
            transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-smooth * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f - Mathf.Exp(-smooth * Time.deltaTime));
        }
    }
}

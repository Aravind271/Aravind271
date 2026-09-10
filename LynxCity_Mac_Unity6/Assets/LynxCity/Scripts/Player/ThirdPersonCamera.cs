using UnityEngine;
using UnityEngine.InputSystem;

namespace LynxCity.Player
{
    public sealed class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 targetOffset = new(0f, 1.34f, 0f);
        public float distance = 4.15f;
        public float sensitivity = 0.12f;
        public float positionSmooth = 17f;
        public float rotationSmooth = 20f;
        public float minDistance = 2.7f;
        public float maxDistance = 5.4f;

        float yaw = 0f;
        float pitch = 13f;
        float desiredDistance;
        bool wardrobeMode;
        float savedYaw, savedPitch, savedDistance;
        Vector3 velocity;

        void Start()
        {
            desiredDistance = distance;
            SetCursor(true);
        }

        void Update()
        {
            var kb = Keyboard.current;
            if (kb != null && kb.escapeKey.wasPressedThisFrame)
                SetCursor(Cursor.lockState != CursorLockMode.Locked);
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
                SetCursor(true);

            if (wardrobeMode) return;

            Vector2 look = Vector2.zero;
            if (Mouse.current != null && Cursor.lockState == CursorLockMode.Locked)
            {
                look = Mouse.current.delta.ReadValue();
                float scroll = Mouse.current.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > .01f) desiredDistance = Mathf.Clamp(desiredDistance - scroll * .0026f, minDistance, maxDistance);
            }
            if (Gamepad.current != null) look += Gamepad.current.rightStick.ReadValue() * 17f;

            yaw += look.x * sensitivity;
            pitch = Mathf.Clamp(pitch - look.y * sensitivity, -6f, 52f);
        }

        void LateUpdate()
        {
            if (!target) return;
            distance = Mathf.Lerp(distance, desiredDistance, 1f - Mathf.Exp(-10f * Time.deltaTime));
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 focus = target.position + targetOffset;
            Vector3 desired = focus - rotation * Vector3.forward * distance;

            Vector3 ray = desired - focus;
            float rayLength = ray.magnitude;
            if (rayLength > .01f)
            {
                var hits = Physics.RaycastAll(focus, ray.normalized, rayLength, ~0, QueryTriggerInteraction.Ignore);
                float closest = rayLength;
                Vector3 hitPoint = desired;
                Vector3 hitNormal = Vector3.zero;
                foreach (var hit in hits)
                {
                    if (hit.transform.root == target.root) continue;
                    if (hit.distance < closest) { closest = hit.distance; hitPoint = hit.point; hitNormal = hit.normal; }
                }
                if (closest < rayLength) desired = hitPoint + hitNormal * .16f;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, 1f / Mathf.Max(1f, positionSmooth));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f - Mathf.Exp(-rotationSmooth * Time.deltaTime));
        }

        public void SetWardrobeMode(bool enabled)
        {
            if (enabled == wardrobeMode || !target) return;
            wardrobeMode = enabled;
            if (enabled)
            {
                savedYaw = yaw; savedPitch = pitch; savedDistance = desiredDistance;
                yaw = target.eulerAngles.y + 180f;
                pitch = 4f;
                desiredDistance = 2.45f;
                targetOffset = new Vector3(0f, 1.28f, 0f);
            }
            else
            {
                yaw = savedYaw; pitch = savedPitch; desiredDistance = savedDistance;
                targetOffset = new Vector3(0f, 1.34f, 0f);
            }
        }

        static void SetCursor(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}

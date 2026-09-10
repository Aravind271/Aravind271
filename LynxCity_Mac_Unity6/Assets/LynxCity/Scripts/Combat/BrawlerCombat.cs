using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using LynxCity.Core;
using LynxCity.Player;

namespace LynxCity.Combat
{
    public class BrawlerCombat : MonoBehaviour
    {
        public float hitRadius = 1.15f;
        public float hitReach = 1.25f;
        public LayerMask hittableMask = ~0;
        public float dodgeDistance = 3.5f;
        public float heatActionCost = 45f;
        int comboIndex;
        float lastAttackTime;
        bool busy;
        LynxMotor motor;

        readonly float[] lightDamage = { 8f, 9f, 11f, 16f };

        void Awake() => motor = GetComponent<LynxMotor>();

        void Update()
        {
            if (busy) return;
            bool light = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame);
            bool heavy = (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame && (Keyboard.current == null || !Keyboard.current.leftAltKey.isPressed)) || (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame);
            bool dodge = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame);
            bool heat = (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame);

            if (light) StartCoroutine(LightAttack());
            else if (heavy) StartCoroutine(HeavyAttack());
            else if (dodge) StartCoroutine(Dodge());
            else if (heat) StartCoroutine(HeatAction());
        }

        IEnumerator LightAttack()
        {
            busy = true;
            if (Time.time - lastAttackTime > 0.85f) comboIndex = 0;
            float dmg = lightDamage[comboIndex];
            comboIndex = (comboIndex + 1) % lightDamage.Length;
            lastAttackTime = Time.time;
            yield return new WaitForSeconds(0.11f);
            Hit(dmg, 4f);
            GameState.Instance?.AddHeat(4.5f);
            yield return new WaitForSeconds(0.22f);
            busy = false;
        }

        IEnumerator HeavyAttack()
        {
            busy = true;
            yield return new WaitForSeconds(0.24f);
            Hit(24f, 9f);
            GameState.Instance?.AddHeat(7f);
            yield return new WaitForSeconds(0.42f);
            busy = false;
        }

        IEnumerator Dodge()
        {
            busy = true;
            motor.ControlsLocked = true;
            float t = 0f;
            Vector3 start = transform.position;
            Vector3 end = start + transform.forward * dodgeDistance;
            while (t < 0.18f)
            {
                t += Time.deltaTime;
                var cc = GetComponent<CharacterController>();
                cc.Move((end - transform.position) * Mathf.Min(1f, Time.deltaTime * 15f));
                yield return null;
            }
            motor.ControlsLocked = false;
            busy = false;
        }

        IEnumerator HeatAction()
        {
            if (GameState.Instance == null || !GameState.Instance.SpendHeat(heatActionCost)) yield break;
            busy = true;
            yield return new WaitForSeconds(0.18f);
            Hit(55f, 16f);
            yield return new WaitForSeconds(0.75f);
            busy = false;
        }

        void Hit(float damage, float impulse)
        {
            Vector3 center = transform.position + Vector3.up * 1.1f + transform.forward * hitReach;
            foreach (var col in Physics.OverlapSphere(center, hitRadius, hittableMask, QueryTriggerInteraction.Ignore))
            {
                if (col.transform.root == transform.root) continue;
                var health = col.GetComponentInParent<Health>();
                if (health != null)
                {
                    health.Damage(damage);
                    var rb = col.attachedRigidbody;
                    if (rb != null) rb.AddForce(transform.forward * impulse, ForceMode.VelocityChange);
                    break;
                }
            }
        }
    }
}

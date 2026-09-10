using UnityEngine;

namespace LynxCity.Combat
{
    [RequireComponent(typeof(Health))]
    public class StreetEnemy : MonoBehaviour
    {
        public Transform target;
        public float aggroRange = 8f;
        public float attackRange = 1.6f;
        public float speed = 2.6f;
        public float attackCooldown = 1.2f;
        float nextAttack;
        Health health;

        void Awake() => health = GetComponent<Health>();
        void Update()
        {
            if (health.IsDead || !target) return;
            float d = Vector3.Distance(transform.position, target.position);
            if (d > aggroRange) return;
            Vector3 dir = target.position - transform.position; dir.y = 0;
            if (dir.sqrMagnitude > 0.01f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 8f);
            if (d > attackRange) transform.position += dir.normalized * speed * Time.deltaTime;
            else if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + attackCooldown;
                target.GetComponent<Health>()?.Damage(7f);
            }
        }

        void OnDefeated()
        {
            GetComponent<Collider>().enabled = false;
            transform.localScale = new Vector3(1.15f, 0.28f, 1.15f);
            transform.position += Vector3.down * 0.7f;
            LynxCity.Core.GameState.Instance?.Earn(650);
        }
    }
}

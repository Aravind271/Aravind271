using UnityEngine;

namespace LynxCity.World
{
    public class CrowdWalker : MonoBehaviour
    {
        public Vector3 center;
        public float radius = 22f;
        public float speed = 1.4f;
        Vector3 target;

        void Start() { center = transform.position; Pick(); }
        void Update()
        {
            Vector3 d = target - transform.position; d.y = 0;
            if (d.magnitude < 1f) { Pick(); return; }
            transform.position += d.normalized * speed * Time.deltaTime;
            if (d.sqrMagnitude > .1f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(d), Time.deltaTime * 5f);
        }
        void Pick()
        {
            Vector2 p = Random.insideUnitCircle * radius;
            target = center + new Vector3(p.x, 0, p.y);
        }
    }
}

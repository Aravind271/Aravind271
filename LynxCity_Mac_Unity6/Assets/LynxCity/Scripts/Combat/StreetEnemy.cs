using UnityEngine;
using LynxCity.Characters;

namespace LynxCity.Combat
{
    [RequireComponent(typeof(Health))]
    public class StreetEnemy : MonoBehaviour
    {
        public Transform target;public float aggroRange=8f,attackRange=1.55f,speed=2.35f,attackCooldown=1.2f;float nextAttack,stunnedUntil;Health health;Rigidbody body;public bool IsDefeated=>health==null||health.IsDead;
        void Awake(){health=GetComponent<Health>();body=GetComponent<Rigidbody>();}
        void Start(){if(!transform.Find("Visual"))HumanFigureFactory.BuildJapaneseAdult(transform,"Visual",Random.Range(.94f,1.06f),HumanFigureFactory.LynxOutfits[1]);}
        void Update(){if(IsDefeated||!target||Time.time<stunnedUntil)return;float d=Vector3.Distance(transform.position,target.position);if(d>aggroRange)return;Vector3 dir=target.position-transform.position;dir.y=0;if(dir.sqrMagnitude>.01f)transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(dir),Time.deltaTime*8f);if(d>attackRange)transform.position+=dir.normalized*speed*Time.deltaTime;else if(Time.time>=nextAttack){nextAttack=Time.time+attackCooldown;var b=target.GetComponent<BrawlerCombat>();if(b)b.ReceiveEnemyHit(7f);else target.GetComponent<Health>()?.Damage(7f);}}
        public void ReceiveHit(float damage,float impulse,float stagger){if(IsDefeated)return;health.Damage(damage);stunnedUntil=Mathf.Max(stunnedUntil,Time.time+stagger);if(body)body.AddForce((transform.position-target.position).normalized*impulse+Vector3.up*Mathf.Min(2f,impulse*.12f),ForceMode.VelocityChange);}
        void OnDefeated(){foreach(var c in GetComponents<Collider>())c.enabled=false;transform.rotation=Quaternion.Euler(85,transform.eulerAngles.y,0);LynxCity.Core.GameState.Instance?.Earn(650);enabled=false;}
    }
}

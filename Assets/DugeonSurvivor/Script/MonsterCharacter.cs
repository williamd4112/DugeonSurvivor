using UnityEngine;
using System.Collections;
using WiFramework;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EventBasedRecycleable))]
    public class MonsterCharacter : MonoBehaviour
    {
        [SerializeField]
        private float m_DeathTime = 5.0f;

        private Rigidbody m_Rigidbody;
        private Animator m_Animator;
        private Damageable m_Damageable;
        private NavMeshAgent m_NavAgent;

        private bool m_Death = false;
        private int m_IdleHash = Animator.StringToHash("Base Layer.Idle");
        private int m_DamageHash = Animator.StringToHash("Base Layer.Damage");
        private int m_KnockbackHash = Animator.StringToHash("Base Layer.Knockback");

        // Use this for initialization
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
            m_Damageable = GetComponent<Damageable>();
            m_NavAgent = GetComponent<NavMeshAgent>();

            m_Damageable.RegisterDamageEvent(onStayHit);
            m_Damageable.SetDeathHandler(onDead);
        }

        public void Move(Vector3 pos, bool attack)
        {
            if (m_Death) return;

            int curState = m_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            if (!attack && curState != m_DamageHash && curState != m_KnockbackHash)
            {
                m_NavAgent.SetDestination(pos);
                if(curState == m_IdleHash)
                    m_NavAgent.Resume();
            }
            else
            {
                m_NavAgent.Stop();
            }
            
            updateAnimator(attack);
        }

        void updateAnimator(bool attack)
        {
            float speed = m_NavAgent.velocity.magnitude;
            m_Animator.SetFloat("Speed", speed);

            if (attack)
                m_Animator.SetTrigger("Attack");
        }

        void onStayHit(Damage damage, int hp)
        {
            if (m_Death) return;
            if (damage.damageAmount > 0)
            {
                m_NavAgent.velocity = Vector3.zero;
                m_Animator.SetTrigger("Damage");
            }
        }

        void onDead()
        {
            if (!m_Death)
            {
                m_Death = true;
                m_Animator.SetTrigger("Death");
                m_NavAgent.Stop();

                foreach (Collider c in GetComponents<Collider>())
                    c.enabled = false;
                foreach (Collider c in GetComponentsInChildren<Collider>())
                    c.enabled = false;

                Recycleable recycle = GetComponent<Recycleable>();
                if (recycle != null && recycle.Handler != null)
                {
                    StartCoroutine(Recycle(recycle));
                }
                else
                {
                    Destroy(gameObject, m_DeathTime);
                }
            }
        }

        IEnumerator Recycle(Recycleable recycle)
        {
            yield return new WaitForSeconds(m_DeathTime);

            foreach (Collider c in GetComponents<Collider>())
                c.enabled = true;
            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = true;
            m_Animator.SetTrigger("Reborn");
            m_Death = false;
            m_Damageable.Restore();
            recycle.StartRecycle(); 
        }
    }

}
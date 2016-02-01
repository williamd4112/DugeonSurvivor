using UnityEngine;
using System.Collections;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Health))]
    public class GameCharacter : MonoBehaviour
    {
        [SerializeField]
        private float m_MoveSpeed = 3.0f;

        private Rigidbody m_Rigidbody;
        private Animator m_Animator;
        private Health m_Health;

        // Use this for initialization
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();

            m_Health.RegisterHealthChangeEvent(onHit);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Move(Vector2 velocity, int attackMask)
        {
            m_Rigidbody.velocity = velocity * m_MoveSpeed;

            float speed = velocity.magnitude;
            updateAnimator(speed, attackMask);
        }

        void updateAnimator(float speed, int attackMask)
        {
            if((attackMask & 0x1) != 0)
                m_Animator.SetTrigger("Attack1");
            if((attackMask & 0x2) != 0)
                m_Animator.SetTrigger("Attack2");
            if((attackMask & 0x4) != 0)
                m_Animator.SetTrigger("Attack3");
            m_Animator.SetFloat("Speed", speed);
        }

        void onHit(int hp, int diff)
        {
            if(hp <= 0)
                m_Animator.SetBool("Death", true);
            else if(diff < 0)
            {
                m_Animator.SetTrigger("Hurt");
            }
              
        }
    }

}
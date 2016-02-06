using UnityEngine;
using System.Collections;
using WiFramework;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(GameCharacter))]
    public class SkeletonMonsterControl : MonoBehaviour
    {
        private GameCharacter m_Monster;
        [SerializeField]
        private Transform m_Target;
        [SerializeField]
        private float m_AttackRadius = 1.9f;

        public Transform Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        // Use this for initialization
        void Start()
        {
            m_Monster = GetComponent<GameCharacter>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            m_Target = PlayerManager.Player.transform;
            float distance = Vector3.Distance(m_Monster.transform.position, m_Target.position);
            bool attack = distance <= m_AttackRadius;

            if (!attack)
            {
                m_Monster.Navigate(m_Target.position);
            }
            else
            {
                m_Monster.Stop();
                m_Monster.Look(m_Target.position);
                m_Monster.Action(0);
            }
        }
    }

}
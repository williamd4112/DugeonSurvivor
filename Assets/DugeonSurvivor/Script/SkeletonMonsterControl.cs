using UnityEngine;
using System.Collections;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(MonsterCharacter))]
    public class SkeletonMonsterControl : MonoBehaviour
    {
        private MonsterCharacter m_Monster;
        [SerializeField]
        private GameObject m_Target;
        [SerializeField]
        private float m_AttackRadius = 1.9f;

        public GameObject Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        // Use this for initialization
        void Start()
        {
            m_Monster = GetComponent<MonsterCharacter>();
            m_Target = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float distance = Vector3.Distance(m_Monster.transform.position, m_Target.transform.position);
            bool attack = distance <= m_AttackRadius;
            m_Monster.Move(m_Target.transform.position, attack);
        }
    }

}
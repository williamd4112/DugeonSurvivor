using UnityEngine;
using System.Collections;

namespace WiFramework
{
    [RequireComponent(typeof(Health))]
    public class Damageable : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_OnHitSound;
        [SerializeField]
        private GameObject m_OnHitEffect;
        private GameObject m_OnHitEffectInstance;
        private bool m_OnHitAvailable;

        [SerializeField]
        private AudioClip m_OnDestroySound;
        [SerializeField]
        private GameObject m_OnDestroyEffect;
        private GameObject m_OnDestroyEffectInstance;
        private bool m_OnDestroyAvailable;

        [SerializeField]
        private int m_MaxDamage = int.MaxValue;

        private Health m_Health;

        // Use this for initialization
        void Start()
        {
            m_Health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
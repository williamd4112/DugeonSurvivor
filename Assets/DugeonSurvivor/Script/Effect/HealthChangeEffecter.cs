using UnityEngine;
using System.Collections;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(Health))]
    public class HealthChangeEffecter : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_OnHPIncreaseEffect;
        [SerializeField]
        private AudioClip m_OnHPIncreaseSoundEffect;
        [SerializeField]
        private GameObject m_OnHPDecreaseEffect;
        [SerializeField]
        private AudioClip m_OnHPDecreaseSoundEffect;

        private Health m_Heath;

        // Use this for initialization
        void Start()
        {
            m_Heath = GetComponent<Health>();
        }
        
    }

}


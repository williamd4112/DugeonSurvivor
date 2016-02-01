using UnityEngine;
using System.Collections;
using WiFramework;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(Health))]
    public class HealthChangeEffecter : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_OnHPIncreaseEffect;
        [SerializeField]
        private AudioClip m_OnHPIncreaseSoundEffect;
        private bool m_OnHPIncreaseAvailable;

        [SerializeField]
        private GameObject m_OnHPDecreaseEffect;
        [SerializeField]
        private AudioClip m_OnHPDecreaseSoundEffect;
        private bool m_OnHPDecreaseAvailable;

        private GameObject m_OnHPIncreaseEffectInstance;
        private GameObject m_OnHPDecreaseEffectInstance;

        private Health m_Heath;

        // Use this for initialization
        void Start()
        {
            m_OnHPIncreaseAvailable = true;
            m_OnHPDecreaseAvailable = true;

            m_Heath = GetComponent<Health>();
            m_Heath.RegisterHealthChangeEvent(OnHealthChange);
        }

        void onRecycle(GameObject obj)
        {
            if(obj == m_OnHPDecreaseEffectInstance)
            {
                m_OnHPDecreaseAvailable = true;
            }
            else if(obj == m_OnHPIncreaseEffectInstance)
            {
                m_OnHPIncreaseAvailable = true;
            }
        }

        private void attachRecycleable(GameObject obj)
        {
            Recycleable recycleable = obj.AddComponent<Recycleable>();
            recycleable.enabled = true;
            recycleable.SetRecycleHandler(onRecycle);
        }

        void OnHealthChange(int hp, int diff)
        {
            if(diff < 0)
            {
                if (m_OnHPDecreaseEffectInstance == null)
                {
                    m_OnHPDecreaseEffectInstance = Instantiate(m_OnHPDecreaseEffect, transform.position, transform.rotation) 
                        as GameObject;
                    if(m_OnHPDecreaseEffectInstance.GetComponent<Recycleable>() == null)
                    {
                        attachRecycleable(m_OnHPDecreaseEffectInstance);
                    }
                }
                
                if(m_OnHPDecreaseAvailable)
                {
                    m_OnHPDecreaseEffectInstance.GetComponent<Recycleable>().RestartRecycle();
                    AudioSource.PlayClipAtPoint(m_OnHPDecreaseSoundEffect, transform.position);
                    m_OnHPDecreaseAvailable = false;
                }
            }
            else if(diff > 0)
            {
                if (m_OnHPIncreaseEffectInstance == null)
                {
                    m_OnHPIncreaseEffectInstance = Instantiate(m_OnHPIncreaseEffect, transform.position, transform.rotation) 
                        as GameObject;
                    if (m_OnHPIncreaseEffectInstance.GetComponent<Recycleable>() == null)
                    {
                        attachRecycleable(m_OnHPIncreaseEffectInstance);
                    }
                }
                
                if(m_OnHPIncreaseAvailable)
                {
                    m_OnHPIncreaseEffectInstance.GetComponent<Recycleable>().RestartRecycle();
                    AudioSource.PlayClipAtPoint(m_OnHPIncreaseSoundEffect, transform.position);
                    m_OnHPIncreaseAvailable = false;
                }
            }


        }
        
    }

}


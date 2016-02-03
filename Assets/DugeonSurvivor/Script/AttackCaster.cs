using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WiFramework;

namespace DugeonSurvivor
{
    public class AttackCaster : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ClickEffect;
        private GameObjectPool m_ClickEffectPool;
        [SerializeField]
        private AudioClip m_ClickSound;
        [SerializeField]
        private GameObjectPool[] m_ProjectilesPools;
        [SerializeField]
        private float[] m_ProjectileEmissionDelays;

        private int m_SelectedProjectile;

        public void Cast(Vector3 pos)
        {
            if(m_SelectedProjectile < m_ProjectilesPools.Length)
            {
                GameObject click = m_ClickEffectPool.Pop();
                if(click != null)
                {
                    click.transform.position = pos;
                    AudioSource.PlayClipAtPoint(m_ClickSound, transform.position);
                }

                StartCoroutine(delayedCast(pos));
            }
        }

        public void SetSelectProjectile(int index)
        {
            if (index < 0 || index >= m_ProjectilesPools.Length)
                return;
            m_SelectedProjectile = index;
        }

        // Use this for initialization
        void Start()
        {
            m_ClickEffectPool = gameObject.AddComponent<GameObjectPool>();
            m_ClickEffectPool.Fill(m_ClickEffect, 1);
        }

        IEnumerator delayedCast(Vector3 pos)
        {
            yield return new WaitForSeconds(m_ProjectileEmissionDelays[m_SelectedProjectile]);
            GameObject projectile = m_ProjectilesPools[m_SelectedProjectile].Pop();
            if (projectile != null)
            {
                projectile.transform.position = pos;
            }
        }
    }
}

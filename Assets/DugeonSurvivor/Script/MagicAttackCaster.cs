using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using WiFramework;

namespace DugeonSurvivor
{
    [System.Serializable]
    public class MagicAttackCastEvent : UnityEvent<Vector3> { }
    [System.Serializable]
    public class SwitchMagicEvent : UnityEvent<Color> { }

    [System.Serializable]
    public class ProjectileEntry
    {
        [SerializeField]
        public GameObjectPool Pool;
        [SerializeField]
        public int Cost;
        [SerializeField]
        public float Delay;
        [SerializeField]
        public Color LightColor;
        [SerializeField]
        public SwitchMagicEvent Event;
    }

    [RequireComponent(typeof(Mana))]
    [RequireComponent(typeof(AudioSource))]
    public class MagicAttackCaster : MonoBehaviour
    {
        [SerializeField]
        private ProjectileEntry[] m_Projectiles;

        public int NumberOfProjectiles { get { return m_Projectiles.Length; } }

        public MagicAttackCastEvent OnCastEvents;
        public UnityEvent ActionEvent;

        private int m_SelectedProjectile;

        private Mana m_Mana;
        private AudioSource m_AudioSource;

        public void Cast(Vector3 pos)
        {
            if(m_SelectedProjectile < m_Projectiles.Length)
            {
                OnCastEvents.Invoke(pos);
                ActionEvent.Invoke();

                int consume = m_Projectiles[m_SelectedProjectile].Cost;
                if (m_Mana.MP >= consume)
                {
                    m_Mana.ChangeValue(-consume);
                    StartCoroutine(delayedCast(pos));
                }
            }
        }

        public void SetSelectProjectile(int index)
        {
            if (index < 0 || index >= m_Projectiles.Length)
                return;
            m_SelectedProjectile = index;
            m_Projectiles[m_SelectedProjectile].Event.Invoke(m_Projectiles[m_SelectedProjectile].LightColor);
        }

        // Use this for initialization
        void Start()
        {
            m_Mana = GetComponent<Mana>();
            m_AudioSource = GetComponent<AudioSource>();
            m_Projectiles[m_SelectedProjectile].Event.Invoke(m_Projectiles[m_SelectedProjectile].LightColor);
        }

        IEnumerator delayedCast(Vector3 pos)
        {
            yield return new WaitForSeconds(m_Projectiles[m_SelectedProjectile].Delay);
            GameObject projectile = m_Projectiles[m_SelectedProjectile].Pool.Pop();
            if (projectile != null)
            {
                projectile.transform.position = pos;
            }
        }
    }
}

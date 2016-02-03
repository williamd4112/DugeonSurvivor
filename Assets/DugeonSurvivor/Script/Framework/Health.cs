using UnityEngine;
using System.Collections;
using System;

namespace WiFramework
{
    public delegate void HealthChangeEvent(int hp, int diff);

    [Serializable]
    public class Health : SingleValueStorage<int>
    {
        [SerializeField]
        private int m_MaxHealth;
        [SerializeField]
        private int m_CurHealth;
        [SerializeField]
        private int m_InitHealth;

        public int HP
        {
            get { return m_CurHealth; }
        }

        private HealthChangeEvent m_HealthChangeEvents;

        public void RegisterHealthChangeEvent(HealthChangeEvent e)
        {
            m_HealthChangeEvents += e;
        }

        public void InvokeHealthChangeEvents(int diff)
        {
            if(m_HealthChangeEvents != null)
                m_HealthChangeEvents.Invoke(m_CurHealth, diff);
        }

        public void ChangeHealth(int diff)
        {
            m_CurHealth = Mathf.Clamp(m_CurHealth + diff, 0, m_MaxHealth);
            InvokeHealthChangeEvents(diff);
        }

        public void RestoreHealth()
        {
            ChangeHealth(m_MaxHealth);
        }

        void Awake()
        {
            m_CurHealth = m_InitHealth;
        }
    }
}


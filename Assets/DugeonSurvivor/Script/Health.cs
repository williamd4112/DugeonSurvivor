using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public delegate void HealthChangeEvent(int hp, int diff);

    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int m_MaxHealth;
        [SerializeField]
        private int m_CurHealth;
        [SerializeField]
        private int m_InitHealth;
        [SerializeField]
        private int m_MaxPerDecrease;

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
            m_HealthChangeEvents.Invoke(m_CurHealth, diff);
        }

        public void ChangeHealth(int diff)
        {
            if(diff < 0)
                diff = -Mathf.Clamp(Mathf.Abs(diff), 0, m_MaxPerDecrease);
            m_CurHealth = Mathf.Clamp(m_CurHealth + diff, 0, m_MaxHealth);
            InvokeHealthChangeEvents(diff);
        }
    }
}


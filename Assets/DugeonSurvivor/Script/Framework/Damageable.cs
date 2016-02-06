using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace WiFramework
{
    [System.Serializable]
    public class DamageEvent : UnityEvent<Damage, bool, int, int> { }
    public class RestoreEvent : UnityEvent { }

    [System.Serializable]
    public class EffectDamageType
    {
        public DamageType Type;
        public Damage EffectDamage;
    }

    [System.Serializable]
    public class DamageTypeEvent : UnityEvent { }
    [System.Serializable]
    public class DamageTypeEventPair
    {
        public DamageType Type;
        public DamageTypeEvent Event; 
    }
    [System.Serializable]
    public class DamageTypeImmortalTimePair
    {
        public DamageType Type;
        public float ImmortalTime;
    }

    [RequireComponent(typeof(Health))]
    public class Damageable : MonoBehaviour
    {
        public enum DamageEventType
        {
            HIT_ENTER,
            HIT_STAY,
            HIT_EXIT,
            DESTROY
        }

        [SerializeField]
        private float m_ImmortalTime = 1.0f;
        [SerializeField]
        private DamageFilter[] m_StaticDamageFilters;
        private List<DamageFilter> m_DynamicDamageFilters;
        private Dictionary<DamageType, Damage> m_EffectDamageTable = new Dictionary<DamageType, Damage>();
        private Dictionary<DamageType, DamageTypeEvent> m_DamageTypeEventTable = new Dictionary<DamageType, DamageTypeEvent>();
        private Dictionary<DamageType, float> m_DamageTypeImmortalTimeTable = new Dictionary<DamageType, float>();

        public EffectDamageType[] EffectDamages;
        public DamageEvent EnterEvents;
        public DamageEvent StayEvents;
        public DamageEvent ExitEvents;
        public DamageEvent DestroyEvents;
        public RestoreEvent RestoreEvents;
        public DamageTypeEventPair[] DamageTypeEventPairs;
        public DamageTypeImmortalTimePair[] DamageTypeImmortalTimePairs;

        private Health m_Health;
        [SerializeField]
        private bool m_Immortal = false;

        private Coroutine countdownRountine;

        public bool Immortal
        {
            get { return m_Immortal; }
            set
            {
                if(value)
                {
                    if (countdownRountine != null)
                        StopCoroutine(countdownRountine);
                }
                else
                {
                    countdownRountine = null;
                }
                m_Immortal = value;
            }
        }

        public void AddDynamicDamageFilter(DamageFilter filter)
        {
            m_DynamicDamageFilters.Add(filter);
        }

        public void AddDamageEvents(UnityAction<Damage, bool, int, int> e, DamageEventType type)
        {
            switch(type)
            {
                case DamageEventType.HIT_ENTER:
                    EnterEvents.AddListener(e);
                    break;
                case DamageEventType.HIT_STAY:
                    StayEvents.AddListener(e);
                    break;
                case DamageEventType.HIT_EXIT:
                    ExitEvents.AddListener(e);
                    break;
                case DamageEventType.DESTROY:
                    DestroyEvents.AddListener(e);
                    break;
                default:
                    break;
            }
        }

        public void AddRestoreEvent(UnityAction e)
        {
            RestoreEvents.AddListener(e);
        }

        public void Restore()
        {
            m_Health.RestoreHealth();
            m_Immortal = false;
            invokeRestoreEvents();
        }

        public void EffectDamage(string typename)
        {
            DamageType t = (DamageType)System.Enum.Parse(typeof(DamageType), typename);
            EffectDamage(t);
        }

        public void EffectDamage(DamageType t)
        {
            Damage damage;
            if(m_EffectDamageTable.TryGetValue(t, out damage))
            {
                Debug.LogFormat("Effect {0}", t);
                StayHit(damage);
            }
        }

        public void EnterHit(Damage damage)
        {
            if (m_Immortal) return;
            int damageAmount = computeDamageAmount(damage);
            invokeDamageTypeEvents(damage);
            invokeEvents(damage, false, m_Health.HP, damageAmount, DamageEventType.HIT_ENTER);
        }

        public void StayHit(Damage damage)
        {
            if (m_Immortal) return;

            int damageAmount = computeDamageAmount(damage);
            
            bool isCritical = damage.CriticalHit;
            if (isCritical)
                damageAmount += Mathf.FloorToInt(damageAmount * damage.CriticalHitBuff);
            m_Health.ChangeHealth(-damageAmount);

            invokeEvents(damage, isCritical, m_Health.HP, damageAmount, DamageEventType.HIT_STAY);

            if (m_Health.HP <= 0)
            {
                invokeEvents(damage, isCritical, m_Health.HP, damageAmount, DamageEventType.DESTROY);
            }

            m_Immortal = true;
            if(countdownRountine == null)
                countdownRountine = StartCoroutine(conutdown(getImmortalTime(damage.Type)));
        }

        public void ExitHit(Damage damage)
        {
            int damageAmount = computeDamageAmount(damage);
            invokeEvents(damage, false, m_Health.HP, damageAmount, DamageEventType.HIT_EXIT);
        }

        int computeDamageAmount(Damage damage)
        {
            int amount = damage.DamageAmount;
            foreach (DamageFilter filter in m_StaticDamageFilters)
                amount -= filter.Compensate;
            foreach (DamageFilter filter in m_DynamicDamageFilters)
                amount -= filter.Compensate;
            return amount;
        }

        float getImmortalTime(DamageType t)
        {
            float delay;
            if(m_DamageTypeImmortalTimeTable.TryGetValue(t, out delay))
            {
                Debug.LogFormat("Delay {0}", delay);
                return delay;
            }
            return 0;
        }

        void invokeDamageTypeEvents(Damage damage)
        {
            DamageTypeEvent e;
            if(m_DamageTypeEventTable.TryGetValue(damage.Type, out e))
            {
                e.Invoke();
            }
        }

        void invokeEvents(Damage damage, bool isCritical, int hp, int diff, DamageEventType type)
        {
            switch (type)
            {
                case DamageEventType.HIT_ENTER:
                    if(EnterEvents != null)
                        EnterEvents.Invoke(damage, isCritical,  hp, diff);
                    break;
                case DamageEventType.HIT_STAY:
                    if(StayEvents != null)
                        StayEvents.Invoke(damage, isCritical, hp, diff);
                    break;
                case DamageEventType.HIT_EXIT:
                    if (ExitEvents != null)
                        ExitEvents.Invoke(damage, isCritical, hp, diff);
                    break;
                case DamageEventType.DESTROY:
                    if (DestroyEvents != null)
                        DestroyEvents.Invoke(damage, isCritical, hp, diff);
                    break;
                default:
                    break;
            }
        }

        void invokeRestoreEvents()
        {
            if (RestoreEvents != null)
                RestoreEvents.Invoke();
        }

        // Use this for initialization
        void Awake()
        {
            m_Health = GetComponent<Health>();
            m_DynamicDamageFilters = new List<DamageFilter>();

            foreach (EffectDamageType t in EffectDamages)
                m_EffectDamageTable.Add(t.Type, t.EffectDamage);
            foreach (DamageTypeEventPair p in DamageTypeEventPairs)
            {
                Debug.LogFormat("{0}: Set Type {1}",gameObject, p.Type);
                m_DamageTypeEventTable.Add(p.Type, p.Event);
            }
            foreach (DamageTypeImmortalTimePair p in DamageTypeImmortalTimePairs)
            {
                m_DamageTypeImmortalTimeTable.Add(p.Type, p.ImmortalTime);
            }
        }

        IEnumerator conutdown(float t)
        {
            yield return new WaitForSeconds(t);
            Immortal = false;
        }
    }

}
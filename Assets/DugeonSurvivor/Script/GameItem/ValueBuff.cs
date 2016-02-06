using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using WiFramework;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(EventBasedRecycleable))]
    [RequireComponent(typeof(DelayOperation))]
    public class ValueBuff : GameItem
    {
        public enum ValueBuffType
        {
            Health,
            Mana
        }

        [SerializeField]
        private int m_Value;
        [SerializeField]
        private float m_DelayBeforeRecycle = 0.0f;
        [SerializeField]
        private AudioClip m_CollectedSound;
        [SerializeField]
        private GameObject m_Effect;
        [SerializeField]
        private ValueBuffType m_Type = ValueBuffType.Health;

        protected override void onCollected(GameObject collector)
        {
            bool success = buff(collector, m_Value);
            if(success)
            {
                AudioSource.PlayClipAtPoint(m_CollectedSound, transform.position);
                m_Effect.SetActive(true);
                GetComponent<DelayOperation>().StartDelayOperation(recycle, m_DelayBeforeRecycle);
            }
        }

        void recycle()
        {
            m_Effect.SetActive(false);
            EventBasedRecycleable r = GetComponent<EventBasedRecycleable>();

            r.StartRecycle();
        }

        bool buff(GameObject collector, int val)
        {
            SingleValueStorage valueStorage = getValueStorage(collector);
            if (valueStorage != null)
            {
                valueStorage.ChangeValue(val);
                return true;
            }
            return false;
        }

        SingleValueStorage getValueStorage(GameObject collector)
        {
            switch(m_Type)
            {
                case ValueBuffType.Health:
                    return collector.GetComponent<Health>();
                case ValueBuffType.Mana:
                    return collector.GetComponent<Mana>();
                default:
                    return null;
            }
        }

    }
}

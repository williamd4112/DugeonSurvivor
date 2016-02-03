using UnityEngine;
using System.Collections;
using WiFramework;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(EventBasedRecycleable))]
    [RequireComponent(typeof(DelayOperation))]
    abstract public class ValueBuff : GameItem
    {
        [SerializeField]
        private int m_Value;
        [SerializeField]
        private float m_DelayBeforeRecycle = 0.0f;
        [SerializeField]
        private AudioClip m_CollectedSound;
        [SerializeField]
        private GameObject m_Effect;

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
            if (r.Handler != null)
                r.StartRecycle();
            else
                Destroy(gameObject);
        }

        abstract protected bool buff(GameObject collector, int val);

    }
}

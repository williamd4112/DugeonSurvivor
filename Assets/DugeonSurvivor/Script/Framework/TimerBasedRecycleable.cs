using UnityEngine;
using System.Collections;

namespace WiFramework
{
    [RequireComponent(typeof(CoroutineTimer))]
    public class TimerBasedRecycleable : Recycleable
    {
        private CoroutineTimer m_Timer;
        private float m_RecycleTime;
        public float RecycleTime
        {
            get { return m_RecycleTime; }
            set { m_RecycleTime = value; }
        }

        private bool m_HasRecycled = true;

        public override void StartRecycle()
        {
            if (m_HasRecycled)
            {
                m_HasRecycled = false;
                m_Timer.SetTime(m_RecycleTime);
                m_Timer.ResetTimer();
                m_Timer.StartTimer();
            }
        }

        void OnEnable()
        {
            m_Timer = GetComponent<CoroutineTimer>();
        }

        void Update()
        {
            if (m_Timer.Timeout && !m_HasRecycled)
            {
                m_HasRecycled = true;
                callRecycleHandler();
            }
        }
    }
}

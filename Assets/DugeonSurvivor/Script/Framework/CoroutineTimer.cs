using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace WiFramework
{
    public delegate void CoroutineTimerCallback();

    [System.Serializable]
    public class CoroutineTimerUnityEvent : UnityEvent { }

    public class CoroutineTimer : MonoBehaviour
    {
        public enum CoroutineTimerMode
        {
            ONESHOT_AUTO,
            PERIODIC_AUTO,
            ONESHOT_MANUAL,
            PERIODIC_MANUAL
        }

        public enum CoroutineTimerAutoTrigger
        {
            START,
            ENABLE,
            AWAKE
        }

        [SerializeField]
        private float m_Time;

        [SerializeField]
        private CoroutineTimerMode m_Mode = CoroutineTimerMode.ONESHOT_MANUAL;
        [SerializeField]
        private CoroutineTimerAutoTrigger m_AutoTrigger = CoroutineTimerAutoTrigger.START;

        private bool m_Timeout = false;
        public bool Timeout
        {
            get { return m_Timeout; }
        }

        private CoroutineTimerCallback m_Callbacks;
        public CoroutineTimerUnityEvent TimeoutEvents;

        public void ResetTimer()
        {
            m_Timeout = false;
        }

        public void StartTimer()
        {
            StartCoroutine(Countdown(m_Time));
        }

        public bool SetTime(float t)
        {
            m_Time = t;
            return true;
        }

        public void RegisterCallback(CoroutineTimerCallback c)
        {
            m_Callbacks += c;
        }

        private void InvokeCallbacks()
        {
            if (m_Callbacks != null)
                m_Callbacks.Invoke();
        }

        void Start()
        {
            if(m_AutoTrigger == CoroutineTimerAutoTrigger.START && m_Mode == CoroutineTimerMode.ONESHOT_AUTO || m_Mode == CoroutineTimerMode.PERIODIC_AUTO)
            {
                ResetTimer();
                StartTimer();
            }
        }

        void Awake()
        {
            if (m_AutoTrigger == CoroutineTimerAutoTrigger.AWAKE && m_Mode == CoroutineTimerMode.ONESHOT_AUTO || m_Mode == CoroutineTimerMode.PERIODIC_AUTO)
            {
                ResetTimer();
                StartTimer();
            }
        }

        void OnEnable()
        {
            if (m_AutoTrigger == CoroutineTimerAutoTrigger.ENABLE && m_Mode == CoroutineTimerMode.ONESHOT_AUTO || m_Mode == CoroutineTimerMode.PERIODIC_AUTO)
            {
                ResetTimer();
                StartTimer();
            }
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Break()
        {
            Debug.Break();
        }


        IEnumerator Countdown(float t)
        {
            yield return new WaitForSeconds(t);
            m_Timeout = true;
            InvokeCallbacks();

            if(TimeoutEvents != null)
                TimeoutEvents.Invoke();

            if(m_Mode == CoroutineTimerMode.PERIODIC_AUTO || m_Mode == CoroutineTimerMode.PERIODIC_MANUAL)
            {
                if (gameObject.activeInHierarchy)
                {
                    ResetTimer();
                    StartTimer();
                }
            }
        }
    }
}

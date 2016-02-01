using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public delegate void CoroutineTimerCallback();

    public class CoroutineTimer : MonoBehaviour
    {
        [SerializeField]
        private float m_Time;

        private bool m_Timeout = false;
        public bool Timeout
        {
            get { return m_Timeout; }
        }

        private CoroutineTimerCallback m_Callbacks;

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
            if (!m_Timeout) return false;
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

        IEnumerator Countdown(float t)
        {
            yield return new WaitForSeconds(t);
            m_Timeout = true;
            InvokeCallbacks();
        }
    }
}

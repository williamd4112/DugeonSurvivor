using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public delegate void RecycleHandler(GameObject obj);

    [RequireComponent(typeof(CoroutineTimer))]
    public class Recycleable : MonoBehaviour
    {
        [SerializeField]
        private float m_LifeTime;

        private CoroutineTimer m_Timer;

        private RecycleHandler m_RecycleHandler;

        public void SetRecycleHandler(RecycleHandler h)
        {
            m_RecycleHandler += h;
        }

        public void RestartRecycle()
        {
            m_Timer.ResetTimer();
            m_Timer.StartTimer();
        }

        void OnEnable()
        {
            m_Timer = GetComponent<CoroutineTimer>();
        }

        void Update()
        {
            if(m_Timer.Timeout)
            {
                if (m_RecycleHandler != null)
                    m_RecycleHandler.Invoke(gameObject);
                else
                    Destroy(gameObject);
            }
        }
    }
}

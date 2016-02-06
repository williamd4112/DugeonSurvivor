using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace WiFramework
{
    [Serializable]
    public class SingleValueChangeUIEvent : UnityEvent<int, int> { }
    [Serializable]
    public class SingleValueChangeEvent : UnityEvent<int, int> { }

    [Serializable]
    abstract public class SingleValueStorage : MonoBehaviour
    {
        [SerializeField]
        private int m_MaxValue = 100;
        [SerializeField]
        private int m_MinValue = 0;
        [SerializeField]
        private int m_InitValue;
        private int m_CurValue;

        public int MaxValue { get { return m_MaxValue; } }
        public int MinValue { get { return m_MinValue; } }
        public int Value { get { return m_CurValue; } }

        public SingleValueChangeEvent m_ValueEvents;
        public SingleValueChangeUIEvent m_ValueUIEvents;
        
        public void Restore()
        {
            ChangeValue(m_MaxValue);
        }        

        public void ChangeValue(int diff)
        {
            m_CurValue = Mathf.Clamp(m_CurValue + diff, m_MinValue, m_MaxValue);
            m_ValueEvents.Invoke(m_CurValue, diff);
            m_ValueUIEvents.Invoke(m_CurValue, m_MaxValue);
        }

        void Start()
        {
            m_CurValue = m_InitValue;
            m_ValueUIEvents.Invoke(m_CurValue, m_MaxValue);
        }

    }

}
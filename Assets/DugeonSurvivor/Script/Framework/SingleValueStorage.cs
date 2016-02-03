using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace WiFramework
{
    [Serializable]
    abstract public class SingleValueStorage<T> : MonoBehaviour
    {
        [Serializable]
        public class SingleValueChangeEvent : UnityEvent { }

        [SerializeField]
        private T m_MaxValue;
        [SerializeField]
        private T m_CurValue;
        [SerializeField]
        private T m_MinValue;
        [SerializeField]
        private T m_InitValue;

        public SingleValueChangeEvent m_ValueEvents;

    }

}
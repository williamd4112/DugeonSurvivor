using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public class WiEffect : MonoBehaviour
    {
        [SerializeField]
        private bool m_UseCustomDuration;
        [SerializeField]
        private float m_CustomDuration;
        private float m_LongestDurationInChildren;
        private float m_Duration;

        public float Duration
        {
            get { return m_Duration; }
        }

        // Use this for initialization
        void Start()
        {
            m_LongestDurationInChildren = 0;
            foreach (ParticleSystem s in GetComponentsInChildren<ParticleSystem>())
            {
                m_LongestDurationInChildren = Mathf.Max(m_LongestDurationInChildren, s.duration);
            }

            m_Duration = (m_UseCustomDuration) ? m_CustomDuration : m_LongestDurationInChildren;
        }
    }

}
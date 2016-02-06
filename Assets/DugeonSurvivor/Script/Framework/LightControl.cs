using UnityEngine;
using System.Collections;

namespace WiFramework
{
    [RequireComponent(typeof(Light))]
    public class LightControl : MonoBehaviour
    {
        [SerializeField]
        private Light m_Light;
        [SerializeField]
        private float m_FadeInSpeed = 5.0f;
        [SerializeField]
        private float m_FadeInIntensity = 4.0f;
        [SerializeField]
        private float m_FadeInRange = 50.0f;

        [SerializeField]
        private float m_FadeOutSpeed = 2.0f;
        [SerializeField]
        private float m_FadeOutIntensity = 2.55f;
        [SerializeField]
        private float m_FadeOutRange = 20.8f;
        [SerializeField]
        public bool m_FadeIn = false;

        public void FadeIn() { m_FadeIn = true; }
        public void FadeOut() { m_FadeIn = false; }

        // Use this for initialization
        void Start()
        {
            m_Light = GetComponent<Light>();
        }

        // Update is called once per frame
        void Update()
        {
            if(m_FadeIn)
            {
                m_Light.intensity = Mathf.Lerp(m_Light.intensity, m_FadeInIntensity, Time.deltaTime * m_FadeInSpeed);
                m_Light.range = Mathf.Lerp(m_Light.range, m_FadeInRange, Time.deltaTime * m_FadeInSpeed);
            }
            else
            {
                m_Light.intensity = Mathf.Lerp(m_Light.intensity, m_FadeOutIntensity, Time.deltaTime * m_FadeOutSpeed);
                m_Light.range = Mathf.Lerp(m_Light.range, m_FadeOutRange, Time.deltaTime * m_FadeOutSpeed);
            }
            
        }
    }
}

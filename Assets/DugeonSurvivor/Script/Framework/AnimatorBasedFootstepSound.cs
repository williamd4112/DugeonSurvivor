using UnityEngine;
using System.Collections;

namespace WiFramework
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class AnimatorBasedFootstepSound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip m_WalkSound;
        [SerializeField]
        private AudioClip m_RunSound;
        [SerializeField]
        private string m_MoveStateName = "Base Layer.Move";
        [SerializeField]
        private string m_SpeedVariableName = "Speed";
        [SerializeField]
        private float m_RunSpeed = 0.8f;

        private AudioSource m_Audiosource;
        private Animator m_Animator;

        private int m_MoveStateNameHash;

        // Use this for initialization
        void Start()
        {
            m_Audiosource = GetComponent<AudioSource>();
            m_Animator = GetComponent<Animator>();
            m_MoveStateNameHash = Animator.StringToHash(m_MoveStateName);

            m_Audiosource.Stop();
        }

        // Update is called once per frame
        void Update()
        {
            int curHash = m_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            if (curHash == m_MoveStateNameHash)
            {
                if (!m_Audiosource.isPlaying)
                {
                    m_Audiosource.clip = (m_Animator.GetFloat(m_SpeedVariableName) >= m_RunSpeed) ? m_RunSound : m_WalkSound;
                    m_Audiosource.Play();
                }
            }
            else
            {
                m_Audiosource.Stop();
            }
        }
    }

}
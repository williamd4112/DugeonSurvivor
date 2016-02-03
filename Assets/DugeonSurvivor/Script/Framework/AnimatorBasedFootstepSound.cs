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

        [SerializeField]
        private float shakeAmt = 2 * 0.2f; // the degrees to shake the camera
        [SerializeField]
        private float shakePeriodTime = 0.42f; // The period of each shake
        [SerializeField]
        private float dropOffTime = 1.6f; // How long it takes the shaking to settle down to nothing

        private WHCameraShake m_CamShake;

        private AudioSource m_Audiosource;
        private Animator m_Animator;

        private int m_MoveStateNameHash;

        // Use this for initialization
        void Start()
        {
            m_Audiosource = GetComponent<AudioSource>();
            m_Animator = GetComponent<Animator>();
            m_CamShake = Camera.main.GetComponent<WHCameraShake>();
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
                    m_Audiosource.clip = (m_Animator.GetFloat(m_SpeedVariableName) >= 0.8f) ? m_RunSound : m_WalkSound;
                    m_Audiosource.Play();

                    GameObject cam = Camera.main.gameObject;
                    LTDescr shakeTween = LeanTween.rotateAroundLocal(cam, Vector3.right, shakeAmt, shakePeriodTime)
                    .setEase(LeanTweenType.easeShake) // this is a special ease that is good for shaking
                    .setLoopClamp()
                    .setRepeat(-1);

                    // Slow the camera shake down to zero
                    LeanTween.value(cam, shakeAmt, 0f, dropOffTime).setOnUpdate(
                        (float val) => {
                            shakeTween.setTo(Vector3.right * val);
                        }
                    ).setEase(LeanTweenType.easeOutQuad);

                }
            }
            else
            {
                m_Audiosource.Stop();
            }
        }
    }

}
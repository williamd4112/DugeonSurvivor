using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(GameCharacter))]
    [RequireComponent(typeof(AttackCaster))]
    public class GameCharacterControl : MonoBehaviour
    {
        private GameCharacter m_Character;

        private Vector3 m_ProjMousePos;
        private bool m_Attack;
        private int m_AttackMode;

        private int floorMask;
        private float camRayLength = 1000f;

        // Use this for initialization
        void Start()
        {
            floorMask = LayerMask.GetMask("Floor");
            m_AttackMode = 0x1;
            m_Character = GetComponent<GameCharacter>();
        }

        // Update is called once per frame
        void Update()
        {

#if MOBILE_INPUT

#else
            if(Input.GetButtonDown("SwitchAttack"))
            {
                m_AttackMode <<= 1;
                m_AttackMode %= 7;
            }

            if(Input.GetButtonDown("Fire1"))
            {
                Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit floorHit;

                if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
                {
                    m_ProjMousePos = floorHit.point;
                    m_Attack = true;
                }
            }
#endif
        }

        void FixedUpdate()
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            m_Character.Move(h, v, m_Attack, m_AttackMode, m_ProjMousePos);
            m_Attack = false;
        }

    }
}

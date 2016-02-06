using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(GameCharacter))]
    [RequireComponent(typeof(MagicAttackCaster))]
    public class GameCharacterControl : MonoBehaviour
    {
        private GameCharacter m_Character;
        private MagicAttackCaster m_Caster;

        private Vector3 m_ProjMousePos;
        private bool m_Attack;
        private int m_AttackMode;
        private int m_CastType;

        private int floorMask;
        private float camRayLength = 1000f;

        // Use this for initialization
        void Start()
        {
            floorMask = LayerMask.GetMask("Floor");
            m_AttackMode = 0;
            m_CastType = 0;
            m_Character = GetComponent<GameCharacter>();
            m_Caster = GetComponent<MagicAttackCaster>();
        }

        // Update is called once per frame
        void Update()
        {

#if MOBILE_INPUT

#else
            if(Input.GetButtonDown("SwitchAttack"))
            {
                m_CastType++;
                m_CastType %= m_Caster.NumberOfProjectiles;
                m_Caster.SetSelectProjectile(m_CastType);
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

            if(m_Attack)
            {
                m_Character.Action(m_AttackMode);
                m_Attack = false;
            }
            else
            {
                m_Character.Move(h, v);
            }
        }

        public void CastAtMousePosition()
        {
            m_Character.Look(m_ProjMousePos);
            m_Caster.Cast(m_ProjMousePos);
        }

        public void LookAtMousePosition()
        {
            m_Character.Look(m_ProjMousePos);
        }
    }
}

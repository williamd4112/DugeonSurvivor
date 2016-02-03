using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public class TouchDamageEmitter : MonoBehaviour
    {
        [SerializeField]
        private string[] m_IgnoreTags;
        [SerializeField]
        private Damage m_Damage;

        void OnCollisionEnter(Collision other)
        {
            if (isValid(other.gameObject))
            {
                other.gameObject.GetComponent<Damageable>().EnterHit(m_Damage);
            }
        }

        void OnCollisionStay(Collision other)
        {
            if(isValid(other.gameObject))
            {
                other.gameObject.GetComponent<Damageable>().StayHit(m_Damage);
            }
        }

        void OnCollisionExit(Collision other)
        {
            if (isValid(other.gameObject))
            {
                other.gameObject.GetComponent<Damageable>().ExitHit(m_Damage);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (isValid(other.gameObject))
            {
                other.gameObject.GetComponent<Damageable>().EnterHit(m_Damage);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (isValid(other.gameObject))
            {
                other.gameObject.GetComponent<Damageable>().StayHit(m_Damage);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (isValid(other.gameObject))
            {
                other.gameObject.GetComponent<Damageable>().ExitHit(m_Damage);
            }
        }

        bool isValid(GameObject obj)
        {
            if (!ignoreCheck(obj))
            {
                Damageable dmg = obj.GetComponent<Damageable>();
                if (dmg != null)
                {
                    return true;
                }
            }
            return false;
        }

        bool ignoreCheck(GameObject obj)
        {
            foreach (string tag in m_IgnoreTags)
                if (obj.CompareTag(tag))
                    return true;
            return false;
        }
    }

}
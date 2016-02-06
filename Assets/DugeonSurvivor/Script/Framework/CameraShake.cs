using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;

namespace WiFramework
{
    [System.Serializable]
    public class CameraShakeType
    {
        [SerializeField]
        public string TypeName;
        [SerializeField]
        public float Amount;
        [SerializeField]
        public float Roughnes;
        [SerializeField]
        public float FadeInTime;
        [SerializeField]
        public float FadeOutTime;

        public CameraShakeInstance instance;
    }

    public class CameraShake : MonoBehaviour
    {
        public CameraShakeType[] Types;
        private Dictionary<string, CameraShakeType> m_Types;

        void Awake()
        {
            m_Types = new Dictionary<string, CameraShakeType>();
            foreach(CameraShakeType t in Types)
            {
                m_Types.Add(t.TypeName, t);
            }
        }

        public void StartShake(string name)
        {
            CameraShakeType type;
            if(m_Types.TryGetValue(name, out type))
            {
                type.instance = CameraShaker.Instance.StartShake(type.Amount, type.Roughnes, type.FadeInTime); ;
            }
        }

        public void StopShake(string name)
        {
            CameraShakeType type;
            if (m_Types.TryGetValue(name, out type))
            {
                type.instance.StartFadeOut(type.FadeOutTime);
            }
        }
    }
}

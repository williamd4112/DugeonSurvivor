using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WiFramework;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(CoroutineTimer))]
    public class RecycleableGameObjectGenerator : MonoBehaviour
    {
        [SerializeField]
        private EventBasedGameObjectPool m_Pool;
        [SerializeField]
        private string m_SpawnPointTag;

        GameObject[] m_SpawnPoints;
        CoroutineTimer m_Timer;

        // Use this for initialization
        void Start()
        {
            m_Timer = GetComponent<CoroutineTimer>();
            m_SpawnPoints = GameObject.FindGameObjectsWithTag(m_SpawnPointTag);

            m_Timer.RegisterCallback(spawn);
            m_Timer.StartTimer();
        }

        void spawn()
        {
            if(m_Pool.Count >= m_SpawnPoints.Length)
            {
                foreach (GameObject obj in m_SpawnPoints)
                {
                    GameObject dummy = m_Pool.Pop();
                    if (dummy != null)
                    {
                        dummy.transform.position = obj.transform.position;
                    }
                }
            }
            m_Timer.ResetTimer();
            m_Timer.StartTimer();
        }
    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WiFramework
{
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField]
        private int m_InitalCapcity = 0;
        [SerializeField]
        private GameObject m_Template;

        private Queue<GameObject> m_Pool;

        [SerializeField]
        private float m_RecyclePeriod = 1.0f;

        private int m_Capacity;

        public int Capacity
        {
            get { return m_Capacity; }
        }

        public float RecyclePeriod
        {
            get { return m_RecyclePeriod; }
            set { m_RecyclePeriod = value; }
        }

        public bool Empty
        {
            get { return m_Pool.Count == 0; }
        }

        public int Count
        {
            get { return m_Pool.Count; }
        }

        void Start()
        {
            Fill(m_Template, m_InitalCapcity);
        }

        public GameObject Pop()
        {
            GameObject obj = null;
            if (!Empty)
            {
                obj = m_Pool.Dequeue();
                obj.SetActive(true);

                listenRecycle(obj);
            }
            return obj;
        }

        public void Fill(GameObject template, int count)
        {
            if (template == null || count <= 0) return;

            m_Pool = new Queue<GameObject>();
  
            m_Capacity = count;
            m_Template = template;
            for (int i = 0; i < m_Capacity; i++)
            {
                GameObject obj = Instantiate(m_Template) as GameObject;
                Recycleable recycle = obj.GetComponent<Recycleable>();

                if (recycle == null)
                {
                    recycle = recyclelize(obj);
                }

                recycle.SetRecycleHandler(pushBackToPool);
                obj.SetActive(false);
                m_Pool.Enqueue(obj);
            }
        }

        virtual protected Recycleable recyclelize(GameObject obj)
        {
            return obj.AddComponent<TimerBasedRecycleable>();
        }

        virtual protected void listenRecycle(GameObject obj)
        {
            TimerBasedRecycleable recycle = obj.GetComponent<TimerBasedRecycleable>();
            recycle.RecycleTime = m_RecyclePeriod;
            recycle.StartRecycle();
        }

        void pushBackToPool(GameObject obj)
        {
            m_Pool.Enqueue(obj);
            Debug.Log(obj + " back");
            obj.SetActive(false);
        }
    }
}

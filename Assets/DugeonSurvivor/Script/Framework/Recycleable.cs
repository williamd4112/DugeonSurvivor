using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public delegate void RecycleHandler(GameObject obj);

    abstract public class Recycleable : MonoBehaviour
    {
        private RecycleHandler m_RecycleHandler;

        public void SetRecycleHandler(RecycleHandler h)
        {
            m_RecycleHandler += h;
        }

        protected void callRecycleHandler()
        {
            if (m_RecycleHandler != null)
                m_RecycleHandler.Invoke(gameObject);
            else
            {
                Destroy(gameObject);
            }
        }

        abstract public void StartRecycle();


    }
}

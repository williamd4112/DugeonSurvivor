using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DugeonSurvivor
{
    public delegate void CollectEvent();

    abstract public class GameItem : MonoBehaviour
    {
        [SerializeField]
        private string m_ItemTag;
        public string ItemTag
        {
            get { return m_ItemTag; }
        }

        [SerializeField]
        private string[] m_CollectableTags = { "Player" };

        private CollectEvent m_CollectEvents;

        public void RegisterCollectEvent(CollectEvent e)
        {
            m_CollectEvents += e;
        }

        void invokeCollectEvent()
        {
            if (m_CollectEvents != null)
                m_CollectEvents.Invoke();
        }

        void OnTriggerEnter(Collider other)
        {
            foreach(string tag in m_CollectableTags)
            {
                if(other.gameObject.CompareTag(tag))
                {
                    invokeCollectEvent();
                    onCollected(other.gameObject);
                    return;
                }
            }
        }

        void OnCollisionEnter(Collision other)
        {
            foreach (string tag in m_CollectableTags)
            {
                if (other.gameObject.CompareTag(tag))
                {
                    invokeCollectEvent();
                    onCollected(other.gameObject);
                    return;
                }
            }
        }

        abstract protected void onCollected(GameObject collector);
        
    }
}
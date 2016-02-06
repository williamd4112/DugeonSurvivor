using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WiFramework
{
    [System.Serializable]
    public class DropItemEntry
    {
        [SerializeField]
        public DropItemType Type;
        [SerializeField, Range(0, 1)]
        public float Probability;
    }

    public class GameItemDrop : MonoBehaviour
    {
        [SerializeField]
        private DropItemEntry[] m_DropItems;
        private Dictionary<DropItemType, float> m_DropItemDescTable;

        public void Drop(DropItemType type)
        {
            float probability;
            if (m_DropItemDescTable.TryGetValue(type, out probability))
            {
                if(Random.Range(0, 1) <= probability)
                    PlayerManager.instance.DropAt(type, transform.position);
            }
        }

        public void Drop()
        {
            DropItemType type = m_DropItems[Random.Range(0, m_DropItems.Length)].Type;
            Drop(type);
        }

        void Awake()
        {
            m_DropItemDescTable = new Dictionary<DropItemType, float>();
            foreach (DropItemEntry entry in m_DropItems)
                m_DropItemDescTable.Add(entry.Type, entry.Probability);
        }
    }

}
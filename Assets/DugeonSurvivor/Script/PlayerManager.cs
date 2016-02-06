using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WiFramework
{
    public enum DropItemType
    {
        HealthBuff,
        ManaBuff
    }

    [System.Serializable]
    public class DropItemPoolEntry
    {
        public DropItemType Type;
        public GameObject Item;
        public int MaxAmount;
        public bool EventBasedRecycle = true;
    }

    public class PlayerManager : MonoBehaviour
    {
        public static GameObject Player;
        public static PlayerManager instance;

        [SerializeField]
        private DropItemPoolEntry[] m_DropItemPoolEntrys;

        private Dictionary<DropItemType, GameObjectPool> m_DropItemTables;
        private Vector3 m_StartPos;

        // Use this for initialization
        void Awake()
        {
            instance = gameObject.GetComponent<PlayerManager>();
            Player = GameObject.Find("Player");
            m_StartPos = Player.transform.position;
            Player.GetComponent<EventBasedRecycleable>().SetRecycleHandler(onRecycle);

            m_DropItemTables = new Dictionary<DropItemType, GameObjectPool>();
            foreach (DropItemPoolEntry entry in m_DropItemPoolEntrys)
            {
                GameObjectPool pool = (entry.EventBasedRecycle) ? gameObject.AddComponent<EventBasedGameObjectPool>() :
                    gameObject.AddComponent<GameObjectPool>();
                pool.Fill(entry.Item, entry.MaxAmount);
                m_DropItemTables.Add(entry.Type, pool);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DropAt(DropItemType type, Vector3 pos)
        {
            GameObjectPool pool;
            if(m_DropItemTables.TryGetValue(type, out pool))
            {
                pool.PopTo(pos);
            }
        }

        void onRecycle(GameObject obj)
        {
            obj.transform.position = m_StartPos;
        }
    }
}

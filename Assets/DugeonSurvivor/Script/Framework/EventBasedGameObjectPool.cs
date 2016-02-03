using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public class EventBasedGameObjectPool : GameObjectPool
    {
        protected override Recycleable recyclelize(GameObject obj)
        {
            return obj.AddComponent<EventBasedRecycleable>();
        }

        protected override void listenRecycle(GameObject obj)
        {
            // Do nothing
        }
    }

}
using UnityEngine;
using System.Collections;
using System;

namespace WiFramework
{
    public class EventBasedRecycleable : Recycleable
    {
        public override void StartRecycle()
        {
            callRecycleHandler();
        }

    }
}

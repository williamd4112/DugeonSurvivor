using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

namespace WiFramework
{
    [Serializable]
    public class Health : SingleValueStorage
    {
        public int HP
        {
            get { return Value; }
        }

        public void ChangeHealth(int diff)
        {
            ChangeValue(diff);
        }

        public void RestoreHealth()
        {
            Restore();
        }
    }
}


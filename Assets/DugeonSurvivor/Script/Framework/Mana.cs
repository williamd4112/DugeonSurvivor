using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public class Mana : SingleValueStorage
    {
        public int MP
        {
            get { return Value; }
        }
    }
}

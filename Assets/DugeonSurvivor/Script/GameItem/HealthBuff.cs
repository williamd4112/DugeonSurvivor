using UnityEngine;
using System.Collections;
using System;
using WiFramework;

namespace DugeonSurvivor
{
    [RequireComponent(typeof(EventBasedRecycleable))]
    public class HealthBuff : ValueBuff
    {
        protected override bool buff(GameObject collector, int val)
        {
            Health health = collector.GetComponent<Health>();
            if(health != null)
            {
                health.ChangeHealth(val);
                return true;
            }
            return false;
        }

    }
}

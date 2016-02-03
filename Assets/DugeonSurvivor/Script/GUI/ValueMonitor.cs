using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public interface ValueMonitorInterface
    {
        int GetMinValue();
        int GetValue();
        int GetMaxValue();
    }

    public class ValueMonitor : MonoBehaviour
    {
        private UIBarScript m_UIBarScript;

        // Use this for initialization
        void Start()
        {
            m_UIBarScript = GetComponent <UIBarScript> ();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

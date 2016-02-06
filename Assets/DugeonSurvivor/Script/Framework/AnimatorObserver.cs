using UnityEngine;
using System.Collections;

namespace WiFramework
{
    public interface AnimatorObserver
    {
        void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    }
}

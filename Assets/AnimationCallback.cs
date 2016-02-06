using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AnimationCallback : StateMachineBehaviour {

    public enum AnimationCallbackType
    {
        Enter,
        Stay,
        Exit
    }

    public interface AnimationListener
    {
        void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    }

    private Queue<AnimationListener> m_Listeners = new Queue<AnimationListener>();

    public void AddListener(AnimationCallback.AnimationListener l)
    {
        m_Listeners.Enqueue(l);
    }

    void invokeEvents(AnimationCallback.AnimationCallbackType t, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch(t)
        {
            case AnimationCallbackType.Enter:
                foreach (AnimationCallback.AnimationListener l in m_Listeners)
                    l.OnStateEnter(animator, stateInfo, layerIndex);
                break;
            case AnimationCallbackType.Stay:
                foreach (AnimationCallback.AnimationListener l in m_Listeners)
                    l.OnStateUpdate(animator, stateInfo, layerIndex);
                break;
            case AnimationCallbackType.Exit:
                foreach (AnimationCallback.AnimationListener l in m_Listeners)
                    l.OnStateExit(animator, stateInfo, layerIndex);
                break;
            default:
                break;
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        invokeEvents(AnimationCallbackType.Enter, animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        invokeEvents(AnimationCallbackType.Stay, animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        invokeEvents(AnimationCallbackType.Exit, animator, stateInfo, layerIndex);
    }

    //// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    //// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}
}

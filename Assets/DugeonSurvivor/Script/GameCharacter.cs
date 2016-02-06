using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using WiFramework;
using System;

namespace DugeonSurvivor
{
    [System.Serializable]
    public class Pair<K, V>
    {
        public K Key;
        public V Value;
    }

    [System.Serializable]
    public class ActionEvent : UnityEvent { }

    [System.Serializable]
    abstract public class ActionEventPair<T>
    {
        [SerializeField]
        public T State;
        [SerializeField]
        public float EnterDelay;
        [SerializeField]
        public float StayDelay;
        [SerializeField]
        public float ExitDelay;
        [SerializeField]
        public ActionEvent EnterEvent;
        [SerializeField]
        public ActionEvent StayEvent;
        [SerializeField]
        public ActionEvent ExitEvent;
    }

    [System.Serializable]
    public class AnimationActionEventPair : ActionEventPair<string> { }
    [System.Serializable]
    public class StatusActionEventPair : ActionEventPair<Status> { }
    [System.Serializable]
    public class DamageStatusPair : Pair<DamageType, Status> { }

    [System.Serializable]
    public enum ActionEventType
    {
        Enter,
        Stay,
        Exit
    }

    [System.Serializable]
    public enum Status
    {
        Normal,
        Fired,
        Freezed
    }

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EventBasedRecycleable))]
    public class GameCharacter : MonoBehaviour, AnimationCallback.AnimationListener
    {
        [SerializeField]
        private float m_MoveSpeed = 3.0f;
        [SerializeField]
        private string m_SpeedVariableName = "Speed";
        [SerializeField]
        private string m_MoveableTag = "Moveable";
        [SerializeField]
        private string[] m_ImmediateActionsNames = { "Attack1", "Attack2", "Attack3"};
        [SerializeField]
        private string m_OnHitActionName = "Hurt";
        [SerializeField]
        private string m_OnCriticalHitActionName = "Knockback";
        [SerializeField]
        private string m_OnDeathActionName = "Death";
        [SerializeField]
        private float m_DeathTime = 1.0f;
        [SerializeField]
        private DamageStatusPair[] DamageToStatusTable;

        private Rigidbody m_Rigidbody;
        private Animator m_Animator;
        private Damageable m_Damageable;
        private NavMeshAgent m_Navigator;
        private AudioSource m_AudioSource;
        private EventBasedRecycleable m_Recycle;

        public AnimationActionEventPair[] AnimationActionEvents;
        public StatusActionEventPair[] StatusActionEvents;
        public UnityEvent OnRestoreEvents;

        private Dictionary<int, AnimationActionEventPair> m_ActionEventsTable = new Dictionary<int, AnimationActionEventPair>();
        private Dictionary<Status, StatusActionEventPair> m_StatusActionEventsTable = new Dictionary<Status, StatusActionEventPair>();
        private Dictionary<DamageType, Status> m_DamageToStatusTable = new Dictionary<DamageType, Status>();

        private int m_MoveableTagHash;
        private bool m_Death = false;
        [SerializeField]
        private Status m_Status = Status.Normal;

        public Status CharacterStatus
        {
            get { return m_Status; }
        }

        // Use this for initialization
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
            m_Damageable = GetComponent<Damageable>();
            m_Navigator = GetComponent<NavMeshAgent>();
            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_Recycle = GetComponent<EventBasedRecycleable>();

            foreach (DamageStatusPair p in DamageToStatusTable)
                m_DamageToStatusTable.Add(p.Key, p.Value);

            foreach (AnimationCallback c in m_Animator.GetBehaviours<AnimationCallback>())
                c.AddListener(this);

            foreach (AnimationActionEventPair p in AnimationActionEvents)
            {
                m_ActionEventsTable.Add(Animator.StringToHash(p.State), p);
            }

            foreach (StatusActionEventPair p in StatusActionEvents)
            {
                m_StatusActionEventsTable.Add(p.State, p);
            }


            m_MoveableTagHash = Animator.StringToHash(m_MoveableTag);
        }

        void OnEnable()
        {
            if(m_Animator != null)
            {
                foreach (AnimationCallback c in m_Animator.GetBehaviours<AnimationCallback>())
                    c.AddListener(this);
            }
        }

        public void Navigate(Vector3 target)
        {
            if (m_Death || m_Status == Status.Freezed) return;
            if(isMoveable())
            {
                m_Navigator.Resume();
                m_Navigator.SetDestination(target);
                updateAnimator(m_Navigator.velocity.magnitude);
            }
        }

        public void Move(float h, float v)
        {
            if (m_Death || m_Status == Status.Freezed) return;
            Vector3 velocity = new Vector3(h, 0, v);

            if(isMoveable())
            {
                if (velocity != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(velocity.normalized);
                m_Rigidbody.velocity = velocity.magnitude * m_MoveSpeed * transform.forward;
            }
            else
            {
                Stop();
            }

            updateAnimator(velocity.magnitude);
        }

        public void Look(Vector3 pos)
        {
            if (m_Death || m_Status == Status.Freezed) return;
            transform.rotation = Quaternion.LookRotation((pos - transform.position).normalized);
        }

        public void Stop()
        {
            m_Navigator.Stop();
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }

        public void Resume()
        {
            m_Navigator.Resume();
        }

        public void Action(int actionMode)
        {
            if (m_Death || m_Status == Status.Freezed) return;
            if (actionMode < m_ImmediateActionsNames.Length)
            {
                Stop();
                m_Animator.SetTrigger(m_ImmediateActionsNames[actionMode]);
            }
        }

        public void SetStatus(Status s)
        {
            bool diff = m_Status != s;
            Status old = m_Status;

            if (diff)
            {
                invokeStatusActionEvents(s, ActionEventType.Enter);
                m_Status = s;
            }

            if (diff)
                invokeStatusActionEvents(old, ActionEventType.Exit);
        }

        public void RecoverStatus()
        {
            SetStatus(Status.Normal);
        }

        public void onEnterHit(Damage damage, bool isCritical, int hp, int diff)
        {
            if (m_Death) return;
            changeStatus(damage);
        }

        public void onStayHit(Damage damage, bool isCritical, int hp, int diff)
        {
            if (m_Death) return;
 
            if (isCritical)
            {
                m_Animator.SetTrigger(m_OnCriticalHitActionName);
            }               
            else
            {
                m_Animator.SetTrigger(m_OnHitActionName);
            }
            changeStatus(damage);
        }

        public void onDead(Damage damage, bool isCritical, int hp, int diff)
        {
            if (m_Death) return;
            m_Animator.SetBool(m_OnDeathActionName, true);       

            setAllColliders(false);
            Stop();
            m_Death = true;
            StartCoroutine(delayDead(m_DeathTime));
        }

        public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            invokeActionEvents(stateInfo.fullPathHash, ActionEventType.Enter);
        }

        public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            invokeActionEvents(stateInfo.fullPathHash, ActionEventType.Stay);
        }

        public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            invokeActionEvents(stateInfo.fullPathHash, ActionEventType.Exit);
        }

        IEnumerator delayDead(float t)
        {
            yield return new WaitForSeconds(t);
            m_Damageable.Restore();
            restore();
            m_AudioSource.clip = null;
            m_Recycle.StartRecycle();
        }

        IEnumerator delayAction(ActionEvent e, float t)
        {
            yield return new WaitForSeconds(t);
            e.Invoke();
        }

        void invokeActionEvents(int key, ActionEventType t)
        {
            AnimationActionEventPair p;
            if (m_ActionEventsTable.TryGetValue(key, out p))
            {
                startAction<string>(p, t);
            }
        }

        void invokeStatusActionEvents(Status s, ActionEventType t)
        {
            StatusActionEventPair p;
            if (m_StatusActionEventsTable.TryGetValue(s, out p))
            {
                startAction<Status>(p, t);
            }
        }

        void startAction<T>(ActionEventPair<T> p, ActionEventType t)
        {
            switch (t)
            {
                case ActionEventType.Enter:
                    StartCoroutine(delayAction(p.EnterEvent, p.EnterDelay));
                    break;
                case ActionEventType.Stay:
                    StartCoroutine(delayAction(p.StayEvent, p.StayDelay));
                    break;
                case ActionEventType.Exit:
                    StartCoroutine(delayAction(p.ExitEvent, p.ExitDelay));
                    break;
                default:
                    break;
            }
        }

        void updateStatus()
        {
            if (m_Death) return;
            invokeStatusActionEvents(m_Status, ActionEventType.Stay);
        }

        void changeStatus(Damage damage)
        {
            Debug.LogFormat("Damage {0}", damage.Type);
            Status s;
            if (m_DamageToStatusTable.TryGetValue(damage.Type, out s))
            {
                SetStatus(s);
            }
        }

        void setAllColliders(bool b)
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
                c.enabled = b;
            foreach (Collider c in GetComponents<Collider>())
                c.enabled = b;
        }

        void restore()
        {
            m_Animator.SetBool(m_OnDeathActionName, false);
            m_Death = false;
            setAllColliders(true);
            OnRestoreEvents.Invoke();
        }

        void updateAnimator(float speed)
        {
            if (m_Death) return;
            m_Animator.SetFloat(m_SpeedVariableName, speed);
        }

        bool isMoveable()
        {
            return (m_Animator.GetCurrentAnimatorStateInfo(0).tagHash == m_MoveableTagHash);
        }

        void Update()
        {
            updateStatus();
        }
    }

}
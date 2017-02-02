using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    /// <summary>
    /// An object the user can interact with
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        public enum DisableConditions
        {
            None,
            Enter,
            Stay,
            Exit
        }

        /// <summary>
        /// Disables the gameobject once the specified condition is met (after invoking events).
        /// </summary>
        public DisableConditions DisableCondition;

        public UnityEvent EnterEvents;
        public UnityEvent ExitEvents;
        public UnityEvent StayEvents;

        [Tooltip("Time in seconds the object needs to remain in Enter state before Stay() is called.")]
        public float StayDuration = 2;

        public void Enter()
        {
            CallEvent(EnterEvents,DisableConditions.Enter,"Enter");
        }

        public void Exit()
        {
            CallEvent(ExitEvents,DisableConditions.Exit,"Exit");
        }

        public void Stay()
        {
            CallEvent(StayEvents,DisableConditions.Stay,"Stay");
        }

        private void CallEvent(UnityEvent unityEvent, DisableConditions condition, string debugmessage="Interactable event")
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke();
            }
            else
            {
                Debug.Log(debugmessage);
            }
            gameObject.SetActive(DisableCondition != condition);
        }

        //Necessary for enabled/disabled checkbox to show on component
        void Start()
        {
        }
    }
}
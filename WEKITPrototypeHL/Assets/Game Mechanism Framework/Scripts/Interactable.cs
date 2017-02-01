using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class Interactable : MonoBehaviour
    {
        public enum DisableConditions
        {
            None,
            Enter,
            Stay,
            Exit
        }


        public DisableConditions DisableCondition;
            //Dedicated variable instead of a part of EnterEvents to ensure safe order of operations

        public UnityEvent EnterEvents;
        public UnityEvent ExitEvents;
        public UnityEvent StayEvents;

        [Tooltip("Time in seconds the object needs to remain in Enter state before Stay() is called.")] public float
            StayDuration = 2;

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
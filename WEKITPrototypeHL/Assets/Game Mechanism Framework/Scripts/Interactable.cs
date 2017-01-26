using System.Collections;
using System.Collections.Generic;
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


        public DisableConditions DisableCondition; //Dedicated variable instead of a part of EnterEvents to ensure safe order of operations
        public UnityEvent EnterEvents;
        public UnityEvent ExitEvents;
        public UnityEvent StayEvents;
        [Tooltip("Time in seconds the object needs to remain in Enter state before Stay() is called.")]
        public float StayDuration=2;

        private Coroutine _stayCR;

        public void Enter()
        {
            _stayCR = StartCoroutine(StayCR());
            if (EnterEvents != null)
            {
                EnterEvents.Invoke();
            }
            else
            {
                Debug.Log("Enter");
            }
            gameObject.SetActive(DisableCondition!=DisableConditions.Enter);
        }

        public void Exit()
        {
            StopCoroutine(_stayCR);
            if (ExitEvents != null)
            {
                ExitEvents.Invoke();
            }
            else
            {
                Debug.Log("Exit");
            }
            gameObject.SetActive(DisableCondition != DisableConditions.Exit);
        }

        public IEnumerator StayCR()
        {
            yield return new WaitForSeconds(StayDuration);
            Stay();
        }

        public void Stay()
        {
            if (StayEvents != null)
            {
                StayEvents.Invoke();
            }
            else
            {
                Debug.Log("Stay");
            }
            gameObject.SetActive(DisableCondition != DisableConditions.Stay);
        }

        //Necessary for enabled/disabled checkbox to show on component
        void Start()
        {
            
        }
    }
}
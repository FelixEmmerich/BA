using System.Collections;
using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// Checks for Interactable objects.
    /// </summary>
    public abstract class InteractableCheck : MonoBehaviour
    {

        [HideInInspector] public GameObject LastHit;

        [HideInInspector] public Interactable LastInteractable;

        private Coroutine _stayCoroutine;

        // Update is called once per frame
        void Update()
        {
            GameObject currentObject = DetectObject();
            if (currentObject!=null)
            {
                if (ObjectIsNewInteractable(currentObject))
                {
                    LastInteractable.Enter();
                    _stayCoroutine = StartCoroutine(Stay());
                }
            }
            else if (LastHit != null)
            {
                //Stay method is not called in Interactable if it loses focus.
                StopCoroutine(_stayCoroutine);
                if (LastInteractable != null)
                {
                    LastInteractable.Exit();
                }
                LastHit = null;
                LastInteractable = null;
            }
        }

        /// <summary>
        /// Finds gameobjects
        /// </summary>
        /// <returns></returns>
        public abstract GameObject DetectObject();

        /// <summary>
        /// Checks if gameobject has an interactable component.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        bool ObjectIsNewInteractable(GameObject go)
        {
            if (go != LastHit)
            {
                LastHit = go;
                LastInteractable = LastHit.GetComponent<Interactable>();
                return (LastInteractable != null && LastInteractable.enabled);
            }
            return false;
        }

        /// <summary>
        /// Stay method is called in Interactable once a certain time has passed.
        /// </summary>
        /// <returns></returns>
        public IEnumerator Stay()
        {
            if (LastInteractable == null)
            {
                yield break;
            }
            yield return new WaitForSeconds(LastInteractable.StayDuration);
            if (LastInteractable == null)
            {
                yield break;
            }
            LastInteractable.Stay();
        }

    }
}
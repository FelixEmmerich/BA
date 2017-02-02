using System.Collections;
using UnityEngine;

namespace GameMechanism
{
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
                StopCoroutine(_stayCoroutine);
                if (LastInteractable != null)
                {
                    LastInteractable.Exit();
                }
                LastHit = null;
                LastInteractable = null;
            }
        }

        public abstract GameObject DetectObject();

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
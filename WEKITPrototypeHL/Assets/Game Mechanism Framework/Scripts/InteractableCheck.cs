using System.Collections;
using UnityEngine;

namespace GameMechanism
{
    public abstract class InteractableCheck : MonoBehaviour
    {
        public abstract Vector3 RayStart { get; }

        public abstract Vector3 RayDirection { get; }

        [HideInInspector] public RaycastHit HitInfo;

        [HideInInspector] public bool Hit;

        [HideInInspector] public GameObject LastHit;

        [HideInInspector] public Interactable LastInteractable;

        [Tooltip(
            "Layers checked during Raycast. If different InteractableCheck-derived scripts are active, it is best to move the targets for each to a separate layer."
        )] public LayerMask CheckLayers;

        public float MaxDistance;

        private Coroutine _stayCoroutine;

        // Update is called once per frame
        void Update()
        {
            Hit = Physics.Raycast(RayStart, RayDirection, out HitInfo,
                MaxDistance, CheckLayers);
            if (Hit)
            {
                GetHitData();
                EnterTargets();
                _stayCoroutine=StartCoroutine(Stay());
            }
            else if (LastHit != null)
            {
                StopCoroutine(_stayCoroutine);
                ExitTargets();
                LastHit = null;
                LastInteractable = null;
            }
        }

        void EnterTargets()
        {
            if (LastInteractable != null && LastInteractable.enabled)
            {
                LastInteractable.Enter();
            }
        }

        void GetHitData()
        {
            GameObject go = HitInfo.collider.gameObject;
            if (go != LastHit)
            {
                LastHit = go;
                LastInteractable = LastHit.GetComponent<Interactable>();
            }
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

        void ExitTargets()
        {
            if (LastInteractable != null)
            {
                LastInteractable.Exit();
            }
        }
    }
}
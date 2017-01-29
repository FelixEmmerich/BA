using UnityEngine;

namespace GameMechanism
{
    public abstract class InteractableCheck : MonoBehaviour
    {

        public abstract Vector3 RayStart { get; }

        public abstract Vector3 RayDirection { get; }

        [HideInInspector]
        public RaycastHit HitInfo;

        [HideInInspector]
        public bool Hit;

        [HideInInspector]
        public GameObject LastHit;

        [Tooltip("Layers checked during Raycast. If different InteractableCheck-derived scripts are active, it is best to move the targets for each to a separate layer.")]
        public LayerMask CheckLayers;

        public float MaxDistance;

        // Update is called once per frame
        void Update()
        {
            Hit = Physics.Raycast(RayStart, RayDirection, out HitInfo,
                MaxDistance, CheckLayers);
            if (Hit)
            {
                EnterTargets();
            }
            else if (LastHit != null)
            {
                ExitTargets();
                LastHit = null;
            }
        }

        void EnterTargets()
        {
            GameObject go = HitInfo.collider.gameObject;
            if (go != LastHit)
            {
                LastHit = go;
                Interactable target = LastHit.GetComponent<Interactable>();
                if (target != null && target.enabled)
                {
                    target.Enter();
                }
            }
        }

        void ExitTargets()
        {
            Interactable target = LastHit.GetComponent<Interactable>();
            if (target != null)
            {
                target.Exit();
            }
        }
    }
}
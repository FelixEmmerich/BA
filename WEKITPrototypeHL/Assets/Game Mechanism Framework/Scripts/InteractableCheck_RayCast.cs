using System.Collections;
using UnityEngine;

namespace GameMechanism
{
    public abstract class InteractableCheck_RayCast : InteractableCheck
    {
        public abstract Vector3 RayStart { get; }

        public abstract Vector3 RayDirection { get; }

        [Tooltip(
            "Layers checked during Raycast. If different InteractableCheck-derived scripts are active, it is best to move the targets for each to a separate layer."
        )]
        public LayerMask CheckLayers;

        public float MaxDistance;

        public override GameObject DetectObject()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(RayStart, RayDirection, out hitInfo,
                MaxDistance, CheckLayers))
            {
                return hitInfo.collider.gameObject;
            }
            return null;
        }
    }
}
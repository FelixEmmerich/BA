using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// Checks for Interactable objects based on camera position and direction.
    /// </summary>
    public class InteractableCheck_Gaze : InteractableCheck_RayCast
    {
        public override Vector3 RayStart
        {
            get { return Camera.main.transform.position; }
        }

        public override Vector3 RayDirection
        {
            get { return Camera.main.transform.forward; }
        }

        void Reset()
        {
            MaxDistance = 5;
        }
    }
}
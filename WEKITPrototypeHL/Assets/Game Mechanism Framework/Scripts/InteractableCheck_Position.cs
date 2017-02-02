using UnityEngine;

namespace GameMechanism
{
    public class InteractableCheck_Position : InteractableCheck_RayCast
    {
        public override Vector3 RayStart
        {
            get { return Camera.main.transform.position; }
        }

        private static readonly Vector3 Down = new Vector3(0, -1, 0);

        public override Vector3 RayDirection
        {
            get { return Down; }
        }

        void Reset()
        {
            MaxDistance = 2.5f;
        }
    }
}
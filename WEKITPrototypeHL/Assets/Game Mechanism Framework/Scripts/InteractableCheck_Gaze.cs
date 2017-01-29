using UnityEngine;

namespace GameMechanism
{
    public class InteractableCheck_Gaze : InteractableCheck
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
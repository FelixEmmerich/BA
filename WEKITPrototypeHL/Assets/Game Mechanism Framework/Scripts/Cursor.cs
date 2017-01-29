using UnityEngine;

namespace GameMechanism
{
    public class Cursor : MonoBehaviour
    {
        /// <summary>
        /// The rotation the cursor object will have when not hitting anything. Set at start.
        /// </summary>
        private Quaternion _standardRotation;

        public float MaxDistance = 5;

        [Tooltip("How far from the actual hitpoint the cursor is supposed to be placed (makes the cursor more visible)")]
        public float HitDistance = 0.01f;

        public LayerMask Layers;

        void Start()
        {
            _standardRotation = transform.rotation;
        }

        void Update()
        {
            RaycastHit hit = Cast();
            transform.position = hit.point+(hit.normal*HitDistance);
            gameObject.transform.forward = hit.normal;
            gameObject.transform.rotation *= _standardRotation;
        }

        public virtual RaycastHit Cast()
        {
            Vector3 cameraPos = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            RaycastHit hitInfo;

            if (!Physics.Raycast(cameraPos, cameraForward, out hitInfo,
                MaxDistance, Layers))
            {
                hitInfo.point = cameraPos + cameraForward * MaxDistance;
                hitInfo.normal = -cameraForward;
            }
            return hitInfo;
        }
    }
}
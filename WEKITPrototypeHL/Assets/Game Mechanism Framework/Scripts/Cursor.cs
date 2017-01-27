using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
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

        public LayerMask Layers;

        void Start()
        {
            _standardRotation = transform.rotation;
        }

        void Update()
        {
            transform.position = Cast().point;
            gameObject.transform.forward = Cast().normal;
            gameObject.transform.rotation *= _standardRotation;
        }

        public virtual RaycastHit Cast()
        {
            Vector3 CameraPos = Camera.main.transform.position;
            Vector3 CameraForward = Camera.main.transform.forward;
            RaycastHit hitInfo;

            if (!Physics.Raycast(CameraPos, CameraForward, out hitInfo,
                MaxDistance, Layers))
            {
                hitInfo.point = CameraPos + CameraForward * MaxDistance;
                hitInfo.normal = -CameraForward;
            }
            return hitInfo;
        }
    }
}
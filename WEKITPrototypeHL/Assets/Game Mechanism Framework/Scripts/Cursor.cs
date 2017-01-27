using System.Collections;
using System.Collections.Generic;
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

        //bool CastAgainstUI = false;

        void Start()
        {
            _standardRotation = transform.rotation;
        }

        void Update()
        {
            Vector3 CameraPos = Camera.main.transform.position;
            Vector3 CameraForward = Camera.main.transform.forward;
            RaycastHit hitInfo;

            /*if (CastAgainstUI)
            {
                if (Graphic.RayCast())
            }*/
            if (Physics.Raycast(CameraPos, CameraForward, out hitInfo,
                MaxDistance, Layers))
            {
                transform.position = hitInfo.point;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }
            else
            {
                transform.position = CameraPos + CameraForward * MaxDistance;
                transform.rotation= Quaternion.FromToRotation(Vector3.up, -CameraForward);
            }
        }
    }
}
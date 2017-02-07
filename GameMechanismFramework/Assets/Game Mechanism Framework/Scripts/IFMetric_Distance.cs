using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class IFMetric_Distance : InformationFilterMetric
    {
        [Tooltip("If true, only x and z coordinates will be compared.")]
        public bool Use2DDistance = true;

        [Tooltip("Will be set to camera if null")]
        public GameObject ReferencePoint;

        public override void Start()
        {
            base.Start();
            if (ReferencePoint == null)
            {
                ReferencePoint = Camera.main.gameObject;
            }
        }

        protected override float GetNewestValue()
        {
            Vector3 referencePos = ReferencePoint.transform.position;

            float distance = Use2DDistance
                ? new Vector2(referencePos.x - transform.position.x, referencePos.z - transform.position.z).magnitude
                : (referencePos - transform.position).magnitude;

            return distance;
        }
    } 
}

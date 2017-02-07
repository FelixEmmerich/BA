using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public class IFMetric_Distance : InformationFilterMetric
    {
        [Tooltip("Will be set to camera if null")]
        public GameObject ReferencePoint;

        protected override float GetNewestValue()
        {
            return (ReferencePoint.transform.position - transform.position).magnitude;
        }
    } 
}

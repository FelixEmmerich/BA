//Based on SpatialUnderstandingCursor from HoloToolkit examples

using System;
using HoloToolkit.Unity;
using UnityEngine;

namespace GameMechanism
{
    public class Cursor_SpatialUnderstanding : Cursor
    {
        public override RaycastHit Cast()
        {
            Vector3 rayPos = Camera.main.transform.position;
            Vector3 rayVec = Camera.main.transform.forward;
            RaycastHit result = new RaycastHit();
            IntPtr raycastResultPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticRaycastResultPtr();
            SpatialUnderstandingDll.Imports.PlayspaceRaycast(
                rayPos.x, rayPos.y, rayPos.z, rayVec.x, rayVec.y, rayVec.z,
                raycastResultPtr);
            var rayCastResult = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticRaycastResult();

            // Override
            if (rayCastResult.SurfaceType != SpatialUnderstandingDll.Imports.RaycastResult.SurfaceTypes.Invalid)
            {
                result.point = rayCastResult.IntersectPoint;
                result.normal = rayCastResult.IntersectNormal;
                return result;
            }
            return base.Cast();
        }
    }
}
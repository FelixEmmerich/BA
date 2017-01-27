using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.UI;

namespace GameMechanism
{
    public class EnvironmentRequirements : MonoBehaviour
    {
        //Types coresponding to and comments taken from PlaySpaceStats (from HoloToolkit's SpatialUnderstandingDLL)
        public enum RequirementCategory
        {
            HorizSurfaceArea, // In m2 : All horizontal faces UP between Ground – 0.15 and Ground + 1.f (include Ground and convenient horiz surface)
            TotalSurfaceArea, // In m2 : All !
            UpSurfaceArea, // In m2 : All horizontal faces UP (no constraint => including ground)
            DownSurfaceArea, // In m2 : All horizontal faces DOWN (no constraint => including ceiling)
            WallSurfaceArea, // In m2 : All Vertical faces (not only walls)
            VirtualCeilingSurfaceArea, // In m2 : estimation of surface of virtual Ceiling.
            VirtualWallSurfaceArea, // In m2 : estimation of surface of virtual Walls.
            NumFloor, // List of Area of each Floor surface (contains count)
            NumCeiling, // List of Area of each Ceiling surface (contains count)
            NumWall_XNeg, // List of Area of each Wall XNeg surface (contains count)
            NumWall_XPos, // List of Area of each Wall XPos surface (contains count)
            NumWall_ZNeg, // List of Area of each Wall ZNeg surface (contains count)
            NumWall_ZPos, // List of Area of each Wall ZPos surface (contains count)
            NumWall, // Number of walls total (XNeg, XPos, ZNeg, ZPos)
            NumPlatform // List of Area of each Horizontal not Floor surface (contains count)
        }

        [Serializable]
        public struct Requirement
        {
            public RequirementCategory Category;
            public float Amount;

            [Tooltip("How required amount is compared to actual amount. If false, a value lower than Amount is required.")]
            public bool GreaterThanOrEqual;
        }

        public Requirement[] Requirements;

        private IntPtr _statsPtr;

        void Start()
        {
            _statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
        }

        public int CheckRequirements()
        {
            if ((SpatialUnderstandingDll.Imports.QueryPlayspaceStats(_statsPtr) != 0))
            {
                for (int i = Requirements.Length - 1; i >= 0; i--)
                {
                    float amount = GetStatsValueFromCategory(Requirements[i].Category);
                    //Return false if required amount does not behave to actual amount as specified
                    if (amount > -1 && (Requirements[i].Amount >= amount ==
                                        Requirements[i].GreaterThanOrEqual))
                    {
                        return 0;
                    }
                }
                return 1;
            }
            Debug.Log("Stats not yet available");
            return -1;
        }

        public int CheckAllRequirements(out bool[] status, out float[] values)
        {
            status = new bool[Requirements.Length];
            values = new float[Requirements.Length];
            if ((SpatialUnderstandingDll.Imports.QueryPlayspaceStats(_statsPtr) != 0))
            {
                int result = 1;
                for (int i = Requirements.Length - 1; i >= 0; i--)
                {
                    float amount = GetStatsValueFromCategory(Requirements[i].Category);

                    values[i] = amount;

                    //Return false if required amount does not behave to actual amount as specified
                    status[i] = amount > -1 && ((amount>=Requirements[i].Amount) ==
                                                Requirements[i].GreaterThanOrEqual);
                    if (!status[i])
                    {
                        result = 0;
                    }
                }
                return result;
            }
            Debug.Log("Stats not yet available");
            return -1;
        }

        public int CheckAllRequirements(out bool[] status)
        {
            float[] values;
            return CheckAllRequirements(out status, out values);
        }

        private float GetStatsValueFromCategory(RequirementCategory category)
        {
            SpatialUnderstandingDll.Imports.PlayspaceStats stats =
                SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
            switch (category)
            {
                case RequirementCategory.HorizSurfaceArea:
                    return stats.HorizSurfaceArea;
                case RequirementCategory.TotalSurfaceArea:
                    return stats.TotalSurfaceArea;
                case RequirementCategory.UpSurfaceArea:
                    return stats.UpSurfaceArea;
                case RequirementCategory.DownSurfaceArea:
                    return stats.DownSurfaceArea;
                case RequirementCategory.WallSurfaceArea:
                    return stats.WallSurfaceArea;
                case RequirementCategory.VirtualCeilingSurfaceArea:
                    return stats.VirtualCeilingSurfaceArea;
                case RequirementCategory.VirtualWallSurfaceArea:
                    return stats.VirtualWallSurfaceArea;
                case RequirementCategory.NumFloor:
                    return stats.NumFloor;
                case RequirementCategory.NumCeiling:
                    return stats.NumCeiling;
                case RequirementCategory.NumWall_XNeg:
                    return stats.NumWall_XNeg;
                case RequirementCategory.NumWall_XPos:
                    return stats.NumWall_XPos;
                case RequirementCategory.NumWall_ZNeg:
                    return stats.NumWall_ZNeg;
                case RequirementCategory.NumWall_ZPos:
                    return stats.NumWall_ZPos;
                case RequirementCategory.NumWall:
                    return stats.NumWall_XPos + stats.NumWall_XNeg + stats.NumWall_ZNeg + stats.NumWall_ZPos;
                case RequirementCategory.NumPlatform:
                    return stats.NumPlatform;
                default:
                    Debug.Log("Category not defined");
                    return -1;
            }
        }
    }
}
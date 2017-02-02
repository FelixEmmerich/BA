//Generate EnvironmentRequirements data from SpatialUnderstandingSpawner data. Note that the accuracy is limited, as available general data is very restricted (e.g. limited to surface stats, no information on space between surfaces).
using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

namespace GameMechanism
{
    public class EnvironmentRequirementsFromUnderstandingSpawner : MonoBehaviour
    {
        public SpatialUnderstandingSpawner Spawner;
        public EnvironmentRequirements Requirements;

        // Use this for initialization
        void Start()
        {

        }

        public static EnvironmentRequirements.Requirement[] GenerateRequirements(SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
        {
            switch (placementType)
            {
                case SpawnInformation.PlacementTypes.OnFloor:
                    return OnFloor(halfDims);
                case SpawnInformation.PlacementTypes.OnWall:
                    return OnWall(halfDims);
                case SpawnInformation.PlacementTypes.OnCeiling:
                    return OnCeiling(halfDims);
                case SpawnInformation.PlacementTypes.OnEdge:
                    return OnEdge(halfDims);
                case SpawnInformation.PlacementTypes.OnFloorAndCeiling:
                    return OnFloorAndCeiling(halfDims);
                case SpawnInformation.PlacementTypes.RandomInAirAwayFromMe:
                    break;
                case SpawnInformation.PlacementTypes.OnEdgeNearCenter:
                    break;
                case SpawnInformation.PlacementTypes.OnFloorAwayFromMe:
                    break;
                case SpawnInformation.PlacementTypes.OnFloorNearMe:
                    break;
                default:
                    return new EnvironmentRequirements.Requirement[0];
            }
        }

        public static EnvironmentRequirements.Requirement[] OnFloor(Vector3 halfDims)
        {
            EnvironmentRequirements.Requirement[] requirements = new EnvironmentRequirements.Requirement[1];
            requirements[0].GreaterThanOrEqual = true;
            requirements[0].Amount = halfDims.x * halfDims.z * 4;
            requirements[0].Category = EnvironmentRequirements.RequirementCategory.HorizSurfaceArea;
            return requirements;
        }

        public static EnvironmentRequirements.Requirement[] OnWall(Vector3 halfDims)
        {
            EnvironmentRequirements.Requirement[] requirements = new EnvironmentRequirements.Requirement[1];
            requirements[0].GreaterThanOrEqual = true;
            requirements[0].Amount = halfDims.x * halfDims.y * 4;
            requirements[0].Category = EnvironmentRequirements.RequirementCategory.WallSurfaceArea;
            return requirements;
        }

        public static EnvironmentRequirements.Requirement[] OnCeiling(Vector3 halfDims)
        {
            EnvironmentRequirements.Requirement[] requirements = new EnvironmentRequirements.Requirement[1];
            requirements[0].GreaterThanOrEqual = true;
            requirements[0].Amount = halfDims.x * halfDims.z * 4;
            requirements[0].Category = EnvironmentRequirements.RequirementCategory.CeilingSurfaceArea;
            return requirements;
        }

        public static EnvironmentRequirements.Requirement[] OnEdge(Vector3 halfDims)
        {
            EnvironmentRequirements.Requirement[] requirements = new EnvironmentRequirements.Requirement[1];
            requirements[0].GreaterThanOrEqual = true;
            requirements[0].Amount = halfDims.x * halfDims.z * 4;
            requirements[0].Category = EnvironmentRequirements.RequirementCategory.PlatformSurfaceArea;
            return requirements;
        }

        public static EnvironmentRequirements.Requirement[] OnFloorAndCeiling(Vector3 halfDims)
        {
            EnvironmentRequirements.Requirement[] requirements = new EnvironmentRequirements.Requirement[2];
            requirements[0].GreaterThanOrEqual = true;
            requirements[0].Amount = halfDims.x * halfDims.z * 4;
            requirements[0].Category = EnvironmentRequirements.RequirementCategory.HorizSurfaceArea;
            requirements[1].GreaterThanOrEqual = true;
            requirements[1].Amount = halfDims.x * halfDims.z * 4;
            requirements[1].Category = EnvironmentRequirements.RequirementCategory.CeilingSurfaceArea;
            return requirements;
        }
    } 
}
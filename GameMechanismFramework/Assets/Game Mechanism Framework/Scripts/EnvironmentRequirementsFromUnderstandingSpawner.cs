using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// //Generate EnvironmentRequirements data from SpatialUnderstandingSpawner data. Note that the accuracy is limited, as available general data is very restricted (e.g. limited to surface stats, no information on space between surfaces).
    /// </summary>
    public class EnvironmentRequirementsFromUnderstandingSpawner : MonoBehaviour
    {
        public SpatialUnderstandingSpawner SpawnerRef;
        public EnvironmentRequirements RequirementsRef;

        // Use this for initialization
        void Start()
        {
            if (SpawnerRef == null)
            {
                SpawnerRef = SpatialUnderstandingSpawner.Instance;
            }
            if (SpawnerRef != null)
            {
                SetRequirements();
            }
        }

        
        /// <summary>
        /// Create a list without overlapping placement types.
        /// </summary>
        private List<SpatialUnderstandingSpawner.SpawnData> CreateReducedSpawnDataList(SpatialUnderstandingSpawner.SpawnData[] data)
        {
            List<SpatialUnderstandingSpawner.SpawnData> result=new List<SpatialUnderstandingSpawner.SpawnData>();
            for (int i = 0; i<data.Length; i++)
            {
                bool newType = true;
                for (int j = 0; j < result.Count; j++)
                {
                    //if placement type is already contained in list, make the half dimensions big enough to fit all requirements
                    if (data[i].PlacementType == result[j].PlacementType)
                    {
                        newType = false;
                        SpatialUnderstandingSpawner.SpawnData newData = new SpatialUnderstandingSpawner.SpawnData()
                        {
                            HalfDims = CombineVectors(data[i].HalfDims,result[j].HalfDims),
                            PlacementType = data[i].PlacementType
                        };
                        result.Add(newData);
                        break;
                    }
                }
                if (newType)
                {
                    SpatialUnderstandingSpawner.SpawnData newData = new SpatialUnderstandingSpawner.SpawnData()
                    {
                        HalfDims = data[i].HalfDims,
                        PlacementType = data[i].PlacementType
                    };
                    result.Add(newData);
                }
            }
            return result;
        }

        public List<EnvironmentRequirements.Requirement> CreateReducedRequirementList(List<EnvironmentRequirements.Requirement> requirements)
        {
            List<EnvironmentRequirements.Requirement> result = new List<EnvironmentRequirements.Requirement>();
            for (int i = 0; i < requirements.Count; i++)
            {
                bool newType = true;
                for (int j = 0; j < result.Count; j++)
                {
                    //if placement type is already contained in list, make the half dimensions big enough to fit all requirements
                    if (requirements[i].Category == result[j].Category)
                    {
                        newType = false;
                        EnvironmentRequirements.Requirement newRequirement = new EnvironmentRequirements.Requirement()
                        {
                            //Under the given circumstances (requirements based on objects to spawn) we are always looking for a greater value
                            Amount = Mathf.Max(requirements[i].Amount, result[j].Amount),
                            Category = requirements[i].Category,
                            GreaterThanOrEqual = true
                        };
                        result.Add(newRequirement);
                        break;
                    }
                }
                if (newType)
                {
                    EnvironmentRequirements.Requirement newRequirement = new EnvironmentRequirements.Requirement()
                    {
                        //Under the given circumstances (requirements based on objects to spawn) we are always looking for a greater value
                        Amount = requirements[i].Amount,
                        Category = requirements[i].Category,
                        GreaterThanOrEqual = true
                    };
                    result.Add(newRequirement);
                }
            }
            return result;
        }

        /// <summary>The maximum for each value in the vector
        /// </summary>
        private Vector3 CombineVectors(Vector3 v1, Vector3 v2)
        {
            Vector3 result = new Vector3();
            result.x = Mathf.Max(v1.x, v2.x);
            result.y = Mathf.Max(v1.y, v2.y);
            result.z = Mathf.Max(v1.z, v2.z);
            return result;
        }

        public void SetRequirements()
        {
            
            if (RequirementsRef != null)
            {
                //Put all spawndata in one list without redundancies
                List<SpatialUnderstandingSpawner.SpawnData> data = CreateReducedSpawnDataList(SpawnerRef.Data);

                List<EnvironmentRequirements.Requirement> requirements = new List<EnvironmentRequirements.Requirement>();

                //Create a list of requirements from the spawndata
                for (int i = 0; i < data.Count; i++)
                {
                    SetRequirements(ref requirements, data[i].PlacementType, data[i].HalfDims);
                }

                //Remove redundancies
                requirements = CreateReducedRequirementList(requirements);

                //Assign result
                RequirementsRef.Requirements = requirements.ToArray();
                RequirementsRef.RequirementsSet.Invoke();
            }
            
        }

        public static void SetRequirements(ref List<EnvironmentRequirements.Requirement> requirements,
            SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
        {
            EnvironmentRequirements.Requirement[] tempRequirements = GenerateRequirements(placementType, halfDims);
            if (tempRequirements != null)
            {
                requirements.AddRange(tempRequirements);
            }
        }

        public static EnvironmentRequirements.Requirement[] GenerateRequirements(
            SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
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
                    return null;
                case SpawnInformation.PlacementTypes.OnEdgeNearCenter:
                    return OnEdge(halfDims);
                case SpawnInformation.PlacementTypes.OnFloorAwayFromMe:
                    return OnFloor(halfDims);
                case SpawnInformation.PlacementTypes.OnFloorNearMe:
                    return OnFloor(halfDims);
                default:
                    return null;
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
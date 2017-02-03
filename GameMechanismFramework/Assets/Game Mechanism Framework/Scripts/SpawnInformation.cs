//Information taken from HoloToolkit examples (LevelSolver) and modified to be more generic - in the examples, the half dimensions are a float randomly generated within a range. Not yet tested thoroughly.

using System;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// Data used for spawning objects according to playspace features.
    /// </summary>
    public static class SpawnInformation
    {
        public struct PlacementQuery
        {
            public PlacementQuery(
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition placementDefinition,
                List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> placementRules = null,
                List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> placementConstraints = null)
            {
                PlacementDefinition = placementDefinition;
                PlacementRules = placementRules ?? new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>();
                PlacementConstraints = placementConstraints ?? new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>();
            }

            public SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition PlacementDefinition;
            public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule> PlacementRules;
            public List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint> PlacementConstraints;
        }

        public enum PlacementTypes
        {
            OnFloor,
            OnWall,
            OnCeiling,
            OnEdge,
            OnFloorAndCeiling,
            RandomInAirAwayFromMe,
            OnEdgeNearCenter,
            OnFloorAwayFromMe,
            OnFloorNearMe
        }

        public static float OnWallMinHeight = 0.5f;
        public static float OnWallMaxHeight = 3f;

        public static PlacementQuery QueryByPlacementType(PlacementTypes type, Vector3 halfDims)
        {
            switch (type)
            {
                case PlacementTypes.OnFloor:
                    return OnFloor(halfDims);
                case PlacementTypes.OnWall:
                    return OnWall(halfDims, OnWallMinHeight, OnWallMaxHeight);
                case PlacementTypes.OnCeiling:
                    return OnCeiling(halfDims);
                case PlacementTypes.OnEdge:
                    return OnEdge(halfDims);
                case PlacementTypes.OnFloorAndCeiling:
                    return OnFloorAndCeiling(halfDims);
                case PlacementTypes.RandomInAirAwayFromMe:
                    return RandomInAir_AwayFromMe(halfDims);
                case PlacementTypes.OnEdgeNearCenter:
                    return OnEdge_NearCenter(halfDims);
                case PlacementTypes.OnFloorAwayFromMe:
                    return OnFloor_AwayFromMe(halfDims);
                case PlacementTypes.OnFloorNearMe:
                    return OnFloor_NearMe(halfDims);
                default:
                    Debug.Log("PlacementType does not exist. Generating on floor.");
                    return OnFloor(halfDims);
            }
        }

        public static PlacementQuery OnFloor(Vector3 halfDims)
        {
            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloor(
                        new Vector3(halfDims.x, halfDims.y, halfDims.z /* * 2.0f*/)),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 3.0f),
                    });
        }

        public static PlacementQuery OnWall(Vector3 halfDims, float heightMin, float heightMax)
        {
            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnWall(
                        new Vector3(halfDims.x, halfDims.y * 0.5f, 0.05f), heightMin, heightMax),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 4.0f),
                    });
        }

        public static PlacementQuery OnCeiling(Vector3 halfDims)
        {
            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnCeiling(
                        halfDims),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 3.0f),
                    });
        }

        public static PlacementQuery OnEdge(Vector3 halfDims)
        {
            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnEdge(
                        halfDims,
                        halfDims),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 3.0f),
                    });
        }

        public static PlacementQuery OnFloorAndCeiling(Vector3 halfDims)
        {
            SpatialUnderstandingDll.Imports.QueryPlayspaceAlignment(
                SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceAlignmentPtr());
            SpatialUnderstandingDll.Imports.PlayspaceAlignment alignment =
                SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceAlignment();

            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloorAndCeiling(
                        new Vector3(halfDims.x, (alignment.CeilingYValue - alignment.FloorYValue) * 0.5f, halfDims.z),
                        new Vector3(halfDims.x, (alignment.CeilingYValue - alignment.FloorYValue) * 0.5f, halfDims.z)),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 3.0f),
                    });
        }

        public static PlacementQuery RandomInAir_AwayFromMe(Vector3 halfDims)
        {
            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_RandomInAir(
                        halfDims),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 3.0f),
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromPosition(
                            Camera.main.transform.position, 2.5f),
                    });
        }

        public static PlacementQuery OnEdge_NearCenter(Vector3 halfDims)
        {
            return new PlacementQuery(
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnEdge(
                    halfDims,
                    halfDims),
                new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                {
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                        Math.Max(halfDims.x, halfDims.z) *
                        2.0f),
                },
                new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>()
                {
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_NearCenter(),
                });
        }

        public static PlacementQuery OnFloor_AwayFromMe(Vector3 halfDims)
        {
            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloor(
                        halfDims),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromPosition(
                            Camera.main.transform.position, 2.0f),
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 3.0f),
                    });
        }

        public static PlacementQuery OnFloor_NearMe(Vector3 halfDims)
        {
            return
                new PlacementQuery(
                    SpatialUnderstandingDllObjectPlacement.ObjectPlacementDefinition.Create_OnFloor(
                        halfDims),
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.Create_AwayFromOtherObjects(
                            Math.Max(halfDims.x, halfDims.z) * 3.0f),
                    },
                    new List<SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint>()
                    {
                        SpatialUnderstandingDllObjectPlacement.ObjectPlacementConstraint.Create_NearPoint(
                            Camera.main.transform.position, 0.5f, 2.0f)
                    });
        }
    }
}
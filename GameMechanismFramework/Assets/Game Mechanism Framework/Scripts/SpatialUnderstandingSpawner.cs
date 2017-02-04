using System;
using System.Collections;
using UnityEngine;
using HoloToolkit.Unity;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GameMechanism
{
    /// <summary>
    /// Spawns objects according to spatial playspace features.
    /// </summary>
    public class SpatialUnderstandingSpawner : Singleton<SpatialUnderstandingSpawner>
    {

        [Serializable]
        public struct SpawnData
        {
            public GameObject Prefab;
            public SpawnInformation.PlacementTypes PlacementType;
            public Vector3 HalfDims;
        }

        public SpawnData[] Data;

        /// <summary>
        /// Called once the scan is finished and the solver initialized
        /// </summary>
        public UnityEvent Initialized;

        private bool _init;
        private SpatialUnderstandingDll _understandingDll;

        // Use this for initialization
        void Start()
        {
            _understandingDll = SpatialUnderstanding.Instance.UnderstandingDLL;
            SpatialUnderstanding.Instance.ScanStateChanged += Init_Spawner;
        }

        /// <summary>
        /// Randomly spawns something from Data
        /// </summary>
        public void Spawn()
        {
            int index = Random.Range(0, Data.Length);
            Spawn(index);
        }

        /// <summary>
        /// Spawns something from Data by its index.
        /// </summary>
        /// <param name="index">Index in Data.</param>
        public void Spawn(int index)
        {
            if (index >= 0 && index < Data.Length)
            {
                SpawnData data = Data[index];
                SpawnInformation.PlacementTypes placementType = data.PlacementType;
                SpawnInformation.PlacementQuery query = SpawnInformation.QueryByPlacementType(placementType,
                    data.HalfDims);

                Spawn(data.Prefab, query);
            }
            else
            {
                Debug.Log("Index out of range");
            }
        }

        public void Spawn(GameObject prefab, SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
        {
            SpawnInformation.PlacementQuery query = SpawnInformation.QueryByPlacementType(placementType, halfDims);
            Spawn(prefab, query);
        }

        public void Spawn(GameObject prefab, SpawnInformation.PlacementQuery query)
        {
            //Don't spawn before the scan is finished.
            if (_init)
            {
                StartCoroutine(ObjectPlacement(prefab, query));
            }
            else
            {
                Debug.Log("Solver not yet initialized");
            }
        }

        IEnumerator ObjectPlacement(GameObject prefab, SpawnInformation.PlacementQuery query)
        {
            //Find a location for the prefab according to the query (definition, rules, and constraints), then place it there
            if (SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(prefab.name,
                    _understandingDll.PinObject(query.PlacementDefinition),
                    query.PlacementRules != null ? query.PlacementRules.Count : 0,
                    _understandingDll.PinObject(query.PlacementRules.ToArray()),
                    query.PlacementConstraints != null ? query.PlacementConstraints.Count : 0,
                    _understandingDll.PinObject(query.PlacementConstraints.ToArray()),
                    _understandingDll.GetStaticObjectPlacementResultPtr()) > 0)
            {
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult placementResult =
                    _understandingDll.GetStaticObjectPlacementResult();
                Quaternion rot = Quaternion.LookRotation(placementResult.Forward, Vector3.up);
                Instantiate(prefab, placementResult.Position, rot);
            }
            yield return null;
        }

        public void Init_Spawner()
        {
            if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Done)
            {
                SpatialUnderstandingDllObjectPlacement.Solver_Init();
                _init = true;
                Initialized.Invoke();
            }
        }

        /// <summary>
        /// Removes all placed prefabs from Solver, meaning their locations are no longer taken into account. Does not destroy GameObjects.
        /// </summary>
        public bool RemoveAllObjects()
        {
            if (_init)
            {
                SpatialUnderstandingDllObjectPlacement.Solver_RemoveAllObjects();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes placed prefabs from Solver, meaning their locations are no longer taken into account. Does not destroy GameObjects.
        /// </summary>
        public bool RemoveObjects(GameObject prefab)
        {
            return RemoveObjects(prefab.name);
        }

        /// <summary>
        /// Removes placed prefabs from Solver, meaning their locations are no longer taken into account. Does not destroy GameObjects.
        /// </summary>
        public bool RemoveObjects (string objectName)
        {
            if (_init)
            {
                int result = SpatialUnderstandingDllObjectPlacement.Solver_RemoveObject(objectName);
                return result != 0;
            }
            return false;
        }
    }
}
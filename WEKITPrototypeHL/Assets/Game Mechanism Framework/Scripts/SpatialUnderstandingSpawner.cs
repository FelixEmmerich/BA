using System.Collections;
using UnityEngine;
using HoloToolkit.Unity;

namespace GameMechanism
{
    public class SpatialUnderstandingSpawner : Singleton<SpatialUnderstandingSpawner>
    {
        public GameObject Prefab;
        public SpawnInformation.PlacementTypes PlacementType;
        [Tooltip("Half dimensions of the object to be spawned")] public Vector3 HalfDims;
        [Tooltip("If true, spawns an object as soon as the scan is complete.")] public bool SpawnImmediately = true;

        private bool _init;
        private SpatialUnderstandingDll _understandingDll;
        public SpawnInformation.PlacementQuery PlacementQuery;

        // Use this for initialization
        void Start()
        {
            _understandingDll = SpatialUnderstanding.Instance.UnderstandingDLL;
            SpatialUnderstanding.Instance.ScanStateChanged += Init_Spawner;
            PlacementQuery = SpawnInformation.QueryByPlacementType(PlacementType, HalfDims);
        }

        public void Spawn()
        {
            Spawn(Prefab, PlacementQuery);
        }

        public void Spawn(GameObject prefab, SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
        {
            SpawnInformation.PlacementQuery query = SpawnInformation.QueryByPlacementType(placementType, halfDims);
            Spawn(prefab, query);
        }

        public void Spawn(GameObject prefab, SpawnInformation.PlacementQuery query)
        {
            //Sollte nicht gemacht werden, bevor Scan fertig ist.
            if (_init)
            {
                //DestroyObjects();
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

        IEnumerator ObjectPlacement(GameObject prefab, SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
        {
            //Generate an environment query by the placement type and half dimensions
            SpawnInformation.PlacementQuery query = SpawnInformation.QueryByPlacementType(placementType, halfDims);

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
                if (SpawnImmediately)
                {
                    Spawn();
                }
            }
        }

        public void DestroyObjects()
        {
            if (_init)
            {
                SpatialUnderstandingDllObjectPlacement.Solver_RemoveObject(Prefab.name);
            }
        }
    }
}
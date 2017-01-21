using System;
using System.Collections;
using UnityEngine;
using HoloToolkit.Unity;

namespace GameMechanism
{
    public class SpatialUnderstandingSpawner : Singleton<SpatialUnderstandingSpawner>
    {
        public GameObject Prefab;
        public SpawnInformation SpawnInformation;
        public SpawnInformation.PlacementTypes PlacementType;
        [Tooltip("Half dimensions of the object to be spawned")] public Vector3 HalfDims;
        public TextToSpeechManager TextToSpeech;
        [Tooltip("If true, spawns an object as soon as the scan is complete.")]
        public bool SpawnImmediately = true;

        private bool _init;
        private SpatialUnderstandingDll _understandingDll;

        // Use this for initialization
        void Start()
        {
            _understandingDll = SpatialUnderstanding.Instance.UnderstandingDLL;
            SpatialUnderstanding.Instance.ScanStateChanged += Init_Spawner;
        }

        public void Spawn()
        {
            Spawn(Prefab, PlacementType, HalfDims);
        }

        public void Spawn(GameObject prefab, SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
        {
            //Sollte nicht gemacht werden, bevor Scan fertig ist.
            if (_init)
            {
                //DestroyObjects();
                TextToSpeech.SpeakText("Spawning object");
                StartCoroutine(ObjectPlacement(prefab, placementType, halfDims));
            }
            else
            {
                TextToSpeech.SpeakText("Not initialized");
            }
        }

        IEnumerator ObjectPlacement(GameObject prefab, SpawnInformation.PlacementTypes placementType, Vector3 halfDims)
        {
            SpawnInformation.PlacementQuery query = SpawnInformation.QueryByPlacementType(placementType, halfDims);

            //Mit Definition nicht so sicher (Online-Beispiel ist falsch bzw. nicht komplett)
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
            TextToSpeech.SpeakText("State changed to "+ SpatialUnderstanding.Instance.ScanState);
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
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

        private bool _init;
        private SpatialUnderstandingDll _understandingDll;

        // Use this for initialization
        void Start()
        {
            _understandingDll = SpatialUnderstanding.Instance.UnderstandingDLL;
            SpatialUnderstanding.Instance.ScanStateChanged += Init_Spawner;
        }

        //Should maybe be a coroutine or a System.Threading task if not run in Unity
        public void Spawn()
        {
            //Sollte nicht gemacht werden, bevor Scan fertig ist.
            if (_init)
            {
                //DestroyObjects();
                TextToSpeech.SpeakText("Spawning object");
                Debug.Log("Spawning object");
                StartCoroutine(ObjectPlacement());
            }
            else
            {
                TextToSpeech.SpeakText("Not initialized");
                Debug.Log("Not initialized yet");
            }
        }

        IEnumerator ObjectPlacement()
        {
            SpawnInformation.PlacementQuery query = SpawnInformation.QueryByPlacementType(PlacementType, HalfDims);

            TextToSpeech.SpeakText("Check for null");
            TextToSpeech.SpeakText("Prefab name" + (Prefab.name==null));
            TextToSpeech.SpeakText("Rules" + (query.PlacementRules == null));
            TextToSpeech.SpeakText("Constraints" + (query.PlacementConstraints == null));
            TextToSpeech.SpeakText("Pointer" + (_understandingDll.GetStaticObjectPlacementResultPtr()==null));

            //Mit Definition nicht so sicher (Online-Beispiel ist falsch bzw. nicht komplett)
            if (SpatialUnderstandingDllObjectPlacement.Solver_PlaceObject(Prefab.name,
                    _understandingDll.PinObject(query.PlacementDefinition),
                    query.PlacementRules!=null?query.PlacementRules.Count:0,
                    _understandingDll.PinObject(query.PlacementRules.ToArray()),
                    query.PlacementConstraints != null ? query.PlacementConstraints.Count : 0,
                    _understandingDll.PinObject(query.PlacementConstraints.ToArray()),
                    _understandingDll.GetStaticObjectPlacementResultPtr()) > 0)
            {
                SpatialUnderstandingDllObjectPlacement.ObjectPlacementResult placementResult =
                    _understandingDll.GetStaticObjectPlacementResult();
                Quaternion rot = Quaternion.LookRotation(placementResult.Forward, Vector3.up);
                Instantiate(Prefab, placementResult.Position, rot);
                TextToSpeech.SpeakText("Spawned box");
            }
            else
            {
                TextToSpeech.SpeakText("Could not spawn");
                Debug.Log("Couldn't spawn object");
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
                Spawn();
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
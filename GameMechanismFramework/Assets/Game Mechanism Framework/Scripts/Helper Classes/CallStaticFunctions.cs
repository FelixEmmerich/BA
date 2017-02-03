using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// Calls static functions. The functions in this script can be called from UnityEvents.
    /// </summary>
    public class CallStaticFunctions : MonoBehaviour
    {
        public void Spawn()
        {
            SpatialUnderstandingSpawner.Instance.Spawn();
        }

        public void Quit()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }

        public void StartSpatialUnderstandingScan()
        {
            SpatialUnderstanding.Instance.RequestBeginScanning();
        }

        public void StopSpatialUnderstandingScan()
        {
            SpatialUnderstanding.Instance.RequestFinishScan();
        }

        public void StartSpatialMappingObserver()
        {
            SpatialMappingManager.Instance.StartObserver();
        }

        public void StopSpatialMappingObserver()
        {
            SpatialMappingManager.Instance.StopObserver();
        }
    }
}
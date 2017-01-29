using UnityEngine;

//
namespace GameMechanism
{
    //Gets the SpatialUnderstandingSpawner Singleton and calls Spawn(). Can be placed called as a UnityEvent.
    public class CallSpatialUnderstandingRespawn : MonoBehaviour
    {
        private SpatialUnderstandingSpawner _spawner;

        void Start()
        {
            _spawner = SpatialUnderstandingSpawner.Instance;
        }

        public void Spawn()
        {
            _spawner.Spawn();
        }
    }

}
using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// Gets the SpatialUnderstandingSpawner Singleton and calls Spawn(). Can be called as a UnityEvent.
    /// </summary>
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
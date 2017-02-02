using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// Class necessary to call Application.Quit() from a UnityEvent
    /// </summary>
    public class QuitGame : MonoBehaviour
    {
        public void Quit()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }
}
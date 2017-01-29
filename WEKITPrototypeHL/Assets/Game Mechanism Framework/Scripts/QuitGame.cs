//Class necessary to call Application.Quit() from a UnityEvent
using UnityEngine;

namespace GameMechanism
{
    public class QuitGame : MonoBehaviour
    {
        public void Quit()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }

    }

}
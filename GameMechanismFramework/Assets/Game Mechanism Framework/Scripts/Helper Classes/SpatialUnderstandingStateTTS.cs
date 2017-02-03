using HoloToolkit.Unity;
using UnityEngine;

namespace GameMechanism
{
    /// <summary>
    /// Announces change in the spatial understanding state
    /// </summary>
    public class SpatialUnderstandingStateTTS : MonoBehaviour
    {
        public TextToSpeechManager TextToSpeechManager;

        // Use this for initialization
        void Start()
        {
            if (TextToSpeechManager == null)
            {
                TextToSpeechManager = gameObject.AddComponent<TextToSpeechManager>();
            }
            SpatialUnderstanding.Instance.ScanStateChanged += StateChange;
        }

        void StateChange()
        {
            TextToSpeechManager.SpeakText("State changed to " + SpatialUnderstanding.Instance.ScanState);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanism
{
    /// <summary>
    /// Fills a GUI image. Can be called by Interactables on Enter() to visualize progress towards Stay().
    /// </summary>
    public class FillCanvasImage : MonoBehaviour
    {
        public Image Image;
        public bool Active = false;
        public float TransitionTime;
        private float _startTime;

        void Update()
        {
            if (Active)
            {
                float amount = (Time.time - _startTime) / TransitionTime;
                if (amount <= 1)
                {
                    Image.fillAmount = amount;
                }
                else
                {
                    Active = false;
                }
            }
        }

        public void BeginFill(float time)
        {
            TransitionTime = time;
            _startTime = Time.time;
            Active = true;
        }

        public void BeginFill(Interactable interactable)
        {
            //Finish a little earlier to ensure the fill animation is completed
            BeginFill(interactable.StayDuration - 0.1f);
        }

        public void AbortFill()
        {
            Active = false;
            Image.fillAmount = 0;
        }
    }
}
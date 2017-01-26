using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanism
{
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
            BeginFill(interactable.StayDuration);
        }

        public void AbortFill()
        {
            Active = false;
            Image.fillAmount = 0;
        }
    } 
}

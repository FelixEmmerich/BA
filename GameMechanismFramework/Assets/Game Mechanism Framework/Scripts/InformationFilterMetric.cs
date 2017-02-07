using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public abstract class InformationFilterMetric : MonoBehaviour
    {
        private float _currentValue=-1;
        public float TimeBetweenUpdates=0.1f;

        public float GetCurrentValue()
        {
            return _currentValue;
        }

        public virtual void Start()
        {
            StartCoroutine(UpdateCurrentValue());
        }

        private IEnumerator UpdateCurrentValue()
        {
            for (;;)
            {
                _currentValue=GetNewestValue();
                yield return new WaitForSeconds(TimeBetweenUpdates);
            }
        }

        protected abstract float GetNewestValue();
    }

}
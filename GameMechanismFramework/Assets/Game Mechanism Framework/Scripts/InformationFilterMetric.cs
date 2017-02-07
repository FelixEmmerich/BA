using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMechanism
{
    public abstract class InformationFilterMetric
    {
        private bool _updatedFlag = false;
        private float _currentValue;

        public float GetCurrentValue()
        {
            if (!_updatedFlag)
            {
                _currentValue = GetNewestValue();
            }
            return _currentValue;
        }

        private void Update()
        {
            _updatedFlag = false;
        }

        protected abstract float GetNewestValue();
    }

}
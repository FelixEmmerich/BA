using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class InformationFilter : MonoBehaviour
    {
        [Serializable]
        public struct Filter
        {
            public float MaxDistance;
            public float MinDistance;
            [Tooltip("If the user is between MinDistance and MaxDistance, this event will be invoked once (reset when the user gets outside the range).")]
            public UnityEvent WithinRangeEvent;
            [Tooltip("Invoked once if the user is closer than MinDistance or further than MaxDistance. Can be used to undo the effects of WithinRangeEvent. Reset when the user gets within range.")]
            public UnityEvent OutsideRangeEvent;
        }

        [Tooltip("List of filters. Sorted by max distance (descending) at start (best performance if pre-sorted).")]
        public Filter[] Filters;

        [Tooltip("If true, only x and z coordinates will be compared.")]
        public bool Use2DDistance=true;

        public float TimeBetweenUpdates = 0.05f;

        private bool[] _previouslyWithinRanges;

        // Use this for initialization
        void Start()
        {
            BubbleSort(Filters);
            _previouslyWithinRanges = new bool[Filters.Length];
            for (int i = _previouslyWithinRanges.Length - 1; i >= 0; i--)
            {
                _previouslyWithinRanges[i] = false;
            }
            StartCoroutine(UpdateFilters());
        }

        private IEnumerator UpdateFilters()
        {
            for (;;)
            {
                DistanceCheck();
                yield return new WaitForSeconds(TimeBetweenUpdates);
            }
        }

        private void DistanceCheck()
        {
            Vector3 cameraPos = Camera.main.transform.position;
            float distance = Use2DDistance? new Vector2(cameraPos.x-transform.position.x,cameraPos.z-transform.position.z).magnitude:(cameraPos - transform.position).magnitude;
            for (int i = 0; i < Filters.Length; i++)
            {
                bool withinRange = (Filters[i].MinDistance < distance && distance < Filters[i].MaxDistance);
                if (withinRange != _previouslyWithinRanges[i])
                {
                    (withinRange?Filters[i].WithinRangeEvent:Filters[i].OutsideRangeEvent).Invoke();
                    _previouslyWithinRanges[i] = withinRange;
                }
            }
        }

        public void BubbleSort(Filter[] filters)
        {
            Filter temp;

            for (int i = 0; i < filters.Length; i++)
            {
                for (int j = 0; j < filters.Length - 1; j++)
                {
                    if (filters[j].MaxDistance < filters[j + 1].MaxDistance)
                    {
                        temp = filters[j + 1];
                        filters[j + 1] = filters[j];
                        filters[j] = temp;
                    }
                }
            }
        }

    }
}
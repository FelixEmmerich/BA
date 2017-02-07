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
        public struct Rule
        {
            public InformationFilterMetric Metric;
            public float Value;
            [Tooltip("Whether the value is a minimum or a maximum.")]
            public bool Minimum;
        }

        [Serializable]
        public struct RuleSet
        {
            public Rule[] Rules;
            public UnityEvent ConditionsMet;
            [HideInInspector]
            public bool _previouslytrue;
        }

        public RuleSet[] RuleSets;

        // Update is called once per frame
        void Update()
        {
            CheckRuleSets();
        }

        public void CheckRuleSets()
        {
            List<InformationFilterMetric> _changedMetrics = new List<InformationFilterMetric>();
            for (int i = 0; i < RuleSets.Length; i++)
            {
                bool conditionsMet = true;
                for (int j = 0; j < RuleSets[i].Rules.Length; j++)
                {
                    if ((RuleSets[i].Rules[j].Metric.GetCurrentValue() >= RuleSets[i].Rules[j].Value) != RuleSets[i].Rules[j].Minimum)
                    {
                        conditionsMet = false;
                        if (RuleSets[i]._previouslytrue)
                        {
                            //TODO: only add if not contained
                            _changedMetrics.Add(RuleSets[i].Rules[j].Metric);
                        }
                    }
                }
                RuleSets[i]._previouslytrue = conditionsMet;
            }
            if (_changedMetrics.Count > 0)
            {
                //Go through everything and call the events in the rulesets that are previouslytrue and use the metrics in the list
            }
        }
    }

}
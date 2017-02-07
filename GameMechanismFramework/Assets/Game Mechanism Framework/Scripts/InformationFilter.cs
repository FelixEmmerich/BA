using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameMechanism
{
    public class InformationFilter : MonoBehaviour
    {

        [Serializable]
        public struct Condition
        {
            public InformationFilterMetric Metric;
            public float Value;
            [Tooltip("Whether the value is a minimum or a maximum.")]
            public bool Minimum;
        }

        [Serializable]
        public struct RuleSet
        {
            public Condition[] Conditions;
            [Tooltip("Called once when conditions switch from not met to met")]
            public UnityEvent ConditionsMet;
            [Tooltip("Called once when conditions switch from met to not met")]
            public UnityEvent ConditionsNotMet;
            [HideInInspector]
            public bool Previouslytrue;
        }

        [Tooltip("UnityEvents are called in order, so sort RuleSets in ascending order. e.g. If multiple RuleSets change one UI text, the last one in this array will be executed last.")]
        public RuleSet[] RuleSets;

        // Update is called once per frame
        void Update()
        {
            CheckRuleSets();
        }

        public void CheckRuleSets()
        {
            for (int i = 0; i < RuleSets.Length; i++)
            {
                bool allConditionsMet = true;

                for (int j = 0; j < RuleSets[i].Conditions.Length; j++)
                {
                    Condition condition = RuleSets[i].Conditions[j];
                    float actualValue = condition.Metric.GetCurrentValue();
                    //Check if actual value behaves to required value as necessary
                    bool conditionMet = (actualValue>=0) && (actualValue >= condition.Value == condition.Minimum);

                    //Add metrics to appropriate lists in case the
                    if (conditionMet == false)
                    {
                        allConditionsMet = false;
                        break;
                    }
                }

                //If the state of the ruleset changed, add the affected metrics to the list 
                if (RuleSets[i].Previouslytrue != allConditionsMet)
                {
                    (allConditionsMet?RuleSets[i].ConditionsMet:RuleSets[i].ConditionsNotMet).Invoke();
                }
                RuleSets[i].Previouslytrue = allConditionsMet;
            }
        }
        
    }

}
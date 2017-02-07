using System;
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

        [Tooltip("UnityEvents are called in order, so sort RuleSets in ascending order. e.g. If multiple rules change one UI text, the last one in this array will be executed last.")]
        public RuleSet[] RuleSets;

        // Update is called once per frame
        void Update()
        {
            CheckRuleSets();
        }

        public void CheckRuleSets()
        {
            List<InformationFilterMetric> changedMetrics = new List<InformationFilterMetric>();
            for (int i = 0; i < RuleSets.Length; i++)
            {
                bool allConditionsMet = true;
                List<InformationFilterMetric> trueMetrics = new List<InformationFilterMetric>();
                List<InformationFilterMetric> falseMetrics = new List<InformationFilterMetric>();

                for (int j = 0; j < RuleSets[i].Rules.Length; j++)
                {
                    //Check if actual value behaves to required value as necessary
                    bool conditionMet = (RuleSets[i].Rules[j].Metric.GetCurrentValue() >= RuleSets[i].Rules[j].Value) !=
                                        RuleSets[i].Rules[j].Minimum;

                    //Add metrics to appropriate lists in case the
                    if (conditionMet == false)
                    {
                        allConditionsMet = false;
                        falseMetrics.Add(RuleSets[i].Rules[j].Metric);
                    }
                    else
                    {
                        trueMetrics.Add(RuleSets[i].Rules[j].Metric); 
                    }
                }

                //If the state of the ruleset changed, add the affected metrics to the list 
                if (RuleSets[i]._previouslytrue != allConditionsMet)
                {
                    ExtendListWithoutOverlap(ref changedMetrics, allConditionsMet?trueMetrics:falseMetrics);
                }
                RuleSets[i]._previouslytrue = allConditionsMet;
            }
            //If any rulesets have changed states, call ConditionsMet in the ones now true that use the affected metrics
            if (changedMetrics.Count > 0)
            {
                for (int i = 0; i < RuleSets.Length; i++)
                {
                    if (RuleSets[i]._previouslytrue && RuleSetContainsAnyMetric(RuleSets[i], changedMetrics))
                    {
                        RuleSets[i].ConditionsMet.Invoke();
                    }
                }
            }
        }

        private void ExtendListWithoutOverlap(ref List<InformationFilterMetric> baseList, List<InformationFilterMetric> extension)
        {
            for (int i = extension.Count - 1; i >= 0; i--)
            {
                if (!baseList.Contains(extension[i]))
                {
                    baseList.Add(extension[i]);
                }
            }   
        }

        private bool RuleSetContainsAnyMetric(RuleSet ruleSet, List<InformationFilterMetric> metrics)
        {
            for (int i = metrics.Count - 1; i >= 0; i--)
            {
                for (int j = ruleSet.Rules.Length - 1; j >= 0; j--)
                {
                    if (metrics[i] == ruleSet.Rules[j].Metric)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

}
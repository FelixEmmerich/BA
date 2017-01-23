using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanism
{
    public class EnvironmentRequirementGUI : MonoBehaviour
    {
        public EnvironmentRequirements Requirements;
        public Canvas Canvas;
        public Text ResultsText;
        [Tooltip("Tags in the Rich format, applied at the result screen for requirements that were met.")]
        public string MetRequirementTags;
        [Tooltip("Closing tags in the Rich format, applied at the result screen for requirements that were met.")]
        public string MetRequirementEndTags;
        [Tooltip("Tags in the Rich format, applied at the result screen for requirements that were not met.")]
        public string FailedRequirementTags;
        [Tooltip("Closing tags in the Rich format, applied at the result screen for requirements that were met.")]
        public string FailedRequirementEndTags;

        // Use this for initialization
        void Start()
        {
            SpatialUnderstanding.Instance.ScanStateChanged += DisplayResults;
        }

        public void DisplayResults()
        {
            if (SpatialUnderstanding.Instance.ScanState==SpatialUnderstanding.ScanStates.Done)
            {
                bool[] status;
                int resultInt = Requirements.CheckAllRequirements(out status);
                if (resultInt >= 0)
                {
                    string finalText="";
                    string[] requirementTextArray = RequirementsToStringArray();
                    for (int i = 0; i < status.Length; i++)
                    {
                        finalText += status[i] ? MetRequirementTags : FailedRequirementTags;
                        finalText += requirementTextArray[i];
                        finalText+= status[i] ? MetRequirementEndTags : FailedRequirementEndTags;
                        finalText += "\n";
                    }
                    ResultsText.enabled = true;
                    ResultsText.text = finalText;
                }
                else
                {
                    Debug.Log("Scan not done yet");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        string[] RequirementsToStringArray()
        {
            string[] result = new string[Requirements.Requirements.Length];
            for (int i = result.Length - 1; i >= 0; i--)
            {
                result[i] = RequirementToString(Requirements.Requirements[i]);
            }
            return result;
        }

        string RequirementToString(EnvironmentRequirements.Requirement requirement)
        {
            string greaterthan = requirement.GreaterThanOrEqual ? "At least " : "Less than ";
            return greaterthan + requirement.Amount + CategoryToString(requirement.Category);
        }

        string CategoryToString(EnvironmentRequirements.RequirementCategory category)
        {
            switch (category)
            {
                case EnvironmentRequirements.RequirementCategory.HorizSurfaceArea:
                    return "m² horizontal surfaces near ground";
                case EnvironmentRequirements.RequirementCategory.TotalSurfaceArea:
                    return "m² total surface area";
                case EnvironmentRequirements.RequirementCategory.UpSurfaceArea:
                    return "m² of surfaces pointing up";
                case EnvironmentRequirements.RequirementCategory.DownSurfaceArea:
                    return "m² of surfaces pointing down";
                case EnvironmentRequirements.RequirementCategory.WallSurfaceArea:
                    return "m² of wall surface area";
                case EnvironmentRequirements.RequirementCategory.VirtualCeilingSurfaceArea:
                    return "m² of virtual ceiling";
                case EnvironmentRequirements.RequirementCategory.VirtualWallSurfaceArea:
                    return "m² of virtual wall surface area";
                case EnvironmentRequirements.RequirementCategory.NumFloor:
                    return " floor surfaces";
                case EnvironmentRequirements.RequirementCategory.NumCeiling:
                    return " ceiling surfaces";
                case EnvironmentRequirements.RequirementCategory.NumWall_XNeg:
                    return " negative x wall surfaces";
                case EnvironmentRequirements.RequirementCategory.NumWall_XPos:
                    return " positive x wall surfaces";
                case EnvironmentRequirements.RequirementCategory.NumWall_ZNeg:
                    return " negative z wall surfaces";
                case EnvironmentRequirements.RequirementCategory.NumWall_ZPos:
                    return " positive z wall surfaces";
                case EnvironmentRequirements.RequirementCategory.NumWall:
                    return " wall surfaces";
                case EnvironmentRequirements.RequirementCategory.NumPlatform:
                    return " horizontal surfaces other than ground";
                default:
                    return category.ToString();
            }
        }
    }

}
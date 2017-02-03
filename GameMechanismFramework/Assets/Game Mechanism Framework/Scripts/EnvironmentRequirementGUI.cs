using System.Collections;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace GameMechanism
{
    /// <summary>
    /// Display EnvironmentRequirements and their success state.
    /// </summary>
    public class EnvironmentRequirementGUI : MonoBehaviour
    {
        public EnvironmentRequirements Requirements;

        [Tooltip("GameObject is disabled on start and enabled when the requirements are met.")]
        public GameObject StartObject;

        [Tooltip("Text object where the requirements are displayed.")]
        public Text RequirementsText;

        [Tooltip("Tags in the Rich format, applied for requirements that were met.")]
        public string MetRequirementTags;

        [Tooltip("Closing tags in the Rich format, applied for requirements that were met.")]
        public string MetRequirementEndTags;

        [Tooltip("Tags in the Rich format, applied for requirements that were not met.")]
        public string FailedRequirementTags;

        [Tooltip("Closing tags in the Rich format, applied for requirements that were not met.")]
        public string FailedRequirementEndTags;

        [Tooltip("Time between checks.")]
        public float UpdateTime = 3;

        private string[] _requirementStrings;

        // Use this for initialization
        void Start()
        {
            Requirements.RequirementsSet.AddListener(DisplayStartScreen);
            _requirementStrings = RequirementsToStringArray(Requirements.Requirements);
            SpatialUnderstanding.Instance.ScanStateChanged += DisplayResults;
            SpatialUnderstanding.Instance.ScanStateChanged += BeginUpdatingResults;
        }

        public void DisplayStartScreen()
        {
            StartObject.SetActive(false);
            string fullstring = "Requirements:\n";
            for (int i = 0; i < _requirementStrings.Length; i++)
            {
                fullstring += _requirementStrings[i] + "\n";
            }
            RequirementsText.text = fullstring;
        }

        public void DisplayResults()
        {
            bool[] status;
            float[] amount;

            int resultInt = Requirements.CheckAllRequirements(out status, out amount);
            if (resultInt >= 0)
            {
                StartObject.SetActive(resultInt == 1);
                string finalText = "";
                for (int i = 0; i < status.Length; i++)
                {
                    finalText += status[i] ? MetRequirementTags : FailedRequirementTags;
                    finalText += _requirementStrings[i];
                    finalText += " -> " + amount[i];
                    finalText += status[i] ? MetRequirementEndTags : FailedRequirementEndTags;
                    finalText += "\n";
                }
                RequirementsText.enabled = true;
                RequirementsText.text = finalText;
            }
            else
            {
                Debug.Log("Scan not done yet");
            }
        }

        void BeginUpdatingResults()
        {
            if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning)
            {
                StartCoroutine(UpdateResults());
            }
        }

        public IEnumerator UpdateResults()
        {
            for (;;)
            {
                DisplayResults();
                yield return new WaitForSeconds(UpdateTime);
            }
        }

        string[] RequirementsToStringArray(EnvironmentRequirements.Requirement[] requirements)
        {
            string[] result = new string[requirements.Length];
            for (int i = result.Length - 1; i >= 0; i--)
            {
                result[i] = RequirementToString(requirements[i]);
            }
            return result;
        }

        string RequirementToString(EnvironmentRequirements.Requirement requirement)
        {
            string result="";
            result += requirement.GreaterThanOrEqual ? "At least " : "Less than ";
            result += requirement.Amount;
            result += CategoryToString(requirement.Category);
            return result;
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
                case EnvironmentRequirements.RequirementCategory.CeilingSurfaceArea:
                    return "m² of ceiling surface area";
                case EnvironmentRequirements.RequirementCategory.PlatformSurfaceArea:
                    return "m² of platform surface area";
                default:
                    return category.ToString();
            }
        }
    }
}
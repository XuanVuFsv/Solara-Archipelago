using Sirenix.OdinInspector;
using UnityEngine;

namespace VitsehLand.Scripts.Stats
{
    [System.Serializable]
    public class FarmingCropStat : CollectableObjectStatComponent
    {
        public enum BodyType
        {
            None = -1,
            Tree = 0,
            Vegetable = 1,
            Climbing = 2
        }

        [DetailedInfoBox("Body Types  of Crop:", 
            "-None for Non-woody plants (e.g., lettuce).\n" +
            "-Tree for fruit-bearing crop (e.g., apple).\n" +
            "-Vegetable for vegetables (eg., carrot, lettuce).\n" +
            "-Climbing for climbing plants (eg., grapes).")]
        public BodyType bodyType;
        [Tooltip("Except None BodyType")]
        public GameObject growingBody;

        public int totalGrowingTime;
        public int totalHarvestingQuantity;
        public int wateringTime;

        public int requiredLevel;
        public string description;

        public int gemEarnWhenHaverst;
    }
}
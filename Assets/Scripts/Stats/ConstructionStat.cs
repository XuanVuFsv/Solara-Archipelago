using System.Collections.Generic;
using UnityEngine;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Construction", menuName = "Construction")]
    public class ConstructionStat : ScriptableObject
    {
        public List<ConstructionUpgradingStat> constructionUpgradingStats = new List<ConstructionUpgradingStat>();
        public int MaxConstructionUpgradingStats
        {
            get { return constructionUpgradingStats.Count; }
        }

        public Sprite icon;
        public string constructionName;

        public int cost;
    }
}
using UnityEngine;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Construction Upgrading Stat", menuName = "Construction Upgrading Stat")]
    public class ConstructionUpgradingStat : ScriptableObject
    {
        public Sprite icon;
        public string constructionUpgradingName;
        public int cost;
    }
}
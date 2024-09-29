using UnityEngine;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Construction Upgrading Stats", menuName = "Construction Upgrading Stats")]
    public class ConstructionUpgradingStats : ScriptableObject
    {
        public Sprite icon;
        public string constructionUpgradingName;
        public int cost;
    }
}
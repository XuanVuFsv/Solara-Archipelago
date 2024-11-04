using UnityEngine;
using VitsehLand.Scripts.Crafting;
using VitsehLand.Scripts.Farming.General;

namespace VitsehLand.Scripts.Stats
{
    [System.Serializable]
    public class CraftedProductStat : CollectableObjectStatComponent
    {
        public RecipeData recipe;
        public float totalProducingTime;
        public int cost;
    }
}
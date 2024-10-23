using UnityEngine;
using VitsehLand.Assets.Scripts.Farming.General;
using VitsehLand.Scripts.Crafting;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Power Stat", menuName = "Power Stat")]
    public class PowerStat : ScriptableObject
    {
        public GameObjectType.FilteredType filteredType;
        public RecipeData recipe;
    }
}
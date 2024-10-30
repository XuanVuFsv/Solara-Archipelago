using UnityEngine;
using VitsehLand.Scripts.Crafting;
using VitsehLand.Scripts.Farming.General;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New Power Stat", menuName = "Power Stat")]
    public class PowerStat : ScriptableObject
    {
        public GameObjectType.FilteredType filteredType;
        public RecipeData recipe;
    }
}
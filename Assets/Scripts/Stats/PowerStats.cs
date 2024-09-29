using UnityEngine;
using VitsehLand.Assets.Scripts.Farming.General;
using VitsehLand.Scripts.Crafting;

namespace VitsehLand.Scripts.Stats
{
    [CreateAssetMenu(fileName = "New PowerCotainer", menuName = "PowerContainer")]
    public class PowerStats : ScriptableObject
    {
        public GameObjectType.FilteredType filteredType;
        public RecipeData recipe;
    }
}
using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Crafting
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
    public class RecipeData : ScriptableObject
    {
        public CropStats cropStats;
        public List<CropStats> items;
        public List<int> ammountPerSlots;
        public Suckable product;
    }
}
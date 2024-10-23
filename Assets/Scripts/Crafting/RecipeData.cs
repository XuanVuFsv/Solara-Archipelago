using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Crafting
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
    public class RecipeData : ScriptableObject
    {
        public CollectableObjectStat collectableObjectStat;
        public List<CollectableObjectStat> items;
        public List<int> ammountPerSlots;
        public Suckable product;
    }
}
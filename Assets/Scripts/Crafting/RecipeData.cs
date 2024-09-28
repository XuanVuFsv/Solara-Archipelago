using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    public CropStats cropStats;
    public List<CropStats> items;
    public List<int> ammountPerSlots;
    public Suckable product;
}

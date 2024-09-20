using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    public AmmoStats ammoStats;
    public string recipeString;
    public List<AmmoStats> item;
    public List<int> ammountPerSlot;
    public Suckable product;
}

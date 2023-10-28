using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    List<AmmoStats> item;
    List<int> ammountPerSlot;
    public Suckable product;
    public int timeToComplete;
}

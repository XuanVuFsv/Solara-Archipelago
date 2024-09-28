using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerCotainer", menuName = "PowerContainer")]
public class PowerStats : ScriptableObject
{
    public GameObjectType.FilteredType filteredType;
    public RecipeData recipe;
}
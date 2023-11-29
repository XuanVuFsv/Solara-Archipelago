using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Construction Upgrading Stats", menuName = "Construction Upgrading Stats")]
public class ConstructionUpgradingStats : ScriptableObject
{
    public Sprite icon;
    public string constructionUpgradingName;
    public int cost;
}

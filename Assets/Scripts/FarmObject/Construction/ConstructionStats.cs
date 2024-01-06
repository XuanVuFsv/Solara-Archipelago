using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Construction", menuName = "Construction")]
public class ConstructionStats : ScriptableObject
{
    public List<ConstructionUpgradingStats> constructionUpgradingStats = new List<ConstructionUpgradingStats>();
    public int MaxConstructionUpgradingStats
    {
        get { return constructionUpgradingStats.Count; }
    }

    public Sprite icon;
    public string constructionName;

    public int cost;
}

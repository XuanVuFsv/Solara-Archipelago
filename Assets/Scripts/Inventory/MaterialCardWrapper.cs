using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class MaterialCardWrapper : MonoBehaviour
{
    //public MaterialCardWrapper(AmmoStats ammoStats, int quantity)
    //{
    //    image.sprite = ammoStats.artwork;
    //    this.ammoStats = ammoStats;
    //    this.quantity = quantity;
    //}

    public Image image;
    public TextMeshProUGUI quanityText;
    public AmmoStats ammoStats;

    public int quantity, requiredQuantity;

    public void OnClick()
    {
        Debug.Log("Click" + " " + ammoStats.name);
        if (ammoStats == null || ammoStats.name == "Null") return;
        CraftingManagerUI.Instance.ShowCurrentItemInformation(ammoStats);
    }
}

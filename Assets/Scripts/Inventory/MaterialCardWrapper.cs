using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class MaterialCardWrapper : MonoBehaviour
{
    //public MaterialCardWrapper(CropStats cropStats, int quantity)
    //{
    //    image.sprite = cropStats.artwork;
    //    this.cropStats = cropStats;
    //    this.quantity = quantity;
    //}

    public Image image;
    public TextMeshProUGUI quanityText;
    public CropStats cropStats;

    public int quantity, requiredQuantity;

    public void OnClick()
    {
        Debug.Log("Click" + " " + cropStats.name);
        if (cropStats == null || cropStats.name == "Null") return;
        CraftingManagerUI.Instance.ShowCurrentItemInformation(cropStats);
    }
}

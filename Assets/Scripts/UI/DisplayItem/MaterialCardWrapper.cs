using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VitsehLand.Assets.Scripts.UI.Crafting;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.UI.DisplayItem
{
    [Serializable]
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
}
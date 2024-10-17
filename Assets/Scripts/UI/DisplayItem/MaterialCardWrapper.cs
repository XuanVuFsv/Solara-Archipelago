using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.UI.DisplayItem
{
    [Serializable]
    public class MaterialCardWrapper : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI quantityText;
        public CropStats cropStats;

        public int quantity, requiredQuantity;
    }
}
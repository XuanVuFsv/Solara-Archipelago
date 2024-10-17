using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VitsehLand.Assets.Scripts.UI.Crafting;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.UI.DisplayItem
{
    public class ItemUI : MonoBehaviour
    {
        public int index = 0;

        public CropStats cropStats;

        public Image icon;
        public TextMeshProUGUI nameItem;

        public event Action<int, CropStats> OnItemClicked = delegate { };

        // Start is called before the first frame update
        void Awake()
        {
            icon = transform.GetChild(0).GetComponent<Image>();
            nameItem = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            icon.gameObject.SetActive(false);
            GetComponent<Button>().interactable = false;
            GetComponent<Button>().onClick.AddListener(OnClick);

            if (cropStats != null) SetItemUI(cropStats);
        }

        public void SetItemUI(CropStats cropStats)
        {
            if (cropStats.name == "Null")
            {
                icon.gameObject.SetActive(false);
                nameItem.text = "";
                return;
            }
            icon.gameObject.SetActive(true);
            GetComponent<Button>().interactable = true;
            icon.sprite = cropStats.artwork;
            nameItem.text = cropStats.name;
        }

        public void OnClick()
        {
            Debug.Log("Click" + " " + cropStats.name);
            if (cropStats == null || cropStats.name == "Null") return;

            OnItemClicked(index, cropStats);
        }

        public void RegisterListener(Action<int, CropStats> listener)
        {
            OnItemClicked += listener;
        }
    }
}
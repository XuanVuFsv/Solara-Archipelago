using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.UI.DisplayItem
{
    public class ItemUI : MonoBehaviour
    {
        public int index = 0;

        public CollectableObjectStat collectableObjectStat;

        public Image icon;
        public TextMeshProUGUI nameItem;

        public event Action<CollectableObjectStat> OnItemClicked = delegate { };

        // Start is called before the first frame update
        void Awake()
        {
            icon = transform.GetChild(0).GetComponent<Image>();
            nameItem = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            icon.gameObject.SetActive(false);
            GetComponent<Button>().interactable = false;
            GetComponent<Button>().onClick.AddListener(OnClick);

            if (collectableObjectStat != null) SetItemUI(collectableObjectStat);
        }

        public void SetItemUI(CollectableObjectStat collectableObjectStat)
        {
            if (collectableObjectStat.name == "Null")
            {
                icon.gameObject.SetActive(false);
                nameItem.text = "";
                return;
            }
            icon.gameObject.SetActive(true);
            GetComponent<Button>().interactable = true;
            icon.sprite = collectableObjectStat.artwork;
            nameItem.text = collectableObjectStat.name;
        }

        public void OnClick()
        {
            Debug.Log("Click" + " " + collectableObjectStat.name);
            if (collectableObjectStat == null || collectableObjectStat.name == "Null") return;

            OnItemClicked(collectableObjectStat);
        }

        public void RegisterListener(Action<CollectableObjectStat> listener)
        {
            OnItemClicked += listener;
        }
    }
}
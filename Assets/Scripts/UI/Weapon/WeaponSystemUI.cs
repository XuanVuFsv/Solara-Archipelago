using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VitsehLand.Scripts.Inventory;
using VitsehLand.Scripts.Weapon.General;

namespace VitsehLand.Scripts.UI.Weapon
{
    [System.Serializable]
    public class DisplayItemElement
    {
        //public TextMeshProUGUI currentAmmoText;
        public TextMeshProUGUI totalAmmoText;
        //public TextMeshProUGUI ammoNameText;
        public Image artwork;
        public GameObject selectedIcon;

        public DisplayItemElement(/*TextMeshProUGUI currentAmmo, */TextMeshProUGUI totalAmmo/*, TextMeshProUGUI ammoName*/, Image artwork, GameObject selectedIcon)
        {
            //currentAmmoText = currentAmmo;
            totalAmmoText = totalAmmo;
            //ammoNameText = ammoName;
            this.artwork = artwork;
            this.selectedIcon = selectedIcon;
        }
    }

    public class WeaponSystemUI : MonoBehaviour
    {
        private static WeaponSystemUI instance;

        public Transform backpack;
        public List<DisplayItemElement> displayItems = new List<DisplayItemElement>();

        public TextMeshProUGUI weaponNameText;
        public TextMeshProUGUI currentAmmoInMagazineText;
        public int currentIndexSlot;

        [SerializeField]
        TextMeshProUGUI deltaTime;

        public ActiveWeapon activeWeapon;

        private float pollingTime = 1f;
        private float time = 1f;
        private int frameCount = 0;

        void MakeInstance()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else instance = this;
        }


        public static WeaponSystemUI Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            MakeInstance();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!backpack)
            {
                backpack = transform.Find("Backpack");
            }

            if (displayItems.Count == 0)
            {
                int i = 0;
                foreach (Transform item in backpack)
                {
                    DisplayItemElement newDisplayItem = new DisplayItemElement(item.Find("Amount").GetComponent<TextMeshProUGUI>(), item.Find("Icon").GetComponent<Image>(), item.Find("Rouned").gameObject);
                    displayItems.Add(newDisplayItem);
                    Item itemInInventory = InventoryController.Instance.GetItemByIndex(i);
                    newDisplayItem.artwork.sprite = itemInInventory.collectableObjectStat.icon;
                    newDisplayItem.totalAmmoText.text = itemInInventory.count.ToString();

                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            time += Time.deltaTime;
            frameCount++;

            if (time >= pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                deltaTime.text = "FPS: " + frameRate.ToString();

                time -= pollingTime;
                frameCount = 0;
            }
            //ammoNameText.text = InventoryController.Instance.GetCurrentItem().ammoStats.name;
        }

        public void UpdateAmmo(int currentAmmo, int remainingAmmo, int index)
        {
            //currentAmmoText.text = currentAmmo + "/" + remainingAmmo;
        }

        public void SetIndexItemSlot(int index)
        {
            currentIndexSlot = index;
        }

        public void SetDisplayItemName(string ammoName, int index)
        {
            //displayItems[index].ammoNameText.text = ammoName;
        }

        public void SetDisplayItemAmmoAmount(int totalAmmount, int index)
        {
            //displayItems[currentIndexSlot].currentAmmoText.text = currentAmmo.ToString();
            displayItems[index].totalAmmoText.text = totalAmmount.ToString();
        }

        public void SetDisplayItemIcon(Sprite icon, int index)
        {
            displayItems[index].artwork.sprite = icon;
        }
    }
}
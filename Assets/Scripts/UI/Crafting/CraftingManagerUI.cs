using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VitsehLand.Scripts.Crafting;
using VitsehLand.Scripts.Farming.Resource;
using VitsehLand.Scripts.Pattern.Singleton;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.UI.DisplayItem;

namespace VitsehLand.Assets.Scripts.UI.Crafting
{
    [System.Serializable]
    public class CraftingManagerUI : Singleton<CraftingManagerUI>
    {
        [Header("Init and simple actions")]
        public List<ItemUI> itemUIs = new List<ItemUI>();
        public CraftingManager craftingManager;
        public TextMeshProUGUI currentItemName, type, description;

        public GameObject body;
        public bool isActive = false;

        public bool cursorAvaiable = false;

        [Header("Main actions components")]
        public GameObject products;
        public GameObject interaction;
        public GameObject queueDisplay;
        public GameObject materials;
        public GameObject information;

        [Header("Crafting components")]
        public Image currentProductImage;
        public Slider slider;
        public TextMeshProUGUI quantityTitle;
        public TextMeshProUGUI cost;
        public TextMeshProUGUI time;

        public List<MaterialCardWrapper> materialCardWrappers = new List<MaterialCardWrapper>();

        [Header("Storage components")]
        public Transform storageParent;
        public List<MaterialCardWrapper> storageCardWrappers = new List<MaterialCardWrapper>();

        public PowerManager powerManager;


        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < craftingManager.allowedProductList.Count; i++)
            {
                Debug.Log(i);
                itemUIs[i].cropStats = craftingManager.allowedProductList[i].cropStats;
                itemUIs[i].SetItemUI(craftingManager.allowedProductList[i].cropStats);
            }

            for (int i = 0; i < storageCardWrappers.Count; i++)
            {
                storageCardWrappers[i] = storageParent.GetChild(i).GetComponent<MaterialCardWrapper>();
            }

            ShowCurrentItemInformation(itemUIs[0].cropStats);
            ShowRecipe();
            LoadMaterialsRequired(itemUIs[0].cropStats.recipe);
            UpdateMaterialStorage();
            Debug.Log("Setup Done");
            body.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (craftingManager.currentQuantity != slider.value)
            {
                craftingManager.currentQuantity = (int)slider.value;
                quantityTitle.text = "Quantity: " + craftingManager.currentQuantity.ToString();
                ReLoadQuantityMaterialsRequired();
            }

            //if (Input.GetKeyDown(KeyCode.Tab))
            //{
            //    if (cursorAvaiable && isActive)
            //    {
            //        Show(false);
            //    }
            //}
        }

        public void CloseCraftingUI()
        {
            if (cursorAvaiable && isActive)
            {
                Show(false);
            }
        }

        public void Show(bool show)
        {
            body.gameObject.SetActive(show);
            ShowRecipe();
            isActive = show;

            if (!cursorAvaiable)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                cursorAvaiable = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                cursorAvaiable = false;
            }
        }

        public void ShowCurrentItemInformation(CropStats cropStats)
        {
            currentItemName.text = cropStats.name;
            type.text = cropStats.filteredType.ToString() + " - Level " + cropStats.requiredLevel.ToString();
            description.text = cropStats.description;
            currentProductImage.sprite = cropStats.artwork;

            cost.text = "Cost: " + cropStats.cost.ToString();
            time.text = "Time: " + cropStats.totalProducingTime.ToString() + "s";
        }

        #region Main Action Components
        public void ShowRecipe()
        {
            products.gameObject.SetActive(true);
            interaction.gameObject.SetActive(true);
            queueDisplay.gameObject.SetActive(false);
            materials.gameObject.SetActive(false);
            information.gameObject.SetActive(true);
        }

        public void ShowQueue()
        {
            products.gameObject.SetActive(false);
            interaction.gameObject.SetActive(false);
            queueDisplay.gameObject.SetActive(true);
            materials.gameObject.SetActive(false);
            information.gameObject.SetActive(false);
        }

        public void ShowMaterials()
        {
            products.gameObject.SetActive(false);
            interaction.gameObject.SetActive(false);
            queueDisplay.gameObject.SetActive(false);
            materials.gameObject.SetActive(true);
            information.gameObject.SetActive(false);
        }
        #endregion

        #region Crafting Components
        public void PlusQuantity()
        {
            if (craftingManager.currentQuantity == craftingManager.maxQuantity) return;
            craftingManager.currentQuantity += 1;
            slider.value = craftingManager.currentQuantity;
            quantityTitle.text = "Quantity: " + craftingManager.currentQuantity.ToString();

            ReLoadQuantityMaterialsRequired();
        }

        public void MinusQuantity()
        {
            if (craftingManager.currentQuantity == 1) return;
            craftingManager.currentQuantity -= 1;
            slider.value = craftingManager.currentQuantity;
            quantityTitle.text = "Quantity: " + craftingManager.currentQuantity.ToString();

            ReLoadQuantityMaterialsRequired();
        }
        #endregion

        public void LoadMaterialsRequired(RecipeData recipeData)
        {
            Debug.Log("Load " + recipeData.cropStats.name);
            Debug.Log(materialCardWrappers.Count);
            Debug.Log(craftingManager.productRecipes.Count);

            for (int i = 0; i < materialCardWrappers.Count; i++)
            {
                materialCardWrappers[i].gameObject.SetActive(true);
                materialCardWrappers[i].cropStats = recipeData.items[i];
                materialCardWrappers[i].image.sprite = craftingManager.productRecipes[recipeData.cropStats.name].items[i].artwork;

                materialCardWrappers[i].quantity = craftingManager.GetItemStorageQuantityByName(recipeData.items[i].name);
                materialCardWrappers[i].requiredQuantity = craftingManager.productRecipes[recipeData.cropStats.name].ammountPerSlots[i]
                    * craftingManager.currentQuantity;

                materialCardWrappers[i].quanityText.text = materialCardWrappers[i].requiredQuantity.ToString()
                + "/" + materialCardWrappers[i].quantity.ToString();

                if (materialCardWrappers[i].requiredQuantity > materialCardWrappers[i].quantity)
                {
                    materialCardWrappers[i].quanityText.color = Color.red;
                }
                else
                {
                    materialCardWrappers[i].quanityText.color = Color.white;
                }
            }
        }

        public void ReLoadQuantityMaterialsRequired()
        {
            for (int i = 0; i < materialCardWrappers.Count; i++)
            {
                materialCardWrappers[i].requiredQuantity = craftingManager.productRecipes
                    [craftingManager.allowedProductList[craftingManager.currentRecipeIndex].cropStats.name].ammountPerSlots[i]
                    * craftingManager.currentQuantity;

                materialCardWrappers[i].quantity = craftingManager.GetItemStorage(materialCardWrappers[i].cropStats.name);

                materialCardWrappers[i].quanityText.text = materialCardWrappers[i].requiredQuantity.ToString()
                + "/" + materialCardWrappers[i].quantity.ToString();

                if (materialCardWrappers[i].requiredQuantity > materialCardWrappers[i].quantity)
                {
                    materialCardWrappers[i].quanityText.color = Color.red;
                }
                else
                {
                    materialCardWrappers[i].quanityText.color = Color.white;
                }
            }
        }

        public void ResetMaterialStorage()
        {
            for (int i = 0; i < storageCardWrappers.Count; i++)
            {
                if (i >= craftingManager.unlockedStorageSlot)
                {
                    //storageCardWrappers[i].gameObject.SetActive(false);
                    //storageCardWrappers[i].image.gameObject.SetActive(false);
                    //storageCardWrappers[i].quanityText.text = "";
                    continue;
                }
                else if (i < craftingManager.unlockedStorageSlot)
                {
                    storageCardWrappers[i].image.gameObject.SetActive(false);
                    storageCardWrappers[i].quanityText.text = "";
                    storageCardWrappers[i].GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                    continue;
                }
                //storageCardWrappers[i].image.gameObject.SetActive(false);
                //storageCardWrappers[i].quanityText.text = "";
            }
        }

        public void UpdateMaterialStorage()
        {
            ResetMaterialStorage();

            for (int i = 0; i < craftingManager.itemStorages.Count; i++)
            {
                if (i >= craftingManager.unlockedStorageSlot)
                {
                    //storageCardWrappers[i].gameObject.SetActive(false);
                    //storageCardWrappers[i].quanityText.text = "";
                }
                else
                {
                    storageCardWrappers[i].image.gameObject.SetActive(true);
                    storageCardWrappers[i].GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                    storageCardWrappers[i].image.sprite = craftingManager.itemStorages[i].cropStats.artwork;
                    storageCardWrappers[i].quanityText.text = craftingManager.itemStorages[i].quantity.ToString();
                    storageCardWrappers[i].cropStats = craftingManager.itemStorages[i].cropStats;
                }
            }
        }


        public void Craft()
        {
            if (!CheckCraftCondition())
            {
                Debug.Log("Not enough material or Energy");
                return;
            }
            craftingManager.Craft();
        }

        public bool CheckCraftCondition()
        {
            foreach (MaterialCardWrapper card in materialCardWrappers)
            {
                if (card.requiredQuantity > card.quantity)
                {
                    return false;
                }
            }
            if (powerManager.currentPower < 10 * craftingManager.currentQuantity)
            {
                craftingManager.WarningNotEnoughPower();
                return false;
            }
            return true;
        }

        public void UpdateItemStorageDatas()
        {
            foreach (MaterialCardWrapper card in materialCardWrappers)
            {
                card.quantity -= card.requiredQuantity;
                craftingManager.SetItemStorage(card.cropStats.name, card.quantity);
                if (card.quantity == 0)
                {
                    craftingManager.RemoveItemStorage(card.cropStats.name);
                }
            }

            craftingManager.UpdateItemStorageList();
            ReLoadQuantityMaterialsRequired();
            UpdateMaterialStorage();
        }
    }
}
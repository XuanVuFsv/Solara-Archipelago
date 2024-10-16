using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VitsehLand.Assets.Scripts.Interactive;
using VitsehLand.Assets.Scripts.UI.Crafting;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Farming.Resource;
using VitsehLand.Scripts.Inventory;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.UI.DisplayItem;

namespace VitsehLand.Scripts.Crafting
{
    [System.Serializable]
    public class CraftingPresenter : ActivateBehaviour
    {
        public CraftingModel model;
        public CraftingView view;

        //public List<RecipeData> allowedProductList = new List<RecipeData>();
        //public Dictionary<string, RecipeData> productRecipes = new Dictionary<string, RecipeData>();

        //public List<ItemStorageData> itemStorages = new List<ItemStorageData>();
        //public Dictionary<string, ItemStorageData> itemStorageDict = new Dictionary<string, ItemStorageData>();

        //public List<Suckable> products = new List<Suckable>();

        public Transform productPos;
        public PowerManager powerManager; //**//

        //public int maxQuantity;
        //public int currentQuantity;

        //public int currentRecipeIndex;

        //public int unlockedStorageSlot = 3;
        //public int maxStorageSlot = 9;

        //public int queueQuantity = 1;
        //public int maxQueueQuantity = 4;
        //public int queueActiveQuantity = 0;
        //public TextMeshProUGUI queueQuantityDisplay;

        //public GameObject warningFullQueue;
        //public GameObject warningNotEnoughPower;

        //public GameObject VFX;

        //public List<CraftQueueHandler> craftQueueHandlers = new List<CraftQueueHandler>();

        // Start is called before the first frame update
        void Awake()
        {
            //foreach (RecipeData recipe in allowedProductList)
            //{
            //    productRecipes.Add(recipe.cropStats.name, recipe);
            //}

            //foreach (ItemStorageData item in itemStorages)
            //{
            //    itemStorageDict.Add(item.cropStats.name, item);
            //}

            //for (int i = 0; i < craftQueueHandlers.Count; i++)
            //{
            //    craftQueueHandlers[i].UIContainer.SetActive(false);
            //}

            model.SetupInitData();
        }

        void Start()
        {
            SetupInitViewElements();
            view.queueQuantityDisplay.text = "You can use " + model.queueQuantity.ToString() + " slot in queue";

            view.RegisterListener(UpdateQuantity);
            view.RegisterListener(Craft);

            for (int i = 0; i < view.itemUIs.Count; i++)
            {
                view.itemUIs[i].RegisterListener(OnClickProduct);
            }

            for (int i = 0; i < model.craftQueueHandlers.Count; i++)
            {
                model.craftQueueHandlers[i].RegisterListener(CompleteCraft);
            }
        }

        public override void ExecuteActivateAction()
        {
            if (view.isActive) return;

            view.Show(true);

            model.UpdateItemStorageList();

            view.UpdateMaterialStorage(model.unlockedStorageSlot, model.itemStorages);
            view.ReLoadQuantityMaterialsRequired(model.GetCurrentRecipe(), 
                model.GetQuantityByMaterialOfRecipe(model.GetCurrentRecipe()),
                model.currentQuantity);
        }

        public void SetupInitViewElements()
        {
            for (int i = 0; i < model.allowedProductList.Count; i++)
            {
                Debug.Log(i);
                view.itemUIs[i].cropStats = model.allowedProductList[i].cropStats;
                view.itemUIs[i].SetItemUI(model.allowedProductList[i].cropStats);
            }

            for (int i = 0; i < view.storageCardWrappers.Count; i++)
            {
                view.storageCardWrappers[i] = view.storageParent.GetChild(i).GetComponent<MaterialCardWrapper>();
            }

            ItemUI firstItemUI = view.itemUIs[0];
            view.ShowCurrentItemInformation(firstItemUI.cropStats);
            view.ShowRecipe();

            Debug.Log(firstItemUI);
            Debug.Log(model.GetQuantityByMaterialOfRecipe(firstItemUI.cropStats.recipe));
            Debug.Log(model.currentQuantity);

            view.LoadMaterialsRequired(firstItemUI.cropStats.recipe,
                model.GetQuantityByMaterialOfRecipe(firstItemUI.cropStats.recipe),
                model.currentQuantity);

            view.UpdateMaterialStorage(model.unlockedStorageSlot, model.itemStorages);
            Debug.Log("Setup Done");
            view.body.gameObject.SetActive(false);
        }

        public void OnClickProduct(int productIndex, CropStats cropStats)
        {
            Debug.Log("Click" + " " + cropStats.name);
            if (cropStats == null || cropStats.name == "Null") return;

            model.currentRecipeIndex = productIndex;

            view.ShowCurrentItemInformation(cropStats);

            view.LoadMaterialsRequired(cropStats.recipe,
                model.GetQuantityByMaterialOfRecipe(cropStats.recipe),
                model.currentQuantity);
        }

        //public int GetItemStorageQuantityByName(string name)
        //{
        //    if (model.itemStorages.Count == 0)
        //    {
        //        Debug.Log("Empty Storage");
        //        return 0;
        //    }
        //    if (!model.itemStorageDict.ContainsKey(name))
        //    {
        //        Debug.Log(name + " not exist");
        //        return 0;
        //    }
        //    return model.itemStorageDict[name].quantity;

        //}

        public void UpdateItemStorageDatas()
        {
            foreach (MaterialCardWrapper card in view.materialCardWrappers)
            {
                card.quantity -= card.requiredQuantity;
                model.SetItemStorage(card.cropStats.name, card.quantity);
                if (card.quantity == 0)
                {
                    model.RemoveItemStorage(card.cropStats.name);
                }
            }

            model.UpdateItemStorageList();
            view.ReLoadQuantityMaterialsRequired(model.GetCurrentRecipe(),
                model.GetQuantityByMaterialOfRecipe(model.GetCurrentRecipe()),
                model.currentQuantity);
            view.UpdateMaterialStorage(model.unlockedStorageSlot, model.itemStorages);
        }

        public void Craft()
        {
            if (!CheckCraftCondition())
            {
                Debug.Log("Not enough material or Energy");
                return;
            }
            StartCraft();
        }

        public bool CheckCraftCondition()
        {
            foreach (MaterialCardWrapper card in view.materialCardWrappers)
            {
                if (card.requiredQuantity > card.quantity)
                {
                    return false;
                }
            }
            if (powerManager.currentPower < 10 * model.currentQuantity)
            {
                StartCoroutine(view.ShowWarningNotEnoughPower());
                return false;
            }
            return true;
        }

        public void StartCraft()
        {
            if (model.queueActiveQuantity < model.queueQuantity)
            {
                Debug.Log("Start craft a " + model.currentQuantity.ToString() + " " + model.GetCurrentRecipe().name);
                int index = model.FindFirstCraftSlotReady();
                if (index >= 0)
                {
                    UpdateItemStorageDatas();
                    model.craftQueueHandlers[index].UIContainer.SetActive(true);

                    model.craftQueueHandlers[index].cropStats = model.GetCurrentRecipe().cropStats;
                    model.craftQueueHandlers[index].product = model.GetCurrentRecipe().product;

                    int totalTime = model.currentQuantity * (int)model.GetCurrentRecipe().cropStats.totalProducingTime;
                    model.craftQueueHandlers[index].Craft(totalTime, productPos);
                    model.queueActiveQuantity++;

                    view.VFX.SetActive(true);
                    powerManager.UsePower(model.currentQuantity * 10);
                }
                else
                {
                    Debug.Log("Something Wrong");
                }
            }
            else
            {
                StartCoroutine(view.ShowWarning());
                Debug.Log("Full Slot");
            }
        }

        void CompleteCraft()
        {
            model.queueActiveQuantity--;
            view.VFX.SetActive(true);
        }

        //public int FindFirstCraftSlotReady()
        //{
        //    for (int i = 0; i < model.queueQuantity; i++)
        //    {
        //        if (model.craftQueueHandlers[i].isReady) return i;
        //    }
        //    return -1;
        //}

        //public void SetItemStorage(string name, int value)
        //{
        //    model.itemStorageDict[name].quantity = value;
        //}

        //public int GetItemStorage(string name)
        //{
        //    if (!model.itemStorageDict.ContainsKey(name)) return 0;
        //    return model.itemStorageDict[name].quantity;
        //}

        //public void RemoveItemStorage(string name)
        //{
        //    model.itemStorages.Remove(model.itemStorageDict[name]);
        //    model.itemStorageDict.Remove(name);
        //}

        //public void UpdateItemStorageList()
        //{
        //    foreach (ItemStorageData item in itemStorages)
        //    {
        //        item.quantity = itemStorageDict[item.cropStats.name].quantity;
        //    }
        //}

        ////**//
        //IEnumerator ShowWarning()
        //{
        //    warningFullQueue.SetActive(true);
        //    yield return new WaitForSeconds(0.5f);
        //    warningFullQueue.SetActive(false);
        //}

        ////**//
        //IEnumerator ShowWarningNotEnoughPower()
        //{
        //    warningNotEnoughPower.SetActive(true);
        //    yield return new WaitForSeconds(0.5f);
        //    warningNotEnoughPower.SetActive(false);
        //}

        //public void WarningNotEnoughPower()
        //{
        //    StartCoroutine(view.ShowWarningNotEnoughPower());
        //}

        //public bool CheckStorage()
        //{
        //    if (itemStorages.Count == unlockedStorageSlot) return false;
        //    return true;
        //}

        public bool AddItemStorage(CropStats cropStats, int quantity)
        {
            return model.AddItemStorage(cropStats, quantity);
        }

        public void UpdateQuantity(int value, CraftingView.QuanityChangedActionType actionType)
        {
            if (actionType == CraftingView.QuanityChangedActionType.Button)
            {
                int quantity = model.currentQuantity + value;
                if (quantity > model.maxQuantity || quantity <= 0) return;
                model.currentQuantity = quantity;
                view.slider.value = model.currentQuantity;
                view.quantityTitle.text = "Quantity: " + model.currentQuantity.ToString();

                view.ReLoadQuantityMaterialsRequired(model.GetCurrentRecipe(),
                    model.GetQuantityByMaterialOfRecipe(model.GetCurrentRecipe()),
                    model.currentQuantity);
            }
            else
            {
                model.currentQuantity = value;
                view.quantityTitle.text = "Quantity: " + model.currentQuantity.ToString();

                view.ReLoadQuantityMaterialsRequired(model.GetCurrentRecipe(),
                    model.GetQuantityByMaterialOfRecipe(model.GetCurrentRecipe()),
                    model.currentQuantity);
            }
        }
    }
}
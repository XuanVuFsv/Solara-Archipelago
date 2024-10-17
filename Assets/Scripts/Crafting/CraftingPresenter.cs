using UnityEngine;
using VitsehLand.Assets.Scripts.Interactive;
using VitsehLand.Assets.Scripts.UI.Crafting;
using VitsehLand.Scripts.Farming.Resource;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.UI.DisplayItem;

namespace VitsehLand.Scripts.Crafting
{
    [System.Serializable]
    public class CraftingPresenter : ActivateBehaviour
    {
        public CraftingModel model;
        public CraftingView view;

        public Transform productPos;
        public PowerManager powerManager;

        // Start is called before the first frame update
        void Awake()
        {
            model.SetupInitData();
        }

        void Start()
        {
            SetupInitViewElements();

            view.slider.value = 1;
            model.currentQuantity = 1;
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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CraftingManager : ActivateBehaviour
{
    public List<RecipeData> allowedProductList = new List<RecipeData>();
    public Dictionary<string, RecipeData> productRecipes = new Dictionary<string, RecipeData>();

    public List<ItemStorageData> itemStorages = new List<ItemStorageData>();
    public Dictionary<string, ItemStorageData> itemStorageDict = new Dictionary<string, ItemStorageData>();

    public List<Suckable> products = new List<Suckable>();

    public Transform productPos;

    public int maxQuantity;
    public int currentQuantity;

    public int currentRecipeIndex;

    public int unlockedStorageSlot = 3;
    public int maxStorageSlot = 9;

    public int queueQuantity=1;
    public int maxQueueQuantity = 4;
    public int queueActiveQuantity = 0;
    public TextMeshProUGUI queueQuantityDisplay;

    public GameObject warningFullQueue;
    public GameObject warningNotEnoughPower;

    public GameObject VFX;

    public List<CraftQueueHandler> craftQueueHandlers = new List<CraftQueueHandler>();

    // Start is called before the first frame update
    void Awake()
    {
        foreach (RecipeData recipe in allowedProductList)
        {
            productRecipes.Add(recipe.cropStats.name, recipe);
        }

        foreach (ItemStorageData item in itemStorages)
        {
            itemStorageDict.Add(item.cropStats.name, item);
        }

        for (int i = 0; i < craftQueueHandlers.Count; i++)
        {
            craftQueueHandlers[i].UIContainer.SetActive(false);
        }

        queueQuantityDisplay.text = "You can use " + queueQuantity.ToString() + " slot in queue";
    }

    public override void ExecuteActivateAction()
    {
        if (CraftingManagerUI.Instance.isActive) return;
        CraftingManagerUI.Instance.Show(true);
        UpdateItemStorageList();
        CraftingManagerUI.Instance.UpdateMaterialStorage();
        CraftingManagerUI.Instance.ReLoadQuantityMaterialsRequired();
    }

    public int GetItemStorageQuantityByName(string name)
    {
        if (itemStorages.Count == 0)
        {
            Debug.Log("Empty Storage");
            return 0;
        }
        if (!itemStorageDict.ContainsKey(name))
        {
            Debug.Log(name + " not exist");
            return 0;
        }
        return itemStorageDict[name].quantity;

    }

    public void Craft()
    {
        if (queueActiveQuantity < queueQuantity)
        {
            Debug.Log("Start craft a " + currentQuantity.ToString() + " " + allowedProductList[currentRecipeIndex].name);
            int index = FindFirstCraftSlotReady();
            if (index >= 0)
            {
                CraftingManagerUI.Instance.UpdateItemStorageDatas();
                craftQueueHandlers[index].UIContainer.SetActive(true);

                craftQueueHandlers[index].cropStats = allowedProductList[currentRecipeIndex].cropStats;
                craftQueueHandlers[index].product = allowedProductList[currentRecipeIndex].product;

                int totalTime = currentQuantity * (int)allowedProductList[currentRecipeIndex].cropStats.totalProducingTime;
                craftQueueHandlers[index].Craft(totalTime);
                CraftingManagerUI.Instance.powerManager.UsePower(currentQuantity * 10);
            }   
            else
            {
                Debug.Log("Something Wrong");
            }    
        }
        else
        {
            StartCoroutine(ShowWarning());
            Debug.Log("Full Slot");
        }
    }

    public int FindFirstCraftSlotReady()
    {
        for (int i = 0; i < queueQuantity; i++)
        {
            if (craftQueueHandlers[i].isReady) return i;
        }
        return -1;
    }

    public void SetItemStorage(string name, int value)
    {
        itemStorageDict[name].quantity = value;
    }

    public int GetItemStorage(string name)
    {
        if (!itemStorageDict.ContainsKey(name)) return 0;
        return itemStorageDict[name].quantity;
    }

    public void RemoveItemStorage(string name)
    {
        itemStorages.Remove(itemStorageDict[name]);
        itemStorageDict.Remove(name);
    }

    public void UpdateItemStorageList()
    {
        foreach (ItemStorageData item in itemStorages)
        {
            item.quantity = itemStorageDict[item.cropStats.name].quantity;
        }
    }

    IEnumerator ShowWarning()
    {
        warningFullQueue.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        warningFullQueue.SetActive(false);
    }

    IEnumerator ShowWarningNotEnoughPower()
    {
        warningNotEnoughPower.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        warningNotEnoughPower.SetActive(false);
    }

    public void WarningNotEnoughPower()
    {
        StartCoroutine(ShowWarningNotEnoughPower());
    }

    public bool CheckStorage()
    {
        if (itemStorages.Count == unlockedStorageSlot) return false;
        return true;
    }

    public bool AddItemStorage(CropStats cropStats, int quantity)
    {
        if (itemStorageDict.ContainsKey(cropStats.name))
        {
            itemStorageDict[cropStats.name].quantity += quantity;
            //UpdateItemStorageList();
            return true;
        }
        else if (CheckStorage())
        {
            itemStorages.Add(new ItemStorageData(cropStats.name, cropStats, ItemStorageData.StorageLocation.CraftMachine, cropStats.cropPrefab, quantity));
            itemStorageDict.Add(cropStats.name, itemStorages[itemStorages.Count - 1]);
            return true;
        }
        else if (!CheckStorage()) return false;
        return false;
    }
}
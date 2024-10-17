using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Inventory;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Crafting
{
    public class CraftingModel : MonoBehaviour
    {
        public List<RecipeData> allowedProductList = new List<RecipeData>();
        public Dictionary<string, RecipeData> productRecipes = new Dictionary<string, RecipeData>();

        public List<ItemStorageData> itemStorages = new List<ItemStorageData>();
        public Dictionary<string, ItemStorageData> itemStorageDict = new Dictionary<string, ItemStorageData>();

        public List<CraftQueueHandler> craftQueueHandlers = new List<CraftQueueHandler>();

        public List<Suckable> products = new List<Suckable>();

        public int maxQuantity;
        public int currentQuantity;

        public int currentRecipeIndex;

        public int unlockedStorageSlot = 3;
        public int maxStorageSlot = 9;

        public int queueQuantity = 1;
        public int maxQueueQuantity = 4;
        public int queueActiveQuantity = 0;

        public void SetupInitData()
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
        }

        public RecipeData GetCurrentRecipe()
        {
            return allowedProductList[currentRecipeIndex];
        }

        /// <summary>
        /// Return a list containing the quantities of materials needed for the specific recipe.
        /// </summary>
        public List<int> GetQuantityByMaterialOfRecipe(RecipeData recipe)
        {
            return recipe.items.Select(item => GetItemStorageQuantityByName(item.name)).ToList();
        }

        public int FindFirstCraftSlotReady()
        {
            for (int i = 0; i < queueQuantity; i++)
            {
                if (craftQueueHandlers[i].isReady) return i;
            }
            return -1;
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
}
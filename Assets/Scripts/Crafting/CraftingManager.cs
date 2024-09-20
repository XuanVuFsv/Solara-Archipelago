using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
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

    public int queueQuantity;
    public int curentQueueIndex;

    public int currentRecipeIndex;
    public bool inCrafting = false;

    public int unlockedSlot = 1;

    // Start is called before the first frame update
    void Start()
    {
        foreach (RecipeData recipe in allowedProductList)
        {
            productRecipes.Add(recipe.ammoStats.name, recipe);
        }

        foreach (ItemStorageData item in itemStorages)
        {
            itemStorageDict.Add(item.ammoStats.name, item);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ExecuteActivateAction()
    {
        if (CraftingManagerUI.Instance.isActive) return;
        CraftingManagerUI.Instance.Show(true);
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
        Debug.Log("Start craft a " + currentQuantity.ToString() + " " + allowedProductList[currentRecipeIndex].name);
    }

    public int GetProductCanBeCrafted()
    {

        return 0;
    }
}
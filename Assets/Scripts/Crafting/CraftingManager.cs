using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : Singleton<CraftingManager>
{
    public List<RecipeData> recipeList = new List<RecipeData>();

    public Dictionary<string, RecipeData> recipes = new Dictionary<string, RecipeData>();
    public List<CraftingSlot> craftingSlots;

    public Transform productPos;
    public Suckable product;
    public string currentRecipe;
    public bool inCrafting = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (RecipeData recipe in recipeList)
        {
            recipes.Add(recipe.recipeString, recipe);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetCraftingState()
    {
        if (!inCrafting)
        {
            //Debug.Log("notinCrafting");
        }

        foreach (CraftingSlot slot in craftingSlots)
        {
            slot.CancelCraftingOnThisItem();
        }
        inCrafting = false;
        product = null;
        productPos = null;
        currentRecipe = "";
    }

    public bool StartCrafting()
    {
        if (inCrafting)
        {
            //Debug.Log("inCrafting");
            return false;
        }

        currentRecipe = "";
        foreach (CraftingSlot slot in craftingSlots)
        {
            //Debug.Log("CreateName");
            currentRecipe += slot.GetCurrentName();
        }

        if (recipes.ContainsKey(currentRecipe))
        {
            //Debug.Log("ContainsKey");
            productPos = recipes[currentRecipe].product.transform;
            foreach (CraftingSlot slot in craftingSlots)
            {
                slot.UseItemToCraft();
            }
        }
        else return false;

        inCrafting = true;
        Invoke("CompleteCrafting", recipes[currentRecipe].ammoStats.totalProducingTime);
        return true;
    }

    public void CompleteCrafting()
    {
        foreach (CraftingSlot slot in craftingSlots)
        {
            slot.CompeleteCrafting();
        }
        product = Instantiate(recipes[currentRecipe].product.gameObject, productPos.position, Quaternion.identity).GetComponent<Suckable>();
        product.ResetVelocity();
        ResetCraftingState();
    }
}
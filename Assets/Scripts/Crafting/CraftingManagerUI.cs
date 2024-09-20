using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < craftingManager.allowedProductList.Count; i++)
        {
            Debug.Log(i);
            itemUIs[i].ammoStats = craftingManager.allowedProductList[i].ammoStats;
            itemUIs[i].SetItemUI(craftingManager.allowedProductList[i].ammoStats);
        }

        for (int i = 0; i < storageCardWrappers.Count; i++)
        {
            storageCardWrappers[i] = storageParent.GetChild(i).GetComponent<MaterialCardWrapper>();
        }

        ShowCurrentItemInformation(itemUIs[0].ammoStats);
        ShowRecipe();
        LoadMaterialsRequired(itemUIs[0].ammoStats.recipe);
        LoadMaterialStorage();
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cursorAvaiable && isActive)
            {
                Show(false);
            }
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

    public void ShowCurrentItemInformation(AmmoStats ammoStats)
    {
        currentItemName.text = ammoStats.name;
        type.text = ammoStats.filteredType.ToString() + " - Level " + ammoStats.requiredLevel.ToString();
        description.text = ammoStats.description;
        currentProductImage.sprite = ammoStats.artwork;

        cost.text = "Cost: " + ammoStats.cost.ToString();
        time.text = "Time: " + ammoStats.totalProducingTime.ToString() + "s";
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
        Debug.Log("Load " + recipeData.ammoStats.name);
        Debug.Log(materialCardWrappers.Count);
        Debug.Log(craftingManager.productRecipes.Count);

        for (int i = 0; i < materialCardWrappers.Count; i++)
        {
            materialCardWrappers[i].gameObject.SetActive(true);
            materialCardWrappers[i].image.sprite = craftingManager.productRecipes[recipeData.ammoStats.name].items[i].artwork;

            materialCardWrappers[i].quantity = craftingManager.GetItemStorageQuantityByName(recipeData.items[i].name);
            materialCardWrappers[i].requiredQuantity = (craftingManager.productRecipes[recipeData.ammoStats.name].ammountPerSlots[i]
                * craftingManager.currentQuantity);

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
            materialCardWrappers[i].requiredQuantity = (craftingManager.productRecipes
                [craftingManager.allowedProductList[craftingManager.currentRecipeIndex].ammoStats.name].ammountPerSlots[i]
                * craftingManager.currentQuantity);

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

    public void InitMaterialStorage()
    {
        for (int i = 0; i < storageCardWrappers.Count; i++)
        {
            if (i >= craftingManager.unlockedSlot)
            {
                //storageCardWrappers[i].gameObject.SetActive(false);
                continue;
            }
            storageCardWrappers[i].image.gameObject.SetActive(false);
            storageCardWrappers[i].quanityText.text = "";
        }
    }

    public void LoadMaterialStorage()
    {
        InitMaterialStorage();

        for (int i = 0; i < craftingManager.itemStorages.Count; i++)
        {
            if (i >= craftingManager.unlockedSlot)
            {
                //storageCardWrappers[i].gameObject.SetActive(false);
                //storageCardWrappers[i].quanityText.text = "";
            }
            else
            {
                storageCardWrappers[i].image.gameObject.SetActive(true);
                storageCardWrappers[i].GetComponent<Image>().color = new Color32(0, 0, 0, 100);
                storageCardWrappers[i].image.sprite = craftingManager.itemStorages[i].ammoStats.artwork;
                storageCardWrappers[i].quanityText.text = craftingManager.itemStorages[i].quantity.ToString();
                storageCardWrappers[i].ammoStats = craftingManager.itemStorages[i].ammoStats;
            }
        }
    }

    public void Craft()
    {
        craftingManager.Craft();
    }
}


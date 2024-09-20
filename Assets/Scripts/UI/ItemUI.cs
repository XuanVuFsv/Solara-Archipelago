using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public int index = 0;

    public AmmoStats ammoStats;

    public Image icon;
    public TextMeshProUGUI nameItem;

    // Start is called before the first frame update
    void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        nameItem = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        icon.gameObject.SetActive(false);
        GetComponent<Button>().interactable = false;

        if (ammoStats != null) SetItemUI(ammoStats);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemUI(AmmoStats ammoStats)
    {
        if (ammoStats.name == "Null")
        {
            icon.gameObject.SetActive(false);
            nameItem.text = "";
            return;
        }
        icon.gameObject.SetActive(true);
        GetComponent<Button>().interactable = true;
        icon.sprite = ammoStats.artwork;
        nameItem.text = ammoStats.name;
    }   
    
    public void OnClick()
    {
        Debug.Log("Click" + " " + ammoStats.name);
        if (ammoStats == null || ammoStats.name == "Null") return;
        CraftingManagerUI.Instance.craftingManager.currentRecipeIndex = index;
        CraftingManagerUI.Instance.ShowCurrentItemInformation(ammoStats);

        CraftingManagerUI.Instance.LoadMaterialsRequired(ammoStats.recipe);
    }    
}

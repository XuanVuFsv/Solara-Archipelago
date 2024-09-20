using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingManagerUI : Singleton<CraftingManagerUI>
{
    public List<AmmoStats> allowedList = new List<AmmoStats>();
    public List<ItemUI> itemUIs = new List<ItemUI>();
    public TextMeshProUGUI currentItemName, type, description;

    public bool cursorAvaiable = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < allowedList.Count; i++)
        {
            Debug.Log(i);
            itemUIs[i].ammoStats = allowedList[i];
            itemUIs[i].SetItemUI(allowedList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!cursorAvaiable)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void ShowCurrentItemInformation(AmmoStats ammoStats)
    {
        currentItemName.text = ammoStats.name;
        type.text = ammoStats.filteredType.ToString() + " - Level " + ammoStats.requiredLevel.ToString();
        description.text = ammoStats.description;
    }
}


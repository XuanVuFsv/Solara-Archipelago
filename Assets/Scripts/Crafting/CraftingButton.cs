using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingButton : MonoBehaviour
{
    public CraftingManager craftingManager;
    public bool onCrafting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (onCrafting || other.tag != "Player") return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("Crafting");
            onCrafting = craftingManager.StartCrafting();
        }
    }
}

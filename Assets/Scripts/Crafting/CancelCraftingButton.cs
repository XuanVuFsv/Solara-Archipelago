using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelCraftingButton : MonoBehaviour
{
    public CraftingManager craftingManager;
    // Start is called before the first frame update

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("Cancel");
            craftingManager.ResetCraftingState();
        }
    }
}
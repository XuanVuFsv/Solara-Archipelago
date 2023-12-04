using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowingSlotResourceManager : MonoBehaviour
{
    public string resourceName;
    public int maxStackPerSlot;
    public Color defaultLockPanelColor, defaultUnlockPanelColor;

    public List<GrowingSlotResourceState> growingSlotResourcesManager = new List<GrowingSlotResourceState>();

    [SerializeField]
    List<bool> initGrowingSlotResourceState = new List<bool>();
    [SerializeField]
    List<bool> initFillResourceState = new List<bool>();

    public Image[] test;

    private void Start()
    {
        Image[] slots = GetComponentsInChildren<Image>(true);
        test = slots;

        int i = 0;
        bool ignoreParent = true;

        foreach (Image slot in slots)
        {
            if (ignoreParent)
            {
                ignoreParent = false;
                continue;
            }

            if (slot.rectTransform.parent != slots[0].rectTransform) continue;

            Image slotPanel = slot.gameObject.GetComponent<Image>();
            Image[] slotElements = slot.gameObject.GetComponentsInChildren<Image>(true);

            //GrowingSlotResourceManager Hierarchy:
            //GameObject has GrowingSlotResourceManager component
            //     -Slot Resource
            //         -- Fill Image
            //         -- Lock Image

            Debug.Log(i);
            growingSlotResourcesManager.Add(
                new GrowingSlotResourceState(
                    slotPanel, slotElements[1], slotElements[2], 
                    defaultUnlockPanelColor, defaultLockPanelColor, growingSlotResourcesManager.Count, maxStackPerSlot));

            growingSlotResourcesManager[i].ChangeLockState(initGrowingSlotResourceState[i]);
            if (i < initFillResourceState.Count) growingSlotResourcesManager[i].ChangeFillState(initFillResourceState[i]);
            else growingSlotResourcesManager[i].ChangeFillState(false);
            i++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Growing Slot Status store data about growing resource like water or fertilizer. </summary>
[System.Serializable]
public class GrowingSlotResourceState : SlotState
{
    [Header("UI Element")]
    public Image fillImage;

    [SerializeField]
    bool hasFill;

    private void Start()
    {
        Image[] slots = GetComponentsInChildren<Image>(true);
        slotPanel = slots[0];
        fillImage = slots[1];
        lockImage = slots[2];
    }

    public bool HasFill
    {
        get { return hasFill; }
    }

    public override void ChangeLockState(bool _isUnlock)
    {
        isUnlock = _isUnlock;
        slotPanel.color = isUnlock ? unlockPanelColor : lockPanelColor;
        fillImage.gameObject.SetActive(isUnlock);
        lockImage.gameObject.SetActive(!isUnlock);
    }

    /// <summary> This method will check isUnlock value automatically. Passing true value while slot has not unlocked will not change fill slot resource state. </summary>
    public void ChangeFillState(bool hasFill)
    {
        if (!isUnlock) return;
        this.hasFill = hasFill;
        fillImage.gameObject.SetActive(hasFill);
    }
}
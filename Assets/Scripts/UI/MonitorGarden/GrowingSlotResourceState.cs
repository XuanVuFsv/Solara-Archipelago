using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Growing Slot Status store data about growing resource like water or fertilizer. </summary>
[System.Serializable]
public class GrowingSlotResourceState
{
    [Header("UI Element")]
    public Image slotPanel;
    public Image fillImage;
    public Image lockImage;

    public Color lockPanelColor;
    public Color unlockPanelColor;

    public bool isUnlock;
    public int index;
    public int maxStack;

    [SerializeField]
    int stack;

    public GrowingSlotResourceState(Image _slotPanel, Image _fillImage, Image _lockImage, Color _unlockPanelColor, Color _lockPanelColor, int _index, int _maxStack)
    {
        slotPanel = _slotPanel;
        fillImage = _fillImage;
        lockImage = _lockImage;
        unlockPanelColor = _unlockPanelColor;
        lockPanelColor = _lockPanelColor;
        index = _index;
        maxStack = _maxStack;
    }

    public bool HasFill
    {
        get { return stack == maxStack; }
    }

    public void AddStack()
    {
        if (stack < maxStack) stack++;
        else if (stack == maxStack)
        {
            ChangeFillState(true);
        }    
    }

    public void ChangeLockState(bool _isUnlock)
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
        fillImage.gameObject.SetActive(hasFill);
    }
}
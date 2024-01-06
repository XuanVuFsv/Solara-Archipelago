using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotManager : MonoBehaviour
{
    public string resourceName;
    public int maxStackPerSlot;
    public Color defaultLockPanelColor, defaultUnlockPanelColor;
}

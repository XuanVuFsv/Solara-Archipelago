using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowingSlotResourceManager : SlotManager
{
    public List<GrowingSlotResourceState> growingSlotResourcesManager = new List<GrowingSlotResourceState>();

    public void FillResourceSlot(int i)
    {
        growingSlotResourcesManager[i].ChangeFillState(true);
    }

    public void MakeResourceSlotEmpty(int i)
    {
        growingSlotResourcesManager[i].ChangeFillState(false);
    }
}

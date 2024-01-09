using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    const int MAX_SLOT = 4;

    public int MaxSlot
    {
        get
        {
            return MAX_SLOT;
        }
    }

    public bool HasBuilt
    {
        get { return gameObject.activeInHierarchy; }
    }

    public List<GardenSlotProperties> gardenSlotProperties = new List<GardenSlotProperties>();
    public MonitorGardenController monitorGardenController;
    public WaterResourceManager waterManager;
    public FertilizerResourceManager fertilizerManager;
    public int activeSlotCount = 1;
    public int currentAvailableSlotIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);
    }

    public int NextSlot()
    {
        currentAvailableSlotIndex++;
        if (currentAvailableSlotIndex > activeSlotCount - 1) currentAvailableSlotIndex = 0;

        return currentAvailableSlotIndex;
    }

    public int PreviousSLot()
    {
        currentAvailableSlotIndex--;
        if (currentAvailableSlotIndex < 0) currentAvailableSlotIndex = activeSlotCount - 1;

        return currentAvailableSlotIndex;
    }

    public int UnLockNewSlot()
    {
        if (activeSlotCount == MAX_SLOT) return - 1;

        bool unlockSuccess = gardenSlotProperties[activeSlotCount].UnLockSlot();
        if (!unlockSuccess)
        {
            //Debug.Log("Not enough gem");
            return -1;
        }

        activeSlotCount++;

        if (activeSlotCount == MAX_SLOT) monitorGardenController.DisableUnlockNewSlotButton();

        return activeSlotCount - 1;
    }

    public bool ClearCurrentField()
    {
        return GetCurrentAvailableSlotIndex().Clear();
    }

    public GardenSlotProperties GetCurrentAvailableSlotIndex()
    {
        return gardenSlotProperties[currentAvailableSlotIndex];
    }

    public void CheckAndGrowPlant()
    {
        foreach (GardenSlotProperties slot in gardenSlotProperties)
        {
            (slot.crop.state as CropGrowing).GrowPlant();
        }    
    }    

    public int FindEmptySlotIndex()
    {
        for (int i = 0; i < activeSlotCount; i++)
        {
            if (gardenSlotProperties[i].IsEmptySlot()) return i;
        }
        return -1;
    }
}

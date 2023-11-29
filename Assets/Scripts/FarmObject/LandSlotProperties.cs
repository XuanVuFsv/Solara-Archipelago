using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandSlotProperties : MonoBehaviour
{
    [Header("Slot properties")]
    public bool isUnLock;
    public int upgradingLevel = 0;
    public Plant currentCrop;

    public int totalGrowingProduct;
    public float totalGrowingTime;

    public bool hasWatering;
    public bool hasFertilizering;

    [Header("Slot properties relate to growing process")]
    public Transform seedPos;
    public GameObject lockedSlotObjects;

    private void Start()
    {
        if (seedPos == null) seedPos = transform.Find("SeedPos");
        if (lockedSlotObjects == null) lockedSlotObjects = transform.Find("LockedSlotObjects").gameObject;
    }

    public LandSlotProperties(bool _isUnlock, Plant _crop, int _upgradingLevel = 0, bool _hasWatering = false, bool _hasFertilizering = false)
    {
        isUnLock = _isUnlock;
        currentCrop = _crop;
        totalGrowingProduct = _crop.wholePlantPoss.Count;
        totalGrowingTime = _crop.ammoStats.totalGrowingTime;

        upgradingLevel = _upgradingLevel;
        hasWatering = _hasWatering;
        hasFertilizering = _hasFertilizering;
    }

    public void UnLockSlot()
    {
        isUnLock = true;
        lockedSlotObjects.SetActive(false);
    }

    public bool IsLockedSot()
    {
        return !isUnLock;
    }

    public void ApplyGrowingTimeEffect(float value)
    {
        if (!isUnLock) return;
        totalGrowingTime *=  value;
    }
}

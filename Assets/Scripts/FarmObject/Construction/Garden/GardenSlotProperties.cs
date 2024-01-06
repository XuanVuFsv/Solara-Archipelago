using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenSlotProperties : MonoBehaviour
{
    public int index;
    public AmmoStats nullAmmo;

    [Header("Slot properties")]
    public bool isUnLock;
    public int upgradingLevel = 0;
    public Plant crop;

    [Header("Slot properties relate to growing process")]
    public int totalGrowingProduct;
    public float totalGrowingTime;

    public bool hasWatering = true;
    public bool hasFertilizering = true;

    [Header("Slot properties relate to growing state object")]
    public Transform seedPos;
    public GameObject lockedSlotObjects;
    public bool showWholeUI;

    [Header("Count Down")]
    public CountDownUI countDownUI;

    private void Start()
    {
        if (seedPos == null) seedPos = transform.Find("SeedPos");
        if (lockedSlotObjects == null) lockedSlotObjects = transform.Find("LockedSlotObjects").gameObject;
    }

    public GardenSlotProperties(bool _isUnlock, Plant _crop, int _upgradingLevel = 0, bool _hasWatering = false, bool _hasFertilizering = false)
    {
        isUnLock = _isUnlock;
        crop = _crop;
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

        isUnLock = true;
        crop = null;
    }

    public bool Clear()
    {
        if (!crop) return false;

        Destroy(crop.gameObject);

        countDownUI.ResetCountDown();
        crop = null;
        totalGrowingProduct = 0;
        totalGrowingTime = 0;
        return true;
    }

    public AmmoStats GetCurrentAmmoStats()
    {
        if (crop == null) return nullAmmo;
        return crop.ammoStats;
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

    public bool IsEmptySlot()
    {
        return isUnLock && crop == null;
    }

    public bool HaveWholeCrop()
    {
        if (!crop) return false;

        return crop.HaveWholeCrop();
    }

    public bool InGrowing()
    {
        if (!crop || crop.state is not CropGrowing) return false;

        return (crop.state as CropGrowing).inGrowing;
    }
}

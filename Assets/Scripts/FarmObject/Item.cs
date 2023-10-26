using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public AmmoStats ammoStats;
    public List<GameObject> totalPlant = new List<GameObject>();
    public int count;
    public bool isActive = true;

    public Item(AmmoStats ammoStats, int count, bool isActive, GameObject ammoObject)
    {
        this.ammoStats = ammoStats;
        AddAmmo(count, ammoObject);
        this.isActive = isActive;
    }

    public int AddAmmo(int newCount, GameObject ammoObject)
    {
        int currentCount = count + newCount;
        if (currentCount <= ammoStats.maxCount)
        {
            count = currentCount;
            totalPlant.Add(ammoObject);
            ammoObject.GetComponent<Suckable>().ChangeToStored();
            return 0;
        }
        else
        {
            count = ammoStats.maxCount;
            return currentCount - ammoStats.maxCount;
        }
    }

    public void UseAmmo(int newCount)
    {
        int currentCount = count - newCount;
        if (currentCount < 0)
        {
            return;
        }
        else
        {
            count = currentCount;
            if (ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.AxieCollector) totalPlant[totalPlant.Count - 1].GetComponent<Suckable>().MoveOut();
        }
    }
}
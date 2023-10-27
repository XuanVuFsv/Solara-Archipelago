using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public AmmoStats ammoStats;
    public List<Suckable> totalPlant = new List<Suckable>();
    public GameObject plantSample;
    public int count;
    public bool isActive = true;

    public Item(AmmoStats ammoStats, int count, bool isActive, Suckable ammoObject)
    {
        this.ammoStats = ammoStats;
        AddAmmo(count, ammoObject);
        this.isActive = isActive;
    }

    public int AddAmmo(int newCount, Suckable ammoObject)
    {
        bool isPlant = ammoObject is Plant;
        if (count == 0)
        {
            if (isPlant) plantSample = ammoObject.gameObject;
            else plantSample = (ammoObject as AmmoPickup).suckableSample;
        }

        if (isPlant) totalPlant.Add(ammoObject);
        else totalPlant.Add((ammoObject as AmmoPickup).suckableSample.GetComponent<Plant>());

        int currentCount = count + newCount;
        if (currentCount <= ammoStats.maxCount)
        {
            count = currentCount;
            ammoObject.GetComponent<Suckable>().ChangeToStored();
            return 0;
        }
        else
        {
            count = ammoStats.maxCount;
            return currentCount - ammoStats.maxCount;
        }
    }

    public void UseAmmo(int newCount, ActiveWeapon.WeaponSlot fromWeaponSlot)
    {
        int currentCount = count - newCount;
        if (currentCount < 0)
        {
            return;
        }
        else
        {
            count = currentCount;
            if (fromWeaponSlot == ActiveWeapon.WeaponSlot.AxieCollector)
            {
                int lastIndex = totalPlant.Count - 1;
                if (lastIndex >= 0)
                {
                    totalPlant[lastIndex].ChangeToSeed();
                    totalPlant[lastIndex].MoveOut();
                    totalPlant.RemoveAt(lastIndex);
                }
                else
                {
                    Plant newPlant = GameObject.Instantiate(plantSample, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Plant>();
                    newPlant.ChangeToSeed();
                    newPlant.MoveOut();
                }
            }
        }
    }
}
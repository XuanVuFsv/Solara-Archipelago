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
        //if (count == 0)
        //{
        //    if (isPlant) plantSample = ammoObject.gameObject;
        //    else plantSample = (ammoObject as AmmoPickup).suckableSample;
        //}

        if (totalPlant.Count > 0 && isPlant) totalPlant.Add(ammoObject);
        else if (totalPlant.Count == 0)
        {
            if (isPlant)
            {
                totalPlant.Add(ammoObject);
                Debug.Log("add");
            }
            else plantSample = (ammoObject as AmmoPickup).suckableSample;       
        }

        int currentCount = count + newCount;
        Debug.Log(currentCount);
        if (currentCount <= ammoStats.maxCount)
        {
            count = currentCount;
            Debug.Log("Change");
            Debug.Log(isPlant);
            if (isPlant)
            {
                ammoObject.GetComponent<Plant>().ChangeToStored();
                plantSample = GameObject.Instantiate(ammoObject.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity);
            }
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
            plantSample.GetComponent<Suckable>().DetachAmmoToObject();
            return;
        }
        else
        {
            count = currentCount;

            int lastIndex = totalPlant.Count - 1;
            if (lastIndex >= 0)
            {
                if (fromWeaponSlot == ActiveWeapon.WeaponSlot.AxieCollector)
                {
                    (totalPlant[lastIndex] as Plant).ChangeToSeed();
                    totalPlant[lastIndex].MoveOut();
                }

                totalPlant.RemoveAt(lastIndex);
            }
            else
            {
                if (fromWeaponSlot == ActiveWeapon.WeaponSlot.AxieCollector)
                {
                    Suckable newPlant = GameObject.Instantiate(plantSample, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Plant>();
                    newPlant.ChangeToSeed();
                    newPlant.MoveOut();
                }
            }
        }
    }

    public void ResetItem(Item resetItem)
    {
        ammoStats = resetItem.ammoStats;
        totalPlant = null;
        plantSample = null;
        count = 0;
    }
}
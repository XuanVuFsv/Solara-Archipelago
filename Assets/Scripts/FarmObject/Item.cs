using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public AmmoStats ammoStats;
    public List<Suckable> totalPlant = new List<Suckable>();
    public Suckable plantSample;
    public int index;
    public int count;
    public int ammountAmmoUsedByAttackWeapon = 0;
    public bool isActive = true;

    public Item(AmmoStats ammoStats, int count, bool isActive, Suckable ammoObject, int index)
    {
        this.ammoStats = ammoStats;
        AddAmmo(count, ammoObject);
        this.isActive = isActive;
        this.index = index;
    }

    public int AddAmmo(int newCount, Suckable ammoObject)
    {
        bool isPlant = ammoObject is Plant;
        //if (count == 0)
        //{
        //    if (isPlant) plantSample = ammoObject.gameObject;
        //    else plantSample = (ammoObject as AmmoPickup).suckableSample;
        //}

        if (isPlant)
        {
            Debug.Log("add");
            totalPlant.Add(ammoObject);
        }
        else if (totalPlant.Count == 0 && !isPlant)
        {
            Debug.Log("Just set plant sample");
            plantSample = (ammoObject as AmmoPickup).suckableSample;
        }

        int currentCount = count + newCount;
        Debug.Log(currentCount);

        if (currentCount <= ammoStats.maxCount)
        {
            count = currentCount;
            Debug.Log(isPlant);

            if (isPlant)
            {
                ammoObject.GetComponent<Plant>().ChangeToStored();
                plantSample = GameObject.Instantiate(ammoObject.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Suckable>();
            }
            else
            {
                GameObject.Destroy(ammoObject.gameObject);
            }
            return 0;
        }
        else
        {
            count = ammoStats.maxCount;
            if (!isPlant) GameObject.Destroy(ammoObject.gameObject);
            return currentCount - ammoStats.maxCount;
        }
    }

    public void UseAmmo(int newCount, ActiveWeapon.WeaponSlot fromWeaponSlot)
    {
        int currentCount = count - newCount;
        Debug.Log(currentCount);
        if (currentCount < 0)
        {
            //plantSample.RemoveUseGameEvent();
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
                    (totalPlant[lastIndex] as Plant).plantData.orginalBody = InventoryController.Instance.gameObject;
                    totalPlant[lastIndex].MoveOut();
                }

                totalPlant.RemoveAt(lastIndex);
            }
            else
            {
                if (fromWeaponSlot == ActiveWeapon.WeaponSlot.AxieCollector)
                {
                    Suckable newPlant = GameObject.Instantiate(plantSample.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Plant>();
                    (newPlant as Plant).plantData.orginalBody = InventoryController.Instance.gameObject;
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
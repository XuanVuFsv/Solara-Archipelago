using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public CropStats cropStats;
    public List<Suckable> totalPlant = new List<Suckable>();
    public Suckable suckableSample;
    public int index;
    public int count;
    public int ammountAmmoUsedByAttackWeapon = 0;
    public bool isActive = true;

    public Item(CropStats cropStats, int count, bool isActive, Suckable ammoObject, int index)
    {
        this.cropStats = cropStats;
        AddAmmo(count, ammoObject);
        this.isActive = isActive;
        this.index = index;
    }

    public bool IsFull()
    {
        return count < cropStats.maxCount;
    }

    public int AddAmmo(int newCount, Suckable ammoObject)
    {
        bool isPlant = ammoObject is Plant;
        bool isPower = ammoObject is PowerContainer;
        bool isNaturalResource = ammoObject is NaturalResource;

        //if (count == 0)
        //{
        //    if (isPlant) suckableSample = ammoObject.gameObject;
        //    else suckableSample = (ammoObject as AmmoPickup).suckableSample;
        //}

        int currentCount = count + newCount;
        //Debug.Log(currentCount);

        if ((isPlant || isPower || isNaturalResource) && currentCount <= cropStats.maxCount)
        {
            //Debug.Log("add");
            totalPlant.Add(ammoObject);
        }
        else if (totalPlant.Count == 0 && !(isPlant || isPower || isNaturalResource))
        {
            Debug.Log("Just set plant sample");
            suckableSample = (ammoObject as AmmoPickup).suckableSample;
        }

        if (currentCount <= cropStats.maxCount)
        {
            count = currentCount;
            //Debug.Log(isPlant);

            if (isPlant || isPower || isNaturalResource)
            {
                ammoObject.GetComponent<Suckable>().ChangeToStored();
                suckableSample = GameObject.Instantiate(ammoObject.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Suckable>();
            }
            //else if (isPower)
            //{
            //    ammoObject.GetComponent<PowerContainer>().ChangeToStored();
            //    suckableSample = GameObject.Instantiate(ammoObject.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Suckable>();
            //}
            //else if (isNaturalResource)
            //{
            //    ammoObject.GetComponent<PowerContainer>().ChangeToStored();
            //    suckableSample = GameObject.Instantiate(ammoObject.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Suckable>();
            //}
            else
            {
                GameObject.Destroy(ammoObject.gameObject);
            }
            return 0;
        }
        else
        {
            count = cropStats.maxCount;
            if (!isPlant) GameObject.Destroy(ammoObject.gameObject);
            return currentCount - cropStats.maxCount;
        }
    }

    public void UseAmmo(int newCount, ActiveWeapon.WeaponSlot fromWeaponSlot)
    {
        int currentCount = count - newCount;
        //Debug.Log(currentCount);
        if (currentCount < 0)
        {
            //suckableSample.RemoveUseGameEvent();
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
                    (totalPlant[lastIndex]).ChangeToUnStored();
                    (totalPlant[lastIndex]).transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

                    //(totalPlant[lastIndex] as Plant).orginalBody = InventoryController.Instance.gameObject;
                    totalPlant[lastIndex].MoveOut();
                }

                totalPlant.RemoveAt(lastIndex);
            }
            else
            {
                if (fromWeaponSlot == ActiveWeapon.WeaponSlot.AxieCollector)
                {
                    //bool isPlant = suckableSample is Plant;
                    //bool isPower = suckableSample is PowerContainer;

                    Suckable newSuckableObject;

                    if (suckableSample is Plant) newSuckableObject = GameObject.Instantiate(suckableSample.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<Plant>();
                    else newSuckableObject = GameObject.Instantiate(suckableSample.gameObject, CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position, Quaternion.identity).GetComponent<PowerContainer>();

                    newSuckableObject.gameObject.SetActive(true);
                    //(newPlant as Plant).plantData.orginalBody = InventoryController.Instance.gameObject;
                    //newPlant.ChangeToSeed();
                    newSuckableObject.MoveOut();
                }
            }
        }
    }

    public void ResetItem(Item resetItem)
    {
        cropStats = resetItem.cropStats;
        totalPlant = null;
        suckableSample = null;
        count = 0;
    }
}
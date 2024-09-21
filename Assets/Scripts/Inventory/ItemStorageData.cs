using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStorageData
{
    public ItemStorageData(string itemID, AmmoStats ammoStats, StorageLocation storageLocation, Suckable suckable, int quantity)
    {
        this.itemID = itemID;  
        this.ammoStats = ammoStats;
        this.storageLocation = storageLocation;
        this.quantity = quantity;
        this.suckableItem = suckable;
    }

    public enum StorageLocation
    {
        Silo = 0,
        CraftMachine = 1
    }
    public string itemID;
    public AmmoStats ammoStats;
    public StorageLocation storageLocation;
    public Suckable suckableItem;
    public int quantity;
}

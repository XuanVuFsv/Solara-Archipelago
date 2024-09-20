using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStorageData
{
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

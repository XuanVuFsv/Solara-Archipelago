using System;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Inventory
{
    [Serializable]
    public class ItemStorageData
    {
        public ItemStorageData(string itemID, CollectableObjectStat collectableObjectStat, StorageLocation storageLocation, Suckable suckable, int quantity)
        {
            this.itemID = itemID;
            this.collectableObjectStat = collectableObjectStat;
            this.storageLocation = storageLocation;
            this.quantity = quantity;
            suckableItem = suckable;
        }

        public enum StorageLocation
        {
            Silo = 0,
            CraftMachine = 1
        }
        public string itemID;
        public CollectableObjectStat collectableObjectStat;
        public StorageLocation storageLocation;
        public Suckable suckableItem;
        public int quantity;
    }
}
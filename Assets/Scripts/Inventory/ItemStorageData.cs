using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Stats;

namespace VitsehLand.Scripts.Inventory
{
    [System.Serializable]
    public class ItemStorageData
    {
        public ItemStorageData(string itemID, CropStats cropStats, StorageLocation storageLocation, Suckable suckable, int quantity)
        {
            this.itemID = itemID;
            this.cropStats = cropStats;
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
        public CropStats cropStats;
        public StorageLocation storageLocation;
        public Suckable suckableItem;
        public int quantity;
    }
}
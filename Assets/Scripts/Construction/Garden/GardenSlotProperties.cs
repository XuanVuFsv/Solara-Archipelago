using UnityEngine;
using VitsehLand.Scripts.Farming.ObjectState.Crop;
using VitsehLand.Scripts.Manager;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.UI.MonitorGarden;

namespace VitsehLand.Scripts.Construction.Garden
{
    public class GardenSlotProperties : MonoBehaviour
    {
        public int index;
        public CollectableObjectStat nullAmmo;

        [Header("Slot properties")]
        public bool isUnLock;
        public int upgradingLevel = 0;
        public int unlockFee;
        public Crop crop;

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
            if (seedPos == null) seedPos = transform.Find("Seed_Pos");
            if (lockedSlotObjects == null) lockedSlotObjects = transform.Find("Locked_Slot_Objects").gameObject;
        }

        public GardenSlotProperties(bool _isUnlock, Crop _crop, int _upgradingLevel = 0, bool _hasWatering = false, bool _hasFertilizering = false)
        {
            isUnLock = _isUnlock;
            crop = _crop;
            totalGrowingProduct = _crop.wholePlantPoss.Count;
            totalGrowingTime = _crop.collectableObjectStat.totalGrowingTime;

            upgradingLevel = _upgradingLevel;
            hasWatering = _hasWatering;
            hasFertilizering = _hasFertilizering;
        }

        public bool UnLockSlot()
        {
            bool canUseGem = GemManager.Instance.UseGem(unlockFee);
            if (!canUseGem)
            {
                Debug.Log("Not enough gem");
                return false;
            }

            isUnLock = true;
            lockedSlotObjects.SetActive(false);

            isUnLock = true;
            crop = null;

            return true;
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

        public CollectableObjectStat GetCurrentCollectableObjectStat()
        {
            if (crop == null) return nullAmmo;
            return crop.collectableObjectStat;
        }

        public bool IsLockedSot()
        {
            return !isUnLock;
        }

        public void ApplyGrowingTimeEffect(float value)
        {
            if (!isUnLock) return;
            totalGrowingTime *= value;
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
}
using UnityEngine;
using VitsehLand.GameCamera;
using VitsehLand.GameCamera.Shaking;
using VitsehLand.Scripts.Farming.General;
using VitsehLand.Scripts.Inventory;
using VitsehLand.Scripts.Pattern.Observer;
using VitsehLand.Scripts.Stats;
using VitsehLand.Scripts.UI.Weapon;
using VitsehLand.Scripts.Weapon.Ammo;
using VitsehLand.Scripts.Weapon.Primary;

namespace VitsehLand.Scripts.Weapon.General
{
    public class WeaponStatsController : MonoBehaviour
    {
        public WeaponStat weaponStat;
        public ActiveWeapon.WeaponSlot weaponSlot;
        public CollectableObjectStatController currentCollectableObjectStatController;
        public GunCameraController gunCamera;

        [SerializeField]
        Suckable ammunitionChestPicked, defaultAmmoOnStart;
        [SerializeField]
        CameraShake cameraShake;
        [SerializeField]
        GameEvent pickAmmoEvent;
        public Item itemInInventory;

        public GameObject magazineObject;
        public AnimationClip weaponAnimation;

        public string weaponName;
        public int currentAmmo, remainingAmmo, maxQuantityStored;
        public bool autoReload = true;

        bool outOfAmmo; //out of ammo in magazine
        bool hasRun = false;

        // Start is called before the first frame update
        void Start()
        {
            cameraShake = gameObject.GetComponent<CameraShake>();
            if (!currentCollectableObjectStatController) currentCollectableObjectStatController = GetComponent<CollectableObjectStatController>();
            Invoke(nameof(OnStart), 0.5f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnStart()
        {
            if (hasRun) return;
            if (weaponSlot != ActiveWeapon.WeaponSlot.AttackGun) return;
            //Debug.Log(hasRun + " from " + defaultAmmoOnStart.name);
            //SetupCollectableObjectStat(defaultAmmoOnStart);
            hasRun = true;
        }

        public Suckable GetDefaultAmmoOnStart()
        {
            return defaultAmmoOnStart;
        }

        public void SetupWeaponStats(WeaponStat weaponStat)
        {
            //Ammo can used by both Attack Gun and Collector Gun
            if (weaponStat.weaponSlot != ActiveWeapon.WeaponSlot.HandGun)
            {
                SetupCollectableObjectStat(currentCollectableObjectStatController.collectableObjectStat == InventoryController.Instance.GetCurrentItem().collectableObjectStat);
            }
            UpdateAmmoState();
            weaponName = weaponStat.name;
            weaponSlot = weaponStat.weaponSlot;
            weaponAnimation = weaponStat.weaponAnimation;

            UpdateWeaponUI();
            //if (weaponStat.weaponSlot != ActiveWeapon.WeaponSlot.HandGun)
            //{
            //    UpdateAmmoAmmountUI(InventoryController.Instance.GetCurrentItem().count, InventoryController.Instance.GetCurrentItem().index);
            //}
        }

        public void SetupCollectableObjectStat(bool isSameAmmo)
        {
            itemInInventory = InventoryController.Instance.GetCurrentItem();

            if (!isSameAmmo)
            {
                //Debug.Log("Diff");
                currentCollectableObjectStatController.collectableObjectStat = itemInInventory.collectableObjectStat;
                currentCollectableObjectStatController.AssignCroptData();

                //currentAmmo = 0;
                //outOfAmmo = true;
                //remainingAmmo = itemInInventory.count;

                if (weaponStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)
                {
                    //Only modify gun camera if switch to Attack Gun
                    gunCamera.SetMultiplier(currentCollectableObjectStatController.multiplierForAmmo);
                    gunCamera.SetHasScope(currentCollectableObjectStatController.zoomType == CollectableObjectStat.ZoomType.HasScope);

                    //Attach Ammo to this object to 
                    if (itemInInventory.collectableObjectStat.featuredType == GameObjectType.FeaturedType.Normal && itemInInventory.collectableObjectStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.AddUsedGameEvent();
                    //PoolingManager.Instance.AddGameEvent("PoolTomatoSetup");


                    //Invoke event for pick ammo
                    pickAmmoEvent.Notify(currentCollectableObjectStatController.amplitudeGainImpulse);
                    pickAmmoEvent.Notify(currentCollectableObjectStatController.multiplierRecoilOnAim);
                }
            }
            else
            {
                //Debug.Log("Same");
            }

            if (weaponStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun && itemInInventory.collectableObjectStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)
            {
                currentAmmo = 0;
                outOfAmmo = true;
                remainingAmmo = itemInInventory.count;
            }

            if (weaponStat.weaponSlot == ActiveWeapon.WeaponSlot.AxieCollector)
            {
                currentAmmo = itemInInventory.count;
                outOfAmmo = false;
                remainingAmmo = itemInInventory.count;
            }

            UpdateAmmoState();
            //UpdateNewAmmo(itemInInventory);
        }

        public void ResetCropStats(CollectableObjectStat resetCropStats)
        {
            currentCollectableObjectStatController.collectableObjectStat = resetCropStats;
            currentCollectableObjectStatController.AssignCroptData();

            maxQuantityStored = 0;
        }

        public void SuckUpAmmo(Suckable ammoPickup)
        {
            //MyDebug.Log(ammoPickup);
            //if (!currentCollectableObjectStatController) Debug.Log("currentCollectableObjectStatController null " + transform.parent.name);
            //Debug.Log(ammoPickup.name);

            //if (!currentCollectableObjectStatController.collectableObjectStat)
            //{

            //    Debug.Log("Add to null " + transform.parent.name);
            //    //currentCollectableObjectStatController.collectableObjectStat = ammoPickup.collectableObjectStat;
            //    //currentCollectableObjectStatController.AssignCroptData();
            //    //ofActiveAmmo = weaponSlot == InventoryController.Instance.GetCurrentItem().collectableObjectStat.weaponSlot;

            //    //if (ofActiveAmmo)
            //    //{
            //    //    //Debug.Log("Set" + currentCollectableObjectStatController.multiplierForAmmo);
            //    //    gunCamera.SetMultiplier(currentCollectableObjectStatController.multiplierForAmmo);
            //    //}
            //    //pickAmmoEvent.Notify(currentCollectableObjectStatController.amplitudeGainImpulse);
            //    //pickAmmoEvent.Notify(currentCollectableObjectStatController.multiplierRecoilOnAim);

            //    //SetNewAmmoCount(ammoPickup);
            //    //ammunitionChestPicked = ammoPickup;
            //}
            //else
            if (currentCollectableObjectStatController.collectableObjectStat.collectableObjectName == ammoPickup.collectableObjectStat.collectableObjectName)
            {
                Debug.Log("Add same ammo");

                AddAmmo(ammoPickup.GetCropContain(), ammoPickup);
                //ammoPickup.AddUsedGameEvent(transform);

                SetupCollectableObjectStat(true);

                UpdateAmmoAmmountUI(itemInInventory.count, itemInInventory.index);

                //if (!ammunitionChestPicked)
                //{
                //    ammoPickup.AttachAmmoToObject(transform, false);
                //    ammunitionChestPicked = ammoPickup;
                //}
                //else Destroy(ammoPickup.gameObject);

                //ammoPickup.SetAmmoContain(0);
            }
            else
            {
                Debug.Log("Add new ammo");

                //if (ammunitionChestPicked)
                //{
                //    ammunitionChestPicked.DetachAmmoToObject(null, true);
                //}
                //ammunitionChestPicked.ammoContain = InventoryController.Instance.GetItem(ammunitionChestPicked.collectableObjectStat).count;

                //currentCollectableObjectStatController.collectableObjectStat = ammoPickup.collectableObjectStat;
                //currentCollectableObjectStatController.AssignCroptData();

                //pickAmmoEvent.Notify(currentCollectableObjectStatController.amplitudeGainImpulse);
                //pickAmmoEvent.Notify(currentCollectableObjectStatController.multiplierRecoilOnAim);

                //SetNewAmmoCount(ammoPickup);
                Item newItem = InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.collectableObjectStat, ammoPickup.GetCropContain(), ammoPickup);
                //if (ammoPickup.collectableObjectStat.featuredType == CropStats.FeaturedType.Normal && ammoPickup.collectableObjectStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) ammoPickup.AddUsedGameEvent(false);

                if (itemInInventory.collectableObjectStat.baseId == "null" && itemInInventory != InventoryController.Instance.GetCurrentItem())
                {
                    Debug.Log("Null item");
                    itemInInventory = InventoryController.Instance.GetCurrentItem();
                    currentCollectableObjectStatController.collectableObjectStat = ammoPickup.collectableObjectStat;
                    currentCollectableObjectStatController.AssignCroptData();
                    SetupCollectableObjectStat(false);
                    UpdateNewAmmo(itemInInventory, itemInInventory.index);
                }
                else UpdateNewAmmo(newItem, newItem.index);


                //Debug.Log(newItem.index);
                //ammunitionChestPicked = ammoPickup;
            }
        }

        //void SetNewAmmoCount(Suckable ammoPickup)
        //{
        //    currentAmmo = 0;
        //    outOfAmmo = true;

        //    itemInInventory = InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.GetCropStats(), ammoPickup.GetAmmoContain(), ammoPickup);
        //    Debug.Log("XXX" + itemInInventory);
        //    remainingAmmo = ammoPickup.GetAmmoContain();
        //    ammoInMagazine = ammoPickup.GetCropStats().ammoAllowedInMagazine;

        //    UpdateAmmoUI();
        //}

        public void SwitchAmmo(int step)
        {
            if (itemInInventory.collectableObjectStat.featuredType == GameObjectType.FeaturedType.Normal && itemInInventory.collectableObjectStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.RemoveUseGameEvent();

            InventoryController.Instance.SwitchItem(step);
            itemInInventory = InventoryController.Instance.GetCurrentItem();
            //if (itemInInventory.collectableObjectStat.featuredType == CropStats.FeaturedType.Normal && itemInInventory.collectableObjectStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.AddUsedGameEvent(false);
            SetupCollectableObjectStat(false);

            UpdateNewAmmo(itemInInventory, itemInInventory.index);
        }

        public void UseAmmo(int count)
        {
            if (currentCollectableObjectStatController.collectableObjectStat.featuredType == GameObjectType.FeaturedType.None || currentCollectableObjectStatController.collectableObjectStat.featuredType == GameObjectType.FeaturedType.Product && weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) return;
            //InventoryController.Instance.GetCurrentItem().AddAmmo(ammo);
            itemInInventory.UseAmmo(count, weaponSlot);
            //remainingAmmo = InventoryController.Instance.GetCurrentItem().count - currentAmmo; //ammo in inventory or bag
            currentAmmo = itemInInventory.count;
            remainingAmmo = itemInInventory.count - currentAmmo;

            if (currentAmmo <= 0)
            {
                //Debug.Log("<0");
                outOfAmmo = true;

                if (itemInInventory.collectableObjectStat.zoomType != CollectableObjectStat.ZoomType.NoZoom && weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) (GetComponent<RaycastWeapon>().weaponHandler as ShootingHandler).HandleRightMouseClick();
                if (itemInInventory.collectableObjectStat.featuredType == GameObjectType.FeaturedType.Normal && itemInInventory.collectableObjectStat.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.RemoveUseGameEvent();
                InventoryController.Instance.ResetCurrentSlot();
                itemInInventory = InventoryController.Instance.GetCurrentItem();

                UpdateNewAmmo(InventoryController.Instance.nullItem, itemInInventory.index);

                ResetCropStats(itemInInventory.collectableObjectStat);
                //UpdateAmmoAmmountUI(itemInInventory.count, itemInInventory.index);
            }
            else
            {
                UpdateAmmoAmmountUI(itemInInventory.count, itemInInventory.index);
            }
            //Debug.Log("Use Ammo");
        }

        public void UpdateAmmoAfterReload()
        {
            if (weaponSlot != ActiveWeapon.WeaponSlot.AttackGun) return;

            int neededAmmo = maxQuantityStored - currentAmmo; //ammo need to fill full magazine

            if (neededAmmo <= remainingAmmo)
            {
                currentAmmo = maxQuantityStored;
                remainingAmmo -= neededAmmo; //ammo in inventory or bag
                outOfAmmo = false;
            }
            else
            {
                currentAmmo += remainingAmmo;
                remainingAmmo = 0;
                outOfAmmo = false;
            }

            //Debug.Log("Update After Reload");
            UpdateAmmoAmmountUI(itemInInventory.count, itemInInventory.index);
        }

        public void UpdateAmmoState()
        {
            if (currentAmmo > 0) outOfAmmo = false;
            else outOfAmmo = true;
        }

        public void UpdateWeaponUI()
        {
            //WeaponSystemUI.Instance.UpdateAmmo(currentAmmo, remainingAmmo);
        }

        public void UpdateNewAmmo(Item item, int index)
        {
            Debug.Log(item.collectableObjectStat.collectableObjectName + item.count + index);
            WeaponSystemUI.Instance.SetDisplayItemIcon(item.collectableObjectStat.icon, index);
            WeaponSystemUI.Instance.SetDisplayItemName(itemInInventory.collectableObjectStat.collectableObjectName, index);
            UpdateAmmoAmmountUI(item.count, index);
        }

        public void UpdateDisplayItemIcon(Sprite sprite, int index)
        {
            WeaponSystemUI.Instance.SetDisplayItemIcon(sprite, index);
        }

        public void UpdateAmmoAmmountUI(int totalAmount, int index)
        {
            //Debug.Log(totalAmount + " " + index);
            WeaponSystemUI.Instance.SetDisplayItemAmmoAmount(totalAmount, index);
        }

        public void AddAmmo(int ammo, Suckable ammoObject)
        {
            itemInInventory.AddAmmo(ammo, ammoObject);
            remainingAmmo = itemInInventory.count - currentAmmo;
        }

        public bool IsOutOfAmmo()
        {
            return outOfAmmo;
        }

        public bool IsContainAmmo()
        {
            return remainingAmmo > 0;
        }

        public bool IsFullMagazine()
        {
            return currentAmmo == maxQuantityStored;
        }
    }
}
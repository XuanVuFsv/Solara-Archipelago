using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class WeaponStatsController: MonoBehaviour
{
    public WeaponStats weaponStats;
    public ActiveWeapon.WeaponSlot weaponSlot;
    public CropStatsController currentCropStatsController;
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
    public int currentAmmo, remainingAmmo, ammoInMagazine;
    public bool autoReload = true;
    
    bool outOfAmmo; //out of ammo in magazine
    bool hasRun = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraShake = gameObject.GetComponent<CameraShake>();
        if (!currentCropStatsController) currentCropStatsController = GetComponent<CropStatsController>();
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
        //SetupCropStats(defaultAmmoOnStart);
        hasRun = true;
    }

    public Suckable GetDefaultAmmoOnStart()
    {
        return defaultAmmoOnStart;
    }

    public void SetupWeaponStats(WeaponStats weaponStats)
    {
        //Ammo can used by both Attack Gun and Collector Gun
        if (weaponStats.weaponSlot != ActiveWeapon.WeaponSlot.HandGun)
        {
            SetupCropStats(currentCropStatsController.cropStats == InventoryController.Instance.GetCurrentItem().cropStats);
        }    
        UpdateAmmoState();
        weaponName = weaponStats.name;
        weaponSlot = weaponStats.weaponSlot;
        weaponAnimation = weaponStats.weaponAnimation;

        UpdateWeaponUI();
        //if (weaponStats.weaponSlot != ActiveWeapon.WeaponSlot.HandGun)
        //{
        //    UpdateAmmoAmmountUI(InventoryController.Instance.GetCurrentItem().count, InventoryController.Instance.GetCurrentItem().index);
        //}
    }

    public void SetupCropStats(bool isSameAmmo)
    {
        itemInInventory = InventoryController.Instance.GetCurrentItem();

        if (!isSameAmmo)
        {
            //Debug.Log("Diff");
            currentCropStatsController.cropStats = itemInInventory.cropStats;
            currentCropStatsController.AssignCroptData();

            //currentAmmo = 0;
            //outOfAmmo = true;
            //remainingAmmo = itemInInventory.count;

            if (weaponStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)
            {
                //Only modify gun camera if switch to Attack Gun
                gunCamera.SetMultiplier(currentCropStatsController.multiplierForAmmo);
                gunCamera.SetHasScope(currentCropStatsController.zoomType == CropStats.ZoomType.HasScope);

                //Attach Ammo to this object to 
                if (itemInInventory.cropStats.featuredType == GameObjectType.FeaturedType.Normal && itemInInventory.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.AddUsedGameEvent(); 
                //PoolingManager.Instance.AddGameEvent("PoolTomatoSetup");


                //Invoke event for pick ammo
                pickAmmoEvent.Notify(currentCropStatsController.amplitudeGainImpulse);
                pickAmmoEvent.Notify(currentCropStatsController.multiplierRecoilOnAim);
            }
        }
        else
        {
            //Debug.Log("Same");
        }

        if (weaponStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun && itemInInventory.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)
        {
            currentAmmo = 0;
            outOfAmmo = true;
            remainingAmmo = itemInInventory.count;
        }

        if (weaponStats.weaponSlot == ActiveWeapon.WeaponSlot.AxieCollector)
        {
            currentAmmo = itemInInventory.count;
            outOfAmmo = false;
            remainingAmmo = itemInInventory.count;
        }

        UpdateAmmoState();
        //UpdateNewAmmo(itemInInventory);
    }

    public void ResetCropStats(CropStats resetCropStats)
    {
        currentCropStatsController.cropStats = resetCropStats;
        currentCropStatsController.AssignCroptData();

        ammoInMagazine = 0;
    }
    
    public void SuckUpAmmo(Suckable ammoPickup)
    {
        //MyDebug.Log(ammoPickup);
        //if (!currentCropStatsController) Debug.Log("currentCropStatsController null " + transform.parent.name);
        //Debug.Log(ammoPickup.name);

        //if (!currentCropStatsController.cropStats)
        //{

        //    Debug.Log("Add to null " + transform.parent.name);
        //    //currentCropStatsController.cropStats = ammoPickup.cropStats;
        //    //currentCropStatsController.AssignCroptData();
        //    //ofActiveAmmo = weaponSlot == InventoryController.Instance.GetCurrentItem().cropStats.weaponSlot;

        //    //if (ofActiveAmmo)
        //    //{
        //    //    //Debug.Log("Set" + currentCropStatsController.multiplierForAmmo);
        //    //    gunCamera.SetMultiplier(currentCropStatsController.multiplierForAmmo);
        //    //}
        //    //pickAmmoEvent.Notify(currentCropStatsController.amplitudeGainImpulse);
        //    //pickAmmoEvent.Notify(currentCropStatsController.multiplierRecoilOnAim);

        //    //SetNewAmmoCount(ammoPickup);
        //    //ammunitionChestPicked = ammoPickup;
        //}
        //else
        if (currentCropStatsController.cropStats.name == ammoPickup.cropStats.name)
        {
            //Debug.Log("Add same ammo");

            AddAmmo(ammoPickup.GetCropContain(), ammoPickup);
            //ammoPickup.AddUsedGameEvent(transform);

            SetupCropStats(true);

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
            //Debug.Log("Add new ammo");

            //if (ammunitionChestPicked)
            //{
            //    ammunitionChestPicked.DetachAmmoToObject(null, true);
            //}
            //ammunitionChestPicked.ammoContain = InventoryController.Instance.GetItem(ammunitionChestPicked.cropStats).count;

            //currentCropStatsController.cropStats = ammoPickup.cropStats;
            //currentCropStatsController.AssignCroptData();

            //pickAmmoEvent.Notify(currentCropStatsController.amplitudeGainImpulse);
            //pickAmmoEvent.Notify(currentCropStatsController.multiplierRecoilOnAim);

            //SetNewAmmoCount(ammoPickup);
            Item newItem = InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.cropStats, ammoPickup.GetCropContain(), ammoPickup);
            //if (ammoPickup.cropStats.featuredType == CropStats.FeaturedType.Normal && ammoPickup.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) ammoPickup.AddUsedGameEvent(false);

            if (itemInInventory.cropStats.name == "Null" && itemInInventory != InventoryController.Instance.GetCurrentItem())
            {
                itemInInventory = InventoryController.Instance.GetCurrentItem();
                currentCropStatsController.cropStats = ammoPickup.cropStats;
                currentCropStatsController.AssignCroptData();
                SetupCropStats(false);
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
        if (itemInInventory.cropStats.featuredType == GameObjectType.FeaturedType.Normal && itemInInventory.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.RemoveUseGameEvent();

        InventoryController.Instance.SwitchItem(step);
        itemInInventory = InventoryController.Instance.GetCurrentItem();
        //if (itemInInventory.cropStats.featuredType == CropStats.FeaturedType.Normal && itemInInventory.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.AddUsedGameEvent(false);
        SetupCropStats(false);

        UpdateNewAmmo(itemInInventory, itemInInventory.index);
    }

    public void UseAmmo(int count)
    {
        if (currentCropStatsController.cropStats.featuredType == GameObjectType.FeaturedType.None || (currentCropStatsController.cropStats.featuredType == GameObjectType.FeaturedType.Product && weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)) return;
        //InventoryController.Instance.GetCurrentItem().AddAmmo(ammo);
        itemInInventory.UseAmmo(count, weaponSlot);
        //remainingAmmo = InventoryController.Instance.GetCurrentItem().count - currentAmmo; //ammo in inventory or bag
        currentAmmo = itemInInventory.count;
        remainingAmmo = itemInInventory.count - currentAmmo;

        if (currentAmmo <= 0)
        {
            //Debug.Log("<0");
            outOfAmmo = true;

            if (itemInInventory.cropStats.zoomType != CropStats.ZoomType.NoZoom && weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) (GetComponent<RaycastWeapon>().weaponHandler as ShootingHandler).HandleRightMouseClick();
            if (itemInInventory.cropStats.featuredType == GameObjectType.FeaturedType.Normal && itemInInventory.cropStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.suckableSample.RemoveUseGameEvent();
            InventoryController.Instance.ResetCurrentSlot();
            itemInInventory = InventoryController.Instance.GetCurrentItem();

            UpdateNewAmmo(InventoryController.Instance.nullItem, itemInInventory.index);

            ResetCropStats(itemInInventory.cropStats);
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

        int neededAmmo = ammoInMagazine - currentAmmo; //ammo need to fill full magazine

        if (neededAmmo <= remainingAmmo)
        {
            currentAmmo = ammoInMagazine;
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
        //Debug.Log(item.cropStats.artwork.name + item.count + index);
        WeaponSystemUI.Instance.SetDisplayItemIcon(item.cropStats.artwork, index);
        //WeaponSystemUI.Instance.SetDisplayItemName(itemInInventory.cropStats.name, index);
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
        return currentAmmo == ammoInMagazine;
    }
}
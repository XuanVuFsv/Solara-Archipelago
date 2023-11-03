using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class WeaponStatsController: MonoBehaviour
{
    public WeaponStats weaponStats;
    public ActiveWeapon.WeaponSlot weaponSlot;
    public AmmoStatsController currentAmmoStatsController;
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
        if (!currentAmmoStatsController) currentAmmoStatsController = GetComponent<AmmoStatsController>();
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
        //SetupAmmoStats(defaultAmmoOnStart);
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
            SetupAmmoStats(currentAmmoStatsController.ammoStats == InventoryController.Instance.GetCurrentItem().ammoStats);
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

    public void SetupAmmoStats(bool isSameAmmo)
    {
        itemInInventory = InventoryController.Instance.GetCurrentItem();

        if (!isSameAmmo)
        {
            Debug.Log("Diff");
            currentAmmoStatsController.ammoStats = itemInInventory.ammoStats;
            currentAmmoStatsController.AssignAmmotData();

            //currentAmmo = 0;
            //outOfAmmo = true;
            //remainingAmmo = itemInInventory.count;

            if (weaponStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)
            {
                //Only modify gun camera if switch to Attack Gun
                gunCamera.SetMultiplier(currentAmmoStatsController.multiplierForAmmo);
                gunCamera.SetHasScope(currentAmmoStatsController.zoomType == AmmoStats.ZoomType.HasScope);

                //Attach Ammo to this object to 
                if (itemInInventory.ammoStats.featuredType == AmmoStats.FeaturedType.Normal && itemInInventory.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.plantSample.GetComponent<Suckable>().AddUsedGameEvent(true);

                //Invoke event for pick ammo
                pickAmmoEvent.Notify(currentAmmoStatsController.amplitudeGainImpulse);
                pickAmmoEvent.Notify(currentAmmoStatsController.multiplierRecoilOnAim);
            }
        }
        else
        {
            Debug.Log("Same");
        }

        if (weaponStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun && itemInInventory.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)
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

    public void ResetAmmoStats(AmmoStats resetAmmoStats)
    {
        currentAmmoStatsController.ammoStats = resetAmmoStats;
        currentAmmoStatsController.AssignAmmotData();

        ammoInMagazine = 0;
    }
    
    public void SuckUpAmmo(Suckable ammoPickup)
    {
        MyDebug.Instance.Log(ammoPickup);
        //if (!currentAmmoStatsController) Debug.Log("currentAmmoStatsController null " + transform.parent.name);
        //Debug.Log(ammoPickup.name);

        if (!currentAmmoStatsController.ammoStats)
        {

            Debug.Log("Add to null " + transform.parent.name);
            //currentAmmoStatsController.ammoStats = ammoPickup.ammoStats;
            //currentAmmoStatsController.AssignAmmotData();
            //ofActiveAmmo = weaponSlot == InventoryController.Instance.GetCurrentItem().ammoStats.weaponSlot;

            //if (ofActiveAmmo)
            //{
            //    //Debug.Log("Set" + currentAmmoStatsController.multiplierForAmmo);
            //    gunCamera.SetMultiplier(currentAmmoStatsController.multiplierForAmmo);
            //}
            //pickAmmoEvent.Notify(currentAmmoStatsController.amplitudeGainImpulse);
            //pickAmmoEvent.Notify(currentAmmoStatsController.multiplierRecoilOnAim);

            //SetNewAmmoCount(ammoPickup);
            //ammunitionChestPicked = ammoPickup;
        }
        else if (currentAmmoStatsController.ammoStats.name == ammoPickup.ammoStats.name)
        {
            Debug.Log("Add same ammo");

            AddAmmo(ammoPickup.GetAmmoContain(), ammoPickup);
            //ammoPickup.AddUsedGameEvent(transform);

            SetupAmmoStats(true);

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
            //ammunitionChestPicked.ammoContain = InventoryController.Instance.GetItem(ammunitionChestPicked.ammoStats).count;

            //currentAmmoStatsController.ammoStats = ammoPickup.ammoStats;
            //currentAmmoStatsController.AssignAmmotData();

            //pickAmmoEvent.Notify(currentAmmoStatsController.amplitudeGainImpulse);
            //pickAmmoEvent.Notify(currentAmmoStatsController.multiplierRecoilOnAim);

            //SetNewAmmoCount(ammoPickup);
            Item newItem = InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.ammoStats, ammoPickup.GetAmmoContain(), ammoPickup);
            if (ammoPickup.ammoStats.featuredType == AmmoStats.FeaturedType.Normal && ammoPickup.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) ammoPickup.AddUsedGameEvent(itemInInventory.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun);

            if (itemInInventory.ammoStats.name == "Null" && itemInInventory != InventoryController.Instance.GetCurrentItem())
            {
                itemInInventory = InventoryController.Instance.GetCurrentItem();
                currentAmmoStatsController.ammoStats = ammoPickup.ammoStats;
                currentAmmoStatsController.AssignAmmotData();
                SetupAmmoStats(false);
                UpdateNewAmmo(itemInInventory, itemInInventory.index);
            }
            else UpdateNewAmmo(newItem, newItem.index);


            Debug.Log(newItem.index);
            //ammunitionChestPicked = ammoPickup;
        }
    }

    //void SetNewAmmoCount(Suckable ammoPickup)
    //{
    //    currentAmmo = 0;
    //    outOfAmmo = true;

    //    itemInInventory = InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.GetAmmoStats(), ammoPickup.GetAmmoContain(), ammoPickup);
    //    Debug.Log("XXX" + itemInInventory);
    //    remainingAmmo = ammoPickup.GetAmmoContain();
    //    ammoInMagazine = ammoPickup.GetAmmoStats().ammoAllowedInMagazine;

    //    UpdateAmmoUI();
    //}

    public void SwitchAmmo(int step)
    {
        if (itemInInventory.ammoStats.featuredType == AmmoStats.FeaturedType.Normal && itemInInventory.ammoStats.weaponSlot == ActiveWeapon.WeaponSlot.AttackGun) itemInInventory.plantSample.GetComponent<Suckable>().RemoveUseGameEvent();

        InventoryController.Instance.SwitchItem(step);
        itemInInventory = InventoryController.Instance.GetCurrentItem();
        SetupAmmoStats(false);

        UpdateNewAmmo(itemInInventory, itemInInventory.index);
    }

    public void UseAmmo(int count)
    {
        if (currentAmmoStatsController.ammoStats.featuredType == AmmoStats.FeaturedType.None || (currentAmmoStatsController.ammoStats.featuredType == AmmoStats.FeaturedType.Product && weaponSlot == ActiveWeapon.WeaponSlot.AttackGun)) return;
        //InventoryController.Instance.GetCurrentItem().AddAmmo(ammo);
        itemInInventory.UseAmmo(count, weaponSlot);
        //remainingAmmo = InventoryController.Instance.GetCurrentItem().count - currentAmmo; //ammo in inventory or bag
        currentAmmo = itemInInventory.count;
        remainingAmmo = itemInInventory.count - currentAmmo;

        if (currentAmmo <= 0)
        {
            Debug.Log("<0");
            outOfAmmo = true;
            InventoryController.Instance.ResetCurrentSlot();
            itemInInventory = InventoryController.Instance.GetCurrentItem();

            UpdateNewAmmo(InventoryController.Instance.nullItem, itemInInventory.index);

            ResetAmmoStats(itemInInventory.ammoStats);
            //UpdateAmmoAmmountUI(itemInInventory.count, itemInInventory.index);
        }
        else
        {
            UpdateAmmoAmmountUI(itemInInventory.count, itemInInventory.index);
        }
        Debug.Log("Use Ammo");
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

        Debug.Log("Update After Reload");
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
        Debug.Log(item.ammoStats.artwork.name + item.count + index);
        WeaponSystemUI.Instance.SetDisplayItemIcon(item.ammoStats.artwork, index);
        //WeaponSystemUI.Instance.SetDisplayItemName(itemInInventory.ammoStats.name, index);
        UpdateAmmoAmmountUI(item.count, index);
    }

    public void UpdateDisplayItemIcon(Sprite sprite, int index)
    {
        WeaponSystemUI.Instance.SetDisplayItemIcon(sprite, index);
    }

    public void UpdateAmmoAmmountUI(int totalAmount, int index)
    {
        Debug.Log(totalAmount + " " + index);
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
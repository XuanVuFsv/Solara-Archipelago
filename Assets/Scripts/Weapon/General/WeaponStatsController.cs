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
    [SerializeField]
    Item itemInInventory;

    public string weaponName;
    public int currentAmmo, remainingAmmo, ammoInMagazine;
    public bool autoReload = true;
    public bool ofActiveAmmo;
    
    bool outOfAmmo; //out of ammo in magazine
    bool hasRun = false;

    //private bool notContainAmmo; //out of ammo in bag and can't refill ammo (magazine still can contain ammo or not)

    //public float penetrattionThickness;
    //public int dropOffDsitance;

    //public int decreseDamageRate;
    //public int damageHead;
    //public int damageBody;
    //public int damageArmsLegs;

    //public float fireRate;
    //public float reloadSpeed;

    //public ParticleSystem hitEffectPrefab;
    public GameObject magazineObject;
    public AnimationClip weaponAnimation;

    //public WaitForSeconds reloadTimer;
    //public Sprite artwork;
    //public Transform casingPrefab;
    //public TrailRenderer bulletTracer;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.parent.name);
        //reloadTimer = new WaitForSeconds(reloadSpeed);
        //Resource Load
        //Debug.Log("Create a WeaponStatsController instance");

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
        SetupAmmoStats(defaultAmmoOnStart);
        hasRun = true;
    }

    public Suckable GetDefaultAmmoOnStart()
    {
        return defaultAmmoOnStart;
    }

    public void SetupWeaponStats(WeaponStats weaponStats, bool sameWeaponSlotWithCurrentAmmo)
    {
        //Debug.Log("SetupWeaponStats");
        UpdateAmmoState();
        weaponName = weaponStats.name;
        weaponSlot = weaponStats.weaponSlot;
        //ammoInMagazine = weaponStats.magazine;
        //fireRate = weaponStats.fireRate;
        //reloadSpeed = weaponStats.reloadSpeed;
        weaponAnimation = weaponStats.weaponAnimation;
        ofActiveAmmo = sameWeaponSlotWithCurrentAmmo;
    }

    public void SetupAmmoStats(Suckable ammoPickup)
    {
        MyDebug.Instance.Log(ammoPickup);
        //if (!currentAmmoStatsController) Debug.Log("currentAmmoStatsController null " + transform.parent.name);
        //Debug.Log(ammoPickup.name);

        if (!currentAmmoStatsController.ammoStats)
        {

            //Debug.Log("Add to null " + transform.parent.name);
            currentAmmoStatsController.ammoStats = ammoPickup.GetAmmoStats();
            currentAmmoStatsController.AssignAmmotData();
            //ofActiveAmmo = weaponSlot == InventoryController.Instance.GetCurrentItem().ammoStats.weaponSlot;

            if (ofActiveAmmo)
            {
                //Debug.Log("Set" + currentAmmoStatsController.multiplierForAmmo);
                gunCamera.SetMultiplier(currentAmmoStatsController.multiplierForAmmo);
            }

            ammoPickup.AttachAmmoToObject(transform, false);

            pickAmmoEvent.Notify(currentAmmoStatsController.amplitudeGainImpulse);
            pickAmmoEvent.Notify(currentAmmoStatsController.multiplierRecoilOnAim);

            SetNewAmmoCount(ammoPickup);
            ammunitionChestPicked = ammoPickup;
        }
        else if (currentAmmoStatsController.ammoStats.name == ammoPickup.GetAmmoStats().name)
        {
            Debug.Log("Add same ammo");

            AddAmmo(ammoPickup.GetAmmoContain());

            if (!ammunitionChestPicked)
            {
                ammoPickup.AttachAmmoToObject(transform, false);
                ammunitionChestPicked = ammoPickup;
            }
            else Destroy(ammoPickup.gameObject);

            ammoPickup.SetAmmoContain(0);
        }
        else
        {
            //Debug.Log("Add new ammo");

            //if (ammunitionChestPicked)
            //{
            //    ammunitionChestPicked.DetachAmmoToObject(null, true);
            //}
            //ammunitionChestPicked.ammoContain = InventoryController.Instance.GetItem(ammunitionChestPicked.ammoStats).count;

            //currentAmmoStatsController.ammoStats = ammoPickup.ammoStats;
            ammoPickup.AttachAmmoToObject(transform, false);
            //currentAmmoStatsController.AssignAmmotData();

            //pickAmmoEvent.Notify(currentAmmoStatsController.amplitudeGainImpulse);
            //pickAmmoEvent.Notify(currentAmmoStatsController.multiplierRecoilOnAim);

            //SetNewAmmoCount(ammoPickup);
            InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.GetAmmoStats(), ammoPickup.GetAmmoContain());
            //ammunitionChestPicked = ammoPickup;
        }
    }

    void SetNewAmmoCount(Suckable ammoPickup)
    {
        currentAmmo = 0;
        outOfAmmo = true;

        itemInInventory = InventoryController.Instance.AddNewAmmoToInventory(ammoPickup.GetAmmoStats(), ammoPickup.GetAmmoContain());
        remainingAmmo = ammoPickup.GetAmmoContain();
        ammoInMagazine = ammoPickup.GetAmmoStats().ammoAllowedInMagazine;

        UpdateAmmoUI();
    }

    public void SwitchAmmo(int step)
    {
        InventoryController.Instance.SwitchItem(step);
    }

    public void UseAmmo(int ammo)
    {
        currentAmmo += ammo;
        //InventoryController.Instance.GetCurrentItem().AddAmmo(ammo);
        itemInInventory.AddAmmo(ammo);
        //remainingAmmo = InventoryController.Instance.GetCurrentItem().count - currentAmmo; //ammo in inventory or bag

        if (currentAmmo == 0)
        {
            outOfAmmo = true;
        }

        UpdateAmmoUI();
    }

    public void UpdateAmmoAfterReload()
    {
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

        UpdateAmmoUI();
    }

    public void UpdateAmmoState()
    {
        if (currentAmmo > 0) outOfAmmo = false;
        else outOfAmmo = true;
    }

    public void UpdateAmmoUI()
    {
        WeaponSystemUI.Instance.UpdateAmmo(currentAmmo, remainingAmmo);
    }

    public void UpdateInventoryUI()
    {
        
    }

    public void AddAmmo(int ammo)
    {
        itemInInventory.AddAmmo(ammo);
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

    //private void OnEnable()
    //{
    //    OnStart();
    //}
}
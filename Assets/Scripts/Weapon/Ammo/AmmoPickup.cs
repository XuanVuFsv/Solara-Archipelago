using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Suckable
{
    public RectTransform ammoUIPrefab, ammoUI;
    public PlantState plantState;
    public GameObject suckableSample;
    public bool canPickup = false;
    public bool hasParent = false;

    [SerializeField] ActiveWeapon activeWeapon;
    // Start is called before the first frame update
    void Start()
    {
        CreateAmmoUI();
        rigid = GetComponent<Rigidbody>();
        //Debug.Log("Create a AmmoPickup instance " + ammoStats.name + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        canPickup = true;
        if (other.CompareTag("Player"))
        {
            if (activeWeapon == null) activeWeapon = other.GetComponent<ActiveWeapon>();

            if (activeWeapon.triggerAmmoList.Count > 0)
            {
                if (!ammoUI)
                {
                    CreateAmmoUI();
                }

                if (activeWeapon.triggerAmmoList.Count == 1)
                {
                    ShowAmmoStats();
                }
                else
                {
                    if (this == activeWeapon.GetNearestAmmo())
                    {
                        ShowAmmoStats();
                    }
                    else
                    {
                        ammoUI.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                ammoUI.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeWeapon = null;

            ammoUI.gameObject.SetActive(false);
            canPickup = false;
        }
    }

    public override void ChangeToStored()
    {
        suckableSample.GetComponent<Plant>().ChangeToStored();
        suckableSample.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;

        gameObject.SetActive(false);
    }

    public void CreateAmmoUI()
    {
        ammoUI = Instantiate(ammoUIPrefab, transform);
        //weaponUI.localScale = CalcualteLocalScale(0.19f, 0.19f, 0.19f, transform.parent.localScale);
        //int multiplier = weaponStats.name.Length - standardLengthWeponName;
        //if (multiplier > 0) weaponUI.GetChild(0).GetComponent<WeaponUI>().panel.localPosition -= offsetPerOverLetter * multiplier * Vector3.right;

        ammoUI.gameObject.SetActive(false);
    }

    public void ShowAmmoStats()
    {
        ammoUI.gameObject.SetActive(true);
        ammoUI.GetChild(0).GetComponent<WeaponUI>().weaponName.text = GetAmmoStats().name;
    }

    public override void AttachAmmoToObject(Transform parent, bool isVisible)
    {
        hasParent = true;

        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(isVisible);

        if (!parent.GetComponent<WeaponStatsController>().ofActiveAmmo) return;

        PoolingManager.Instance.AddGameEvent("Pool" + GetAmmoStats().name + "Setup");
        Debug.Log("Add Game Event Pool" + GetAmmoStats().name + "Setup");

        //if (ammoStats.name == "Berry")
        //{
        //    //Debug.Log("Add " + ammoStats.name);
        //    //PoolingManager.Instance.SetActiveForPool("BulletHoleManager", true);
        //    PoolingManager.Instance.AddGameEvent("Pool" + ammoStats.name + "Setup");
        //}
        //else if (ammoStats.name == "Tomato")
        //{
        //    //Debug.Log("Add " + ammoStats.name);
        //    //PoolingManager.Instance.SetActiveForPool("BulletHoleManager1", true);
        //    PoolingManager.Instance.AddGameEvent("PoolSetup1");
        //}
        //else if (ammoStats.name == "Star")
        //{
        //    //Debug.Log("Add " + ammoStats.name);
        //    PoolingManager.Instance.AddGameEvent("PoolSetup2");
        //}
    }

    public void DetachAmmoToObject(Transform parent, bool isVisible)
    {
        //hasParent = false;

        //transform.parent = parent;
        //transform.eulerAngles = Vector3.zero;
        //gameObject.SetActive(isVisible);

        PoolingManager.Instance.RemoveGameEvent("Pool" + GetAmmoStats().name + "Setup");
        Destroy(gameObject);

        //if (ammoStats.name == "Berry")
        //{
        //    //PoolingManager.Instance.SetActiveForPool("BulletHoleManager", false);
        //    PoolingManager.Instance.RemoveGameEvent("PoolSetup");
        //}
        //else if (ammoStats.name == "Tomato")
        //{
        //    //PoolingManager.Instance.SetActiveForPool("BulletHoleManager1", false);
        //    PoolingManager.Instance.RemoveGameEvent("PoolSetup1");
        //}
        //else if (ammoStats.name == "Star")
        //{
        //    //PoolingManager.Instance.SetActiveForPool("BulletHoleManager1", false);
        //    PoolingManager.Instance.RemoveGameEvent("PoolSetup2");
        //}
    }
}

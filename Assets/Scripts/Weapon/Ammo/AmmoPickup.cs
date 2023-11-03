using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Suckable
{
    //public RectTransform ammoUIPrefab, ammoUI;
    public PlantState plantState;
    public GameObject suckableSample;
    //public bool canPickup = false;
    //public bool hasParent = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateAmmoUI();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        //Debug.Log("Create a AmmoPickup instance " + ammoStats.name + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //canPickup = true;
        //if (other.CompareTag("Player"))
        //{
        //    if (activeWeapon == null) activeWeapon = other.GetComponent<ActiveWeapon>();

        //    if (activeWeapon.triggerAmmoList.Count > 0)
        //    {
        //        if (!ammoUI)
        //        {
        //            CreateAmmoUI();
        //        }

        //        if (activeWeapon.triggerAmmoList.Count == 1)
        //        {
        //            ShowAmmoStats();
        //        }
        //        else
        //        {
        //            if (this == activeWeapon.GetNearestAmmo())
        //            {
        //                ShowAmmoStats();
        //            }
        //            else
        //            {
        //                ammoUI.gameObject.SetActive(false);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ammoUI.gameObject.SetActive(false);
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    activeWeapon = null;

        //    ammoUI.gameObject.SetActive(false);
        //    canPickup = false;
        //}
    }

    public override void ChangeToStored()
    {
        suckableSample.transform.position = CollectHandler.Instance.shootingInputData.bulletSpawnPoint.position;
        rigid.velocity = Vector3.zero;
        suckableSample.GetComponent<Plant>().ChangeToStored();
        collider.isTrigger = false;

        gameObject.SetActive(false);
    }

    public void CreateAmmoUI()
    {
        //ammoUI = Instantiate(ammoUIPrefab, transform);
        ////weaponUI.localScale = CalcualteLocalScale(0.19f, 0.19f, 0.19f, transform.parent.localScale);
        ////int multiplier = weaponStats.name.Length - standardLengthWeponName;
        ////if (multiplier > 0) weaponUI.GetChild(0).GetComponent<WeaponUI>().panel.localPosition -= offsetPerOverLetter * multiplier * Vector3.right;

        //ammoUI.gameObject.SetActive(false);
    }

    public void ShowAmmoStats()
    {
        //ammoUI.gameObject.SetActive(true);
        //ammoUI.GetChild(0).GetComponent<WeaponUI>().weaponName.text = ammoStats.name;
    }
}

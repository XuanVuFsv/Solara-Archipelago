using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHandler : Singleton<CollectHandler>, IAxieCollectorWeaponStragety
{
    public ShootingInputData shootingInputData;
    public AnimationCurve velocityCurve;
    public WeaponStatsController weaponStatsController;

    RaycastHit hit;
    [SerializeField]
    private float maxDistance = 3;
    [SerializeField]
    private float distanceThresholdToGotAmmo = 0.45f;
    [SerializeField]
    private float radiusSphereCastToCheckSucked = 0.1f;

    public float minSuckUpSpeed, maxSuckUpSpeed;
    public int moveOutForce;
    public float acceleratonSuckUpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        weaponStatsController = GetComponent<WeaponStatsController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInputData(object _inputData)
    {
        shootingInputData = _inputData as ShootingInputData;
    }

    public ShootingInputData GetShootingInputData()
    {
        return shootingInputData;
    }

    public bool HasShootingInputData()
    {
        return (shootingInputData != null && shootingInputData.bulletSpawnPoint != null);
    }

    public void HandleLeftMouseClick()
    {
        CollectingHandle();
    }

    public void HandleRightMouseClick()
    {
        //Debug.Log("Handle Right Click");
        ShootOutHandle();
    }

    public void CollectingHandle()
    {
        if (Physics.Raycast(shootingInputData.raycastOrigin.position, shootingInputData.fpsCameraTransform.forward, out hit, maxDistance, shootingInputData.layerMask))
        {
            //Debug.Log(hit.transform.name);
            Suckable suckedObject = hit.transform.GetComponent<Suckable>();
            if (suckedObject is Plant && !(suckedObject as Plant).CanSuckUp())
            {
                //Debug.Log(suckedObject.transform.parent + suckedObject.name + (suckedObject as Plant).plantState);
                //Debug.Log(!(suckedObject as Plant).CanSuckUp());
                return;
            }
            //suckedObject.ResetVelocity();
            suckedObject?.GoToAxieCollector();
            if (suckedObject as Plant) (suckedObject as Plant).inCrafting = false;



            //Debug.Log(Vector3.Distance(shootingInputData.raycastOrigin.position, hit.transform.position));
            if (Physics.SphereCast(shootingInputData.raycastOrigin.position, radiusSphereCastToCheckSucked, shootingInputData.fpsCameraTransform.forward, out hit, distanceThresholdToGotAmmo, shootingInputData.layerMask))
            {
                try
                {
                    if(suckedObject.ammoStats != null)
                    {
                        //Debug.Log(suckedObject.ammoStats.name);
                        if (suckedObject.ammoStats.name == "AXS")
                        {
                            //Debug.Log("Earn");
                            AXSManager.Instance.Add((suckedObject.ammoContain * 1f) / 10f);
                            suckedObject.ammoContain = 0;
                            (suckedObject as Plant).DestroyThis();
                        }
                        else
                        {
                            AmmoStats ammoStats = suckedObject.ammoStats;
                            weaponStatsController.SuckUpAmmo(suckedObject);
                        }
                    }
                    //InventoryController.Instance.AddNewAmmoToInventory(ammoStats, suckedObject.GetAmmoContain());
                }
                catch
                {
                }
            }
        }
    }

    public void ShootOutHandle()
    {
        if (weaponStatsController.itemInInventory.ammoStats.name == "Null" || weaponStatsController.itemInInventory == null) return;
        weaponStatsController.UseAmmo(weaponStatsController.currentAmmoStatsController.bulletCount);
    }
}

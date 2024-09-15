using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectHandler : Singleton<CollectHandler>, IAxieCollectorWeaponStragety
{
    public ShootingInputData shootingInputData;
    public AnimationCurve velocityCurve;
    public WeaponStatsController weaponStatsController;

    RaycastHit hit;
    [SerializeField]
    private float maxDistance = 3;

    public float distanceThresholdToGotAmmo = 0.45f;
    public float radiusSphereCastToCheckSucked = 0.1f;
    public float minSuckUpSpeed, maxSuckUpSpeed;
    public int moveOutForce;
    public float acceleratonSuckUpSpeed;
    public float suckUpSpeedScale = 1f;

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
            Debug.Log(hit.transform.name);

            //suckedObject.ResetVelocity();

            if (suckedObject as WaterObject) (suckedObject as WaterObject).suckedPosition = hit.transform.position;
            if (suckedObject as Plant) (suckedObject as Plant).inCrafting = false;
            suckedObject?.GoToAxieCollector();

            //Debug.Log(Vector3.Distance(shootingInputData.raycastOrigin.position, hit.transform.position));
            if (Physics.SphereCast(shootingInputData.raycastOrigin.position, radiusSphereCastToCheckSucked, shootingInputData.fpsCameraTransform.forward, out hit, distanceThresholdToGotAmmo, shootingInputData.layerMask))
            {
                try
                {
                    if (suckedObject is Plant)
                    {
                        if (suckedObject.ammoStats != null)
                        {
                            AmmoStats ammoStats = suckedObject.ammoStats;
                            weaponStatsController.SuckUpAmmo(suckedObject);
                        }
                    }
                    else if (suckedObject is WaterObject)
                    {
                        Debug.Log("Detect Water");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
    }

    public void ShootOutHandle()
    {
        if (weaponStatsController.itemInInventory.ammoStats.name == "Null" || weaponStatsController.itemInInventory == null) return;
        weaponStatsController.UseAmmo(1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(shootingInputData.bulletSpawnPoint.position, shootingInputData.fpsCameraTransform.forward * hit.distance);
        Gizmos.DrawSphere(shootingInputData.bulletSpawnPoint.position + shootingInputData.fpsCameraTransform.forward * hit.distance, radiusSphereCastToCheckSucked);
    }
}

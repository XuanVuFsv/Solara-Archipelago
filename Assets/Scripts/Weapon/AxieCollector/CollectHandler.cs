using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectHandler : Singleton<CollectHandler>, IAxieCollectorWeaponStragety
{
    public enum WaterMode
    {
        Off = 0,
        Salt = 1,
        Fresh = 2,
    }

    public ShootingInputData shootingInputData;
    public AnimationCurve velocityCurve;
    public WeaponStatsController weaponStatsController;

    RaycastHit hit;
    [SerializeField]
    private float maxDistance = 3;

    public int moveOutForce;
    public float distanceThresholdToGotAmmo = 0.45f;
    public float radiusSphereCastToCheckSucked = 0.1f;
    public float minSuckUpSpeed, maxSuckUpSpeed;
    public float acceleratonSuckUpSpeed;
    public float suckSpeed = 1f;

    public WaterMode waterMode;

    // Start is called before the first frame update
    void Start()
    {
        weaponStatsController = GetComponent<WeaponStatsController>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchToWaterMode();
    }

    public void SwitchToWaterMode()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            waterMode += 1;
            if ((int)waterMode > 2) waterMode = 0;
        }
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
            if (hit.transform.CompareTag("Cluster"))
            {
                ClusterResource cluster = hit.transform.GetComponent<ClusterResource>();
                cluster.StartCollect();
                return;
            }

            Suckable suckedObject = hit.transform.GetComponent<Suckable>();
            if (suckedObject is Plant && !(suckedObject as Plant).CanSuckUp())
            {
                return;
            }

            if (suckedObject as WaterObject) (suckedObject as WaterObject).suckedPosition = hit.transform.position;

            suckedObject.GoToAxieCollector();

            if (suckedObject is WaterObject)
            {
                WaterManager.Instance.CollectWater((suckedObject as WaterObject).state, suckSpeed * Time.deltaTime);
            }

            if (Physics.SphereCast(shootingInputData.raycastOrigin.position, radiusSphereCastToCheckSucked, shootingInputData.fpsCameraTransform.forward, out hit, distanceThresholdToGotAmmo, shootingInputData.layerMask))
            {
                try
                {
                    if (suckedObject is Plant || suckedObject is PowerContainer || suckedObject is NaturalResource)
                        {
                        if (suckedObject.ammoStats != null)
                        {
                            AmmoStats ammoStats = suckedObject.ammoStats;
                            weaponStatsController.SuckUpAmmo(suckedObject);
                        }
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
        if (waterMode != WaterMode.Off) {
            WaterManager.Instance.BlowWater(suckSpeed * Time.deltaTime);
            return;
        }

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

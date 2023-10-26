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

    public int minSuckUpSpeed, maxSuckUpSpeed;
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
        ShootOutHandle();
    }

    public void CollectingHandle()
    {
        if (Physics.Raycast(shootingInputData.raycastOrigin.position, shootingInputData.fpsCameraTransform.forward, out hit, maxDistance, shootingInputData.layerMask))
        {
            //Debug.Log(hit.transform.name);
            Suckable suckedObject = hit.transform.GetComponent<Suckable>();
            suckedObject?.GoToAxieCollector();

            if (Vector3.Distance(shootingInputData.bulletSpawnPoint.position, hit.transform.position) <= 0.5f)
            {
                AmmoStats ammoStats = suckedObject.GetAmmoStats();
                weaponStatsController.SuckUpAmmo(suckedObject);
                //InventoryController.Instance.AddNewAmmoToInventory(ammoStats, suckedObject.GetAmmoContain());
            }
        }
    }

    public void ShootOutHandle()
    {
        weaponStatsController.UseAmmo(1);
    }
}

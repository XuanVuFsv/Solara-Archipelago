using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHandler : MonoBehaviour, IAxieCollectorWeaponStragety
{
    public ShootingInputData shootingInputData;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
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

    }

    public void ShootOutHandle()
    {

    }
}

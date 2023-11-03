using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

// The RaycastWeapon as Context defines the interface of interest to clients.
public class RaycastWeapon : MonoBehaviour
{
    public IWeaponStragety weaponHandler;

    public AmmoStatsController ammoStatsController;
    public ShootController shootController;
    CameraShake cameraShake;
    GunCameraShake gunCameraShake;

    public GameObject bulletPrefab, magazineObject;

    public GameEvent hitEvent;

    public Transform raycastOrigin;
    public Transform fpsCameraTransform;

    public Transform bulletSpawnPoint, casingSpawnPoint;

    public ParticleSystem hitEffectPrefab;
    public ParticleSystem muzzleFlash;

    //public AnimationClip weaponAnimation;

    WeaponStats weaponStats;
    int layerMask;

    private void Start()
    {
        cameraShake = GetComponent<CameraShake>();
        weaponStats = GetComponent<WeaponStatsController>().weaponStats;

        fpsCameraTransform = Camera.main.transform;
        layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Ignore Player") | 1 << LayerMask.NameToLayer("Only Player"));

        if (gameObject.activeInHierarchy) SetAsWeaponStrategy();
    }

    public async UniTaskVoid SetAsInputData(LayerMask _layerMask)
    {
        await UniTask.WaitUntil(() => ammoStatsController.ammoStats != null);
        ShootingInputData shootingInputData = new ShootingInputData(shootController, ammoStatsController.ammoStats.shootingHandleType, ammoStatsController, raycastOrigin, fpsCameraTransform, hitEvent, cameraShake, bulletSpawnPoint, _layerMask);
        weaponHandler.SetInputData(shootingInputData);
    }

    public void SetAsWeaponStrategy()
    {
        if (gameObject.activeInHierarchy == false) return;

        if (GetComponent<ShootingHandler>())
        {
            weaponHandler = GetComponent<ShootingHandler>();
            var _setInput = SetAsInputData(layerMask);
        }
        else if (GetComponent<ActionHandler>())
        {
            weaponHandler = GetComponent<ActionHandler>();
            var setInput = SetAsInputData(layerMask);
        }
        else
        {
            weaponHandler = gameObject.GetComponent<CollectHandler>();
            var setInput = SetAsInputData(layerMask = (1 << LayerMask.NameToLayer("Suckable")));
        }
    }

    /// <summary>
    /// Run this method to shoot and select what type of bullet <see cref="AmmoStats.ShootingHandleType"/> and how to bullet interact to other objects
    /// </summary>
    public void HandleLeftMouseClick()
    {
        if (muzzleFlash) muzzleFlash.Emit(1);
        weaponHandler.HandleLeftMouseClick();
    }

    /// <summary>
    /// Run this method to aim. An ammo type can aim or not and how a gun aim with different ammo will depend on their ammo type. Check this:
    /// <see cref="AmmoStats.ZoomType"></see>
    /// </summary>
    public void HandleRightMouseClick()
    {
        weaponHandler.HandleRightMouseClick();
    }

    public void StopFiring()
    {
        cameraShake.ResetRecoil();
    }

    public CameraShake GetCameraShake()
    {
        return cameraShake;
    }

    public GunCameraShake GetGunCameraShake()
    {
        return gunCameraShake;
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor) return;
        Gizmos.color = Color.green;
        Vector3 direction = fpsCameraTransform.forward;

        if (ammoStatsController.ammoStats.zoomType == AmmoStats.ZoomType.HasScope)
        {
            Vector3 localDirection = Vector3.forward + cameraShake.GetCurrentPatternVector();
            direction = fpsCameraTransform.TransformDirection(localDirection).normalized;
        }
        //foreach (Vector3 pattern in currentShootingMechanic.bulletDirectionPattern)
        //{
        //    Vector3 localDirection = Vector3.forward + pattern;
        //    Vector3 direction = fpsCameraTransform.TransformDirection(localDirection).normalized;

        //    Gizmos.DrawRay(raycastOrigin.position, direction);
        //}

        Gizmos.DrawRay(raycastOrigin.position, direction);
        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + direction * ammoStatsController.range);
    }

    private void OnEnable()
    {

    }
}
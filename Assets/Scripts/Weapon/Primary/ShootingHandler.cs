using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHandler : MonoBehaviour, IPrimaryWeaponStragety
{
    public ShootingInputData shootingInputData;
    public RaycastWeapon raycastWeapon;
    public WeaponStatsController weaponStatsController;
    RaycastHit hit;

    private void Start()
    {
        raycastWeapon = GetComponent<RaycastWeapon>();
        weaponStatsController = GetComponent<WeaponStatsController>();
    }

    public void SetInputData(object _inputData)
    {
        shootingInputData = _inputData as ShootingInputData;
    }

    public ShootingInputData GetShootingInputData()
    {
        return shootingInputData;
    }

    public void HandleLeftMouseClick()
    {
        if (weaponStatsController.itemInInventory.ammoStats.weaponSlot != ActiveWeapon.WeaponSlot.AttackGun) return;
        ShootingHandle();
        shootingInputData.source.PlayOneShot(shootingInputData.source.clip);
    }

    public void HandleRightMouseClick()
    {
        if (shootingInputData.ammoStatsController.zoomType == AmmoStats.ZoomType.NoZoom) return;
        PlayAimAnimation();
    }

    public void ShootingHandle()
    {
        if (shootingInputData.ammoStatsController.ammoStats.weaponSlot != ActiveWeapon.WeaponSlot.AttackGun) return;
        shootingInputData.shootController.ApplyAttackAnimation();
        if (shootingInputData.shootingHandleType == AmmoStats.ShootingHandleType.Raycast)
        {
            //Debug.Log("Shoot");
            RaycastHandle();
        }
        else if (shootingInputData.shootingHandleType == AmmoStats.ShootingHandleType.InstantiateBullet)
        {
            InstantiateBulletHandle();
        }
    }

    void RaycastHandle()
    {
        if (shootingInputData.ammoStatsController.bulletCount == 1)
        {
            Enemy enemy;
            shootingInputData.cameraShake.GenerateRecoil(shootingInputData.ammoStatsController.zoomType);
            //Debug.Log("Shoot");

            if (Physics.Raycast(shootingInputData.raycastOrigin.position, shootingInputData.fpsCameraTransform.forward, out hit, shootingInputData.ammoStatsController.range, shootingInputData.layerMask))
            {
                //Debug.Log(hit.transform);
                //hitEffectPrefab.transform.position = hit.point;
                //hitEffectPrefab.transform.forward = hit.normal;
                //hitEffectPrefab.Emit(5);

                PoolingManager.Instance.Get("Pool" + shootingInputData.ammoStatsController.ammoStats.name + "Setup");
                shootingInputData.hitEvent.Notify(hit);
                //currentHitObject = hit.collider.name;
                enemy = hit.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Destroy(enemy.gameObject);
                    }

                //Tracer here
                //Damage Handle here

                #region Minigame Test
                //tracer.transform.position = hit.point;
                if (hit.transform.gameObject.tag == "Wall")
                {
                    GameObject wall = hit.transform.gameObject;
                    WallSpawner.Instance.DestroyWall(wall.GetComponent<WallBehaviour>().index);
                }
                #endregion
            }
            //else tracer.transform.position += _fpsCameraTransform.forward * range;
            weaponStatsController.UseAmmo(1);
        }
        else
        {
            //Debug.Log("Shoot");
            List<RaycastHit> raycastHits = new List<RaycastHit>();

            //int i = 0;
            bool destroyedObstacle = false;
            Enemy enemy;

            foreach (Vector3 pattern in shootingInputData.ammoStatsController.ammoStats.bulletDirectionPattern)
            {
                Vector3 localDirection = Vector3.forward + pattern;
                Vector3 direction = shootingInputData.fpsCameraTransform.TransformDirection(localDirection).normalized;

                shootingInputData.cameraShake.GenerateRecoil(shootingInputData.ammoStatsController.zoomType);
                if (Physics.Raycast(shootingInputData.raycastOrigin.position, direction, out hit, shootingInputData.ammoStatsController.range, shootingInputData.layerMask))
                {
                    raycastHits.Add(hit);
                    //Debug.Log("Shooting " + hit.transform.name + " Pool: " + "Pool" + shootingInputData.ammoStatsController.ammoStats.name + "Setup");
                    PoolingManager.Instance.Get("Pool" + shootingInputData.ammoStatsController.ammoStats.name + "Setup");
                    shootingInputData.hitEvent.Notify(hit);
                    enemy = hit.transform.GetComponent<Enemy>();
                    #region Minigame Test
                    //tracer.transform.position = hit.point;
                    if (!destroyedObstacle && hit.transform.gameObject.tag == "Wall")
                    {
                        GameObject wall = hit.transform.gameObject;
                        WallSpawner.Instance.DestroyWall(wall.GetComponent<WallBehaviour>().index);
                        destroyedObstacle = true;
                    }
                    #endregion
                }
            }

            weaponStatsController.UseAmmo(1);
        }
    }

    void InstantiateBulletHandle()
    {
        Vector3 direction = shootingInputData.fpsCameraTransform.forward;

        if (shootingInputData.ammoStatsController.ammoStats.zoomType == AmmoStats.ZoomType.HasScope)
        {
            MyDebug.Log("Shoot");
            Vector3 localDirection = Vector3.forward + shootingInputData.cameraShake.GetCurrentPatternVector();
            direction = shootingInputData.fpsCameraTransform.TransformDirection(localDirection).normalized;
        }

        GameObject newBullet = Instantiate(shootingInputData.ammoStatsController.ammoStats.bulletObject, shootingInputData.bulletSpawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<BulletBehaviour>().TriggerBullet(shootingInputData.ammoStatsController.ammoStats.name, shootingInputData.ammoStatsController.force, direction);
        shootingInputData.cameraShake.GenerateRecoil(shootingInputData.ammoStatsController.zoomType);

        weaponStatsController.UseAmmo(1);
    }

    public void PlayAimAnimation()
    {
        //MyDebug.Log("Handle Right Click");

        ShootController shootController = shootingInputData.shootController;
        if (!shootController.rigController) return;

        shootController.inAim = !shootController.inAim;
        shootController.aimEvent.Notify(shootController.inAim);

        if (shootController.inAim) shootController.aimEvent.Notify(shootingInputData.ammoStatsController.multiplierRecoilOnAim);

        shootController.rigController.SetBool("inAim", shootController.inAim);
    }
}
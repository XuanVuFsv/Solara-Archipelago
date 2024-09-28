using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropStatsController : MonoBehaviour
{
    public CropStats cropStats;

    //public bool canAttack;

    public CropStats.ZoomType zoomType;

    //public int dropOffDsitance;

    //public int decreseDamageRate;
    //public int damageHead;
    //public int damageBody;
    //public int damageArmsLegs;
    public int damage;
    public int damageDealToSpecificEnemy;
    public int range;
    public int force;
    public int amplitudeGainImpulse;

    public float fireRate;
    public float reloadSpeed;
    public float multiplierRecoilOnAim;
    public float multiplierForAmmo;

    public bool hasAssignCropData = false;

    public RaycastWeapon raycastWeapon;
    public WeaponStatsController weaponStatsController;
    public ParticleSystem hitEffectPrefab;
    public GameObject trailTracer;
    public GameObject bulletObject;

    public int bulletCount;

    public WaitForSeconds reloadTimer;

    [SerializeField]
    CameraShake cameraShake;

    // Start is called before the first frame update
    void Awake()
    {

    }

    private void Start()
    {
        cameraShake = gameObject.GetComponent<CameraShake>();
        raycastWeapon = GetComponent<RaycastWeapon>();
        weaponStatsController = GetComponent<WeaponStatsController>();

        if (cropStats)
        {
            AssignCroptData();
        }
    }

    void SetCropStats(CropStats newCropStats)
    {
        cropStats = newCropStats;
    }

    public void AssignCroptData()
    {
        //if (hasAssignCropData) return;
        //GetComponent<RaycastWeapon>().currentShootingMechanic = cropStats.shootingMechanic;
        //canAttack = cropStats.canAttack;

        zoomType = cropStats.zoomType;

        //dropOffDsitance = cropStats.dropOffDsitance;
        //decreseDamageRate = cropStats.decreseDamageRate;

        //damageHead = cropStats.damageHead;
        //damageBody = cropStats.damageBody;
        //damageArmsLegs = cropStats.damageArmsLegs;
        damage = cropStats.damage;
        damageDealToSpecificEnemy = cropStats.damageDealToSpecificEnemy;
        range = cropStats.range;
        force = cropStats.force;
        amplitudeGainImpulse = cropStats.amplitudeGainImpulse;
        multiplierRecoilOnAim = cropStats.multiplierRecoilOnAim;
        multiplierForAmmo = cropStats.multiplierForAmmo;

        fireRate = cropStats.fireRate;
        reloadSpeed = cropStats.reloadSpeed;

        bulletCount = cropStats.bulletCount;

        reloadTimer = new WaitForSeconds(reloadSpeed * 0);

        hitEffectPrefab = cropStats.hitEffectPrefab;
        trailTracer = cropStats.trailTracer;
        bulletObject = cropStats.bulletObject;

        cameraShake.AssignRecoilPattern(cropStats.recoildPattern);

        weaponStatsController.ammoInMagazine = cropStats.ammoAllowedInMagazine;
        raycastWeapon.SetAsWeaponStrategy();
    }
}